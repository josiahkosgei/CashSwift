using CashSwift.API.Messaging.APIClients;
using CashSwift.API.Messaging.Integration.ServerPing;
using CashSwift.API.Messaging.Integration.Transactions;
using CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations;
using CashSwift.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations;
using CashSwift.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Integration
{
    public class IntegrationServiceClient :
      APIClient,
      IMonitoringController,
      ITransactionController,
      IAccountsController
    {
        public IntegrationServiceClient(
          string apiBaseAddress,
          Guid AppID,
          byte[] appKey,
          IConfiguration configuration)
          : base(new CashSwiftAPILogger(nameof(IntegrationServiceClient), configuration), apiBaseAddress, AppID, appKey, configuration)
        {
        }

        public async Task<AccountNumberValidationResponse> ValidateAccountNumberAsync(
          AccountNumberValidationRequest request)
        {
            return await SendAsync<AccountNumberValidationResponse>("api/v2.0/Banking/AccountValidate", request);
        }

        public async Task<ReferenceAccountNumberValidationResponse> ValidateReferenceAccountNumberAsync(
          ReferenceAccountNumberValidationRequest request)
        {
            return await SendAsync<ReferenceAccountNumberValidationResponse>("api/v2.0/Banking/ReferenceAccountNumberValidate", request);
        }

        public async Task<IntegrationServerPingResponse> ServerPingAsync(
          IntegrationServerPingRequest request)
        {
            return await SendAsync<IntegrationServerPingResponse>("api/v2.0/Monitoring/ServerPing", request);
        }

        public async Task<PostTransactionResponse> PostTransactionAsync(
          PostTransactionRequest request)
        {
            return await SendAsync<PostTransactionResponse>("api/v2.0/Banking/FundsTransfer", request);
        }

        public async Task<PostCITTransactionResponse> PostCITTransactionAsync(
          PostCITTransactionRequest request)
        {
            return await SendAsync<PostCITTransactionResponse>("api/v2.0/Banking/CITFundsTransfer", request);
        }
    }
}
