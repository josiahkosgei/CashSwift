// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_1.DataOutput
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_1
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/Account/Service/AccountFundsTransfer/Post/2.1/DataIO")]
    [XmlRoot(Namespace = "urn://co-opbank.co.ke/Banking/Account/Service/AccountFundsTransfer/Post/2.1/DataIO", IsNullable = false)]
    public class DataOutput
    {
        private postOutput postOutputField;

        [XmlElement(Namespace = "urn://co-opbank.co.ke/Banking/Account/DataModel/AccountFundsTransfer/Post/2.1/AccountFundsTransfer.post")]
        public postOutput postOutput
        {
            get
            {
                return postOutputField;
            }
            set
            {
                postOutputField = value;
            }
        }
    }
}
