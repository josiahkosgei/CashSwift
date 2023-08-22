
//BusinessObjects.Authentication.XAF.Permissions.CheckerPermissionRequest


using DevExpress.ExpressApp.Security;
using System;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions
{
    public class CheckerPermissionRequest : IPermissionRequest
    {
        public CheckerPermissionRequest(Type objectType) => ObjectType = objectType;

        public Type ObjectType { get; private set; }

        public object GetHashObject() => ObjectType.FullName;
    }
}
