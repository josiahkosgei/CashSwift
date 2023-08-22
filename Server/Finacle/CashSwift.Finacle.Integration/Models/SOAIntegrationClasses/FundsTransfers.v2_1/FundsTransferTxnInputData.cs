// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferTxnInputData
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferTxnInputData
    {
        private string tellerTxnReferenceField;

        private string tellerTxnTypeField;

        private string hostTxnCodeField;

        private string txnModeField;

        private bool compensateField;

        private bool postChargeSepField;

        private string originalTxnRefField;

        private string originalHostTxnRefField;

        private string userExtensionField;

        private string hostExtensionField;

        public string tellerTxnReference
        {
            get
            {
                return tellerTxnReferenceField;
            }
            set
            {
                tellerTxnReferenceField = value;
            }
        }

        public string tellerTxnType
        {
            get
            {
                return tellerTxnTypeField;
            }
            set
            {
                tellerTxnTypeField = value;
            }
        }

        public string hostTxnCode
        {
            get
            {
                return hostTxnCodeField;
            }
            set
            {
                hostTxnCodeField = value;
            }
        }

        public string txnMode
        {
            get
            {
                return txnModeField;
            }
            set
            {
                txnModeField = value;
            }
        }

        public bool compensate
        {
            get
            {
                return compensateField;
            }
            set
            {
                compensateField = value;
            }
        }

        public bool postChargeSep
        {
            get
            {
                return postChargeSepField;
            }
            set
            {
                postChargeSepField = value;
            }
        }

        public string originalTxnRef
        {
            get
            {
                return originalTxnRefField;
            }
            set
            {
                originalTxnRefField = value;
            }
        }

        public string originalHostTxnRef
        {
            get
            {
                return originalHostTxnRefField;
            }
            set
            {
                originalHostTxnRefField = value;
            }
        }

        public string userExtension
        {
            get
            {
                return userExtensionField;
            }
            set
            {
                userExtensionField = value;
            }
        }

        public string hostExtension
        {
            get
            {
                return hostExtensionField;
            }
            set
            {
                hostExtensionField = value;
            }
        }
    }
}