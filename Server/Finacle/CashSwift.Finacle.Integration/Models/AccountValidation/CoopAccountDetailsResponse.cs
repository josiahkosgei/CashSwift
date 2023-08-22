using CashSwift.Finacle.Integration.Models.SOAIntegrationClasses;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.AccountValidation
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public class CoopAccountDetailsResponse : CoopMessageBase
    {
        private EnvelopeHeader headerField;

        private EnvelopeBody bodyField;

        public string MessageID => Header?.ResponseHeader?.MessageID;

        public string Currency => Body?.AccountDetailsResponse?.CurrencyCode;

        public string AccountNumber => Body?.AccountDetailsResponse?.AccountNumber;

        public string AccountName => Body?.AccountDetailsResponse?.AccountName;

        public string RequestUUID => Header?.ResponseHeader?.MessageID;

        public bool Success => (Header?.ResponseHeader?.StatusCode.Equals("0")).GetValueOrDefault();

        public bool CanDeposit { get; set; }

        public string StatusCode { get; set; }

        public string StatusMessage { get; set; }

        public string ValidationStatus { get; set; }

        public string ValidationMessage { get; set; }

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

        internal void ValidateResponse()
        {
            if (!Success)
            {
                StatusCode = Header?.ResponseHeader?.StatusCode;
                StatusMessage = Header?.ResponseHeader?.StatusDescription;
                ValidationStatus = Header?.ResponseHeader?.StatusMessages?.MessageCode;
                ValidationMessage = Header?.ResponseHeader?.StatusMessages?.MessageDescription;
            }
            else if ((Body?.AccountDetailsResponse?.Dormant.Equals("Y")).GetValueOrDefault())
            {
                StatusCode = "Dormant";
                StatusMessage = "Account is dormant";
                ValidationStatus = Body?.AccountDetailsResponse?.AccountRightsIndicator;
                ValidationMessage = "This account cannot perform a deposit. Kindly contact customer support";
            }
            else if ((Body?.AccountDetailsResponse?.Closed.Equals("Y")).GetValueOrDefault())
            {
                StatusCode = "Closed";
                StatusMessage = "Account is closed";
                ValidationStatus = Body?.AccountDetailsResponse?.AccountRightsIndicator;
                ValidationMessage = "This account cannot perform a deposit. Kindly contact customer support";
            }
            else
            {
                CanDeposit = true;
                ValidationStatus = Header?.ResponseHeader?.StatusMessages?.MessageCode;
                ValidationMessage = Header?.ResponseHeader?.StatusMessages?.MessageDescription;
            }
        }
    }
}