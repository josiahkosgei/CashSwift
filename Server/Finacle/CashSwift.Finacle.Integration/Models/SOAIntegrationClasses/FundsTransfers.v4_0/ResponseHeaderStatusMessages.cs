// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v4_0.EnvelopeHeader

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v4_0
{
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
    public partial class ResponseHeaderStatusMessages
    {

        private string messageCodeField;

        private string messageDescriptionField;

        private string messageTypeField;

        /// <remarks/>
        public string MessageCode
        {
            get
            {
                return messageCodeField;
            }
            set
            {
                messageCodeField = value;
            }
        }

        /// <remarks/>
        public string MessageDescription
        {
            get
            {
                return messageDescriptionField;
            }
            set
            {
                messageDescriptionField = value;
            }
        }

        /// <remarks/>
        public string MessageType
        {
            get
            {
                return messageTypeField;
            }
            set
            {
                messageTypeField = value;
            }
        }
    }

}