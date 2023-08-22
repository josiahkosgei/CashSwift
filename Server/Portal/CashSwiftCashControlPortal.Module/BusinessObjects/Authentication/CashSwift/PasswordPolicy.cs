
//BusinessObjects.Authentication..PasswordPolicy


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [NavigationItem("User Management")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class PasswordPolicy : XPLiteObject
    {
        private Guid fid;
        private int fmin_length;
        private int fmin_lowercase;
        private int fmin_digits;
        private int fmin_uppercase;
        private int fmin_special;
        private string fallowed_special;
        private int fexpiry_days;
        private int fhistory_size;
        private bool fuse_history;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public int min_length
        {
            get => fmin_length;
            set => SetPropertyValue<int>(nameof(min_length), ref fmin_length, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public int min_lowercase
        {
            get => fmin_lowercase;
            set => SetPropertyValue<int>(nameof(min_lowercase), ref fmin_lowercase, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public int min_digits
        {
            get => fmin_digits;
            set => SetPropertyValue<int>(nameof(min_digits), ref fmin_digits, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public int min_uppercase
        {
            get => fmin_uppercase;
            set => SetPropertyValue<int>(nameof(min_uppercase), ref fmin_uppercase, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public int min_special
        {
            get => fmin_special;
            set => SetPropertyValue<int>(nameof(min_special), ref fmin_special, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public string allowed_special
        {
            get => fallowed_special;
            set => SetPropertyValue(nameof(allowed_special), ref fallowed_special, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public int expiry_days
        {
            get => fexpiry_days;
            set => SetPropertyValue<int>(nameof(expiry_days), ref fexpiry_days, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public int history_size
        {
            get => fhistory_size;
            set => SetPropertyValue<int>(nameof(history_size), ref fhistory_size, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public bool use_history
        {
            get => fuse_history;
            set => SetPropertyValue(nameof(use_history), ref fuse_history, value);
        }

        public PasswordPolicy(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
