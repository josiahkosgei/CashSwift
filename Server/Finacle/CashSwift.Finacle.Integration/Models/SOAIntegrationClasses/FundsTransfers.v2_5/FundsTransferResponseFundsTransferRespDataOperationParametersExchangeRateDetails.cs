// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_5.FundsTransferResponseFundsTransferRespDataOperationParametersExchangeRateDetails
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_5
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/2.5")]
    public class FundsTransferResponseFundsTransferRespDataOperationParametersExchangeRateDetails
    {
        private decimal exchangeRateField;

        private string toCurrencyField;

        public string FromCurrency { get; set; }

        public decimal ExchangeRate
        {
            get
            {
                return exchangeRateField;
            }
            set
            {
                exchangeRateField = value;
            }
        }

        public string ExchangeRateFlag { get; set; }

        public string ToCurrency
        {
            get
            {
                return toCurrencyField;
            }
            set
            {
                toCurrencyField = value;
            }
        }
    }
}