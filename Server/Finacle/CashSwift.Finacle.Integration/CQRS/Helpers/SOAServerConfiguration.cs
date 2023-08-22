using CashSwift.Finacle.Integration.Models;

namespace CashSwift.Finacle.Integration.CQRS.Helpers
{

    public class SOAServerConfiguration:IIntegrationServerConfiguration
    {
        public string ServerURI { get; set; }
        public string AmountFormat { get; set; }
        public string DateFormat { get; set; }
        public bool IsDebug { get; set; }
        public string ContentType { get; set; }
        public virtual PostConfiguration PostConfiguration { get; set; }
        public virtual AccountValidationConfiguration AccountValidationConfiguration { get; set; }
        public virtual MonitoringConfiguration MonitoringConfiguration { get; set; }
    }
}
