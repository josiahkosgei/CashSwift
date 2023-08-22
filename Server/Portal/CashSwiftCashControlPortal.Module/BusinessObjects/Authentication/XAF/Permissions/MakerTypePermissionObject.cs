
//BusinessObjects.Authentication.XAF.Permissions.MakerTypePermissionObject
// Assembly: CashSwiftCashControlPortal.Module, Version=5.19.3.0, Culture=neutral, PublicKeyToken=null
// MVID: A65EA673-D4FA-4E5C-B5D1-17D9ACB0EC19
// Assembly location: C:\DEV\maniwa\Coop\Coop\Server\App\Web\6.0\bin\CashSwiftCashControlPortal.Module.dll

using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions
{
    [XafDisplayName("Portal Operation Permissions")]
    public class MakerTypePermissionObject : PermissionPolicyTypePermissionObject
    {
        public MakerTypePermissionObject(Session session)
          : base(session)
        {
        }

        [XafDisplayName("Maker")]
        public SecurityPermissionState? MakerState
        {
            get => GetPropertyValue<SecurityPermissionState?>(nameof(MakerState));
            set => SetPropertyValue(nameof(MakerState), value);
        }

        [XafDisplayName("Checker")]
        public SecurityPermissionState? CheckerState
        {
            get => GetPropertyValue<SecurityPermissionState?>(nameof(CheckerState));
            set => SetPropertyValue(nameof(CheckerState), value);
        }
    }
}
