using CashSwift.API.Messaging.Integration.Validations.AccountNumberValidations;
using CashSwift.Finacle.Integration.CQRS.Helpers;
using CashSwift.Finacle.Integration.Models.SOAIntegrationClasses;
using System.Globalization;
namespace CashSwift.Finacle.Integration.Models.AccountValidation
{
    public class
        CoopAccountDetailsRequest : CoopMessageBase
    {
        private AccountValidationConfiguration AccountValidationConfiguration;

        public new string RawXML { get; }

        private string RequestUUID { get; }
        private string CorrelationID { get; }

        private DateTime MessageDateTime { get; }

        private string MessageDateTimeString => MessageDateTime.ToString(AccountValidationConfiguration.DateFormat, CultureInfo.InvariantCulture);

        private string AccountNumber { get; }

        public CoopAccountDetailsRequest(
          AccountNumberValidationRequest request,
          AccountValidationConfiguration accountValidationConfiguration)
        {
            AccountValidationConfiguration = accountValidationConfiguration;
            RequestUUID = request.MessageID;
            MessageDateTime = request.MessageDateTime;
            AccountNumber = request.AccountNumber;
            CorrelationID = Guid.NewGuid().ToString().ToLower();
            RawXML= $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:mes=\"urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader\" xmlns:com=\"urn://co-opbank.co.ke/CommonServices/Data/Common\" xmlns:bsac=\"urn://co-opbank.co.ke/BS/Account/BSAccountDetails.3.0\">\r\n   <soapenv:Header>\r\n      <mes:RequestHeader>\r\n         <com:CreationTimestamp>{MessageDateTime.ToString(AccountValidationConfiguration.DateFormat, CultureInfo.InvariantCulture)}</com:CreationTimestamp>\r\n         <com:CorrelationID>{CorrelationID}</com:CorrelationID>\r\n         <mes:FaultTO/>\r\n         <mes:MessageID>{RequestUUID}</mes:MessageID>\r\n         <mes:ReplyTO/>\r\n         <mes:Credentials>\r\n            <mes:SystemCode>{accountValidationConfiguration.SystemCode}</mes:SystemCode>\r\n            <mes:BankID>{accountValidationConfiguration.BankID}</mes:BankID>\r\n         </mes:Credentials>\r\n      </mes:RequestHeader>\r\n   </soapenv:Header>\r\n   <soapenv:Body>\r\n      <bsac:AccountDetailsRequest>\r\n         <bsac:AccountNumber>{AccountNumber}</bsac:AccountNumber>\r\n      </bsac:AccountDetailsRequest>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>";
        }
    }
}
