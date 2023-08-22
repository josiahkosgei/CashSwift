using System;

namespace CashSwift.API.Messaging.Models
{
    public class FundsTransfer
    {
        public string MessageReference { get; set; }
        public string SystemCode { get; set; }
        public DateTime TransactionDatetime { get; set; }
        public DateTime ValueDate { get; set; }
        public string TransactionID { get; set; }
        public string TransactionType { get; set; }
        public string TransactionSubType { get; set; }
        public TransactionResponseDetails TransactionResponseDetails { get; set; }
        public TransactionItems TransactionItems { get; set; }
        public TransactionCharges TransactionCharges { get; set; }
        public TransactionItem TransactionItem { get; set; }
    }


}
