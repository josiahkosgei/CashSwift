
//BusinessObjects.Authentication.XAF.Permissions.CheckerPermission


using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using System;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions
{
    public class CheckerPermission : IOperationPermission
    {
        public CheckerPermission(Type objectType, SecurityPermissionState state)
        {
            ObjectType = objectType;
            State = state;
        }

        public Type ObjectType { get; private set; }

        public string Operation => "Checker";

        public SecurityPermissionState State { get; set; }
    }
}
