
//BusinessObjects.CoreBanking.AccountPermissionItem


using CashSwiftCashControlPortal.Module.BusinessObjects.Translations;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.CoreBanking
{
    [FriendlyKeyProperty("account")]
    [DefaultProperty("account_permission")]
    [Persistent("cb.AccountPermissionItem")]
    public class AccountPermissionItem : XPLiteObject
    {
        private Guid fid;
        private AccountPermission faccount_permission;
        private Account faccount;
        private UserTextItem ferror_message;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("AccountPermissionItemReferencesAccountPermission")]
        public AccountPermission account_permission
        {
            get => faccount_permission;
            set => SetPropertyValue(nameof(account_permission), ref faccount_permission, value);
        }

        [Association("AccountPermissionItemReferencesAccount")]
        public Account account
        {
            get => faccount;
            set => SetPropertyValue(nameof(account), ref faccount, value);
        }

        [NoForeignKey]
        public UserTextItem error_message
        {
            get => ferror_message;
            set => SetPropertyValue(nameof(error_message), ref ferror_message, value);
        }

        public AccountPermissionItem(Session session)
          : base(session)
        {
        }
    }
}
