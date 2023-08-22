using CashSwift.API.Messaging.Authentication;
using CashSwift.API.Messaging.Authentication.Clients;
using CashSwift.Library.Standard.Security;
using CashSwift.Library.Standard.Utilities;
using CashSwiftDataAccess.Data;
using CashSwiftDataAccess.Entities;
using CashSwiftDeposit.ViewModels.RearScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashSwiftDeposit.ViewModels
{
    internal class UserChangePasswordFormViewModel : FormViewModelBase
    {
        public bool IsAuthorise = false;

        private ApplicationUser User { get; set; }

        private DepositorDBContext DBContext { get; set; }

        private string OldPassword { get; set; }

        private string NewPassword { get; set; }

        private string ConfirmPassword { get; set; }

        private object NextObject { get; set; }

        private UserLoginViewModel.LoginSuccessCallBack LoginSuccessCallBackDelegate { get; set; }

        public UserChangePasswordFormViewModel(
          ApplicationViewModel applicationViewModel,
          ApplicationUser user,
          DepositorDBContext dbcontext,
          ICashSwiftWindowConductor conductor,
          object callingObject,
          object nextObject,
          bool isAuthorise = false,
          UserLoginViewModel.LoginSuccessCallBack loginSuccessCallBack = null)
          : base(applicationViewModel, conductor, callingObject, true)
        {
            if (dbcontext == null)
            {
                DBContext = new DepositorDBContext();
                User = DBContext.ApplicationUsers.FirstOrDefault(x => x.id == user.id);
            }
            else
            {
                DBContext = dbcontext;
                User = user;
            }
            NextObject = nextObject;
            if (!(Application.Current.FindResource("UserChangePasswordFormScreenTitle") is string str1))
                str1 = "Change Password";
            ScreenTitle = str1;
            ScreenTitle = ScreenTitle + ": " + User.username;
            IsAuthorise = isAuthorise;
            LoginSuccessCallBackDelegate = loginSuccessCallBack;
            List<FormListItem> fields1 = Fields;
            FormListItem formListItem1 = new FormListItem();
            FormListItem formListItem2 = formListItem1;
            if (!(Application.Current.FindResource("User_OldPassword_Caption") is string str2))
                str2 = "Old Password";
            formListItem2.DataLabel = str2;
            formListItem1.Validate = new Func<string, string>(ValidateOldPassword);
            formListItem1.ValidatedText = OldPassword;
            formListItem1.DataTextBoxLabel = OldPassword;
            formListItem1.FormListItemType = FormListItemType.ALPHAPASSWORD;
            FormListItem formListItem3 = formListItem1;
            fields1.Add(formListItem3);
            List<FormListItem> fields2 = Fields;
            FormListItem formListItem4 = new FormListItem();
            FormListItem formListItem5 = formListItem4;
            if (!(Application.Current.FindResource("User_NewPassword_Caption") is string str3))
                str3 = "New Password";
            formListItem5.DataLabel = str3;
            formListItem4.Validate = new Func<string, string>(ValidateNewPassword);
            formListItem4.ValidatedText = NewPassword;
            formListItem4.DataTextBoxLabel = NewPassword;
            formListItem4.FormListItemType = FormListItemType.ALPHAPASSWORD;
            FormListItem formListItem6 = formListItem4;
            fields2.Add(formListItem6);
            List<FormListItem> fields3 = Fields;
            FormListItem formListItem7 = new FormListItem();
            FormListItem formListItem8 = formListItem7;
            if (!(Application.Current.FindResource("User_ConfirmPassword_Caption") is string str4))
                str4 = "Confirm Password";
            formListItem8.DataLabel = str4;
            formListItem7.Validate = new Func<string, string>(ValidateConfirmPassword);
            formListItem7.ValidatedText = ConfirmPassword;
            formListItem7.DataTextBoxLabel = ConfirmPassword;
            formListItem7.FormListItemType = FormListItemType.ALPHAPASSWORD;
            FormListItem formListItem9 = formListItem7;
            fields3.Add(formListItem9);
            ActivateItemAsync(new FormListViewModel(this));
        }

        public string ValidateOldPassword(string oldPassword)
        {
            if (string.IsNullOrWhiteSpace(oldPassword))
                return "Current Password Cannot be blank";
            OldPassword = oldPassword;
            return null;
        }

        public string ValidateNewPassword(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                return "New Password Cannot be blank";
            NewPassword = newPassword;
            return null;
        }

        public string ValidateConfirmPassword(string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(confirmPassword))
                return "Confirm Password Cannot be blank";
            ConfirmPassword = confirmPassword;
            return null;
        }

        public override string SaveForm()
        {
            int num = FormValidation();
            if (num > 0)
            {
                FormErrorText = string.Format("Form validation failed with {0} errors. Kindly correct them and try again", num);
                return FormErrorText;
            }
            if (ValidatePassword() == 0)
                return null;
            NewPassword = null;
            OldPassword = null;
            ConfirmPassword = null;
            foreach (FormListItem field in Fields)
            {
                field.ValidatedText = null;
                field.DataTextBoxLabel = null;
            }
            return FormErrorText;
        }

        public override int FormValidation()
        {
            int num = 0;
            foreach (FormListItem field in Fields)
            {
                field.ErrorMessageTextBlock = null;
                Func<string, string> validate = field.Validate;
                string str = validate != null ? validate((field.FormListItemType & FormListItemType.PASSWORD) > FormListItemType.NONE ? field.DataTextBoxLabel : field.ValidatedText) : null;
                if (str != null)
                {
                    field.ErrorMessageTextBlock = str;
                    ++num;
                }
            }
            return num;
        }

        private int ValidatePassword()
        {
            try
            {
                if (!ApplicationViewModel.DeviceConfiguration.ALLOW_OFFLINE_AUTH)
                {
                    Device device = ApplicationViewModel.GetDevice(DBContext);
                    ChangePasswordRequest changePasswordRequest = new ChangePasswordRequest
                    {
                        AppID = device.app_id,
                        AppName = device.machine_name,
                        SessionID = Guid.NewGuid().ToString(),
                        DeviceID = device.id,
                        Language = ApplicationViewModel.CurrentLanguage,
                        MessageID = Guid.NewGuid().ToString(),
                        MessageDateTime = DateTime.Now,
                        UserId = User.id,
                        Username = User.username,
                        OldPassword = OldPassword.Encrypt(device.app_key),
                        NewPassword = NewPassword.Encrypt(device.app_key),
                        ConfirmPassword = ConfirmPassword.Encrypt(device.app_key)
                    };
                    ChangePasswordRequest request = changePasswordRequest;
                    AuthenticationServiceClient client = new AuthenticationServiceClient(ApplicationViewModel.DeviceConfiguration.API_AUTH_API_URI, device.app_id, device.app_key, null);
                    ApplicationViewModel.Log.InfoFormat("UserLoginViewModel", nameof(ValidatePassword), "API", "Sending request {0}", request);
                    ChangePasswordResponse result = Task.Run(() => client.ChangePasswordAsync(request)).Result;
                    ApplicationViewModel.Log.DebugFormat("UserLoginViewModel", nameof(ValidatePassword), "API", "Received response {0}", result);
                    if (result.IsSuccess)
                        ApplicationViewModel.Log.InfoFormat("UserLoginViewModel", nameof(ValidatePassword), "SUCCESS", "Change password SUCCESS for request {0}, User {1}", request.MessageID, User.username);
                    else
                        ApplicationViewModel.Log.WarningFormat("UserLoginViewModel", nameof(ValidatePassword), "FAIL", "Change password FAIL for request {0}, User {1}: {2}>{3}", request.MessageID, User.username, result.PublicErrorMessage, result.ServerErrorMessage);
                    ApplicationViewModel.SaveToDatabase(DBContext);
                    FormErrorText = result.PublicErrorMessage;
                    return result.IsSuccess ? 0 : 1;
                }
                PasswordPolicy passwordPolicy = DBContext.PasswordPolicies.FirstOrDefault();
                if (passwordPolicy == null)
                {
                    FormErrorText = "error, no password policy defined";
                    return 1;
                }
                PasswordPolicyItems Policy = new PasswordPolicyItems()
                {
                    HistorySize = passwordPolicy.history_size,
                    Lower_Case_length = passwordPolicy.min_lowercase,
                    Minimum_Length = passwordPolicy.min_length,
                    Numeric_length = passwordPolicy.min_digits,
                    Special_length = passwordPolicy.min_special,
                    Upper_Case_length = passwordPolicy.min_uppercase
                };
                IList<PasswordHistory> list = DBContext.PasswordHistories.Where(x => x.User == (Guid?)User.id).OrderByDescending(x => x.LogDate).Take(passwordPolicy.history_size).ToList();
                if (!string.IsNullOrWhiteSpace(OldPassword) && !string.IsNullOrWhiteSpace(NewPassword) && !string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    if (PasswordStorage.VerifyPassword(OldPassword, User.password))
                    {
                        if (NewPassword.ToUpper() == ConfirmPassword.ToUpper())
                        {
                            IList<PasswordPolicyResult> passwordPolicyResultList = PasswordPolicyManager.Validate(NewPassword, Policy);
                            if (passwordPolicyResultList == null)
                            {
                                if ((bool)passwordPolicy.use_history && list != null)
                                {
                                    foreach (PasswordHistory passwordHistory in (IEnumerable<PasswordHistory>)list)
                                    {
                                        if (PasswordStorage.VerifyPassword(NewPassword, passwordHistory.Password))
                                        {
                                            FormErrorText = string.Format("Invalid: Cannot use the last {0} passwords", passwordPolicy.history_size);
                                            return 1;
                                        }
                                    }
                                }
                                string hash = PasswordStorage.CreateHash(NewPassword);
                                User.password = hash;
                                User.password_reset_required = false;
                                User.PasswordHistories.Add(new PasswordHistory()
                                {
                                    LogDate = new DateTime?(DateTime.Now),
                                    id = Guid.NewGuid(),
                                    Password = hash
                                });
                                ApplicationViewModel.SaveToDatabase(DBContext);
                                return 0;
                            }
                            StringBuilder stringBuilder = new StringBuilder();
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.MINIMUM_LENGTH))
                                stringBuilder.AppendLine(string.Format("Password must be at least {0:#} characters long", passwordPolicy.min_length));
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.UPPER_CASE_LENGTH))
                                stringBuilder.AppendLine(string.Format("Password must have at least {0:#} UPPERCASE characters", passwordPolicy.min_uppercase));
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.LOWER_CASE_LENGTH))
                                stringBuilder.AppendLine(string.Format("Password must have at least {0:#} lowercase characters", passwordPolicy.min_lowercase));
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.SPECIAL_LENGTH))
                                stringBuilder.AppendLine(string.Format("Password must have at least {0:#} special characters from {1}", passwordPolicy.min_special, passwordPolicy.allowed_special));
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.NUMERIC_LENGTH))
                                stringBuilder.AppendLine(string.Format("Password must have at least {0:#} numeric characters 1234567890", passwordPolicy.min_digits));
                            if (passwordPolicyResultList.Contains(PasswordPolicyResult.HISTORY))
                                stringBuilder.AppendLine(string.Format("Cannot use one of the last {0:#} passwords", passwordPolicy.history_size));
                            FormErrorText = stringBuilder.ToString();
                        }
                        else
                        {
                            FormErrorText = "Invalid Confirm Password";
                            return 1;
                        }
                    }
                    else
                    {
                        FormErrorText = "Current Password is incorrect";
                        return 1;
                    }
                }
                else
                    FormErrorText = "Empty fields detected";
            }
            catch (Exception ex)
            {
                ApplicationViewModel.Log.Error(nameof(UserChangePasswordFormViewModel), "Error", nameof(ValidatePassword), ex.MessageString(), Array.Empty<object>());
                FormErrorText = "Error. Contact Administrator.";
                return 1;
            }
            return 1;
        }

        public override void FormClose(bool success)
        {
            if (success)
            {
                Conductor.ShowDialog(NextObject);
                UserLoginViewModel.LoginSuccessCallBack callBackDelegate = LoginSuccessCallBackDelegate;
                if (callBackDelegate == null)
                    return;
                callBackDelegate(User, IsAuthorise);
            }
            else
                Conductor.ShowDialog(CallingObject);
        }
    }
}
