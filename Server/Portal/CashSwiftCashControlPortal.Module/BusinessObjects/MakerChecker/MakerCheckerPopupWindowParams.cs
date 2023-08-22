
//BusinessObjects.MakerChecker.MakerCheckerPopupWindowParams


using DevExpress.ExpressApp.DC;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.MakerChecker
{
    [DomainComponent]
    public class MakerCheckerPopupWindowParams
    {
        [FieldSize(100)]
        [XafDisplayName("Reason")]
        public string reason { get; set; }
    }
}
