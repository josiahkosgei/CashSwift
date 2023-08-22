
//Controllers.ApplicationUserViewController


using CashSwift.Library.Standard.Security;

using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift;

namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class ApplicationUserViewController : ViewController
    {
        private IContainer components;
        private SimpleAction LockUserSimpleAction;
        private SimpleAction UnlockUserSimpleAction;
        private SimpleAction DeleteUserSimpleAction;
        private SimpleAction UnDeleteUserSimpleAction;
        private SimpleAction ResetUserPasswordAction;
        private SimpleAction UnlockUserOnDevice;

        public ApplicationUserViewController() => InitializeComponent();

        protected override void OnActivated()
        {
            base.OnActivated();
            Frame?.GetController<ResetPasswordController>()?.Actions["ResetPassword"]?.Active?.SetItemValue("", false);
        }

        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        protected override void OnDeactivated() => base.OnDeactivated();

        private void LockUserSimpleAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ApplicationUser initiatingUser = ObjectSpace.GetObject(SecuritySystem.CurrentUser as ApplicationUser);
                foreach (ApplicationUser selectedObject in (ArrayList)e.SelectedObjects)
                {
                    ApplicationUser applicationUser = ObjectSpace.GetObject(selectedObject);
                    if (applicationUser.IsActive || applicationUser.DepositorEnabled)
                        applicationUser.LockUser(true, initiatingUser);
                }
                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void UnlockUserSimpleAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ApplicationUser initiatingUser = ObjectSpace.GetObject(SecuritySystem.CurrentUser as ApplicationUser);
                foreach (ApplicationUser selectedObject in (ArrayList)e.SelectedObjects)
                {
                    ApplicationUser applicationUser = ObjectSpace.GetObject(selectedObject);
                    if (!applicationUser.IsActive)
                        applicationUser.UnlockUser(true, initiatingUser);
                }
                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void UnDeleteUserSimpleAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ApplicationUser initiatingUser = ObjectSpace.GetObject(SecuritySystem.CurrentUser as ApplicationUser);
                if (initiatingUser == null)
                    return;
                foreach (ApplicationUser selectedObject in (ArrayList)e.SelectedObjects)
                {
                    ApplicationUser applicationUser = ObjectSpace.GetObject(selectedObject);
                    if (applicationUser.UserDeleted)
                        applicationUser.UnDeleteUser(initiatingUser);
                }
                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void DeleteUserSimpleAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ApplicationUser initiatingUser = ObjectSpace.GetObject(SecuritySystem.CurrentUser as ApplicationUser);
                if (initiatingUser == null)
                    return;
                foreach (ApplicationUser selectedObject in (ArrayList)e.SelectedObjects)
                {
                    ApplicationUser applicationUser = ObjectSpace.GetObject(selectedObject);
                    if (!applicationUser.UserDeleted)
                        applicationUser.DeleteUser(initiatingUser);
                }
                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ChangeApplicationUserPasswordAction_CustomizePopupWindowParams(
          object sender,
          CustomizePopupWindowParamsEventArgs e)
        {
            ApplicationUser currentObject = (ApplicationUser)View.CurrentObject;
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            PasswordPolicy passwordPolicy = objectSpace.GetObjectsQuery<PasswordPolicy>().FirstOrDefault();
            if (passwordPolicy == null)
                return;
            PasswordPolicyItems passwordPolicyItems = new PasswordPolicyItems()
            {
                HistorySize = passwordPolicy.history_size,
                Lower_Case_length = passwordPolicy.min_lowercase,
                Minimum_Length = passwordPolicy.min_length,
                Numeric_length = passwordPolicy.min_digits,
                Special_length = passwordPolicy.min_special,
                Upper_Case_length = passwordPolicy.min_uppercase
            };
            IList<PasswordHistory> list = objectSpace.GetObjectsQuery<PasswordHistory>().OrderByDescending(x => x.LogDate).Take(passwordPolicy.history_size).ToList();
            ApplicationUserChangePassword userChangePassword = objectSpace.CreateObject<ApplicationUserChangePassword>();
            userChangePassword.PasswordHistory = list;
            userChangePassword.PasswordPolicy = passwordPolicy;
            userChangePassword.User = objectSpace.GetObject(currentObject);
            string detailViewId = Application.FindDetailViewId(typeof(ApplicationUserChangePassword));
            DetailView detailView = Application.CreateDetailView(objectSpace, detailViewId, true, userChangePassword);
            detailView.ViewEditMode = ViewEditMode.Edit;
            e.View =  detailView;
        }

        private void ChangeApplicationUserPasswordAction_Execute(
          object sender,
          PopupWindowShowActionExecuteEventArgs e)
        {
            ApplicationUserChangePassword viewCurrentObject = (ApplicationUserChangePassword)e.PopupWindowViewCurrentObject;
            ApplicationUser currentObject = (ApplicationUser)View.CurrentObject;
            PasswordPolicy passwordPolicy = ObjectSpace.GetObjectsQuery<PasswordPolicy>().FirstOrDefault();
            if (passwordPolicy == null)
                return;
            PasswordPolicyItems Policy = new PasswordPolicyItems()
            {
                HistorySize = passwordPolicy.history_size,
                Lower_Case_length = passwordPolicy.min_lowercase,
                Minimum_Length = passwordPolicy.min_length,
                Numeric_length = passwordPolicy.min_digits,
                Special_length = passwordPolicy.min_special,
                Upper_Case_length = passwordPolicy.min_uppercase
            };
            IList<PasswordHistory> list = ObjectSpace.GetObjectsQuery<PasswordHistory>().OrderByDescending(x => x.LogDate).Take(passwordPolicy.history_size).ToList();
            if (string.IsNullOrWhiteSpace(viewCurrentObject.OldPassword) || string.IsNullOrWhiteSpace(viewCurrentObject.NewPassword) || string.IsNullOrWhiteSpace(viewCurrentObject.ConfirmPassword) || !(viewCurrentObject.NewPassword.ToUpper() == viewCurrentObject.ConfirmPassword.ToUpper()) || !currentObject.ComparePassword(viewCurrentObject.OldPassword))
                return;
            StringBuilder stringBuilder = new StringBuilder(100);
            IList<PasswordPolicyResult> passwordPolicyResultList = PasswordPolicyManager.Validate(viewCurrentObject.NewPassword, Policy);
            if (passwordPolicyResultList == null)
            {
                if (!passwordPolicy.use_history)
                    return;
                foreach (PasswordHistory passwordHistory in (IEnumerable<PasswordHistory>)list)
                {
                    if (PasswordStorage.VerifyPassword(viewCurrentObject.NewPassword, passwordHistory.Password))
                        return;
                }
                currentObject.SetPassword(viewCurrentObject.NewPassword);
                View.ObjectSpace.CommitChanges();
            }
            else
            {
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.MINIMUM_LENGTH))
                    stringBuilder.AppendLine(string.Format("Password must be at least {0:#} characters long", passwordPolicy.min_length));
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.UPPER_CASE_LENGTH))
                    stringBuilder.AppendLine(string.Format("Password must have at least {0:#} UPPERCASE characters", passwordPolicy.min_uppercase));
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.LOWER_CASE_LENGTH))
                    stringBuilder.AppendLine(string.Format("Password must have at least {0:#} lowercase characters", passwordPolicy.min_lowercase));
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.SPECIAL_LENGTH))
                    stringBuilder.AppendLine(string.Format("Password must have at least {0:#} special characters", passwordPolicy.min_special));
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.NUMERIC_LENGTH))
                    stringBuilder.AppendLine(string.Format("Password must have at least {0:#} numeric characters 1234567890", passwordPolicy.min_digits));
                if (passwordPolicyResultList.Contains(PasswordPolicyResult.HISTORY))
                    stringBuilder.AppendLine(string.Format("Cannot use one of the last {0:#} passwords", passwordPolicy.history_size));
                throw new ArgumentException("Invalid Password " + Environment.NewLine + " " + stringBuilder.ToString());
            }
        }

        private void ResetUserPasswordAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            PasswordPolicy policy = ObjectSpace.GetObjectsQuery<PasswordPolicy>().FirstOrDefault();
            HandleUserPasswordReset(View.ObjectSpace.GetObject((ApplicationUser)View.CurrentObject), policy);
            ObjectSpace.CommitChanges();
            View.ObjectSpace.Refresh();
        }

        private void HandleUserPasswordReset(ApplicationUser user, PasswordPolicy policy)
        {
            string password = PasswordGenerator.Generate((uint)policy.min_length, (uint)policy.min_lowercase, (uint)policy.min_uppercase, (uint)policy.min_digits, (uint)policy.min_special);
            user.SetPassword(password);
            user.ChangePasswordOnFirstLogon = true;
            new EmailManager(ObjectSpace).SendPasswordResetEmail(user, password);
        }

        private void UnlockUserOnDevice_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            try
            {
                ApplicationUser initiatingUser = ObjectSpace.GetObject(SecuritySystem.CurrentUser as ApplicationUser);
                foreach (ApplicationUser selectedObject in (ArrayList)e.SelectedObjects)
                {
                    ApplicationUser applicationUser = ObjectSpace.GetObject(selectedObject);
                    if (!applicationUser.DepositorEnabled)
                        applicationUser.UnlockUserOnDepositor(true, initiatingUser);
                }
                ObjectSpace.CommitChanges();
                ObjectSpace.Refresh();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components =  new Container();
            LockUserSimpleAction = new SimpleAction(components);
            UnlockUserSimpleAction = new SimpleAction(components);
            DeleteUserSimpleAction = new SimpleAction(components);
            UnDeleteUserSimpleAction = new SimpleAction(components);
            ResetUserPasswordAction = new SimpleAction(components);
            UnlockUserOnDevice = new SimpleAction(components);
            LockUserSimpleAction.ActionMeaning = ActionMeaning.Accept;
            LockUserSimpleAction.Caption = "Lock";
            LockUserSimpleAction.ConfirmationMessage = "This operation will lock the selected user(s). Continue?";
            LockUserSimpleAction.Id = "8fec1dce-8fbe-40f2-837d-2ca8169080de";
            LockUserSimpleAction.ImageName = "locked";
            LockUserSimpleAction.TargetObjectsCriteria = "[id]!=CurrentUserId() && [UserDeleted]=false && ([IsActive]=true||[DepositorEnabled]=true) && IsAllowedByUserGroup([id])  && UserHasActivityPermission('USER_LOCK')";
            LockUserSimpleAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            LockUserSimpleAction.TargetObjectType = typeof(ApplicationUser);
            LockUserSimpleAction.TargetViewId = "ApplicationUser_ListView";
            LockUserSimpleAction.ToolTip = "Lock the selected user(s). Locked users cannot login and interact on the platform";
            LockUserSimpleAction.Execute += new SimpleActionExecuteEventHandler(LockUserSimpleAction_Execute);
            UnlockUserSimpleAction.ActionMeaning = ActionMeaning.Accept;
            UnlockUserSimpleAction.Caption = "Unlock";
            UnlockUserSimpleAction.ConfirmationMessage = "This operation will unlock the selected user(s). Continue?";
            UnlockUserSimpleAction.Id = "3d4144af-f755-4cc7-9b03-6b12f57f7688";
            UnlockUserSimpleAction.ImageName = "unlocked";
            UnlockUserSimpleAction.TargetObjectsCriteria = "[id]!=CurrentUserId() && [UserDeleted]=false && [IsActive]=false && IsAllowedByUserGroup([id])  && UserHasActivityPermission('USER_ALLOWPORTAL')";
            UnlockUserSimpleAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            UnlockUserSimpleAction.TargetObjectType = typeof(ApplicationUser);
            UnlockUserSimpleAction.TargetViewId = "ApplicationUser_ListView";
            UnlockUserSimpleAction.ToolTip = "Unlock the selected user(s). Selected Users will be able to login and interact on the platform";
            UnlockUserSimpleAction.Execute += new SimpleActionExecuteEventHandler(UnlockUserSimpleAction_Execute);
            DeleteUserSimpleAction.ActionMeaning = ActionMeaning.Accept;
            DeleteUserSimpleAction.Caption = "Disable User";
            DeleteUserSimpleAction.ConfirmationMessage = "This will delete the user(s). Continue?";
            DeleteUserSimpleAction.Id = "301af533-42fe-40a3-b6d2-5c44dfa55759";
            DeleteUserSimpleAction.ImageName = "disable_user";
            DeleteUserSimpleAction.TargetObjectsCriteria = "[id]!=CurrentUserId() && [UserDeleted]=false && IsAllowedByUserGroup([id])  && UserHasActivityPermission('USER_DELETE')";
            DeleteUserSimpleAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            DeleteUserSimpleAction.TargetObjectType = typeof(ApplicationUser);
            DeleteUserSimpleAction.TargetViewId = "ApplicationUser_ListView";
            DeleteUserSimpleAction.ToolTip = "This will delete the user(s). Deleted users cannot utilise the platform but will not be permanently deleted and can be restored.";
            DeleteUserSimpleAction.Execute += new SimpleActionExecuteEventHandler(DeleteUserSimpleAction_Execute);
            UnDeleteUserSimpleAction.ActionMeaning = ActionMeaning.Accept;
            UnDeleteUserSimpleAction.Caption = "Enable User";
            UnDeleteUserSimpleAction.ConfirmationMessage = "This will re-enabe the deleted user(s). Continue?";
            UnDeleteUserSimpleAction.Id = "0b895935-42ce-41f3-b456-d2400061c9eb";
            UnDeleteUserSimpleAction.ImageName = "enable_user";
            UnDeleteUserSimpleAction.TargetObjectsCriteria = "[id]!=CurrentUserId() && [UserDeleted]=true && IsAllowedByUserGroup([id])  && UserHasActivityPermission('USER_UNDELETE')";
            UnDeleteUserSimpleAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            UnDeleteUserSimpleAction.TargetObjectType = typeof(ApplicationUser);
            UnDeleteUserSimpleAction.TargetViewId = "ApplicationUser_ListView_Deleted";
            UnDeleteUserSimpleAction.ToolTip = "Re-enable deleted user(s).";
            UnDeleteUserSimpleAction.Execute += new SimpleActionExecuteEventHandler(UnDeleteUserSimpleAction_Execute);
            ResetUserPasswordAction.ActionMeaning = ActionMeaning.Accept;
            ResetUserPasswordAction.Caption = "Reset Password";
            ResetUserPasswordAction.Category = "Tools";
            ResetUserPasswordAction.ConfirmationMessage = "This action will reset the chosen user(s) passwords. Continue?";
            ResetUserPasswordAction.Id = "0bd4a283-0575-47cb-b576-9d66156a0722";
            ResetUserPasswordAction.ImageName = "Action_ResetPassword";
            ResetUserPasswordAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            ResetUserPasswordAction.TargetObjectsCriteria = "[id]!=CurrentUserId() && [UserDeleted]=false && IsAllowedByUserGroup([id])  && UserHasActivityPermission('USER_PWRESET')  && [IsActiveDirectoryUser]=false && (!IsNullOrEmpty([email]))";
            ResetUserPasswordAction.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            ResetUserPasswordAction.TargetObjectType = typeof(ApplicationUser);
            ResetUserPasswordAction.TargetViewId = "";
            ResetUserPasswordAction.TargetViewNesting = Nesting.Root;
            ResetUserPasswordAction.TargetViewType = ViewType.ListView;
            ResetUserPasswordAction.ToolTip = "Reset user password";
            ResetUserPasswordAction.TypeOfView = typeof(ListView);
            ResetUserPasswordAction.Execute += new SimpleActionExecuteEventHandler(ResetUserPasswordAction_Execute);
            UnlockUserOnDevice.ActionMeaning = ActionMeaning.Accept;
            UnlockUserOnDevice.Caption = "UnlockOnDevice";
            UnlockUserOnDevice.ConfirmationMessage = "This operation will unlock the selected user(s). Continue?";
            UnlockUserOnDevice.Id = "1C26DD16-A235-4DFC-831F-80D4AF4E18B3";
            UnlockUserOnDevice.ImageName = "unlocked_device";
            UnlockUserOnDevice.TargetObjectsCriteria = "[id]!=CurrentUserId() && [UserDeleted]=false && [DepositorEnabled]=false && IsAllowedByUserGroup([id])  && UserHasActivityPermission('USER_ALLOWDEVICE')";
            UnlockUserOnDevice.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueForAll;
            UnlockUserOnDevice.TargetObjectType = typeof(ApplicationUser);
            UnlockUserOnDevice.TargetViewId = "ApplicationUser_ListView";
            UnlockUserOnDevice.ToolTip = "Unlock the selected user(s) on the device. Selected Users will be able to login and interact on the device";
            UnlockUserOnDevice.Execute += new SimpleActionExecuteEventHandler(UnlockUserOnDevice_Execute);
            Actions.Add(LockUserSimpleAction);
            Actions.Add(UnlockUserSimpleAction);
            Actions.Add(DeleteUserSimpleAction);
            Actions.Add(UnDeleteUserSimpleAction);
            Actions.Add(ResetUserPasswordAction);
            Actions.Add(UnlockUserOnDevice);
            TargetObjectType = typeof(ApplicationUser);
        }
    }
}
