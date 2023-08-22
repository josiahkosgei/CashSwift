// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_5.CoopPostResponseV2_5
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_5
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public class CoopPostResponseV2_5 : CoopFundsTransferResponseBase
    {
        public new string MessageID => Header?.ResponseHeader?.MessageID;

        public new string Currency => null;

        public new string CBReference => Body?.FundsTransferResponse?.FundsTransferRespData?.OperationParameters?.TransactionID;

        public new string CBMessageType => Header?.ResponseHeader?.StatusMessages?.MessageType;

        public new DateTime CBTransactionDate => Body?.FundsTransferResponse?.FundsTransferRespData?.OperationParameters?.ValueDate ?? DateTime.MinValue;

        public new string RequestUUID => Header?.ResponseHeader?.MessageID;

        public new string StatusCode => Header?.ResponseHeader?.StatusCode;

        public new string StatusMessage => Header?.ResponseHeader?.StatusDescription;

        public new bool Success => StatusMessage == "Success";

        public new string ValidationStatus => Header?.ResponseHeader?.StatusMessages?.MessageCode;

        public new string ValidationMessage => Header?.ResponseHeader?.StatusMessages?.MessageDescription;

        public EnvelopeHeader Header { get; set; }

        public EnvelopeBody Body { get; set; }
    }
}