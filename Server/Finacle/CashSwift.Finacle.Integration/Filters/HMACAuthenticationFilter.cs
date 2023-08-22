using System.Reflection;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using CashSwift.API.Messaging;
using CashSwift.Finacle.Integration.DataAccess.Dapper;
using CashSwift.Library.Standard.Logging;
using CashSwift.Library.Standard.Security;
using CashSwift.Library.Standard.Utilities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace CashSwift.Finacle.Integration.Filters
{
    public class HMACAuthenticationFilter : IAsyncActionFilter, IFilterMetadata
    {
        private readonly int requestMaxAgeInSeconds = 300;

        private IAPIUserDataAccess DBContext;

        private ICashSwiftAPILogger Log;

        private bool skipHMACAuthentication;

        public HMACAuthenticationFilter(IConfiguration configuration, IAPIUserDataAccess dBContext, ICashSwiftAPILogger logger)
        {
            Log = logger;
            DBContext = dBContext;
            skipHMACAuthentication = configuration.GetSection("Environment").GetValue("DisableHMAC", defaultValue: false);
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await OnActionExecutingAsync(context);
            if (context.Result == null)
            {
                await OnActionExecutedAsync(await next());
            }
        }

        public async Task OnActionExecutedAsync(ActionExecutedContext context)
        {
            if (skipHMACAuthentication)
            {
                return;
            }
            string requestUri = context.HttpContext.Request.GetEncodedUrl();
            string requestHttpMethod = context.HttpContext.Request.Method;
            IActionResult result = context.Result;
            if (!(result is ObjectResult json))
            {
                return;
            }
            object value = json.Value;
            if (value is APIMessageBase response)
            {
                var APIUser = await DBContext.GetAPIUserAsync(response.AppID);
                if (APIUser == null)
                {
                    Log.Error(null, null, null, "HMACAuthentication", "User null in DB", "OnActionExecutedAsync", $"User {response.AppID} is invalid");
                    context.Result = new UnauthorizedResult();
                }
                string content = JsonConvert.SerializeObject(response);
                context.HttpContext.Response.Headers.Add("hmacAuth", APIHashing.GetAuthHeader(APIUser.AppId, requestUri, requestHttpMethod, APIUser.AppKey, content));
            }
        }

        public async Task OnActionExecutingAsync(ActionExecutingContext context)
        {
            if (skipHMACAuthentication)
            {
                return;
            }
            ControllerActionDescriptor descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            IEnumerable<CustomAttributeData> attributes = descriptor.MethodInfo.CustomAttributes;
            if (!attributes.Any((a) => a.AttributeType == typeof(HMACAuthentication)))
            {
                skipHMACAuthentication = true;
                return;
            }
            HttpRequest Request = context.HttpContext.Request;
            string rawAuthzHeader = Request.Headers["hmacauth"].FirstOrDefault();
            Log.Trace(null, null, null, "HMACAuthentication", "Header", "OnActionExecutingAsync", "rawAuthzHeader = {0}", rawAuthzHeader);
            if (!string.IsNullOrEmpty(rawAuthzHeader))
            {
                string[] autherizationHeaderArray = GetAutherizationHeaderValues(rawAuthzHeader);
                if (autherizationHeaderArray != null)
                {
                    Guid APPId = new Guid(autherizationHeaderArray[0]);
                    string incomingBase64Signature = autherizationHeaderArray[1];
                    string nonce = autherizationHeaderArray[2];
                    string requestTimeStamp = autherizationHeaderArray[3];
                    if (!await IsValidRequestAsync(context, APPId, incomingBase64Signature, nonce, requestTimeStamp))
                    {
                        Log.Error(null, null, null, "HMACAuthentication", "Unauthorized", "OnActionExecutingAsync", "Invalid message");
                        context.Result = new UnauthorizedResult();
                    }
                }
                else
                {
                    Log.Error(null, null, null, "HMACAuthentication", "Unauthorized", "OnActionExecutingAsync", "Invalid auth header array");
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                Log.Error(null, null, null, "HMACAuthentication", "Unauthorized", "OnActionExecutingAsync", "No auth header");
                context.Result = new UnauthorizedResult();
            }
        }

        private string[]? GetAutherizationHeaderValues(string rawAuthzHeader)
        {
            string[] array = rawAuthzHeader.Split(':');
            if (array.Length == 4)
            {
                return array;
            }
            return null;
        }

        private async Task<bool> IsValidRequestAsync(ActionExecutingContext context, Guid APP_ID, string incomingBase64Signature, string nonce, string requestTimeStamp)
        {
            HttpRequest Request = context.HttpContext.Request;
            string requestUri = Request.GetEncodedUrl();
            string requestHttpMethod = Request.Method;
            var APIUser = await DBContext.GetAPIUserAsync(APP_ID);
            if (APIUser == null)
            {
                Log.Error(null, null, null, "HMACAuthentication", "Unauthorized", "IsValidRequestAsync", $"User {APP_ID} is invalid");
                return false;
            }
            byte[] sharedKey = APIUser.AppKey;
            if (isReplayRequest(nonce, requestTimeStamp))
            {
                Log.Error(null, null, null, "HMACAuthentication", "Unauthorized", "IsValidRequestAsync", "Repeat");
                return false;
            }
            APIMessageBase APIMessage = context.ActionArguments["request"] as APIMessageBase;
            Log.Info(APIMessage.SessionID, APIMessage.MessageID, APIMessage.AppName, "HMACAuthentication", "Unauthorized", "IsValidRequestAsync", "Request = {0}", APIMessage);
            if (APIMessage.AppID != APIUser.AppId)
            {
                Log.Info(APIMessage.SessionID, APIMessage.MessageID, APIMessage.AppName, "HMACAuthentication", "Unauthorized", "IsValidRequestAsync", "Request Message AppID {0} != Header Api User AppID {1}", APIMessage.AppID, APIUser.AppId);
                return false;
            }
            string body = APIMessage.ToStringWithHidden();
            string requestContentBase64String = CashSwiftHashing.SHA256WithEncode(body, Encoding.Unicode);
            string data = $"{APP_ID}{requestHttpMethod}{requestUri}{requestTimeStamp}{nonce}{requestContentBase64String}";
            byte[] secretKeyBytes = sharedKey;
            byte[] signature = Encoding.UTF8.GetBytes(data);
            using HMACSHA256 hmac = new HMACSHA256(secretKeyBytes);
            byte[] signatureBytes = hmac.ComputeHash(signature);
            string signatureString = signatureBytes.ToBase64String();
            bool r = incomingBase64Signature.Equals(signatureString, StringComparison.Ordinal);
            if (!r)
            {
                Log.Error(APIMessage.SessionID, APIMessage.MessageID, APIMessage.AppName, "HMACAuthentication", "Unauthorized", "IsValidRequestAsync", "Invalid hash {0} != {1}", incomingBase64Signature, signatureString);
            }
            return r;
        }

        private bool isReplayRequest(string nonce, string requestTimeStamp)
        {
            if (MemoryCache.Default.Contains(nonce))
            {
                Log.Error(null, null, null, "HMACAuthentication", "Unauthorized", "isReplayRequest", "Nonce");
                return true;
            }
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            long num = Convert.ToInt64((DateTime.UtcNow - dateTime).TotalSeconds);
            long num2 = Convert.ToInt64(requestTimeStamp);
            if (Math.Abs(num - num2) > requestMaxAgeInSeconds)
            {
                Log.Error(null, null, null, "HMACAuthentication", "Unauthorized", "isReplayRequest", "Time: serverTotalSeconds {0} - requestTotalSeconds {1} = {2} > requestMaxAgeInSeconds {3}", num, num2, Math.Abs(num - num2), requestMaxAgeInSeconds);
                return true;
            }
            MemoryCache.Default.Add(nonce, requestTimeStamp, DateTimeOffset.UtcNow.AddSeconds(requestMaxAgeInSeconds));
            return false;
        }

        private static byte[]? ComputeHash(byte[] content)
        {
            byte[] result = null;
            if (content.Length != 0)
            {
                result = CashSwiftHashing.ComputeHash(content, SHA256.Create());
            }
            return result;
        }
    }
}