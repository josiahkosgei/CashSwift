using CashSwift.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using CashSwift.API.Messaging.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using CashSwift.Library.Standard.Utilities;
using System.Net;

namespace CashSwift.API.Messaging.APIClients
{
    public abstract class FinacleIntegrationAPIClient
    {
        private CashSwiftAPILogger Log;
        private string apiBaseAddress;
        private IConfiguration configuration;

        protected FinacleIntegrationAPIClient(CashSwiftAPILogger cashSwiftAPILogger, string apiBaseAddress, IConfiguration configuration)
        {
            Log = cashSwiftAPILogger;
            this.apiBaseAddress = apiBaseAddress;
            this.configuration = configuration;
        }

        public async Task<T> SendAsync<T>(string endpoint, APIMessageBase message)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                HttpClient httpClient = new HttpClient(clientHandler);
                Log.Trace(message.SessionID, message.MessageID, message.AppName, nameof(FinacleIntegrationAPIClient), "Generate Json", nameof(SendAsync), "Converting APIMessage to Json");
                //string stringWithHidden = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Ignore});
                string Message = message.ToString();
                Log.Debug(message.SessionID, message.MessageID, message.AppName, nameof(FinacleIntegrationAPIClient), "Generate Json", nameof(SendAsync), Message);
                Log.Trace(message.SessionID, message.MessageID, message.AppName, nameof(FinacleIntegrationAPIClient), "Generate HTTPMessage content", nameof(SendAsync), "Generating string content");
                //Encoding unicode = Encoding.UTF8;
                //StringContent content = new StringContent(stringWithHidden, unicode, "application/json");
                //content.Headers.Add("SessionID", message.SessionID);
                //content.Headers.Add("MessageID", message.MessageID);
                //content.Headers.Add("AppName", message.AppName);

                Log.Debug(message.SessionID, message.MessageID, message.AppName, nameof(FinacleIntegrationAPIClient), "API TX", nameof(SendAsync), "Sending to {0} >> {1}", apiBaseAddress + endpoint, Message);
                HttpResponseMessage response = await httpClient.GetAsync(apiBaseAddress + endpoint);
                Log.Debug(message.SessionID, message.MessageID, message.AppName, nameof(FinacleIntegrationAPIClient), "API Rx", nameof(SendAsync), "Received http response {0}", response.ToString());
                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    Log.Debug(message.SessionID, message.MessageID, message.AppName, nameof(FinacleIntegrationAPIClient), "API Rx", nameof(SendAsync), "Received http response {0}", str);
                    T responseObject = JsonConvert.DeserializeObject<T>(str);
                    Guid? nullable = responseObject is APIMessageBase apiMessageBase ? new Guid?(apiMessageBase.AppID) : new Guid?();
                    Guid appId = message.AppID;
                    if ((nullable.HasValue ? (nullable.HasValue ? (nullable.GetValueOrDefault() != appId ? 1 : 0) : 0) : 1) != 0)
                        throw new Exception("Invalid response app id");
                    //await ValidateResponse(response, JsonConvert.SerializeObject(responseObject, Formatting.Indented, new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Ignore}));
                    return responseObject;
                }
                else
                {
                    Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, response.ReasonPhrase);
                    throw new Exception(string.Format("API Call failed with status code [{0}]: {1}", response.StatusCode, response.ReasonPhrase));
                }
            }
            catch (Exception ex)
            {
                Log.Error(message.SessionID, message.MessageID, message.AppName, nameof(FinacleIntegrationAPIClient), nameof(SendAsync), ex.GetType().Name, ex.MessageString());
                throw;
            }
        }
        public async Task<T> SendAsync<T>(string endpoint, FundsTransferRequestDto request)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                HttpClient httpClient = new HttpClient(clientHandler);
                string stringWithHidden = JsonConvert.SerializeObject(request, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                Encoding unicode = Encoding.UTF8;
                StringContent content = new StringContent(stringWithHidden, unicode, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(apiBaseAddress + endpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    string str = await response.Content.ReadAsStringAsync();
                    Log.Info(nameof(FinacleIntegrationAPIClient), nameof(SendAsync), response.GetType().Name,str);
                    T responseObject = JsonConvert.DeserializeObject<T>(str);
                    Type type = responseObject.GetType();
                    FundsTransferResponseDto _responseObject = null;
                    if (type.Name == "FundsTransferResponseDto")
                    {
                        _responseObject = responseObject as FundsTransferResponseDto;
                        _responseObject.IsSuccess = _responseObject.result.Envelope.Header.ResponseHeader.StatusCode == "S_001" ? true : false;
                        _responseObject.PublicErrorMessage = _responseObject.result.Envelope.Header.ResponseHeader.StatusDescription;
                        _responseObject.PublicErrorCode = _responseObject.result.Envelope.Header.ResponseHeader.StatusCode;
                        _responseObject.TransactionID = _responseObject.result.Envelope.Body.FundsTransfer.TransactionID;
                        _responseObject.TransactionDateTime = _responseObject.result.Envelope.Body.FundsTransfer.TransactionDatetime;
                        _responseObject.MessageDateTime = _responseObject.result.Envelope.Body.FundsTransfer.TransactionDatetime;
                        responseObject = (T)Convert.ChangeType(_responseObject, typeof(T));
                    }
                    return responseObject;
                }
                else
                {
                    string str = await response.Content.ReadAsStringAsync();
                    Log.Info(nameof(FinacleIntegrationAPIClient), nameof(SendAsync), response.GetType().Name,str);
                    T responseObject = JsonConvert.DeserializeObject<T>(str);
                    Type type = responseObject.GetType();
                    FundsTransferResponseDto _responseObject = null;
                    if (type.Name == "FundsTransferResponseDto")
                    {
                        _responseObject = responseObject as FundsTransferResponseDto;
                        _responseObject.IsSuccess = _responseObject.result.Envelope.Header.ResponseHeader.StatusCode == "S_001" ? true : false;
                        _responseObject.PublicErrorMessage = _responseObject.result.Envelope.Header.ResponseHeader.StatusDescription;
                        _responseObject.PublicErrorCode = _responseObject.result.Envelope.Header.ResponseHeader.StatusCode;
                        responseObject = (T)Convert.ChangeType(_responseObject, typeof(T));
                    }
                    Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, _responseObject.PublicErrorMessage);
                    return responseObject;
                }
            }
            catch (Exception ex)
            {
                Log.Error(nameof(FinacleIntegrationAPIClient), nameof(SendAsync), ex.GetType().Name, ex.MessageString());
                throw;
            }
        }

    }
}
