namespace CashSwift.Finacle.Integration.CQRS.Helpers
{
    public class MonitoringConfiguration
    {
        public int PingInterval { get; set; }
        public string PingAccountNumber { get; set; }
        public string Currency { get; set; }
        public string CoreBankingString { get; set; }
    }
}
