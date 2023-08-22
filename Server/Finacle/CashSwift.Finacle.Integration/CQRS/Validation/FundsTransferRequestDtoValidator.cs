using CashSwift.Finacle.Integration.Models.FundsTransfer;
using FluentValidation;

namespace CashSwift.Finacle.Integration.CQRS.Validation
{
    public class FundsTransferRequestDtoValidator : AbstractValidator<FundsTransferRequest>
    {
        public FundsTransferRequestDtoValidator()
        {
            RuleFor(x => x.AccountNumber_Cr).NotNull().WithMessage("Account Number is required");
            RuleFor(x => x.AccountNumber_Cr).Matches(@"^\d{14}$").WithMessage("Account Number is Invalid");

            RuleFor(x => x.AccountNumber_Dr).NotNull().WithMessage("Account Number is required");
            RuleFor(x => x.AccountNumber_Dr).Matches(@"^\d{14}$").WithMessage("Account Number is Invalid");

            RuleFor(x => x.Narrative_Cr).NotNull().WithMessage("Narrative is required");
            RuleFor(x => x.Narrative_Cr).MaximumLength(16).WithMessage("Narrative chararecters length of 16 chararcters exceeded ");

            RuleFor(x => x.Narrative_Dr).NotNull().WithMessage("Narrative is required");
            RuleFor(x => x.Narrative_Dr).MaximumLength(16).WithMessage("Narrative chararecters length of 16 chararcters exceeded ");
        }
    }
}
