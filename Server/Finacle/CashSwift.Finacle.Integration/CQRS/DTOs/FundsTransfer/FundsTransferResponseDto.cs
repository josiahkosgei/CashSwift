using CashSwift.Finacle.Integration.CQRS.DTOs.ValidateAccount;

namespace CashSwift.Finacle.Integration.CQRS.DTOs.FundsTransfer
{
    public class FundsTransferResponseDto
    {
        public ResponseHeaderTypeDto ResponseHeaderType { get; set; }
        public FundsTransferTypeDto FundsTransferType { get; set; }
    }
}
