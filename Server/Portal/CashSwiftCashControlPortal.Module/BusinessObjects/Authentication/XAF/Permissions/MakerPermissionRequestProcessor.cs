
//BusinessObjects.Authentication.XAF.Permissions.MakerPermissionRequestProcessor


using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions
{
    public class MakerPermissionRequestProcessor : PermissionRequestProcessorBase
    {
        public MakerPermissionRequestProcessor(IPermissionDictionary permissionDictionary) => this.permissionDictionary = permissionDictionary;

        public override bool IsGranted(IPermissionRequest permissionRequest)
        {
            MakerPermissionRequest MakerPermissionRequest = permissionRequest as MakerPermissionRequest;
            if (MakerPermissionRequest == null)
                return false;
            IEnumerable<MakerPermission> source = permissionDictionary.GetPermissions<MakerPermission>().Where(p => p.ObjectType == MakerPermissionRequest.ObjectType);
            return source.Count() == 0 ? IsGrantedByPolicy(permissionDictionary) : source.Any(p => p.State == SecurityPermissionState.Allow);
        }
    }
}
