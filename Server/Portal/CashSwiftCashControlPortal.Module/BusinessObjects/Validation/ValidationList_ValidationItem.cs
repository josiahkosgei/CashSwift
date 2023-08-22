
//BusinessObjects.Validation.ValidationList_ValidationItem


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
    [FriendlyKeyProperty("validation_list_id")]
    [DefaultProperty("validation_list_id")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class ValidationList_ValidationItem : XPLiteObject
    {
        private Guid fid;
        private ValidationList fvalidation_list_id;
        private ValidationItem fvalidation_item_id;
        private int forder;
        private bool fenabled;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [Association("ValidationList_ValidationItemReferencesValidationList")]
        [RuleRequiredField(DefaultContexts.Save)]
        public ValidationList validation_list_id
        {
            get => fvalidation_list_id;
            set => SetPropertyValue(nameof(validation_list_id), ref fvalidation_list_id, value);
        }

        [Indexed("validation_list_id", Name = "UX_ValidationList_ValidationItem_UniqueItem", Unique = true)]
        [Association("ValidationList_ValidationItemReferencesValidationItem")]
        [RuleRequiredField(DefaultContexts.Save)]
        public ValidationItem validation_item_id
        {
            get => fvalidation_item_id;
            set => SetPropertyValue(nameof(validation_item_id), ref fvalidation_item_id, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        public int order
        {
            get => forder;
            set => SetPropertyValue<int>(nameof(order), ref forder, value);
        }

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        public ValidationList_ValidationItem(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
