// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.HeaderReplyStatusMessages
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/SharedResources/Schemas/SOAMessages/SoapHeader")]
    public class HeaderReplyStatusMessages
    {
        private HeaderReplyStatusMessagesStatusMessage statusMessageField;

        public HeaderReplyStatusMessagesStatusMessage StatusMessage
        {
            get
            {
                return statusMessageField;
            }
            set
            {
                statusMessageField = value;
            }
        }
    }
}