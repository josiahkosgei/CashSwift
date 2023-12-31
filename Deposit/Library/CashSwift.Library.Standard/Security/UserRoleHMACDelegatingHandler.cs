﻿// Security.UserRoleHMACDelegatingHandler


using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CashSwift.Library.Standard.Security
{
    public class UserRoleHMACDelegatingHandler : DelegatingHandler
    {
        private Guid APPId;
        private byte[] APIKey;

        public UserRoleHMACDelegatingHandler(Guid aPPId, byte[] apiKey)
        {
            APPId = aPPId;
            APIKey = apiKey;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
          HttpRequestMessage request,
          CancellationToken cancellationToken)
        {
            string requestUri = request.RequestUri.AbsoluteUri;
            string requestHttpMethod = request.Method.Method;
            string content = await request.Content.ReadAsStringAsync();
            request.Headers.Add("hmacAuth", APIHashing.GetAuthHeader(APPId, requestUri, requestHttpMethod, APIKey, content));
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
