
//BusinessObjects.Authentication.CashSwift.APIUser


using CashSwift.Library.Standard.Security;
using CashSwift.Library.Standard.Utilities;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift
{
    [NavigationItem("Server Management")]
    [FriendlyKeyProperty("Name")]
    [DefaultProperty("Name")]
    [Persistent("api.APIUser")]
    public class APIUser : XPLiteObject
    {
        private Guid fid;
        private Guid _appId;
        private string fname;
        [DefaultValue(true)]
        private bool fenabled;
        private byte[] _appKey;

        public APIUser(Session session)
          : base(session)
        {
        }

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
        [Size(128)]
        [DisplayName("Name")]
        [Persistent("name")]
        public string Name
        {
            get => fname;
            set => SetPropertyValue(nameof(Name), ref fname, value.Left(128));
        }

        public bool Enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(Enabled), ref fenabled, value);
        }

        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        [Indexed(Name = "UX_API_ID", Unique = true)]
        public Guid AppId
        {
            get => _appId;
            set => SetPropertyValue(nameof(AppId), ref _appId, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Size(32)]
        public byte[] AppKey
        {
            get => _appKey;
            set => SetPropertyValue(nameof(AppKey), ref _appKey, value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            AppId = Guid.NewGuid();
            AppKey = PasswordGenerator.GenerateRandom(32L);
        }
    }
}
