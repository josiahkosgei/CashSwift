// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferAtmDetailsProcessingCode
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferAtmDetailsProcessingCode
    {
        private string txnTypeField;

        private string acctType1Field;

        private string acctType2Field;

        public string txnType
        {
            get
            {
                return txnTypeField;
            }
            set
            {
                txnTypeField = value;
            }
        }

        public string acctType1
        {
            get
            {
                return acctType1Field;
            }
            set
            {
                acctType1Field = value;
            }
        }

        public string acctType2
        {
            get
            {
                return acctType2Field;
            }
            set
            {
                acctType2Field = value;
            }
        }
    }

}