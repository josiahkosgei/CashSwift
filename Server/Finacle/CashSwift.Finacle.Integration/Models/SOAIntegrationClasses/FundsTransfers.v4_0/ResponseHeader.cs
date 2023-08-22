// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v4_0.EnvelopeHeader

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v4_0
{
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
    [System.Xml.Serialization.XmlRoot(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader", IsNullable = false)]
    public partial class ResponseHeader
    {

        private string correlationIDField;

        private string messageIDField;

        private string statusCodeField;

        private string statusDescriptionField;

        private ResponseHeaderStatusMessages statusMessagesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Common")]
        public string CorrelationID
        {
            get
            {
                return correlationIDField;
            }
            set
            {
                correlationIDField = value;
            }
        }

        /// <remarks/>
        public string MessageID
        {
            get
            {
                return messageIDField;
            }
            set
            {
                messageIDField = value;
            }
        }

        /// <remarks/>
        public string StatusCode
        {
            get
            {
                return statusCodeField;
            }
            set
            {
                statusCodeField = value;
            }
        }

        /// <remarks/>
        public string StatusDescription
        {
            get
            {
                return statusDescriptionField;
            }
            set
            {
                statusDescriptionField = value;
            }
        }

        /// <remarks/>
        public ResponseHeaderStatusMessages StatusMessages
        {
            get
            {
                return statusMessagesField;
            }
            set
            {
                statusMessagesField = value;
            }
        }
    }

}