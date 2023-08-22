namespace CashSwift.Finacle.Integration.CQRS.Helpers
{
    public class PostConfiguration
    {
        public string ServerURI { get; set; }
        public double SOAVersion { get; set; }
        public string ChannelID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DateFormat { get; set; }
        public string SystemCode { get; set; }
        public string BankID { get; set; }
        public string UserID { get; set; }
        public string TransactionCode { get; set; }
        public string Realm { get; set; }
        public string TransactionType { get; set; }
        public string TransactionSubType { get; set; }
    }
}
