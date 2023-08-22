// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_5.ResponseHeader
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_5
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
    [XmlRoot(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader", IsNullable = false)]
    public class ResponseHeader
    {
        private string messageIDField;

        private string statusDescriptionField;

        private ResponseHeaderStatusMessages statusMessagesField;

        [XmlElement(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Common")]
        public string CorrelationID { get; set; }

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

        public string StatusCode { get; set; }

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

        public string StatusDescriptionKey { get; set; }

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