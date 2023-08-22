namespace CashSwift.Finacle.Integration.CQRS.DTOs.FundsTransfer
{
    public class FundsTransferTypeDto
    {
        public string MessageReference { get; set; }
        public string SystemCode { get; set; }
        public string TransactionDatetime { get; set; }
        public DateTime ValueDate { get; set; }
        public bool ValueDateSpecified { get; set; }
        public string TransactionID { get; set; }
        public string TransactionType { get; set; }
        public string TransactionSubType { get; set; }
        public FundsTransferTypeTransactionItemDto[] TransactionItem { get; set; }
        public FundsTransferTypeChargeDto[] TransactionCharges { get; set; }
    }
}