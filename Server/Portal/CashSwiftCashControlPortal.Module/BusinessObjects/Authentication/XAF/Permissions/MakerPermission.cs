
//BusinessObjects.Authentication.XAF.Permissions.MakerPermission


using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using System;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions
{
    public class MakerPermission : IOperationPermission
    {
        public MakerPermission(Type objectType, SecurityPermissionState state)
        {
            ObjectType = objectType;
            State = state;
        }

        public Type ObjectType { get; private set; }

        public string Operation => "Maker";

        public SecurityPermissionState State { get; set; }
    }
}
