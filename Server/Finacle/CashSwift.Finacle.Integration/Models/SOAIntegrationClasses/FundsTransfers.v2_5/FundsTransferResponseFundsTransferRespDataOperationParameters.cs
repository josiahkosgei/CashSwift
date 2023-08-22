// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_5.FundsTransferResponseFundsTransferRespDataOperationParameters
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_5
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/2.5")]
    public class FundsTransferResponseFundsTransferRespDataOperationParameters
    {
        private string messageTypeField;

        private DateTime transactionDatetimeField;

        private string noOfElementsField;

        private string transactionIDField;

        private FundsTransferResponseFundsTransferRespDataOperationParametersExchangeRateDetails exchangeRateDetailsField;

        public string MessageReference { get; set; }

        public string MessageType
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

        public string UserID { get; set; }

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

        public DateTime ValueDate { get; set; }

        public string NoOfElements
        {
            get
            {
                return noOfElementsField;
            }
            set
            {
                noOfElementsField = value;
            }
        }

        public string MessageStatus { get; set; }

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

        public FundsTransferResponseFundsTransferRespDataOperationParametersExchangeRateDetails ExchangeRateDetails
        {
            get
            {
                return exchangeRateDetailsField;
            }
            set
            {
                exchangeRateDetailsField = value;
            }
        }
    }
}