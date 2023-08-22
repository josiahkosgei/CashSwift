
//BusinessObjects.Devices.Device


using CashSwift.Library.Standard.Security;
using CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift;
using CashSwiftCashControlPortal.Module.BusinessObjects.CITs;
using CashSwiftCashControlPortal.Module.BusinessObjects.Monitoring;
using CashSwiftCashControlPortal.Module.BusinessObjects.Screens;
using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Devices
{
    [NavigationItem("Device Management")]
    [FriendlyKeyProperty("machine_name")]
    [DefaultProperty("name")]
    [Appearance("enabled0", AppearanceItemType = "ViewItem", BackColor = "LightSalmon", Context = "ListView", Criteria = "enabled=false", FontColor = "Black", Priority = 0, TargetItems = "*")]
    public class Device : XPLiteObject
    {
        private Guid fid;
        private string fname;
        private string fdescription;
        private string fdevice_number;
        private string fdevice_location;
        private string fmachine_name;
        private string fmac_address;
        private Branch fbranch_id;
        private DeviceType ftype_id;
        private bool fenabled;
        private ConfigGroup fconfig_group;
        private UserGroup fuser_group;
        private GUIScreenList fGUIScreen_list;
        private LanguageList flanguage_list;
        private CurrencyList fcurrency_list;
        private TransactionTypeList ftransaction_type_list;
        private int flogin_cycles;
        private int flogin_attempts;
        private byte[] fapp_key;
        private Guid fapp_id;

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

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The value was already registered within the system.")]
        [Size(50)]
        public string device_number
        {
            get => fdevice_number;
            set => SetPropertyValue(nameof(device_number), ref fdevice_number, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [Size(50)]
        public string device_location
        {
            get => fdevice_location;
            set => SetPropertyValue(nameof(device_location), ref fdevice_location, value);
        }

        [RuleRequiredField(DefaultContexts.Save)]
        [RuleRegularExpression("^[^=\\\\\\/\\*\\-\\+ _^]+[^\\\\\"';*#\\\\|\\/()=+%<>^$]*$", CustomMessageTemplate = "Invalid characters detected #, *, \", ', ;, \\, |, /, (, ), =, +, %, <, >, ^, $", SkipNullOrEmptyValues = true)]
        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The value was already registered within the system.")]
        [Size(128)]
        public string machine_name
        {
            get => fmachine_name;
            set => SetPropertyValue(nameof(machine_name), ref fmachine_name, value?.ToUpperInvariant());
        }

        [RuleUniqueValue(DefaultContexts.Save, CustomMessageTemplate = "The value was already registered within the system.")]
        [Size(128)]
        public string mac_address
        {
            get => fmac_address;
            set => SetPropertyValue(nameof(mac_address), ref fmac_address, value?.ToUpperInvariant());
        }

        [Association("DeviceReferencesBranch")]
        [RuleRequiredField(DefaultContexts.Save)]
        public Branch branch_id
        {
            get => fbranch_id;
            set => SetPropertyValue(nameof(branch_id), ref fbranch_id, value);
        }

        [Association("DeviceReferencesDeviceType")]
        [RuleRequiredField(DefaultContexts.Save)]
        public DeviceType type_id
        {
            get => ftype_id;
            set => SetPropertyValue(nameof(type_id), ref ftype_id, value);
        }

        [DisplayName("Enabled")]
        [Persistent("enabled")]
        [ModelDefault("AllowEdit", "False")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        [Association("DeviceReferencesConfigGroup")]
        [RuleRequiredField(DefaultContexts.Save)]
        public ConfigGroup config_group
        {
            get => fconfig_group;
            set => SetPropertyValue(nameof(config_group), ref fconfig_group, value);
        }

        [Association("DeviceReferencesUserGroup")]
        [RuleRequiredField(DefaultContexts.Save)]
        public UserGroup user_group
        {
            get => fuser_group;
            set => SetPropertyValue(nameof(user_group), ref fuser_group, value);
        }

        [Association("DeviceReferencesGUIScreenList")]
        [RuleRequiredField(DefaultContexts.Save)]
        public GUIScreenList GUIScreen_list
        {
            get => fGUIScreen_list;
            set => SetPropertyValue(nameof(GUIScreen_list), ref fGUIScreen_list, value);
        }

        [Association("DeviceReferencesLanguageList")]
        [RuleRequiredField(DefaultContexts.Save)]
        public LanguageList language_list
        {
            get => flanguage_list;
            set => SetPropertyValue(nameof(language_list), ref flanguage_list, value);
        }

        [Association("DeviceReferencesCurrencyList")]
        [RuleRequiredField(DefaultContexts.Save)]
        public CurrencyList currency_list
        {
            get => fcurrency_list;
            set => SetPropertyValue(nameof(currency_list), ref fcurrency_list, value);
        }

        [Association("DeviceReferencesTransactionTypeList")]
        [RuleRequiredField(DefaultContexts.Save)]
        public TransactionTypeList transaction_type_list
        {
            get => ftransaction_type_list;
            set => SetPropertyValue(nameof(transaction_type_list), ref ftransaction_type_list, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int login_cycles
        {
            get => flogin_cycles;
            set => SetPropertyValue<int>(nameof(login_cycles), ref flogin_cycles, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public int login_attempts
        {
            get => flogin_attempts;
            set => SetPropertyValue<int>(nameof(login_attempts), ref flogin_attempts, value);
        }

        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        [Size(32)]
        public byte[] app_key
        {
            get => fapp_key;
            set => SetPropertyValue(nameof(app_key), ref fapp_key, value);
        }

        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid app_id
        {
            get => fapp_id;
            set => SetPropertyValue(nameof(app_id), ref fapp_id, value);
        }

        [Association("CITReferencesDevice")]
        public XPCollection<CIT> CITs => GetCollection<CIT>(nameof(CITs));

        [Association("DepositorSessionReferencesDevice")]
        public XPCollection<DepositorSession> DepositorSessions => GetCollection<DepositorSession>(nameof(DepositorSessions));

        [Association("DeviceLockReferencesDevice")]
        public XPCollection<DeviceLock> DeviceLocks => GetCollection<DeviceLock>(nameof(DeviceLocks));

        [Association("TransactionReferencesDevice")]
        public XPCollection<Transaction> Transactions => GetCollection<Transaction>(nameof(Transactions));

        [Association("DevicePrinterReferencesDevice")]
        public XPCollection<DevicePrinter> DevicePrinters => GetCollection<DevicePrinter>(nameof(DevicePrinters));

        [Association("DeviceSuspenseAccountReferencesDevice")]
        public XPCollection<DeviceSuspenseAccount> DeviceSuspenseAccounts => GetCollection<DeviceSuspenseAccount>(nameof(DeviceSuspenseAccounts));

        [Association("DeviceStatusReferencesDevice")]
        public XPCollection<DeviceStatus> DeviceStatusCollection => GetCollection<DeviceStatus>(nameof(DeviceStatusCollection));

        [Association("ApplicationLogReferencesDevice")]
        [NoForeignKey]
        public XPCollection<ApplicationLog> ApplicationLogs => GetCollection<ApplicationLog>(nameof(ApplicationLogs));

        [Association("DeviceCITSuspenseAccountReferencesDevice")]
        public XPCollection<DeviceCITSuspenseAccount> DeviceCITSuspenseAccounts => GetCollection<DeviceCITSuspenseAccount>(nameof(DeviceCITSuspenseAccounts));

        public Device(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!Session.IsNewObject(this))
                return;
            app_key = PasswordGenerator.GenerateRandom(32L);
            app_id = Guid.NewGuid();
            Guid guid = Guid.NewGuid();
            APIUser apiUser = new APIUser(Session)
            {
                id = guid,
                Name = machine_name,
                AppId = app_id,
                AppKey = app_key,
                Enabled = true
            };
        }
    }
}
