using CashSwift.API.Messaging.APIClients;
using CashSwift.API.Messaging.Integration.ServerPing;
//using CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations;
using CashSwift.API.Messaging.Models;
using CashSwift.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Integration.Controllers
{
    public class FinacleIntegrationServiceClient : FinacleIntegrationAPIClient, IMonitoringController, IFinacleIntegrationController
    {
        public FinacleIntegrationServiceClient(
            string apiBaseAddress,
            IConfiguration configuration)
            : base(new CashSwiftAPILogger(nameof(FinacleIntegrationServiceClient), configuration), apiBaseAddress, configuration)
        {
        }

        public async Task<Validations.AccountNumberValidations.AccountNumberValidationResponse> ValidateAccountNumberAsync(AccountNumberValidationRequestDto request)
        {
            return await SendAsync<Validations.AccountNumberValidations.AccountNumberValidationResponse>($"api/Banking/AccountValidate", request);
        }


        public async Task<IntegrationServerPingResponse> ServerPingAsync(IntegrationServerPingRequest request)
        {
            return await SendAsync<IntegrationServerPingResponse>("healthchecks-api", request);
        }

        public async Task<FundsTransferResponseDto> FundsTransferAsync(
            FundsTransferRequestDto request)
        {
            return await SendAsync<FundsTransferResponseDto>("api/Banking/FundsTransfer", request);
        }
    }
}