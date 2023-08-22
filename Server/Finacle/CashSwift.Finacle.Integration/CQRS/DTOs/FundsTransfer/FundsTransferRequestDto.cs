using CashSwift.Finacle.Integration.CQRS.DTOs.ValidateAccount;

namespace CashSwift.Finacle.Integration.CQRS.DTOs.FundsTransfer
{
    public class FundsTransferRequestDto
    {
        public RequestHeaderTypeDto RequestHeaderType { get; set; }
        public FundsTransferTypeDto FundsTransferType { get; set; }
    }
}
