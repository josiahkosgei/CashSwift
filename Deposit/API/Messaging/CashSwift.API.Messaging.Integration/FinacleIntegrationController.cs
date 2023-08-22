//using CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations;
using CashSwift.API.Messaging.Models;
using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Integration.Controllers
{
    public interface IFinacleIntegrationController
    {
        Task<Validations.AccountNumberValidations.AccountNumberValidationResponse> ValidateAccountNumberAsync(AccountNumberValidationRequestDto request);
        Task<FundsTransferResponseDto> FundsTransferAsync(FundsTransferRequestDto request);

    }
}
