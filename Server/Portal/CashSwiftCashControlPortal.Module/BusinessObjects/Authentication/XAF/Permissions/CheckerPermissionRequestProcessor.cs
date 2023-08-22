
//BusinessObjects.Authentication.XAF.Permissions.CheckerPermissionRequestProcessor


using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions
{
    public class CheckerPermissionRequestProcessor : PermissionRequestProcessorBase
    {
        public CheckerPermissionRequestProcessor(IPermissionDictionary permissionDictionary) => this.permissionDictionary = permissionDictionary;

        public override bool IsGranted(IPermissionRequest permissionRequest)
        {
            CheckerPermissionRequest CheckerPermissionRequest = permissionRequest as CheckerPermissionRequest;
            if (CheckerPermissionRequest == null)
                return false;
            IEnumerable<CheckerPermission> source = permissionDictionary.GetPermissions<CheckerPermission>().Where(p => p.ObjectType == CheckerPermissionRequest.ObjectType);
            return source.Count() == 0 ? IsGrantedByPolicy(permissionDictionary) : source.Any(p => p.State == SecurityPermissionState.Allow);
        }
    }
}
