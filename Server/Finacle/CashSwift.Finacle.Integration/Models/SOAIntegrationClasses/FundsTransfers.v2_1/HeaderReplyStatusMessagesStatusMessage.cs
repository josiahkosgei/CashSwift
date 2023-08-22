// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.HeaderReplyStatusMessagesStatusMessage
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/SharedResources/Schemas/SOAMessages/SoapHeader")]
    public class HeaderReplyStatusMessagesStatusMessage
    {
        private string messageTypeField;

        private string applicationIDField;

        private string messageCodeField;

        private string messageDescriptionField;

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

        public string ApplicationID
        {
            get
            {
                return applicationIDField;
            }
            set
            {
                applicationIDField = value;
            }
        }

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
    }
}