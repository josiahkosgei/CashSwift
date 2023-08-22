// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v4_0.EnvelopeBody

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v4_0
{
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionItem
    {

        private ulong transactionReferenceField;

        private string transactionItemKeyField;

        private ulong accountNumberField;

        private string debitCreditFlagField;

        private decimal transactionAmountField;

        private string transactionCurrencyField;

        private string narrativeField;

        private string transactionCodeField;

        private decimal availableBalanceField;

        private decimal bookedBalanceField;

        private object temporaryODDetailsField;

        /// <remarks/>
        public ulong TransactionReference
        {
            get
            {
                return transactionReferenceField;
            }
            set
            {
                transactionReferenceField = value;
            }
        }

        /// <remarks/>
        public string TransactionItemKey
        {
            get
            {
                return transactionItemKeyField;
            }
            set
            {
                transactionItemKeyField = value;
            }
        }

        /// <remarks/>
        public ulong AccountNumber
        {
            get
            {
                return accountNumberField;
            }
            set
            {
                accountNumberField = value;
            }
        }

        /// <remarks/>
        public string DebitCreditFlag
        {
            get
            {
                return debitCreditFlagField;
            }
            set
            {
                debitCreditFlagField = value;
            }
        }

        /// <remarks/>
        public decimal TransactionAmount
        {
            get
            {
                return transactionAmountField;
            }
            set
            {
                transactionAmountField = value;
            }
        }

        /// <remarks/>
        public string TransactionCurrency
        {
            get
            {
                return transactionCurrencyField;
            }
            set
            {
                transactionCurrencyField = value;
            }
        }

        /// <remarks/>
        public string Narrative
        {
            get
            {
                return narrativeField;
            }
            set
            {
                narrativeField = value;
            }
        }

        /// <remarks/>
        public string TransactionCode
        {
            get
            {
                return transactionCodeField;
            }
            set
            {
                transactionCodeField = value;
            }
        }

        /// <remarks/>
        public decimal AvailableBalance
        {
            get
            {
                return availableBalanceField;
            }
            set
            {
                availableBalanceField = value;
            }
        }

        /// <remarks/>
        public decimal BookedBalance
        {
            get
            {
                return bookedBalanceField;
            }
            set
            {
                bookedBalanceField = value;
            }
        }

        /// <remarks/>
        public object TemporaryODDetails
        {
            get
            {
                return temporaryODDetailsField;
            }
            set
            {
                temporaryODDetailsField = value;
            }
        }
    }

}
