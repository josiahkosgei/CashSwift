
//BusinessObjects.Translations.UserTextItemCategory


using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;
using System.Drawing;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Translations
{
    [NavigationItem("Language")]
    [FriendlyKeyProperty("name")]
    [DefaultProperty("name")]
    [Persistent("xlns.TextItemCategory")]
    public class UserTextItemCategory : XPLiteObject, ITreeNode, ITreeNodeImageProvider
    {
        private Guid fid;
        private string fname;
        private string fdescription;
        private UserTextItemCategory fParent;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
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

        [Persistent("Parent")]
        [Association("UserTextItemCategoryReferencesUserTextItemCategory")]
        [Browsable(false)]
        public UserTextItemCategory ParentID
        {
            get => fParent;
            set => SetPropertyValue(nameof(ParentID), ref fParent, value);
        }

        [Association("UserTextItemCategoryReferencesUserTextItemCategory")]
        public XPCollection<UserTextItemCategory> UserTextItemCategoryCollection => GetCollection<UserTextItemCategory>(nameof(UserTextItemCategoryCollection));

        [Association("UserTextItemReferencesUserTextItemCategory")]
        public XPCollection<UserTextItem> UserTextItems => GetCollection<UserTextItem>(nameof(UserTextItems));

        public UserTextItemCategory(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        public IBindingList Children => UserTextItemCategoryCollection;

        public string Name => name;

        public ITreeNode Parent => ParentID;

        public Image GetImage(out string imageName)
        {
            imageName = Children == null || Children.Count <= 0 ? "ModelEditor_Settings" : "BO_Category";
            return ImageLoader.Instance.GetImageInfo(imageName).Image;
        }
    }
}
