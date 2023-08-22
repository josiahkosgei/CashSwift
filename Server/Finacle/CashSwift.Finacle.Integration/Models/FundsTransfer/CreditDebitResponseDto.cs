namespace CashSwift.Finacle.Integration.Models.FundsTransfer
{


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [System.Xml.Serialization.XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public partial class Envelope
    {

        private EnvelopeHeader headerField;

        private EnvelopeBody bodyField;

        /// <remarks/>
        public EnvelopeHeader Header
        {
            get
            {
                return headerField;
            }
            set
            {
                headerField = value;
            }
        }

        /// <remarks/>
        public EnvelopeBody Body
        {
            get
            {
                return bodyField;
            }
            set
            {
                bodyField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeHeader
    {

        private ResponseHeader responseHeaderField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
        public ResponseHeader ResponseHeader
        {
            get
            {
                return responseHeaderField;
            }
            set
            {
                responseHeaderField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
    [System.Xml.Serialization.XmlRoot(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader", IsNullable = false)]
    public partial class ResponseHeader
    {

        private string correlationIDField;

        private string messageIDField;

        private string statusCodeField;

        private string statusDescriptionField;

        private ResponseHeaderStatusMessages statusMessagesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Common")]
        public string CorrelationID
        {
            get
            {
                return correlationIDField;
            }
            set
            {
                correlationIDField = value;
            }
        }

        /// <remarks/>
        public string MessageID
        {
            get
            {
                return messageIDField;
            }
            set
            {
                messageIDField = value;
            }
        }

        /// <remarks/>
        public string StatusCode
        {
            get
            {
                return statusCodeField;
            }
            set
            {
                statusCodeField = value;
            }
        }

        /// <remarks/>
        public string StatusDescription
        {
            get
            {
                return statusDescriptionField;
            }
            set
            {
                statusDescriptionField = value;
            }
        }

        /// <remarks/>
        public ResponseHeaderStatusMessages StatusMessages
        {
            get
            {
                return statusMessagesField;
            }
            set
            {
                statusMessagesField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
    public partial class ResponseHeaderStatusMessages
    {

        private byte messageCodeField;

        private string messageDescriptionField;

        private object messageTypeField;

        /// <remarks/>
        public byte MessageCode
        {
            get
            {
                return messageCodeField;
            }
            set
            {
                messageCodeField = value;
            }
        }

        /// <remarks/>
        public string MessageDescription
        {
            get
            {
                return messageDescriptionField;
            }
            set
            {
                messageDescriptionField = value;
            }
        }

        /// <remarks/>
        public object MessageType
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
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeBody
    {

        private FundsTransferDto fundsTransferField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
        public FundsTransferDto FundsTransfer
        {
            get
            {
                return fundsTransferField;
            }
            set
            {
                fundsTransferField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    [System.Xml.Serialization.XmlRoot(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0", IsNullable = false)]
    public partial class FundsTransferDto
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

        private FundsTransferTransactionItem1 transactionItemField;

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
        public FundsTransferTransactionItem1 TransactionItem
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

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionResponseDetails
    {

        private string remarksField;

        /// <remarks/>
        public string Remarks
        {
            get
            {
                return remarksField;
            }
            set
            {
                remarksField = value;
            }
        }
    }

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

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionCharges
    {

        private FundsTransferTransactionChargesCharge chargeField;

        /// <remarks/>
        public FundsTransferTransactionChargesCharge Charge
        {
            get
            {
                return chargeField;
            }
            set
            {
                chargeField = value;
            }
        }
    }

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

    /// <remarks/>
    [Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/4.0")]
    public partial class FundsTransferTransactionItem1
    {

        private DateTime valueDateField;

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
    }


}