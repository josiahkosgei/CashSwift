using System;

namespace CashSwift.API.Messaging.Models
{
    public class FundsTransferResponseDto : PostTransactionResponseBase
    {
        public string message { get; set; }
        public string PublicErrorMessage { get; set; }
        public string PublicErrorCode { get; set; }
        public Result result { get; set; }
        public bool IsSuccess { get; set; }
        public string PostResponseCode { get; set; }
        public string PostResponseMessage { get; set; }
        public DateTime MessageDateTime { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string TransactionID { get; set; }
        public string RequestID { get; set; }
        public string MessageID { get; set; }
        public string ServerErrorCode { get; set; }
        public string ServerErrorMessage { get; set; }
    }


}
