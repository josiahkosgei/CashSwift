// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v4_0.EnvelopeBody

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v4_0
{
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    [System.Xml.Serialization.XmlRoot(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0", IsNullable = false)]
    public partial class FundsTransfer
    {

        private string messageReferenceField;

        private byte systemCodeField;

        private DateTime transactionDatetimeField;

        private DateTime valueDateField;

        private string transactionIDField;

        private string transactionTypeField;

        private string transactionSubTypeField;

        private FundsTransferTransactionResponseDetails transactionResponseDetailsField;

        private FundsTransferTransactionItem[] transactionItemsField;

        private FundsTransferTransactionCharges transactionChargesField;

        private FundsTransferTransactionItem transactionItemField;

        /// <remarks/>
        public string MessageReference
        {
            get
            {
                return messageReferenceField;
            }
            set
            {
                messageReferenceField = value;
            }
        }

        /// <remarks/>
        public byte SystemCode
        {
            get
            {
                return systemCodeField;
            }
            set
            {
                systemCodeField = value;
            }
        }

        /// <remarks/>
        public DateTime TransactionDatetime
        {
            get
            {
                return transactionDatetimeField;
            }
            set
            {
                transactionDatetimeField = value;
            }
        }

        /// <remarks/>
        public DateTime ValueDate
        {
            get
            {
                return valueDateField;
            }
            set
            {
                valueDateField = value;
            }
        }

        /// <remarks/>
        public string TransactionID
        {
            get
            {
                return transactionIDField;
            }
            set
            {
                transactionIDField = value;
            }
        }

        /// <remarks/>
        public string TransactionType
        {
            get
            {
                return transactionTypeField;
            }
            set
            {
                transactionTypeField = value;
            }
        }

        /// <remarks/>
        public string TransactionSubType
        {
            get
            {
                return transactionSubTypeField;
            }
            set
            {
                transactionSubTypeField = value;
            }
        }

        /// <remarks/>
        public FundsTransferTransactionResponseDetails TransactionResponseDetails
        {
            get
            {
                return transactionResponseDetailsField;
            }
            set
            {
                transactionResponseDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItem("TransactionItem", IsNullable = false)]
        public FundsTransferTransactionItem[] TransactionItems
        {
            get
            {
                return transactionItemsField;
            }
            set
            {
                transactionItemsField = value;
            }
        }

        /// <remarks/>
        public FundsTransferTransactionCharges TransactionCharges
        {
            get
            {
                return transactionChargesField;
            }
            set
            {
                transactionChargesField = value;
            }
        }

        /// <remarks/>
        public FundsTransferTransactionItem TransactionItem
        {
            get
            {
                return transactionItemField;
            }
            set
            {
                transactionItemField = value;
            }
        }
    }

}
