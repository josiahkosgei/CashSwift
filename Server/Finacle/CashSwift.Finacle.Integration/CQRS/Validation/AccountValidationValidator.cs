using CashSwift.Finacle.Integration.CQRS.DTOs.ValidateAccount;
using FluentValidation;

namespace CashSwift.Finacle.Integration.CQRS.Validation
{

    public class AccountValidationValidator : AbstractValidator<AccountDetailsRequestTypeDto>
    {
        public AccountValidationValidator()
        {
            RuleFor(x => x.AccountNumber).NotNull().WithMessage("Account Number is required");
            // RuleFor(x => x.AccountNumber).Length(14).WithMessage("Account Number is Invalid");
            // RuleFor(x => x.AccountNumber).Matches(@"^\d{14}$").WithMessage("Account Number is Invalid");
            RuleFor(x => x.AccountNumber).Matches(@"^\d{14}$").WithMessage("Account Number is Invalid");
        }
    }
}
