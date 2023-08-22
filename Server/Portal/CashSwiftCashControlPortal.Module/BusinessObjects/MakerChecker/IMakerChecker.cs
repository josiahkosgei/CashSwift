
//BusinessObjects.MakerChecker.IMakerChecker


using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using DevExpress.ExpressApp.Model;
using System;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.MakerChecker
{
    internal interface IMakerChecker
    {
        [ModelDefault("AllowEdit", "False")]
        ApplicationUser initialising_user { get; set; }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        DateTime init_date { get; set; }

        [ModelDefault("AllowEdit", "False")]
        ApplicationUser authorising_user { get; set; }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        DateTime auth_date { get; set; }

        [ModelDefault("AllowEdit", "False")]
        MakerCheckerDecision? auth_response { get; set; }

        [ModelDefault("AllowEdit", "False")]
        string reason { get; set; }
    }
}
