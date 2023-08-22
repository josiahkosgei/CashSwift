namespace CashSwift.Finacle.Integration.Models.AccountValidation
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public partial class Envelope
    {

        private EnvelopeHeader headerField;

        private EnvelopeBody bodyField;

        /// <remarks/>
        public EnvelopeHeader Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public EnvelopeBody Body
        {
            get
            {
                return this.bodyField;
            }
            set
            {
                this.bodyField = value;
            }
        }
    }

    
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeHeader
    {

        private ResponseHeader responseHeaderField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
        public ResponseHeader ResponseHeader
        {
            get
            {
                return this.responseHeaderField;
            }
            set
            {
                this.responseHeaderField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader", IsNullable = false)]
    public partial class ResponseHeader
    {

        private string correlationIDField;

        private string messageIDField;

        private string statusCodeField;

        private string statusDescriptionField;

        private object statusDescriptionKeyField;

        private ResponseHeaderStatusMessages statusMessagesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Common")]
        public string CorrelationID
        {
            get
            {
                return this.correlationIDField;
            }
            set
            {
                this.correlationIDField = value;
            }
        }

        /// <remarks/>
        public string MessageID
        {
            get
            {
                return this.messageIDField;
            }
            set
            {
                this.messageIDField = value;
            }
        }

        /// <remarks/>
        public string StatusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }

        /// <remarks/>
        public string StatusDescription
        {
            get
            {
                return this.statusDescriptionField;
            }
            set
            {
                this.statusDescriptionField = value;
            }
        }

        /// <remarks/>
        public object StatusDescriptionKey
        {
            get
            {
                return this.statusDescriptionKeyField;
            }
            set
            {
                this.statusDescriptionKeyField = value;
            }
        }

        /// <remarks/>
        public ResponseHeaderStatusMessages StatusMessages
        {
            get
            {
                return this.statusMessagesField;
            }
            set
            {
                this.statusMessagesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/CommonServices/Data/Message/MessageHeader")]
    public partial class ResponseHeaderStatusMessages
    {

        private string messageCodeField;

        private string messageDescriptionField;

        private object messageTypeField;

        /// <remarks/>
        public string MessageCode
        {
            get
            {
                return this.messageCodeField;
            }
            set
            {
                this.messageCodeField = value;
            }
        }

        /// <remarks/>
        public string MessageDescription
        {
            get
            {
                return this.messageDescriptionField;
            }
            set
            {
                this.messageDescriptionField = value;
            }
        }

        /// <remarks/>
        public object MessageType
        {
            get
            {
                return this.messageTypeField;
            }
            set
            {
                this.messageTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeBody
    {

        private AccountDetailsResponse accountDetailsResponseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "urn://co-opbank.co.ke/BS/Account/BSAccountDetails.3.0")]
        public AccountDetailsResponse AccountDetailsResponse
        {
            get
            {
                return this.accountDetailsResponseField;
            }
            set
            {
                this.accountDetailsResponseField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/BS/Account/BSAccountDetails.3.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://co-opbank.co.ke/BS/Account/BSAccountDetails.3.0", IsNullable = false)]
    public partial class AccountDetailsResponse
    {

        private string accountNumberField;

        private string accountNameField;

        private string currencyCodeField;

        private string jointAccountField;

        private string productIDField;

        private string productContextCodeField;

        private string productNameField;

        private decimal clearedBalanceField;

        private decimal bookedBalanceField;

        private decimal blockedBalanceField;

        private decimal availableBalanceField;

        private string branchNameField;

        private string branchCodeField;

        private string phoneNumberField;

        private string customerCodeField;

        private string emailField;

        private string dormantField;

        private string stoppedField;

        private string closedField;

        private string accountRightsIndicatorField;

        private string postalAddressField;

        private string townField;

        private System.DateTime openDateField;

        private string statusField;

        /// <remarks/>
        public string AccountNumber
        {
            get
            {
                return this.accountNumberField;
            }
            set
            {
                this.accountNumberField = value;
            }
        }

        /// <remarks/>
        public string AccountName
        {
            get
            {
                return this.accountNameField;
            }
            set
            {
                this.accountNameField = value;
            }
        }

        /// <remarks/>
        public string CurrencyCode
        {
            get
            {
                return this.currencyCodeField;
            }
            set
            {
                this.currencyCodeField = value;
            }
        }

        /// <remarks/>
        public string JointAccount
        {
            get
            {
                return this.jointAccountField;
            }
            set
            {
                this.jointAccountField = value;
            }
        }

        /// <remarks/>
        public string ProductID
        {
            get
            {
                return this.productIDField;
            }
            set
            {
                this.productIDField = value;
            }
        }

        /// <remarks/>
        public string ProductContextCode
        {
            get
            {
                return this.productContextCodeField;
            }
            set
            {
                this.productContextCodeField = value;
            }
        }

        /// <remarks/>
        public string ProductName
        {
            get
            {
                return this.productNameField;
            }
            set
            {
                this.productNameField = value;
            }
        }

        /// <remarks/>
        public decimal ClearedBalance
        {
            get
            {
                return this.clearedBalanceField;
            }
            set
            {
                this.clearedBalanceField = value;
            }
        }

        /// <remarks/>
        public decimal BookedBalance
        {
            get
            {
                return this.bookedBalanceField;
            }
            set
            {
                this.bookedBalanceField = value;
            }
        }

        /// <remarks/>
        public decimal BlockedBalance
        {
            get
            {
                return this.blockedBalanceField;
            }
            set
            {
                this.blockedBalanceField = value;
            }
        }

        /// <remarks/>
        public decimal AvailableBalance
        {
            get
            {
                return this.availableBalanceField;
            }
            set
            {
                this.availableBalanceField = value;
            }
        }

        /// <remarks/>
        public string BranchName
        {
            get
            {
                return this.branchNameField;
            }
            set
            {
                this.branchNameField = value;
            }
        }

        /// <remarks/>
        public string BranchCode
        {
            get
            {
                return this.branchCodeField;
            }
            set
            {
                this.branchCodeField = value;
            }
        }

        /// <remarks/>
        public string PhoneNumber
        {
            get
            {
                return this.phoneNumberField;
            }
            set
            {
                this.phoneNumberField = value;
            }
        }

        /// <remarks/>
        public string CustomerCode
        {
            get
            {
                return this.customerCodeField;
            }
            set
            {
                this.customerCodeField = value;
            }
        }

        /// <remarks/>
        public string Email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        public string Dormant
        {
            get
            {
                return this.dormantField;
            }
            set
            {
                this.dormantField = value;
            }
        }

        /// <remarks/>
        public string Stopped
        {
            get
            {
                return this.stoppedField;
            }
            set
            {
                this.stoppedField = value;
            }
        }

        /// <remarks/>
        public string Closed
        {
            get
            {
                return this.closedField;
            }
            set
            {
                this.closedField = value;
            }
        }

        /// <remarks/>
        public string AccountRightsIndicator
        {
            get
            {
                return this.accountRightsIndicatorField;
            }
            set
            {
                this.accountRightsIndicatorField = value;
            }
        }

        /// <remarks/>
        public string PostalAddress
        {
            get
            {
                return this.postalAddressField;
            }
            set
            {
                this.postalAddressField = value;
            }
        }

        /// <remarks/>
        public string Town
        {
            get
            {
                return this.townField;
            }
            set
            {
                this.townField = value;
            }
        }

        /// <remarks/>
        public System.DateTime OpenDate
        {
            get
            {
                return this.openDateField;
            }
            set
            {
                this.openDateField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

}
