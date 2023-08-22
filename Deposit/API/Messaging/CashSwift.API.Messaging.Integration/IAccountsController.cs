using CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations;
using CashSwift.API.Messaging.Integration.Validations.ReferenceAccountNumberValidations;
using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Integration
{
    public interface IAccountsController
    {
        Task<AccountNumberValidationResponse> ValidateAccountNumberAsync(
          AccountNumberValidationRequest request);

        Task<ReferenceAccountNumberValidationResponse> ValidateReferenceAccountNumberAsync(
          ReferenceAccountNumberValidationRequest request);
    }
}
