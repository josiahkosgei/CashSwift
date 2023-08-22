
//BusinessObjects.CoreBanking.AccountPermission


using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using CashSwiftCashControlPortal.Module.BusinessObjects.Translations;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.CoreBanking
{
    [FriendlyKeyProperty("tx_type.name")]
    [DefaultProperty("tx_type.name")]
    [Persistent("cb.AccountPermission")]
    public class AccountPermission : XPLiteObject
    {
        private Guid fid;
        private TransactionTypeListItem ftx_type;
        private AccountPermissionListType flist_type;
        private UserTextItem ferror_message;
        private bool fenabled;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Indexed(Name = "UX_AccountPermission_tx_type", Unique = true)]
        [Persistent("tx_type")]
        [Browsable(false)]
        public TransactionTypeListItem tx_type
        {
            get => ftx_type;
            set
            {
                if (ftx_type == value)
                    return;
                TransactionTypeListItem ftxType = ftx_type;
                ftx_type = value;
                if (IsLoading)
                    return;
                if (ftxType != null && ftxType.AccountPermission == this)
                    ftxType.AccountPermission =  null;
                if (ftx_type != null)
                    ftx_type.AccountPermission = this;
                OnChanged("TransactionTypeListItem");
            }
        }

        public AccountPermissionListType list_type
        {
            get => flist_type;
            set => SetPropertyValue(nameof(list_type), ref flist_type, value);
        }

        [NoForeignKey]
        public UserTextItem error_message
        {
            get => ferror_message;
            set => SetPropertyValue(nameof(error_message), ref ferror_message, value);
        }

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        [Association("AccountPermissionItemReferencesAccountPermission")]
        public XPCollection<AccountPermissionItem> AccountPermissionItems => GetCollection<AccountPermissionItem>(nameof(AccountPermissionItems));

        [ManyToManyAlias("AccountPermissionItems", "account")]
        public IList<Account> Accounts => GetList<Account>(nameof(Accounts));

        public AccountPermission(Session session)
          : base(session)
        {
        }
    }
}
