// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v4_0.EnvelopeBody

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v4_0
{
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionChargesCharge
    {

        private object eventTypeField;

        private object eventIdField;

        /// <remarks/>
        public object EventType
        {
            get
            {
                return eventTypeField;
            }
            set
            {
                eventTypeField = value;
            }
        }

        /// <remarks/>
        public object EventId
        {
            get
            {
                return eventIdField;
            }
            set
            {
                eventIdField = value;
            }
        }
    }

}
