
//BusinessObjects.Authentication.XAF.ApplicationUserChangePassword


using CashSwift.Library.Standard.Security;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF
{
    [RuleCriteria("PasswordConfirmPassword", "ChangePassword", "ConfirmPassword == NewPassword", "Confirm Password and New Password should be equal")]
    public class ApplicationUserChangePassword : BaseObject
    {
        public ApplicationUserChangePassword(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        protected override void OnSaving()
        {
            base.OnSaving();
            OldPassword =  null;
            NewPassword =  null;
            ConfirmPassword =  null;
        }

        [RuleRequiredField("RuleRequiredField for OldPassword", "ChangePassword", "Old Password must be specified")]
        [PasswordPropertyText(true)]
        public string OldPassword { get; set; }

        [RuleRequiredField("RuleRequiredField for NewPassword", "ChangePassword", "New Password must be specified")]
        [PasswordPropertyText(true)]
        public string NewPassword { get; set; }

        [RuleRequiredField("RuleRequiredField for ConfirmPassword", "ChangePassword", "Confirm Password must be specified")]
        [PasswordPropertyText(true)]
        public string ConfirmPassword { get; set; }

        [Browsable(false)]
        public ApplicationUser User { get; set; }

        [Browsable(false)]
        public PasswordPolicy PasswordPolicy { get; set; }

        [Browsable(false)]
        public IList<PasswordHistory> PasswordHistory { get; set; }

        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("PasswordMinimumLength", "ChangePassword", "The password must be at least {TargetObject.PasswordPolicy.min_length} character(s) long.", UsedProperties = "NewPassword")]
        public bool passMinimumLength
        {
            get
            {
                int? length = NewPassword?.Length;
                int minLength = PasswordPolicy.min_length;
                return length.GetValueOrDefault() >= minLength & length.HasValue;
            }
        }

        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("PasswordMinimumLowercase", "ChangePassword", "The password must have at least {TargetObject.PasswordPolicy.min_lowercase} lowercase character(s) [a-z].", UsedProperties = "NewPassword")]
        public bool passMinimumLowercase
        {
            get
            {
                try
                {
                    return PasswordPolicyManager.LowerCaseCount(NewPassword) >= PasswordPolicy.min_lowercase;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("PasswordMinimumDigits", "ChangePassword", "The password must have at least {TargetObject.PasswordPolicy.min_digits} numeric digit(s) [0-9].", UsedProperties = "NewPassword")]
        public bool passMinimumDigits => PasswordPolicyManager.NumericCount(NewPassword) >= PasswordPolicy.min_digits;

        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("PasswordMinimumUppercase", "ChangePassword", "The password must have at least {TargetObject.PasswordPolicy.min_uppercase} UPPERCASE character(s) [A-Z].", UsedProperties = "NewPassword")]
        public bool passMinimumUppercase => PasswordPolicyManager.UpperCaseCount(NewPassword) >= PasswordPolicy.min_uppercase;

        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("PasswordMinimumSpecial", "ChangePassword", "The password must have at least {TargetObject.PasswordPolicy.min_special} special character(s).", UsedProperties = "NewPassword")]
        public bool passMinimumSpecial => PasswordPolicyManager.NonAlphaCount(NewPassword) >= PasswordPolicy.min_special;

        [NonPersistent]
        [Browsable(false)]
        [RuleFromBoolProperty("PasswordHistory", "ChangePassword", "The new password cannot be one of the last {TargetObject.PasswordPolicy.history_size} password(s) you have used.", UsedProperties = "NewPassword")]
        public bool passHistory
        {
            get
            {
                if (PasswordPolicy.use_history)
                {
                    foreach (PasswordHistory passwordHistory in (IEnumerable<PasswordHistory>)PasswordHistory)
                    {
                        if (PasswordStorage.VerifyPassword(NewPassword, passwordHistory.Password))
                            return false;
                    }
                }
                return true;
            }
        }
    }
}
