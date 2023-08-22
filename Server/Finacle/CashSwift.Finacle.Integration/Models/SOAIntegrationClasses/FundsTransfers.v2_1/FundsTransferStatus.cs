// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferStatus
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferStatus
    {
        private string overallStatusField;

        public string overallStatus
        {
            get
            {
                return overallStatusField;
            }
            set
            {
                overallStatusField = value;
            }
        }
    }
}