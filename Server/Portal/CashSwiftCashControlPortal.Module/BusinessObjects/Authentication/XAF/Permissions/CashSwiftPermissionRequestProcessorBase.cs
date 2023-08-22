
//BusinessObjects.Authentication.XAF.Permissions.PermissionRequestProcessorBase


using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions
{
  public abstract class PermissionRequestProcessorBase : 
    PermissionRequestProcessorBase<IPermissionRequest>
  {
    protected IPermissionDictionary permissionDictionary;

    protected bool IsGrantedByPolicy(IPermissionDictionary permissionDictionary) => GetPermissionPolicy(permissionDictionary) == SecurityPermissionPolicy.AllowAllByDefault;

    protected SecurityPermissionPolicy GetPermissionPolicy(
      IPermissionDictionary permissionDictionary)
    {
      SecurityPermissionPolicy permissionPolicy = SecurityPermissionPolicy.DenyAllByDefault;
      List<SecurityPermissionPolicy> list = permissionDictionary.GetPermissions<PermissionPolicy>().Select(p => p.SecurityPermissionPolicy).ToList();
      if (list != null && list.Count != 0)
      {
        if (list.Any(p => p == SecurityPermissionPolicy.AllowAllByDefault))
          permissionPolicy = SecurityPermissionPolicy.AllowAllByDefault;
        else if (list.Any(p => p == SecurityPermissionPolicy.ReadOnlyAllByDefault))
          permissionPolicy = SecurityPermissionPolicy.ReadOnlyAllByDefault;
      }
      return permissionPolicy;
    }
  }
}
