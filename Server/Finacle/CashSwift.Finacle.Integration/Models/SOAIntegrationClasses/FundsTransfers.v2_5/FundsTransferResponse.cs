// CashSwift.Integrations.CooperativeBank.SOAIntegrationClasses.FundsTransfers.v2_5.FundsTransferResponse
using System.ComponentModel;
using System.Xml.Serialization;

namespace CashSwift.Finacle.Integration.Models.SOAIntegrationClasses.FundsTransfers.v2_5
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/2.5")]
    [XmlRoot(Namespace = "urn://co-opbank.co.ke/Banking/CanonicalDataModel/FundsTransfer/2.5", IsNullable = false)]
    public class FundsTransferResponse
    {
        public FundsTransferResponseFundsTransferRespData FundsTransferRespData { get; set; }
    }
}