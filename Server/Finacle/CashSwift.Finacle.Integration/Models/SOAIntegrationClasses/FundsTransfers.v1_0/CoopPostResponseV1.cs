// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v1_0.CoopPostResponseV1
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v1_0
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public class CoopPostResponseV1 : CoopFundsTransferResponseBase
    {
        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class EnvelopeHeader
        {
            [XmlElement(Namespace = "urn://co-opbank.co.ke/SharedResources/Schemas/SOAMessages/SoapHeader")]
            public HeaderReply HeaderReply { get; set; }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/SharedResources/Schemas/SOAMessages/SoapHeader")]
        [XmlRoot(Namespace = "urn://co-opbank.co.ke/SharedResources/Schemas/SOAMessages/SoapHeader", IsNullable = false)]
        public class HeaderReply
        {
            public string MessageID { get; set; }

            public string CorrelationID { get; set; }

            public string StatusCode { get; set; }

            public string StatusDescription { get; set; }

            public string StatusDescriptionKey { get; set; }

            public HeaderReplyStatusMessages StatusMessages { get; set; }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/SharedResources/Schemas/SOAMessages/SoapHeader")]
        public class HeaderReplyStatusMessages
        {
            public HeaderReplyStatusMessagesStatusMessage StatusMessage { get; set; }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/SharedResources/Schemas/SOAMessages/SoapHeader")]
        public class HeaderReplyStatusMessagesStatusMessage
        {
            public string MessageType { get; set; }

            public string ApplicationID { get; set; }

            public string MessageCode { get; set; }

            public string MessageDescription { get; set; }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class EnvelopeBody
        {
            [XmlElement(Namespace = "urn://co-opbank.co.ke/Banking/Account/Service/AccountFundsTransfer/Post/1.0/DataIO")]
            public DataOutput DataOutput { get; set; }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/Account/Service/AccountFundsTransfer/Post/1.0/DataIO")]
        [XmlRoot(Namespace = "urn://co-opbank.co.ke/Banking/Account/Service/AccountFundsTransfer/Post/1.0/DataIO", IsNullable = false)]
        public class DataOutput
        {
            [XmlElement(Namespace = "urn://co-opbank.co.ke/Banking/Account/DataModel/FundsTransfer/Post/1.0/FundsTransfer.post")]
            public postOutput postOutput { get; set; }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/Account/DataModel/FundsTransfer/Post/1.0/FundsTransfer.post")]
        [XmlRoot(Namespace = "urn://co-opbank.co.ke/Banking/Account/DataModel/FundsTransfer/Post/1.0/FundsTransfer.post", IsNullable = false)]
        public class postOutput
        {
            [XmlElement(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.0")]
            public FundsTransfer FundsTransfer { get; set; }
        }

        [Serializable]
        [DesignerCategory("code")]
        [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.0")]
        [XmlRoot(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.0", IsNullable = false)]
        public class FundsTransfer
        {
            public string ReferenceNumber { get; set; }

            public string MessageType { get; set; }

            public DateTime TransactionDate { get; set; }
        }

        [NonSerialized]
        private static XmlSerializer _serializer = new XmlSerializer(typeof(CoopPostResponseV1));

        public new string MessageID => Header?.HeaderReply?.MessageID;

        public new string Currency => null;

        public new string CBReference => Body?.DataOutput?.postOutput?.FundsTransfer?.ReferenceNumber;

        public new string CBMessageType => Body?.DataOutput?.postOutput?.FundsTransfer?.MessageType;

        public new DateTime CBTransactionDate => Body?.DataOutput?.postOutput?.FundsTransfer?.TransactionDate ?? DateTime.MinValue;

        public new string RequestUUID => Header?.HeaderReply?.MessageID;

        public new string StatusCode => Header?.HeaderReply?.StatusCode;

        public new string StatusMessage => Header?.HeaderReply?.StatusDescription;

        public new bool Success => StatusMessage == "Success";

        public new string ValidationStatus => Header?.HeaderReply?.StatusMessages?.StatusMessage?.MessageCode;

        public new string ValidationMessage => Header?.HeaderReply?.StatusMessages?.StatusMessage?.MessageDescription;

        public EnvelopeHeader Header { get; set; }

        public EnvelopeBody Body { get; set; }

        internal new static XmlSerializer Serializer => _serializer;
    }
}