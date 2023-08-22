// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.postOutput
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/Account/DataModel/AccountFundsTransfer/Post/2.1/AccountFundsTransfer.post")]
    [XmlRoot(Namespace = "urn://co-opbank.co.ke/Banking/Account/DataModel/AccountFundsTransfer/Post/2.1/AccountFundsTransfer.post", IsNullable = false)]
    public class postOutput
    {
        private FundsTransfer fundsTransferField;

        [XmlElement(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/1.1")]
        public FundsTransfer FundsTransfer
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
}