// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_5.FundsTransferResponseFundsTransferRespDataTransactionItem
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_5
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/2.5")]
    public class FundsTransferResponseFundsTransferRespDataTransactionItem
    {
        private string accountNumberField;

        private decimal transactionAmountField;

        private string transactionReferenceField;

        private decimal baseEquivalentField;

        private string transactionCodeField;

        private decimal bookedBalanceField;

        private string statusField;

        public string TransactionItemKey { get; set; }

        public string AccountNumber
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

        public string DebitCreditFlag { get; set; }

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

        public string TransactionCurrency { get; set; }

        public string TransactionReference
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

        public string Narrative { get; set; }

        public decimal BaseEquivalent
        {
            get
            {
                return baseEquivalentField;
            }
            set
            {
                baseEquivalentField = value;
            }
        }

        public string SourceBranch { get; set; }

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

        [XmlElement("AvailableBalance")]
        public string AvailableBalanceString { get; set; }

        [XmlIgnore]
        public decimal? AvailableBalance
        {
            get
            {
                if (string.IsNullOrEmpty(AvailableBalanceString))
                {
                    return null;
                }
                return uint.Parse(AvailableBalanceString);
            }
        }

        [XmlElement("BookedBalance")]
        public string BookedBalanceString { get; set; }

        [XmlIgnore]
        public decimal? BookedBalance
        {
            get
            {
                if (string.IsNullOrEmpty(BookedBalanceString))
                {
                    return null;
                }
                return uint.Parse(BookedBalanceString);
            }
        }

        public string Status
        {
            get
            {
                return statusField;
            }
            set
            {
                statusField = value;
            }
        }
    }
}