namespace CashSwift.API.Messaging.Communication.SMSes
{
    public interface ISMSConfiguration
    {
        string SMSHost { get; set; }

        bool UseSSL { get; set; }
    }
}
