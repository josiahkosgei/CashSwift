// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.FundsTransferAtmMessageHeader
using System.ComponentModel;
using System.Xml.Serialization;


namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
    public class FundsTransferAtmMessageHeader
    {
        private string versionNumField;

        private FundsTransferAtmMessageHeaderAtmMsgType atmMsgTypeField;

        public string versionNum
        {
            get
            {
                return versionNumField;
            }
            set
            {
                versionNumField = value;
            }
        }

        public FundsTransferAtmMessageHeaderAtmMsgType atmMsgType
        {
            get
            {
                return atmMsgTypeField;
            }
            set
            {
                atmMsgTypeField = value;
            }
        }
    }
}