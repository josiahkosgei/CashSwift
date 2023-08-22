using System;

namespace CashSwift.API.Messaging.Models
{
    public class TransactionItem
    {
        public string TransactionReference { get; set; }
        public string TransactionItemKey { get; set; }
        public string AccountNumber { get; set; }
        public string DebitCreditFlag { get; set; }
        public string TransactionAmount { get; set; }
        public string TransactionCurrency { get; set; }
        public string Narrative { get; set; }
        public string TransactionCode { get; set; }
        public string AvailableBalance { get; set; }
        public string BookedBalance { get; set; }
        public object TemporaryODDetails { get; set; }
        public DateTime ValueDate { get; set; }
    }


}
