
//BusinessObjects.Authentication.CashSwift.UserLockType


using DevExpress.Persistent.Base;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    public enum UserLockType
    {
        [ImageName("locked")] Lock,
        [ImageName("unlocked")] Unlock,
        [ImageName("disable")] Disable,
        [ImageName("enable")] Enable,
        [ImageName("unlocked_device")] UnlockOnDepositor,
    }
}
