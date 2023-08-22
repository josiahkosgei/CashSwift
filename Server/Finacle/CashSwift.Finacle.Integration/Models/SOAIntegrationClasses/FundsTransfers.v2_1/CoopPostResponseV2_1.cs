// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.CoopPostResponseV2_1
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public class CoopPostResponseV2_1 : CoopFundsTransferResponseBase
    {
        private EnvelopeHeader headerField;

        private EnvelopeBody bodyField;

        public new string MessageID => Header?.HeaderReply?.MessageID;

        public new string Currency => null;

        public new string CBReference => Body?.DataOutput?.postOutput?.FundsTransfer?.txnInputData?.tellerTxnReference;

        public new string CBMessageType => Body?.DataOutput?.postOutput?.FundsTransfer?.messageType;

        public new DateTime CBTransactionDate => Body?.DataOutput?.postOutput?.FundsTransfer?.atmDetails?.txnDateTime ?? DateTime.MinValue;

        public new string RequestUUID => Header?.HeaderReply?.MessageID;

        public new string StatusCode => Header?.HeaderReply?.StatusCode;

        public new string StatusMessage => Header?.HeaderReply?.StatusDescription;

        public new bool Success => StatusMessage == "Success";

        public new string ValidationStatus => Header?.HeaderReply?.StatusMessages?.StatusMessage?.MessageCode;

        public new string ValidationMessage => Header?.HeaderReply?.StatusMessages?.StatusMessage?.MessageDescription;

        public EnvelopeHeader Header
        {
            get
            {
                return headerField;
            }
            set
            {
                headerField = value;
            }
        }

        public EnvelopeBody Body
        {
            get
            {
                return bodyField;
            }
            set
            {
                bodyField = value;
            }
        }
    }
}
