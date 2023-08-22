namespace CashSwift.API.Messaging.Models
{
    public class AccountNumberValidationRequest
    {

        public string Language { get; set; } = "en-gb";
        public string AccountNumber { get; set; }

        public string Currency { get; set; }

        public int TransactionType { get; set; }
    }
}
