using CashSwift.API.Messaging.APIClients;
using CashSwift.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Authentication.Clients
{
    public class AuthenticationServiceClient : APIClient, IAuthenticationService
    {
        public AuthenticationServiceClient(
          string apiBaseAddress,
          Guid AppID,
          byte[] appKey,
          IConfiguration configuration)
          : base(new CashSwiftAPILogger(nameof(AuthenticationServiceClient), configuration), apiBaseAddress, AppID, appKey, configuration)
        {
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(
          AuthenticationRequest request)
        {
            return await SendAsync<AuthenticationResponse>("api/User/Authenticate", request);
        }

        public async Task<ChangePasswordResponse> ChangePasswordAsync(
          ChangePasswordRequest request)
        {
            return await SendAsync<ChangePasswordResponse>("api/User/ChangePassword", request);
        }
    }
}
