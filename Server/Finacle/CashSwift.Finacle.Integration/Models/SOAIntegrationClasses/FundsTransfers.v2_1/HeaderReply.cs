// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.HeaderReply
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/SharedResources/Schemas/SOAMessages/SoapHeader")]
    [XmlRoot(Namespace = "urn://co-opbank.co.ke/SharedResources/Schemas/SOAMessages/SoapHeader", IsNullable = false)]
    public class HeaderReply
    {
        private string messageIDField;

        private string correlationIDField;

        private string statusCodeField;

        private string statusDescriptionField;

        private string statusDescriptionKeyField;

        private HeaderReplyStatusMessages statusMessagesField;

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

        public string StatusDescriptionKey
        {
            get
            {
                return statusDescriptionKeyField;
            }
            set
            {
                statusDescriptionKeyField = value;
            }
        }

        public HeaderReplyStatusMessages StatusMessages
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