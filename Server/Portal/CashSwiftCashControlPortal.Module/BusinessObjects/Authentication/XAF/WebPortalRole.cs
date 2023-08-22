
//BusinessObjects.Authentication.XAF.WebPortalRole



using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Linq;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF
{
    [NavigationItem("User Management")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    [ImageName("BO_Role")]
    [Persistent("WebPortalRole")]
    public class WebPortalRole : PermissionPolicyRoleBase, IPermissionPolicyRoleWithUsers
    {
        private string fdescription;

        [Size(255)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [DisplayName("Description")]
        public string description
        {
            get => fdescription;
            set => SetPropertyValue(nameof(description), ref fdescription, value);
        }

        [Association("ApplicationUser-WebPortalRole")]
        public XPCollection<ApplicationUser> ApplicationUsers => GetCollection<ApplicationUser>(nameof(ApplicationUsers));

        IEnumerable<IPermissionPolicyUser> IPermissionPolicyRoleWithUsers.Users => ApplicationUsers.OfType<IPermissionPolicyUser>();

        public WebPortalRole(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.AddObjectPermission<ApplicationUser>("Read;Navigate", "[id] = CurrentUserId()", new SecurityPermissionState?(SecurityPermissionState.Allow));
            this.AddMemberPermission<ApplicationUser>("Write", "ChangePasswordOnFirstLogon", "[id] = CurrentUserId()", new SecurityPermissionState?(SecurityPermissionState.Allow));
            this.AddMemberPermission<ApplicationUser>("Write", "StoredPassword", "[id] = CurrentUserId()", new SecurityPermissionState?(SecurityPermissionState.Allow));
            this.AddObjectPermission<ApplicationUserLoginDetail>("Read;Write;Delete;Navigate", "[User].[id] = CurrentUserId()", new SecurityPermissionState?(SecurityPermissionState.Allow));
            this.AddObjectPermission<WebPortalLoginLogEntry>("Read;Write;Delete;Navigate", "[ApplicationUserLoginDetail].[User].[id] = CurrentUserId()", new SecurityPermissionState?(SecurityPermissionState.Allow));
            this.AddObjectPermission<UserLockLogEntry>("Read;Write;Delete;Navigate", "[ApplicationUserLoginDetail].[User].[id] = CurrentUserId()", new SecurityPermissionState?(SecurityPermissionState.Allow));
            this.AddMemberPermission<WebPortalRole>("Read", "Name", "[ApplicationUsers][[id] = CurrentUserId()]", new SecurityPermissionState?(SecurityPermissionState.Allow));
            this.AddTypePermissionsRecursively<ModelDifference>("Read;Write", new SecurityPermissionState?(SecurityPermissionState.Allow));
            this.AddTypePermissionsRecursively<ModelDifferenceAspect>("Read;Write", new SecurityPermissionState?(SecurityPermissionState.Allow));
            this.AddNavigationPermission("My Details", new SecurityPermissionState?(SecurityPermissionState.Allow));
        }
    }
}
