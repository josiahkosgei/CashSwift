
//BusinessObjects.CoreBanking.Account


using CashSwift.Library.Standard.Statuses;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Drawing;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.CoreBanking
{
    [NavigationItem("Account Management")]
    [FriendlyKeyProperty("account_number")]
    [DefaultProperty("account_name")]
    [Persistent("cb.Account")]
    public class Account : XPLiteObject
    {
        private Guid fid;
        private const int IconMaxWidth = 512;
        private const int IconMaxHeight = 512;
        private int fIcon;
        private ApplicationConfiguration.Currency fcurrency;
        private string faccount_number;
        private string faccount_name;
        private bool fenabled;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
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
                    throw new Exception(string.Format("Image too large at {0:0}px W x {1:0}px H. Maximum size is {2:0}px W x {3}px H.", value?.Width, value?.Height, 512, 512));
                SetPropertyValue(nameof(Icon), value);
            }
        }

        [Size(3)]
        [NoForeignKey]
        [RuleRequiredField(DefaultContexts.Save)]
        public ApplicationConfiguration.Currency currency
        {
            get => fcurrency;
            set => SetPropertyValue(nameof(currency), ref fcurrency, value);
        }

        [Size(50)]
        [Indexed(Name = "UX_account_number", Unique = true)]
        public string account_number
        {
            get => faccount_number;
            set => SetPropertyValue(nameof(account_number), ref faccount_number, value);
        }

        [Size(50)]
        public string account_name
        {
            get => faccount_name;
            set => SetPropertyValue(nameof(account_name), ref faccount_name, value);
        }

        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        [Association("AccountPermissionItemReferencesAccount")]
        public XPCollection<AccountPermissionItem> AccountPermissionItems => GetCollection<AccountPermissionItem>(nameof(AccountPermissionItems));

        [ManyToManyAlias("AccountPermissionItems", "account_permission")]
        public IList<AccountPermission> AccountPermissions => GetList<AccountPermission>(nameof(AccountPermissions));

        public Account(Session session)
          : base(session)
        {
        }
    }
}
