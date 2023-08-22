
//BusinessObjects.Validation.ValidationText


using CashSwiftCashControlPortal.Module.BusinessObjects.Translations;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Validation
{
    [FriendlyKeyProperty("ErrorMessage")]
    [DefaultProperty("ValidationItem.name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class ValidationText : XPLiteObject
    {
        private Guid fid;
        private ValidationItem fvalidation_item_id;
        private UserTextItem ferror_message;
        private UserTextItem fsuccess_message;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Browsable(false)]
        [Persistent("validation_item_id")]
        public ValidationItem ValidationItem
        {
            get => fvalidation_item_id;
            set
            {
                if (fvalidation_item_id == value)
                    return;
                ValidationItem fvalidationItemId = fvalidation_item_id;
                fvalidation_item_id = value;
                if (IsLoading)
                    return;
                if (fvalidationItemId != null && fvalidationItemId.ValidationText == this)
                    fvalidationItemId.ValidationText =  null;
                if (fvalidation_item_id != null)
                    fvalidation_item_id.ValidationText = this;
                OnChanged(nameof(ValidationItem));
            }
        }

        [Association("ValidationText_ErrorMessageReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_ValidationText.error_message'")]
        [Persistent("error_message")]
        public UserTextItem ErrorMessage
        {
            get => ferror_message;
            set => SetPropertyValue(nameof(ErrorMessage), ref ferror_message, value);
        }

        [Association("ValidationText_SuccessMessageReferencesUserTextItem")]
        [DataSourceCriteria("[TextItemTypeID.token] = 'sys_ValidationText.success_message'")]
        [Persistent("success_message")]
        public UserTextItem SuccessMessage
        {
            get => fsuccess_message;
            set => SetPropertyValue(nameof(SuccessMessage), ref fsuccess_message, value);
        }

        public ValidationText(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
