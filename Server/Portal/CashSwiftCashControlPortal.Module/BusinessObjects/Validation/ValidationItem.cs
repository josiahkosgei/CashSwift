
//BusinessObjects.Validation.ValidationItem


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Validation
{
    [NavigationItem("Application Management")]
    [FriendlyKeyProperty("description")]
    [DefaultProperty("name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class ValidationItem : XPLiteObject
    {
        private Guid fid;
        private string fname;
        private string fdescription;
        private string fcategory;
        private ValidationType ftype_id;
        private bool fenabled;
        private ValidationText fvalidation_text_id;

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

        [Size(10)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        public string category
        {
            get => fcategory;
            set => SetPropertyValue(nameof(category), ref fcategory, value);
        }

        [Association("ValidationItemReferencesValidationType")]
        [RuleRequiredField(DefaultContexts.Save)]
        public ValidationType type_id
        {
            get => ftype_id;
            set => SetPropertyValue(nameof(type_id), ref ftype_id, value);
        }

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Persistent("validation_text_id")]
        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.InDetailView)]
        public ValidationText ValidationText
        {
            get => fvalidation_text_id;
            set
            {
                if (fvalidation_text_id == value)
                    return;
                ValidationText fvalidationTextId = fvalidation_text_id;
                fvalidation_text_id = value;
                if (IsLoading)
                    return;
                if (fvalidationTextId != null && fvalidationTextId.ValidationItem == this)
                    fvalidationTextId.ValidationItem =  null;
                if (fvalidation_text_id != null)
                    fvalidation_text_id.ValidationItem = this;
                OnChanged(nameof(ValidationText));
            }
        }

        [Association("ValidationItemValueReferencesValidationItem")]
        public XPCollection<ValidationItemValue> ValidationItemValues => GetCollection<ValidationItemValue>(nameof(ValidationItemValues));

        [Association("ValidationList_ValidationItemReferencesValidationItem")]
        public XPCollection<ValidationList_ValidationItem> ValidationList_ValidationItems => GetCollection<ValidationList_ValidationItem>(nameof(ValidationList_ValidationItems));

        public ValidationItem(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            ValidationText = new ValidationText(Session);
        }
    }
}
