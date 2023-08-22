using CashSwift.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using MediatR;

namespace CashSwift.Finacle.Integration.CQRS.Commands.ValidateAccount
{
    public class ValidateAccountCommand : IRequest<ValidateAccountResponseDto>
    {
        public ValidateAccountRequestDto ValidateAccountRequest { get; set; }
    }
}
