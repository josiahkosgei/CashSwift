
//BusinessObjects.Validation.ValidationItemValue


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
    [FriendlyKeyProperty("value1")]
    [DefaultProperty("validation_item_id.name")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class ValidationItemValue : XPLiteObject
    {
        private Guid fid;
        private ValidationItem fvalidation_item_id;
        private string fvalue1;
        private int forder;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Indexed("order", Name = "UX_ValidationItemValue", Unique = true)]
        [Association("ValidationItemValueReferencesValidationItem")]
        [RuleRequiredField(DefaultContexts.Save)]
        public ValidationItem validation_item_id
        {
            get => fvalidation_item_id;
            set => SetPropertyValue(nameof(validation_item_id), ref fvalidation_item_id, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [Size(255)]
        [DisplayName("Value")]
        [Persistent("value")]
        public string value1
        {
            get => fvalue1;
            set => SetPropertyValue(nameof(value1), ref fvalue1, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public int order
        {
            get => forder;
            set => SetPropertyValue<int>(nameof(order), ref forder, value);
        }

        public ValidationItemValue(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
