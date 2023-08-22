// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferAtmDetailsOriginalDataEle
using System.ComponentModel;
using System.Xml.Serialization;


namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferAtmDetailsOriginalDataEle
    {
        private string orgMsgIdField;

        private string orgAuditNoField;

        private DateTime orgDateTimeField;

        private string orgAcIDCField;

        public string orgMsgId
        {
            get
            {
                return orgMsgIdField;
            }
            set
            {
                orgMsgIdField = value;
            }
        }

        public string orgAuditNo
        {
            get
            {
                return orgAuditNoField;
            }
            set
            {
                orgAuditNoField = value;
            }
        }

        public DateTime orgDateTime
        {
            get
            {
                return orgDateTimeField;
            }
            set
            {
                orgDateTimeField = value;
            }
        }

        public string orgAcIDC
        {
            get
            {
                return orgAcIDCField;
            }
            set
            {
                orgAcIDCField = value;
            }
        }
    }
}