
//BusinessObjects.Authentication.XAF.Permissions.MakerPermissionRequest


using DevExpress.ExpressApp.Security;
using System;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions
{
    public class MakerPermissionRequest : IPermissionRequest
    {
        public MakerPermissionRequest(Type objectType) => ObjectType = objectType;

        public Type ObjectType { get; private set; }

        public object GetHashObject() => ObjectType.FullName;
    }
}
