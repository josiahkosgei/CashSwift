// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_5.FundsTransferResponseFundsTransferRespData
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_5
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/2.5")]
    public class FundsTransferResponseFundsTransferRespData
    {
        private FundsTransferResponseFundsTransferRespDataTransactionItem[] transactionItemsField;

        public FundsTransferResponseFundsTransferRespDataOperationParameters OperationParameters { get; set; }

        [XmlArrayItem("TransactionItem", IsNullable = false)]
        public FundsTransferResponseFundsTransferRespDataTransactionItem[] TransactionItems
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
    }
}