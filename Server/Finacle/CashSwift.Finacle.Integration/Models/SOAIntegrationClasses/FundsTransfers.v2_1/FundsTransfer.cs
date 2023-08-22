// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransfer
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    [XmlRoot(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1", IsNullable = false)]
    public class FundsTransfer
    {
        private string versionField;

        private FundsTransferStatus statusField;

        private string messageTypeField;

        private string origCtxtIdField;

        private FundsTransferAtmMessageHeader atmMessageHeaderField;

        private FundsTransferTxnInputData txnInputDataField;

        private FundsTransferTxnOutputData txnOutputDataField;

        private FundsTransferDebitNarrative debitNarrativeField;

        private FundsTransferCreditNarrative creditNarrativeField;

        private FundsTransferDebitPosting debitPostingField;

        private FundsTransferCreditPosting creditPostingField;

        private FundsTransferAccountTfrPostings accountTfrPostingsField;

        private FundsTransferCharges chargesField;

        private FundsTransferTxnFXData txnFXDataField;

        private FundsTransferAtmDetails atmDetailsField;

        public string version
        {
            get
            {
                return versionField;
            }
            set
            {
                versionField = value;
            }
        }

        public FundsTransferStatus status
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

        public string messageType
        {
            get
            {
                return messageTypeField;
            }
            set
            {
                messageTypeField = value;
            }
        }

        public string origCtxtId
        {
            get
            {
                return origCtxtIdField;
            }
            set
            {
                origCtxtIdField = value;
            }
        }

        public FundsTransferAtmMessageHeader atmMessageHeader
        {
            get
            {
                return atmMessageHeaderField;
            }
            set
            {
                atmMessageHeaderField = value;
            }
        }

        public FundsTransferTxnInputData txnInputData
        {
            get
            {
                return txnInputDataField;
            }
            set
            {
                txnInputDataField = value;
            }
        }

        public FundsTransferTxnOutputData txnOutputData
        {
            get
            {
                return txnOutputDataField;
            }
            set
            {
                txnOutputDataField = value;
            }
        }

        public FundsTransferDebitNarrative debitNarrative
        {
            get
            {
                return debitNarrativeField;
            }
            set
            {
                debitNarrativeField = value;
            }
        }

        public FundsTransferCreditNarrative creditNarrative
        {
            get
            {
                return creditNarrativeField;
            }
            set
            {
                creditNarrativeField = value;
            }
        }

        public FundsTransferDebitPosting debitPosting
        {
            get
            {
                return debitPostingField;
            }
            set
            {
                debitPostingField = value;
            }
        }

        public FundsTransferCreditPosting creditPosting
        {
            get
            {
                return creditPostingField;
            }
            set
            {
                creditPostingField = value;
            }
        }

        public FundsTransferAccountTfrPostings accountTfrPostings
        {
            get
            {
                return accountTfrPostingsField;
            }
            set
            {
                accountTfrPostingsField = value;
            }
        }

        public FundsTransferCharges charges
        {
            get
            {
                return chargesField;
            }
            set
            {
                chargesField = value;
            }
        }

        public FundsTransferTxnFXData txnFXData
        {
            get
            {
                return txnFXDataField;
            }
            set
            {
                txnFXDataField = value;
            }
        }

        public FundsTransferAtmDetails atmDetails
        {
            get
            {
                return atmDetailsField;
            }
            set
            {
                atmDetailsField = value;
            }
        }
    }
}
