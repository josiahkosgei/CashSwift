// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_5.ResponseHeaderStatusMessages
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_5
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
    public class ResponseHeaderStatusMessages
    {
        private string messageCodeField;

        private string messageTypeField;

        public string ApplicationID { get; set; }

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

        public string MessageDescription { get; set; }

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