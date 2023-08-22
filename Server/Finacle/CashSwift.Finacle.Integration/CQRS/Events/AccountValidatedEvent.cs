using CashSwift.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using MediatR;

namespace CashSwift.Finacle.Integration.CQRS.Events
{
    public class AccountValidated : INotification
    {
        public ValidateAccountResponseDto _validateAccountResponseDto { get; }

        public AccountValidated(ValidateAccountResponseDto validateAccountResponseDto)
        {
            _validateAccountResponseDto = validateAccountResponseDto;
        }
    }
}
