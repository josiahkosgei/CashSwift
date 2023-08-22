// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v1_0.CoopPostRequestV1
using System.Globalization;
using CashSwift.API.Messaging.Integration.Transactions;
using CashSwift.Finacle.Integration.CQRS.Helpers;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v1_0
{
    public class CoopPostRequestV1 : CoopFundsTransferRequestBase
    {
        private PostConfiguration PostConfiguration;

        public CoopPostRequestV1(PostTransactionRequest request, string tx_narration, PostConfiguration postConfiguration)
        {
            PostConfiguration = postConfiguration;
            RequestUUID = request.MessageID;
            MessageDateTime = request.MessageDateTime;
            RawXML = $"<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:soap=\"urn://co-opbank.co.ke/SharedResources/Schemas/SOAMessages/SoapHeader\" xmlns:dat=\"urn://co-opbank.co.ke/Banking/Account/Service/AccountFundsTransfer/Post/1.0/DataIO\" xmlns:fun=\"urn://co-opbank.co.ke/Banking/Account/DataModel/FundsTransfer/Post/1.0/FundsTransfer.post\" xmlns:ns=\"urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.0\">\r\n   <soapenv:Header>\r\n      <soap:HeaderRequest>\r\n         <soap:MessageID>{RequestUUID}</soap:MessageID>\r\n         <soap:Credentials>\r\n            <soap:SystemCode>010</soap:SystemCode>\r\n         </soap:Credentials>\r\n      </soap:HeaderRequest>\r\n   </soapenv:Header>\r\n   <soapenv:Body>\r\n      <dat:DataInput>\r\n         <fun:postInput>\r\n            <fun:FundsTransfer>\r\n               <ns:FundsTransfer>\r\n                  <ns:MessageType>FUNDTRFTXN</ns:MessageType>\r\n                  <!--Optional:-->\r\n                  <ns:TransactionDate>{request.Transaction.DateTime.ToString(PostConfiguration.DateFormat, CultureInfo.InvariantCulture)}</ns:TransactionDate>\r\n                  <!--Optional:-->\r\n                  <ns:Narrative1>{tx_narration}</ns:Narrative1>\r\n                  <!--Optional:-->\r\n                  <ns:Narrative2>{request.DepositorName}</ns:Narrative2>\r\n                  <!--Optional:-->\r\n                  <ns:Posting>\r\n                        <!--Optional:-->\r\n                        <ns:PayType>Account</ns:PayType>\r\n                        <!--Optional:-->\r\n                        <ns:TransactionReference>{request.DeviceReferenceNumber}</ns:TransactionReference>\r\n                        <!--Optional:-->\r\n                        <ns:DebitAccountID>{request.Transaction.DebitAccount.AccountNumber}</ns:DebitAccountID>\r\n                        <!--Optional:-->\r\n                        <ns:TransactionAmount>{request.Transaction.Amount:0.00}</ns:TransactionAmount>\r\n                        <!--Optional:-->\r\n                        <ns:TransactionCurrency>{request.Transaction.DebitAccount.Currency.ToUpperInvariant()}</ns:TransactionCurrency>\r\n                        <!--Optional:-->\r\n                        <ns:ExchangeRate>1</ns:ExchangeRate>\r\n                        <!--Optional:-->\r\n                        <ns:CreditAccountID>{request.Transaction.CreditAccount.AccountNumber}</ns:CreditAccountID>\r\n                  </ns:Posting>\r\n               </ns:FundsTransfer>\r\n            </fun:FundsTransfer>\r\n         </fun:postInput>\r\n      </dat:DataInput>\r\n   </soapenv:Body>\r\n</soapenv:Envelope>";
        }
    }
}