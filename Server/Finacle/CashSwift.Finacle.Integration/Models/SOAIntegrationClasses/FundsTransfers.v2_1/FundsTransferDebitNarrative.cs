// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferDebitNarrative
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferDebitNarrative
    {
        private string referenceField;

        private string narrativeLine1Field;

        private string narrativeLine2Field;

        private string narrativeLine3Field;

        private string narrativeLine4Field;

        private string userExtensionField;

        private string hostExtensionField;

        public string reference
        {
            get
            {
                return referenceField;
            }
            set
            {
                referenceField = value;
            }
        }

        public string narrativeLine1
        {
            get
            {
                return narrativeLine1Field;
            }
            set
            {
                narrativeLine1Field = value;
            }
        }

        public string narrativeLine2
        {
            get
            {
                return narrativeLine2Field;
            }
            set
            {
                narrativeLine2Field = value;
            }
        }

        public string narrativeLine3
        {
            get
            {
                return narrativeLine3Field;
            }
            set
            {
                narrativeLine3Field = value;
            }
        }

        public string narrativeLine4
        {
            get
            {
                return narrativeLine4Field;
            }
            set
            {
                narrativeLine4Field = value;
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