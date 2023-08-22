// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferAtmDetailsAmountCardHldBill
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferAtmDetailsAmountCardHldBill
    {
        private FundsTransferAtmDetailsAmountCardHldBillCardCcyAmt cardCcyAmtField;

        private FundsTransferAtmDetailsAmountCardHldBillMinorCurrAmt minorCurrAmtField;

        private FundsTransferAtmDetailsAmountCardHldBillValueAmt valueAmtField;

        public FundsTransferAtmDetailsAmountCardHldBillCardCcyAmt cardCcyAmt
        {
            get
            {
                return cardCcyAmtField;
            }
            set
            {
                cardCcyAmtField = value;
            }
        }

        public FundsTransferAtmDetailsAmountCardHldBillMinorCurrAmt minorCurrAmt
        {
            get
            {
                return minorCurrAmtField;
            }
            set
            {
                minorCurrAmtField = value;
            }
        }

        public FundsTransferAtmDetailsAmountCardHldBillValueAmt valueAmt
        {
            get
            {
                return valueAmtField;
            }
            set
            {
                valueAmtField = value;
            }
        }
    }
}