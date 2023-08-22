namespace CashSwift.Finacle.Integration.Models
{
    public interface IIntegrationServerConfiguration
	{
		string AmountFormat { get; set; }

		string DateFormat { get; set; }

		bool IsDebug { get; set; }

		string ServerURI { get; set; }
	}
}
