
//BusinessObjects.Transactions.TransactionTypeListItem


using CashSwift.Library.Standard.Statuses;
using CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration;
using CashSwiftCashControlPortal.Module.BusinessObjects.CoreBanking;
using CashSwiftCashControlPortal.Module.BusinessObjects.Screens;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Drawing;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Transactions
{
    [NavigationItem("Application Management")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    [VisibleInReports]
    [VisibleInDashboards]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class TransactionTypeListItem : XPLiteObject
    {
        private int fid;
        private string fname;
        private string fdescription;
        private bool fvalidate_reference_account;
        private string fdefault_account;
        private string fdefault_account_name;
        private Currency fdefault_account_currency;
        private bool fvalidate_default_account;
        private bool fenabled;
        private TransactionType ftx_type;
        private GUIScreenList ftx_type_guiscreenlist;
        private string fcb_tx_type;
        private const int IconMaxWidth = 512;
        private const int IconMaxHeight = 512;
        private int fIcon;
        private TransactionText ftx_text;
        private TransactionLimitList ftx_limit_list;
        private AccountPermission faccount_permission;
        private bool finit_user_required;
        private bool fauth_user_required;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public int id
        {
            get => fid;
            set => SetPropertyValue<int>(nameof(id), ref fid, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The name was already registered within the system.")]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [Size(50)]
        [DisplayName("Name")]
        [Persistent("name")]
        public string name
        {
            get => fname;
            set => SetPropertyValue(nameof(name), ref fname, value);
        }

        [Size(255)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [DisplayName("Description")]
        public string description
        {
            get => fdescription;
            set => SetPropertyValue(nameof(description), ref fdescription, value);
        }

        public bool validate_reference_account
        {
            get => fvalidate_reference_account;
            set => SetPropertyValue(nameof(validate_reference_account), ref fvalidate_reference_account, value);
        }

        [Size(50)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        public string default_account
        {
            get => fdefault_account;
            set => SetPropertyValue(nameof(default_account), ref fdefault_account, value);
        }

        [Size(50)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        public string default_account_name
        {
            get => fdefault_account_name;
            set => SetPropertyValue(nameof(default_account_name), ref fdefault_account_name, value);
        }

        [Size(3)]
        [RuleRequiredField(DefaultContexts.Save)]
        [Association("TransactionTypeListItemReferencesCurrency")]
        public Currency default_account_currency
        {
            get => fdefault_account_currency;
            set => SetPropertyValue(nameof(default_account_currency), ref fdefault_account_currency, value);
        }

        public bool validate_default_account
        {
            get => fvalidate_default_account;
            set => SetPropertyValue(nameof(validate_default_account), ref fvalidate_default_account, value);
        }

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        [Association("TransactionTypeListItemReferencesTransactionType")]
        [RuleRequiredField(DefaultContexts.Save)]
        public TransactionType tx_type
        {
            get => ftx_type;
            set => SetPropertyValue(nameof(tx_type), ref ftx_type, value);
        }

        [Association("TransactionTypeListItemReferencesGUIScreenList")]
        [RuleRequiredField(DefaultContexts.Save)]
        public GUIScreenList tx_type_guiscreenlist
        {
            get => ftx_type_guiscreenlist;
            set => SetPropertyValue(nameof(tx_type_guiscreenlist), ref ftx_type_guiscreenlist, value);
        }

        [Size(50)]
        [RuleRequiredField(DefaultContexts.Save)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        public string cb_tx_type
        {
            get => fcb_tx_type;
            set => SetPropertyValue(nameof(cb_tx_type), ref fcb_tx_type, value);
        }

        [Size(-1)]
        [ImageEditor(DetailViewImageEditorFixedHeight = 100, DetailViewImageEditorFixedWidth = 200, DetailViewImageEditorMode = ImageEditorMode.PictureEdit, ImageSizeMode = ImageSizeMode.Zoom, ListViewImageEditorCustomHeight = 35, ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit)]
        [ValueConverter(typeof(ImageValueConverter))]
        public Image Icon
        {
            get => GetPropertyValue<Image>(nameof(Icon));
            set
            {
                if (value != null && value.Width > 512 || value != null && value.Height > 512)
                    throw new CashSwiftException(string.Format("Image too large at {0:0}px W x {1:0}px H. Maximum size is {2:0}px W x {3}px H.", value?.Width, value?.Height, 512, 512));
                SetPropertyValue(nameof(Icon), value);
            }
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Persistent("tx_text")]
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public TransactionText TransactionText
        {
            get => ftx_text;
            set
            {
                if (ftx_text == value)
                    return;
                TransactionText ftxText = ftx_text;
                ftx_text = value;
                if (IsLoading)
                    return;
                if (ftxText != null && ftxText.TransactionTypeListItem == this)
                    ftxText.TransactionTypeListItem =  null;
                if (ftx_text != null)
                    ftx_text.TransactionTypeListItem = this;
                OnChanged(nameof(TransactionText));
            }
        }

        [Association("TransactionTypeListItemReferencesTransactionLimitList")]
        [RuleRequiredField(DefaultContexts.Save)]
        public TransactionLimitList tx_limit_list
        {
            get => ftx_limit_list;
            set => SetPropertyValue(nameof(tx_limit_list), ref ftx_limit_list, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Persistent("account_permission")]
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public AccountPermission AccountPermission
        {
            get => faccount_permission;
            set
            {
                if (faccount_permission == value)
                    return;
                AccountPermission faccountPermission = faccount_permission;
                faccount_permission = value;
                if (IsLoading)
                    return;
                if (faccountPermission != null && faccountPermission.tx_type == this)
                    faccountPermission.tx_type =  null;
                if (faccount_permission != null)
                    faccount_permission.tx_type = this;
                OnChanged(nameof(AccountPermission));
            }
        }

        public bool init_user_required
        {
            get => finit_user_required;
            set => SetPropertyValue(nameof(init_user_required), ref finit_user_required, value);
        }

        public bool auth_user_required
        {
            get => fauth_user_required;
            set => SetPropertyValue(nameof(auth_user_required), ref fauth_user_required, value);
        }

        [Association("TransactionReferencesTransactionTypeListItem")]
        public XPCollection<Transaction> Transactions => GetCollection<Transaction>(nameof(Transactions));

        [Association("TransactionTypeList_TransactionTypeListItemReferencesTransactionTypeListItem")]
        public XPCollection<TransactionTypeList_TransactionTypeListItem> TransactionTypeList_TransactionTypeListItems => GetCollection<TransactionTypeList_TransactionTypeListItem>(nameof(TransactionTypeList_TransactionTypeListItems));

        public TransactionTypeListItem(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            TransactionText = new TransactionText(Session);
            AccountPermission = new AccountPermission(Session);
        }
    }
}
