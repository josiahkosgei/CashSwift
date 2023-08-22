
//BusinessObjects.Authentication.XAF.ApplicationUserMode


using System;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF
{
    [Flags]
    public enum ApplicationUserMode
    {
        None = 0,
        WebPortalAccess = 1,
        DepositorAccess = 2,
        Both = 0,
    }
}
