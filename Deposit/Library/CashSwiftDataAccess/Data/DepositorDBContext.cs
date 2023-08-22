
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using CashSwiftDataAccess.Entities;
using ApplicationException = CashSwiftDataAccess.Entities.ApplicationException;


namespace CashSwiftDataAccess.Data
{
    public partial class DepositorDBContext : DbContext
    {
        public DepositorDBContext()
        : base(GetConnectionString())
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
            Database.SetInitializer<DepositorDBContext>(null);
        }
        private static string GetConnectionString()
        {

            try
            {

                var connectionString = ConfigurationManager.ConnectionStrings["DepositorDBContextConn"].ConnectionString;
                return connectionString;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<AlertAttachmentType> AlertAttachmentTypes { get; set; }
        public virtual DbSet<AlertEmail> AlertEmails { get; set; }
        public virtual DbSet<AlertEmailAttachment> AlertEmailAttachments { get; set; }
        public virtual DbSet<AlertEvent> AlertEvents { get; set; }
        public virtual DbSet<AlertMessageRegistry> AlertMessageRegistries { get; set; }
        public virtual DbSet<AlertMessageType> AlertMessageTypes { get; set; }
        public virtual DbSet<AlertSMS> AlertSMSs { get; set; }
        public virtual DbSet<ApplicationException> ApplicationExceptions { get; set; }
        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Branch> Branchs { get; set; }
        public virtual DbSet<CIT> CITs { get; set; }
        public virtual DbSet<CITDenomination> CITDenominationss { get; set; }
        public virtual DbSet<CITPrintout> CITPrintouts { get; set; }
        public virtual DbSet<CITTransaction> CITTransactions { get; set; }
        public virtual DbSet<Config> Configs { get; set; }
        public virtual DbSet<ConfigCategory> ConfigCategories { get; set; }
        public virtual DbSet<ConfigGroup> ConfigGroups { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CrashEvent> CrashEvents { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<CurrencyList> CurrencyLists { get; set; }
        public virtual DbSet<CurrencyListCurrency> CurrencyListCurrencies { get; set; }
        public virtual DbSet<DenominationDetail> DenominationDetails { get; set; }
        public virtual DbSet<DepositorSession> DepositorSessions { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<DeviceCITSuspenseAccount> DeviceCITSuspenseAccounts { get; set; }
        public virtual DbSet<DeviceConfig> DeviceConfigs { get; set; }
        public virtual DbSet<DeviceLock> DeviceLocks { get; set; }
        public virtual DbSet<DeviceLogin> DeviceLogins { get; set; }
        public virtual DbSet<DevicePrinter> DevicePrinters { get; set; }
        public virtual DbSet<DeviceStatus> DeviceStatus { get; set; }
        public virtual DbSet<DeviceSuspenseAccount> DeviceSuspenseAccounts { get; set; }
        public virtual DbSet<DeviceType> DeviceTypes { get; set; }
        public virtual DbSet<EscrowJam> EscrowJams { get; set; }
        public virtual DbSet<GUIPrepopItem> GUIPrepopItems { get; set; }
        public virtual DbSet<GUIPrepopList> GUIPrepopLists { get; set; }
        public virtual DbSet<GUIPrepopListItem> GUIPrepopListItems { get; set; }
        public virtual DbSet<GUIScreen> GUIScreens { get; set; }
        public virtual DbSet<GUIScreenList> GUIScreenLists { get; set; }
        public virtual DbSet<GUIScreenText> GUIScreenTexts { get; set; }
        public virtual DbSet<GUIScreenType> GUIScreenTypes { get; set; }
        public virtual DbSet<GuiScreenListScreen> GuiScreenListScreens { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LanguageList> LanguageLists { get; set; }
        public virtual DbSet<LanguageListLanguage> LanguageListLanguages { get; set; }
        public virtual DbSet<PasswordHistory> PasswordHistories { get; set; }
        public virtual DbSet<PasswordPolicy> PasswordPolicies { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PrinterStatus> PrinterStatuss { get; set; }
        public virtual DbSet<Printout> Printouts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SessionException> SessionExceptions { get; set; }
        public virtual DbSet<TextItem> TextItems { get; set; }
        public virtual DbSet<TextItemCategory> TextItemCategories { get; set; }
        public virtual DbSet<TextItemType> TextItemTypes { get; set; }
        public virtual DbSet<TextTranslation> TextTranslations { get; set; }
        public virtual DbSet<ThisDevice> ThisDevices { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionException> TransactionExceptions { get; set; }
        public virtual DbSet<TransactionLimitList> TransactionLimitLists { get; set; }
        public virtual DbSet<TransactionLimitListItem> TransactionLimitListItems { get; set; }
        public virtual DbSet<TransactionText> TransactionTexts { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }
        public virtual DbSet<TransactionTypeList> TransactionTypeLists { get; set; }
        public virtual DbSet<TransactionTypeListItem> TransactionTypeListItems { get; set; }
        public virtual DbSet<TransactionTypeListTransactionTypeListItem> TransactionTypeListTransactionTypeListItems { get; set; }
        public virtual DbSet<UptimeComponentState> UptimeComponentStates { get; set; }
        public virtual DbSet<UptimeMode> UptimeModes { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<UserLock> UserLocks { get; set; }
        public virtual DbSet<ValidationItem> ValidationItems { get; set; }
        public virtual DbSet<ValidationItemValue> ValidationItemValues { get; set; }
        public virtual DbSet<ValidationList> ValidationLists { get; set; }
        public virtual DbSet<ValidationListValidationItem> ValidationListValidationItems { get; set; }
        public virtual DbSet<ValidationText> ValidationTexts { get; set; }
        public virtual DbSet<ValidationType> ValidationTypes { get; set; }
        public virtual DbSet<sysTextItem> sysTextItems { get; set; }
        public virtual DbSet<sysTextItemCategory> sysTextItemCategories { get; set; }
        public virtual DbSet<sysTextItemType> sysTextItemTypes { get; set; }
        public virtual Microsoft.Data.Entity.DbSet<sysTextTranslation> sysTextTranslations { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<AlertEmail>()
                    .HasRequired(t => t.alert_event)
                    .WithMany(t => t.AlertEmails)
                    .HasForeignKey(d => d.alert_event_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                .Entity<AlertEmail>()
                .Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder
                .Entity<AlertEvent>()
                .Property(t => t.alert_event_id)
                .HasColumnAnnotation("FK_AlertEmailEvent_AlertEmailEvent", new IndexAnnotation(new IndexAttribute()));


            //modelBuilder.Entity<AlertEmailAttachment>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.hash);

            //    entity.Property(e => e.type);
            //});

            //modelBuilder.Entity<AlertEvent>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.created).HasDefaultValueSql("(getdate())");

            //    entity.HasOne(d => d.alert_event)
            //        .WithMany(p => p.Inversealert_event)
            //        .ForeignKey(d => d.alert_event_id)
            //        .ConstraintName("FK_AlertEmailEvent_AlertEmailEvent");
            //});


            modelBuilder.Entity<AlertEvent>()
                    .HasOptional(t => t.alert_event)
                    .WithMany(t => t.Inversealert_event)
                    .HasForeignKey(d => d.alert_event_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                .Entity<AlertEvent>()
                .Property(t => t.alert_event_id)
                .HasColumnAnnotation("FK_AlertEmailEvent_AlertEmailEvent", new IndexAnnotation(new IndexAttribute()));


            //modelBuilder.Entity<AlertMessageRegistry>(entity =>
            //{
            //    entity.Index(e => e.alert_type_id)
            //        .Name("ialert_type_id_AlertMessageRegistry");

            //    entity.Index(e => e.role_id)
            //        .Name("irole_id_AlertMessageRegistry");

            //    entity.Index(e => new { e.alert_type_id, e.role_id })
            //        .Name("UX_AlertMessageRegistry")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.email_enabled).HasDefaultValueSql("((1))");

            //    entity.HasOne(d => d.alert_type)
            //        .WithMany(p => p.AlertMessageRegistries)
            //        .ForeignKey(d => d.alert_type_id)
            //       // .WillCascadeOnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_AlertMessageRegistry_AlertMessageType");

            //    entity.HasOne(d => d.role)
            //        .WithMany(p => p.AlertMessageRegistries)
            //        .ForeignKey(d => d.role_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_AlertMessageRegistry_Role");
            //});

            modelBuilder
                .Entity<AlertMessageRegistry>()
                .Property(t => t.alert_type_id)
                .HasColumnAnnotation("FK_AlertMessageRegistry_AlertMessageType", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<AlertMessageRegistry>()
                    .HasRequired(t => t.alert_type)
                    .WithMany(t => t.AlertMessageRegistries)
                    .HasForeignKey(d => d.alert_type_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                .Entity<AlertMessageRegistry>()
                .Property(t => t.role_id)
                .HasColumnAnnotation("FK_AlertMessageRegistry_Role", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<AlertMessageRegistry>()
                    .HasRequired(t => t.role)
                    .WithMany(t => t.AlertMessageRegistries)
                    .HasForeignKey(d => d.role_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<AlertMessageType>(entity =>
            //{
            //    entity.Property(e => e.id).ValueGeneratedNever();

            //    entity.Property(e => e.enabled).HasDefaultValueSql("((1))");
            //});

            //modelBuilder.Entity<AlertSMS>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.created).HasDefaultValueSql("(getdate())");

            //    entity.HasOne(d => d.alert_event)
            //        .WithMany(p => p.AlertSMSes)
            //        .ForeignKey(d => d.alert_event_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_AlertSMS_AlertEvent");
            //});

            modelBuilder
                .Entity<AlertSMS>()
                .Property(t => t.alert_event_id)
                .HasColumnAnnotation("FK_AlertSMS_AlertEvent", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<AlertSMS>()
                    .HasRequired(t => t.alert_event)
                    .WithMany(t => t.AlertSMSes)
                    .HasForeignKey(d => d.alert_event_id)
                    .WillCascadeOnDelete(true);



            //modelBuilder.Entity<ApplicationException>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");
            //});

            //modelBuilder.Entity<ApplicationLog>(entity =>
            //{
            //    //entity.Comment("Stores the general application log that the GUI and other local systems write to");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.component);//.HasComment("Which internal component produced the log entry e.g. GUI, APIs, DeviceController etc");

            //    entity.Property(e => e.event_detail);//.HasComment("the details of the log message");

            //    entity.Property(e => e.event_name);//.HasComment("The name of the log event");

            //    entity.Property(e => e.event_type);//.HasComment("the type of the log event used for grouping and sorting");

            //    entity.Property(e => e.log_date);//.HasComment("Datetime the system deems for the log entry.");

            //    entity.Property(e => e.log_level);//.HasComment("the LogLevel");

            //    entity.Property(e => e.session_id);//.HasComment("The session this log entry belongs to");
            //});

            //modelBuilder.Entity<ApplicationUser>(entity =>
            //{
            //    entity.Index(e => e.ApplicationUserLoginDetail)
            //        .Name("iApplicationUserLoginDetail_ApplicationUser");

            //    entity.Index(e => e.role_id)
            //        .Name("irole_id_ApplicationUser");

            //    entity.Index(e => e.user_group)
            //        .Name("iuser_group_ApplicationUser");

            //    entity.Index(e => e.username)
            //        .Name("UX_ApplicationUser_Username")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.email_enabled).HasDefaultValueSql("((1))");

            //    entity.Property(e => e.password)
            //        //.Unicode(false)
            //        ;

            //    entity.HasOne(d => d.role)
            //        .WithMany(p => p.ApplicationUsers)
            //        .ForeignKey(d => d.role_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_ApplicationUser_Role");

            //    entity.HasOne(d => d.UserGroup)
            //        .WithMany(p => p.ApplicationUsers)
            //        .ForeignKey(d => d.user_group)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_ApplicationUser_UserGroup");
            //});

            modelBuilder
                .Entity<ApplicationUser>()
                .Property(t => t.role_id)
                .HasColumnAnnotation("FK_ApplicationUser_Role", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ApplicationUser>()
                    .HasRequired(t => t.role)
                    .WithMany(t => t.ApplicationUsers)
                    .HasForeignKey(d => d.role_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                .Entity<ApplicationUser>()
                .Property(t => t.role_id)
                .HasColumnAnnotation("FK_ApplicationUser_UserGroup", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ApplicationUser>()
                    .HasRequired(t => t.UserGroup)
                    .WithMany(t => t.ApplicationUsers)
                    .HasForeignKey(d => d.user_group)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Bank>(entity =>
            //{
            //    entity.Index(e => e.country_code)
            //        .Name("icountry_code_Bank");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.country_code)
            //        //.Unicode(false)
            //        ;

            //    entity.HasOne(d => d.country_codeNavigation)
            //        .WithMany(p => p.Banks)
            //        .ForeignKey(d => d.country_code)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_Bank_Country");
            //});

            modelBuilder
                .Entity<Bank>()
                .Property(t => t.country_code)
                .HasColumnAnnotation("FK_Bank_Country", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Bank>()
                    .HasRequired(t => t.Country)
                    .WithMany(t => t.Banks)
                    .HasForeignKey(d => d.country_code)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Branch>(entity =>
            //{
            //    entity.Index(e => e.bank_id)
            //        .Name("ibank_id_Branch");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.Bank)
            //        .WithMany(p => p.Branches)
            //        .ForeignKey(d => d.bank_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_Branch_Bank");
            //});

            modelBuilder
                .Entity<Branch>()
                .Property(t => t.bank_id)
                .HasColumnAnnotation("FK_Branch_Bank", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Branch>()
                    .HasRequired(t => t.Bank)
                    .WithMany(t => t.Branches)
                    .HasForeignKey(d => d.bank_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<CIT>(entity =>
            //{
            //    entity.Index(e => e.auth_user)
            //        .Name("iauth_user_CIT");

            //    entity.Index(e => e.device_id)
            //        .Name("idevice_id_CIT");

            //    entity.Index(e => e.start_user)
            //        .Name("istart_user_CIT");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.cit_date).HasDefaultValueSql("(getdate())");

            //    entity.HasOne(d => d.AuthUserNavigation)
            //        .WithMany(p => p.CITAuthUsers)
            //        .ForeignKey(d => d.auth_user)
            //        .ConstraintName("FK_CIT_ApplicationUser_AuthUser");

            //    entity.HasOne(d => d.Device)
            //        .WithMany(p => p.CITs)
            //        .ForeignKey(d => d.device_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_CIT_DeviceList");

            //    entity.HasOne(d => d.StartUserNavigation)
            //        .WithMany(p => p.CITStartUsers)
            //        .ForeignKey(d => d.start_user)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_CIT_ApplicationUser_StartUser");
            //});

            modelBuilder
                    .Entity<CIT>()
                    .Property(t => t.auth_user)
                    .HasColumnAnnotation("FK_CIT_ApplicationUser_AuthUser", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<CIT>()
                    .HasOptional(t => t.AuthUser)
                    .WithMany(t => t.CITAuthUsers)
                    .HasForeignKey(d => d.auth_user)
                    .WillCascadeOnDelete(true);
            modelBuilder
                    .Entity<CIT>()
                    .Property(t => t.device_id)
                    .HasColumnAnnotation("FK_CIT_DeviceList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<CIT>()
                    .HasRequired(t => t.Device)
                    .WithMany(t => t.CITs)
                    .HasForeignKey(d => d.device_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                .Entity<CIT>()
                .Property(t => t.start_user)
                .HasColumnAnnotation("FK_CIT_ApplicationUser_StartUser", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<CIT>()
                    .HasRequired(t => t.StartUser)
                    .WithMany(t => t.CITStartUsers)
                    .HasForeignKey(d => d.start_user)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<CITDenomination>(entity =>
            //{
            //    entity.Index(e => e.cit_id)
            //        .Name("icit_id_CITDenominations");

            //    entity.Index(e => e.currency_id)
            //        .Name("icurrency_id_CITDenominations");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.currency_id)
            //        //.Unicode(false)
            //        ;

            //    entity.HasOne(d => d.CIT)
            //        .WithMany(p => p.CITDenominations)
            //        .ForeignKey(d => d.cit_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_CITDenominations_CIT");

            //    entity.HasOne(d => d.Currency)
            //        .WithMany(p => p.CITDenominations)
            //        .ForeignKey(d => d.currency_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_CITDenominations_Currency");
            //});
            modelBuilder
                    .Entity<CITDenomination>()
                    .Property(t => t.cit_id)
                    .HasColumnAnnotation("FK_CITDenominations_CIT", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<CITDenomination>()
                    .HasRequired(t => t.CIT)
                    .WithMany(t => t.CITDenominations)
                    .HasForeignKey(d => d.cit_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<CITDenomination>()
                    .Property(t => t.currency_id)
                    .HasColumnAnnotation("FK_CITDenominations_Currency", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<CITDenomination>()
                    .HasRequired(t => t.Currency)
                    .WithMany(t => t.CITDenominations)
                    .HasForeignKey(d => d.currency_id)
                    .WillCascadeOnDelete(true);
            ;
            //modelBuilder.Entity<CITPrintout>(entity =>
            //{
            //    entity.Index(e => e.cit_id)
            //        .Name("icit_id_CITPrintout");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.datetime).HasDefaultValueSql("(getdate())");

            //    entity.HasOne(d => d.cit)
            //        .WithMany(p => p.CITPrintouts)
            //        .ForeignKey(d => d.cit_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_CITPrintout_CIT");
            //});

            modelBuilder
                    .Entity<CITPrintout>()
                    .Property(t => t.cit_id)
                    .HasColumnAnnotation("FK_CITPrintout_CIT", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<CITPrintout>()
                    .HasRequired(t => t.cit)
                    .WithMany(t => t.CITPrintouts)
                    .HasForeignKey(d => d.cit_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<CITTransaction>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.currency)
            //        //.Unicode(false)
            //        ;

            //    entity.HasOne(d => d.CIT)
            //        .WithMany(p => p.CITTransactions)
            //        .ForeignKey(d => d.cit_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_CITTransaction_CIT");
            //});

            modelBuilder
                    .Entity<CITTransaction>()
                    .Property(t => t.cit_id)
                    .HasColumnAnnotation("FK_CITTransaction_CIT", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<CITTransaction>()
                    .HasRequired(t => t.CIT)
                    .WithMany(t => t.CITTransactions)
                    .HasForeignKey(d => d.cit_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Config>(entity =>
            //{
            //    entity.Index(e => e.category_id)
            //        .Name("icategory_id_Config");

            //    entity.HasOne(d => d.category)
            //        .WithMany(p => p.Configs)
            //        .ForeignKey(d => d.category_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_Config_ConfigCategory");
            //});

            modelBuilder
                    .Entity<Config>()
                    .Property(t => t.category_id)
                    .HasColumnAnnotation("FK_Config_ConfigCategory", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Config>()
                    .HasRequired(t => t.category)
                    .WithMany(t => t.Configs)
                    .HasForeignKey(d => d.category_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<ConfigCategory>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");
            //});

            //modelBuilder.Entity<ConfigGroup>(entity =>
            //{
            //    entity.Index(e => e.parent_group)
            //        .Name("iparent_group_ConfigGroup");

            //    entity.HasOne(d => d.parent_groupNavigation)
            //        .WithMany(p => p.Inverseparent_groupNavigation)
            //        .ForeignKey(d => d.parent_group)
            //        .ConstraintName("FK_ConfigGroup_ConfigGroup");
            //});

            modelBuilder
                    .Entity<ConfigGroup>()
                    .Property(t => t.parent_group)
                    .HasColumnAnnotation("FK_ConfigGroup_ConfigGroup", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ConfigGroup>()
                    .HasOptional(t => t.parent_groupNavigation)
                    .WithMany(t => t.Inverseparent_groupNavigation)
                    .HasForeignKey(d => d.parent_group)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Country>(entity =>
            //{
            //    entity.Property(e => e.country_code)
            //        //.Unicode(false)

            //        .HasDefaultValueSql("('')");

            //    entity.Property(e => e.country_name)
            //        //.Unicode(false)
            //        .HasDefaultValueSql("('')");
            //});

            //modelBuilder.Entity<CrashEvent>(entity =>
            //{
            //    // entity.HasComment("contains a crash report");

            //    entity.Index(e => e.device_id)
            //        .Name("idevice_id_exp_CrashEvent_7BCB8F5E");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");
            //});


            //modelBuilder.Entity<CurrencyList>(entity =>
            //{
            //    entity.Index(e => e.default_currency)
            //        .Name("idefault_currency_CurrencyList");

            //    entity.Property(e => e.default_currency)
            //        //.Unicode(false)
            //        ;

            //    entity.HasOne(d => d.DefaultCurrencyNavigation)
            //        .WithMany(p => p.CurrencyLists)
            //        .ForeignKey(d => d.default_currency)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_CurrencyList_Currency");
            //});

            modelBuilder
                    .Entity<CurrencyList>()
                    .Property(t => t.default_currency)
                    .HasColumnAnnotation("FK_CurrencyList_Currency", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<CurrencyList>()
                    .HasRequired(t => t.DefaultCurrencyNavigation)
                    .WithMany(t => t.CurrencyLists)
                    .HasForeignKey(d => d.default_currency)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<CurrencyListCurrency>(entity =>
            //{
            //    entity.Index(e => e.currency_item)
            //        .Name("icurrency_item_CurrencyList_Currency");

            //    entity.Index(e => e.currency_list)
            //        .Name("icurrency_list_CurrencyList_Currency");

            //    entity.Index(e => new { e.currency_list, e.currency_item })
            //        .Name("UX_CurrencyList_Currency_CurrencyItem")
            //        .Unique();

            //    entity.Index(e => new { e.currency_list, e.currency_order })
            //        .Name("UX_Currency_CurrencyList_Order")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.currency_item)
            //        //.Unicode(false)
            //        ;

            //    entity.HasOne(d => d.Currency)
            //        .WithMany(p => p.CurrencyListCurrencies)
            //        .ForeignKey(d => d.currency_item)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_Currency_CurrencyList_Currency");

            //    entity.HasOne(d => d.CurrencyList)
            //        .WithMany(p => p.CurrencyListCurrencies)
            //        .ForeignKey(d => d.currency_list)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_Currency_CurrencyList_CurrencyList");
            //});

            modelBuilder
                    .Entity<CurrencyListCurrency>()
                    .Property(t => t.currency_item)
                    .HasColumnAnnotation("FK_Currency_CurrencyList_Currency", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<CurrencyListCurrency>()
                    .HasRequired(t => t.Currency)
                    .WithMany(t => t.CurrencyListCurrencies)
                    .HasForeignKey(d => d.currency_item)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<CurrencyListCurrency>()
                    .Property(t => t.currency_list)
                    .HasColumnAnnotation("FK_Currency_CurrencyList_CurrencyList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<CurrencyListCurrency>()
                    .HasRequired(t => t.CurrencyList)
                    .WithMany(t => t.CurrencyListCurrencies)
                    .HasForeignKey(d => d.currency_list)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<DenominationDetail>(entity =>
            //{
            //    entity.Index(e => e.tx_id)
            //        .Name("itx_id_DenominationDetail");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.tx)
            //        .WithMany(p => p.DenominationDetails)
            //        .ForeignKey(d => d.tx_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DenominationDetail_Transaction");
            //});

            modelBuilder
                    .Entity<DenominationDetail>()
                    .Property(t => t.tx_id)
                    .HasColumnAnnotation("FK_DenominationDetail_Transaction", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DenominationDetail>()
                    .HasRequired(t => t.tx)
                    .WithMany(t => t.DenominationDetails)
                    .HasForeignKey(d => d.tx_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<DepositorSession>(entity =>
            //{
            //    entity.Index(e => e.device_id)
            //        .Name("idevice_id_DepositorSession");

            //    entity.Index(e => e.language_code)
            //        .Name("ilanguage_code_DepositorSession");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");


            //    entity.HasOne(d => d.device)
            //        .WithMany(p => p.DepositorSessions)
            //        .ForeignKey(d => d.device_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DepositorSession_DeviceList");

            //    entity.HasOne(d => d.language_codeNavigation)
            //        .WithMany(p => p.DepositorSessions)
            //        .ForeignKey(d => d.language_code)
            //        .ConstraintName("FK_DepositorSession_Language");
            //});

            modelBuilder
                    .Entity<DepositorSession>()
                    .Property(t => t.device_id)
                    .HasColumnAnnotation("FK_DepositorSession_DeviceList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DepositorSession>()
                    .HasRequired(t => t.device)
                    .WithMany(t => t.DepositorSessions)
                    .HasForeignKey(d => d.device_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<DepositorSession>()
                    .Property(t => t.language_code)
                    .HasColumnAnnotation("FK_DepositorSession_DeviceList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DepositorSession>()
                    .HasRequired(t => t.language_codeNavigation)
                    .WithMany(t => t.DepositorSessions)
                    .HasForeignKey(d => d.language_code)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Device>(entity =>
            //{

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.GUIScreen_list).HasDefaultValueSql("((1))");

            //    entity.Property(e => e.app_key);

            //    entity.Property(e => e.mac_address)
            //        //.Unicode(false)
            //        ;

            //    entity.Property(e => e.transaction_type_list).HasDefaultValueSql("((1))");

            //    entity.HasOne(d => d.GUIScreenListNavigation)
            //        .WithMany(p => p.Devices)
            //        .ForeignKey(d => d.GUIScreen_list)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceList_GUIScreenList");

            //    entity.HasOne(d => d.Branch)
            //        .WithMany(p => p.Devices)
            //        .ForeignKey(d => d.branch_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceList_Branch");

            //    entity.HasOne(d => d.ConfigGroupNavigation)
            //        .WithMany(p => p.Devices)
            //        .ForeignKey(d => d.config_group)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceList_ConfigGroup");

            //    entity.HasOne(d => d.CurrencyListNavigation)
            //        .WithMany(p => p.Devices)
            //        .ForeignKey(d => d.currency_list)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceList_CurrencyList");

            //    entity.HasOne(d => d.LanguageListNavigation)
            //        .WithMany(p => p.Devices)
            //        .ForeignKey(d => d.language_list)
            //        .ConstraintName("FK_Device_LanguageList");

            //    entity.HasOne(d => d.TransactionTypeListNavigation)
            //        .WithMany(p => p.Devices)
            //        .ForeignKey(d => d.transaction_type_list)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceList_TransactionTypeList");

            //    entity.HasOne(d => d.DeviceType)
            //        .WithMany(p => p.Devices)
            //        .ForeignKey(d => d.type_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceList_DeviceType");

            //    entity.HasOne(d => d.UserGroupNavigation)
            //        .WithMany(p => p.Devices)
            //        .ForeignKey(d => d.user_group)
            //        .ConstraintName("FK_DeviceList_UserGroup");
            //});

            modelBuilder
                    .Entity<Device>()
                    .Property(t => t.GUIScreen_list)
                    .HasColumnAnnotation("FK_DeviceList_GUIScreenList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Device>()
                    .HasRequired(t => t.GUIScreenList)
                    .WithMany(t => t.Devices)
                    .HasForeignKey(d => d.GUIScreen_list)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Device>()
                    .Property(t => t.branch_id)
                    .HasColumnAnnotation("FK_DeviceList_Branch", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Device>()
                    .HasRequired(t => t.Branch)
                    .WithMany(t => t.Devices)
                    .HasForeignKey(d => d.branch_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Device>()
                    .Property(t => t.config_group)
                    .HasColumnAnnotation("FK_DeviceList_ConfigGroup", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Device>()
                    .HasRequired(t => t.ConfigGroup)
                    .WithMany(t => t.Devices)
                    .HasForeignKey(d => d.config_group)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Device>()
                    .Property(t => t.currency_list)
                    .HasColumnAnnotation("FK_DeviceList_CurrencyList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Device>()
                    .HasRequired(t => t.CurrencyList)
                    .WithMany(t => t.Devices)
                    .HasForeignKey(d => d.currency_list)
                    .WillCascadeOnDelete(true);
            modelBuilder
                    .Entity<Device>()
                    .Property(t => t.language_list)
                    .HasColumnAnnotation("FK_Device_LanguageList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Device>()
                    .HasOptional(t => t.LanguageList)
                    .WithMany(t => t.Devices)
                    .HasForeignKey(d => d.language_list)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Device>()
                    .Property(t => t.transaction_type_list)
                    .HasColumnAnnotation("FK_DeviceList_TransactionTypeList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Device>()
                    .HasRequired(t => t.TransactionTypeList)
                    .WithMany(t => t.Devices)
                    .HasForeignKey(d => d.transaction_type_list)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Device>()
                    .Property(t => t.type_id)
                    .HasColumnAnnotation("FK_DeviceList_DeviceType", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Device>()
                    .HasRequired(t => t.DeviceType)
                    .WithMany(t => t.Devices)
                    .HasForeignKey(d => d.type_id)
                    .WillCascadeOnDelete(true);
            modelBuilder
                    .Entity<Device>()
                    .Property(t => t.user_group)
                    .HasColumnAnnotation("FK_DeviceList_UserGroup", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Device>()
                    .HasOptional(t => t.UserGroup)
                    .WithMany(t => t.Devices)
                    .HasForeignKey(d => d.user_group)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<DeviceCITSuspenseAccount>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.currency_code)
            //        //.Unicode(false)
            //        ;

            //    entity.HasOne(d => d.Currency)
            //        .WithMany(p => p.DeviceCITSuspenseAccounts)
            //        .ForeignKey(d => d.currency_code)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceCITSuspenseAccount_Currency");

            //    entity.HasOne(d => d.Device)
            //        .WithMany(p => p.DeviceCITSuspenseAccounts)
            //        .ForeignKey(d => d.device_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceCITSuspenseAccount_Device");
            //});

            modelBuilder
                    .Entity<DeviceCITSuspenseAccount>()
                    .Property(t => t.currency_code)
                    .HasColumnAnnotation("FK_DeviceCITSuspenseAccount_Currency", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DeviceCITSuspenseAccount>()
                    .HasRequired(t => t.Currency)
                    .WithMany(t => t.DeviceCITSuspenseAccounts)
                    .HasForeignKey(d => d.currency_code)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<DeviceCITSuspenseAccount>()
                    .Property(t => t.device_id)
                    .HasColumnAnnotation("FK_DeviceCITSuspenseAccount_Device", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DeviceCITSuspenseAccount>()
                    .HasRequired(t => t.Device)
                    .WithMany(t => t.DeviceCITSuspenseAccounts)
                    .HasForeignKey(d => d.device_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<DeviceConfig>(entity =>
            //{
            //    entity.Index(e => e.config_id)
            //        .Name("iconfig_id_DeviceConfig");

            //    entity.Index(e => e.group_id)
            //        .Name("igroup_id_DeviceConfig");

            //    entity.Index(e => new { e.config_id, e.group_id })
            //        .Name("UX_DeviceConfig")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.config)
            //        .WithMany(p => p.DeviceConfigs)
            //        .ForeignKey(d => d.config_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceConfig_Config");

            //    entity.HasOne(d => d.group)
            //        .WithMany(p => p.DeviceConfigs)
            //        .ForeignKey(d => d.group_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceConfig_ConfigGroup");
            //});

            modelBuilder
                    .Entity<DeviceConfig>()
                    .Property(t => t.config_id)
                    .HasColumnAnnotation("FK_DeviceConfig_Config", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DeviceConfig>()
                    .HasRequired(t => t.config)
                    .WithMany(t => t.DeviceConfigs)
                    .HasForeignKey(d => d.config_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<DeviceConfig>()
                    .Property(t => t.group_id)
                    .HasColumnAnnotation("FK_DeviceConfig_ConfigGroup", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DeviceConfig>()
                    .HasRequired(t => t.group)
                    .WithMany(t => t.DeviceConfigs)
                    .HasForeignKey(d => d.group_id)
                    .WillCascadeOnDelete(true);


            //modelBuilder.Entity<DeviceLock>(entity =>
            //{
            //    entity.Index(e => e.device_id)
            //        .Name("idevice_DeviceLock");

            //    entity.Index(e => e.locking_user)
            //        .Name("ilocking_user_DeviceLock");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.device)
            //        .WithMany(p => p.DeviceLocks)
            //        .ForeignKey(d => d.device_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceLock_Device");
            //});

            modelBuilder
                    .Entity<DeviceLock>()
                    .Property(t => t.device_id)
                    .HasColumnAnnotation("FK_DeviceLock_Device", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DeviceLock>()
                    .HasRequired(t => t.device)
                    .WithMany(t => t.DeviceLocks)
                    .HasForeignKey(d => d.device_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<DeviceLogin>(entity =>
            //{
            //    entity.Index(e => e.User)
            //        .Name("iUser_DeviceLogin");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.UserNavigation)
            //        .WithMany(p => p.DeviceLogins)
            //        .ForeignKey(d => d.User)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceLogin_ApplicationUser");

            //    entity.HasOne(d => d.device)
            //        .WithMany(p => p.DeviceLogins)
            //        .ForeignKey(d => d.device_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceLogin_Device");
            //});

            modelBuilder
                    .Entity<DeviceLogin>()
                    .Property(t => t.User)
                    .HasColumnAnnotation("FK_DeviceLogin_ApplicationUser", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DeviceLogin>()
                    .HasRequired(t => t.UserNavigation)
                    .WithMany(t => t.DeviceLogins)
                    .HasForeignKey(d => d.User)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<DeviceLogin>()
                    .Property(t => t.device_id)
                    .HasColumnAnnotation("FK_DeviceLogin_Device", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DeviceLogin>()
                    .HasRequired(t => t.device)
                    .WithMany(t => t.DeviceLogins)
                    .HasForeignKey(d => d.device_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<DevicePrinter>(entity =>
            //{
            //    entity.Index(e => e.device_id)
            //        .Name("idevice_id_DevicePrinter");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.is_infront).HasDefaultValueSql("((1))");

            //    entity.HasOne(d => d.device)
            //        .WithMany(p => p.DevicePrinters)
            //        .ForeignKey(d => d.device_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DevicePrinter_DeviceList");
            //});

            modelBuilder
                    .Entity<DevicePrinter>()
                    .Property(t => t.device_id)
                    .HasColumnAnnotation("FK_DevicePrinter_DeviceList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DevicePrinter>()
                    .HasRequired(t => t.device)
                    .WithMany(t => t.DevicePrinters)
                    .HasForeignKey(d => d.device_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<DeviceStatus>(entity =>
            //{
            //    entity.Index(e => e.device_id)
            //        .Name("idevice_id_DeviceStatus");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.ba_currency)
            //        //.Unicode(false)
            //        ;

            //    entity.Property(e => e.bag_note_capacity);
            //});

            modelBuilder
                    .Entity<DeviceStatus>()
                    .Property(t => t.device_id)
                    .HasColumnAnnotation("FK_DevicePrinter_Device", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DeviceStatus>()
                    .HasRequired(t => t.Device)
                    .WithMany(t => t.DeviceStatuses)
                    .HasForeignKey(d => d.device_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<DeviceSuspenseAccount>(entity =>
            //{
            //    entity.Index(e => e.currency_code)
            //        .Name("icurrency_code_DeviceSuspenseAccount");

            //    entity.Index(e => e.device_id)
            //        .Name("idevice_id_DeviceSuspenseAccount");

            //    entity.Index(e => new { e.device_id, e.currency_code })
            //        .Name("UX_Device_SuspenseAccount")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.currency_code)
            //        //.Unicode(false)
            //        ;

            //    entity.HasOne(d => d.currency_codeNavigation)
            //        .WithMany(p => p.DeviceSuspenseAccounts)
            //        .ForeignKey(d => d.currency_code)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceSuspenseAccount_Currency");

            //    entity.HasOne(d => d.device)
            //        .WithMany(p => p.DeviceSuspenseAccounts)
            //        .ForeignKey(d => d.device_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_DeviceSuspenseAccount_DeviceList");
            //});

            modelBuilder
                    .Entity<DeviceSuspenseAccount>()
                    .Property(t => t.currency_code)
                    .IsUnicode()
                    .HasColumnAnnotation("FK_DeviceSuspenseAccount_Currency", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DeviceSuspenseAccount>()
                    .HasRequired(t => t.currency_codeNavigation)
                    .WithMany(t => t.DeviceSuspenseAccounts)
                    .HasForeignKey(d => d.currency_code)

                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<DeviceSuspenseAccount>()
                    .Property(t => t.device_id)
                    .HasColumnAnnotation("FK_DeviceSuspenseAccount_DeviceList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<DeviceSuspenseAccount>()
                    .HasRequired(t => t.device)
                    .WithMany(t => t.DeviceSuspenseAccounts)
                    .HasForeignKey(d => d.device_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<EscrowJam>(entity =>
            //{
            //    entity.Property(e => e.id).ValueGeneratedNever();

            //    entity.HasOne(d => d.AuthorisingUser)
            //        .WithMany(p => p.EscrowJamAuthorisingUsers)
            //        .ForeignKey(d => d.authorising_user)
            //        .ConstraintName("FK_EscrowJam_AppUser_Approver");

            //    entity.HasOne(d => d.InitialisingUser)
            //        .WithMany(p => p.EscrowJamInitialisingUsers)
            //        .ForeignKey(d => d.initialising_user)
            //        .ConstraintName("FK_EscrowJam_AppUser_Initiator");

            //    entity.HasOne(d => d.Transaction)
            //        .WithMany(p => p.EscrowJams)
            //        .ForeignKey(d => d.transaction_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_EscrowJam_Transaction");
            //});

            modelBuilder
                    .Entity<EscrowJam>()
                    .Property(t => t.authorising_user)
                    .HasColumnAnnotation("FK_EscrowJam_AppUser_Approver", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<EscrowJam>()
                    .HasOptional(t => t.AuthorisingUser)
                    .WithMany(t => t.EscrowJamAuthorisingUsers)
                    .HasForeignKey(d => d.authorising_user)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<EscrowJam>()
                    .Property(t => t.initialising_user)
                    .HasColumnAnnotation("FK_EscrowJam_AppUser_Initiator", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<EscrowJam>()
                    .HasOptional(t => t.InitialisingUser)
                    .WithMany(t => t.EscrowJamInitialisingUsers)
                    .HasForeignKey(d => d.initialising_user)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<EscrowJam>()
                    .Property(t => t.transaction_id)
                    .HasColumnAnnotation("FK_EscrowJam_Transaction", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<EscrowJam>()
                    .HasRequired(t => t.Transaction)
                    .WithMany(t => t.EscrowJams)
                    .HasForeignKey(d => d.transaction_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<GUIPrepopItem>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.enabled).HasDefaultValueSql("((1))");

            //    entity.HasOne(d => d.TextItemNavigation)
            //        .WithMany(p => p.GUIPrepopItems)
            //        .ForeignKey(d => d.value)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_GUIPrepopItem_TextItem");
            //});

            modelBuilder
                    .Entity<GUIPrepopItem>()
                    .Property(t => t.value)
                    .HasColumnAnnotation("FK_GUIPrepopItem_TextItem", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIPrepopItem>()
                    .HasRequired(t => t.TextItemNavigation)
                    .WithMany(t => t.GUIPrepopItems)
                    .HasForeignKey(d => d.value)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<GUIPrepopList>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.UseDefault).HasDefaultValueSql("((1))");

            //    entity.Property(e => e.enabled).HasDefaultValueSql("((1))");
            //});

            //modelBuilder.Entity<GUIPrepopListItem>(entity =>
            //{
            //    entity.Index(e => e.Item)
            //        .Name("iItem_GUIPrepopList_Item");

            //    entity.Index(e => e.List)
            //        .Name("iList_GUIPrepopList_Item");

            //    entity.Index(e => new { e.Item, e.List })
            //        .Name("UX_GUIPrepopList_Item")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.GUIPrepopItemNavigation)
            //        .WithMany(p => p.GUIPrepopListItems)
            //        .ForeignKey(d => d.Item)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_GUIPrepopList_Item_GUIPrepopItem");

            //    entity.HasOne(d => d.GUIPrepopListNavigation)
            //        .WithMany(p => p.GUIPrepopListItems)
            //        .ForeignKey(d => d.List)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_GUIPrepopList_Item_GUIPrepopList");
            //});

            modelBuilder
                    .Entity<GUIPrepopListItem>()
                    .Property(t => t.Item)
                    .HasColumnAnnotation("FK_GUIPrepopList_Item_GUIPrepopItem", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIPrepopListItem>()
                    .HasRequired(t => t.GUIPrepopItemNavigation)
                    .WithMany(t => t.GUIPrepopListItems)
                    .HasForeignKey(d => d.Item)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<GUIPrepopListItem>()
                    .Property(t => t.List)
                    .HasColumnAnnotation("FK_GUIPrepopList_Item_GUIPrepopList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIPrepopListItem>()
                    .HasRequired(t => t.GUIPrepopListNavigation)
                    .WithMany(t => t.GUIPrepopListItems)
                    .HasForeignKey(d => d.List)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<GUIScreen>(entity =>
            //{
            //    entity.Index(e => e.gui_text)
            //        .Name("igui_text_GUIScreen");

            //    entity.Index(e => e.type)
            //        .Name("itype_GUIScreen");


            //    entity.HasOne(d => d.GUIScreenTextNavigation)
            //        .WithMany(p => p.GUIScreens)
            //        .ForeignKey(d => d.gui_text)
            //        .ConstraintName("FK_GUIScreen_GUIScreenText");

            //    entity.HasOne(d => d.GUIScreenType)
            //        .WithMany(p => p.GUIScreens)
            //        .ForeignKey(d => d.type)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_GUIScreen_GUIScreenType");
            //});

            modelBuilder
                    .Entity<GUIScreen>()
                    .Property(t => t.gui_text)
                    .HasColumnAnnotation("FK_GUIScreen_GUIScreenText", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIScreen>()
                    .HasOptional(t => t.GUIScreenText)
                    .WithMany(t => t.GUIScreens)
                    .HasForeignKey(d => d.gui_text)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<GUIScreen>()
                    .Property(t => t.type)
                    .HasColumnAnnotation("FK_GUIScreen_GUIScreenType", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIScreen>()
                    .HasRequired(t => t.GUIScreenType)
                    .WithMany(t => t.GUIScreens)
                    .HasForeignKey(d => d.type)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<GUIScreenText>(entity =>
            //{
            //    entity.Index(e => e.btn_accept_caption)
            //        .Name("ibtn_accept_caption_GUIScreenText");

            //    entity.Index(e => e.btn_back_caption)
            //        .Name("ibtn_back_caption_GUIScreenText");

            //    entity.Index(e => e.btn_cancel_caption)
            //        .Name("ibtn_cancel_caption_GUIScreenText");

            //    entity.Index(e => e.full_instructions)
            //        .Name("ifull_instructions_GUIScreenText");

            //    entity.Index(e => e.guiscreen_id)
            //        .Name("iguiscreen_id_GUIScreenText");

            //    entity.Index(e => e.screen_title)
            //        .Name("iscreen_title_GUIScreenText");

            //    entity.Index(e => e.screen_title_instruction)
            //        .Name("iscreen_title_instruction_GUIScreenText");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.btn_accept_captionNavigation)
            //        .WithMany(p => p.GUIScreenTextbtn_accept_captionNavigation)
            //        .ForeignKey(d => d.btn_accept_caption)
            //        .ConstraintName("FK_GUIScreenText_btn_accept_caption");

            //    entity.HasOne(d => d.btn_back_captionNavigation)
            //        .WithMany(p => p.GUIScreenTextbtn_back_captionNavigation)
            //        .ForeignKey(d => d.btn_back_caption)
            //        .ConstraintName("FK_GUIScreenText_btn_back_caption");

            //    entity.HasOne(d => d.btn_cancel_captionNavigation)
            //        .WithMany(p => p.GUIScreenTextbtn_cancel_captionNavigation)
            //        .ForeignKey(d => d.btn_cancel_caption)
            //        .ConstraintName("FK_GUIScreenText_btn_cancel_caption");

            //    entity.HasOne(d => d.full_instructionsNavigation)
            //        .WithMany(p => p.GUIScreenTextfull_instructionsNavigation)
            //        .ForeignKey(d => d.full_instructions)
            //        .ConstraintName("FK_GUIScreenText_full_instructions");

            //    entity.HasOne(d => d.GUIScreen)
            //        .WithMany(p => p.GUIScreenTexts)
            //        .ForeignKey(d => d.guiscreen_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_GUIScreenText_GUIScreen");

            //    entity.HasOne(d => d.screen_titleNavigation)
            //        .WithMany(p => p.GUIScreenTextscreen_titleNavigation)
            //        .ForeignKey(d => d.screen_title)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_GUIScreenText_screen_title");

            //    entity.HasOne(d => d.screen_title_instructionNavigation)
            //        .WithMany(p => p.GUIScreenTextscreen_title_instructionNavigation)
            //        .ForeignKey(d => d.screen_title_instruction)
            //        .ConstraintName("FK_GUIScreenText_screen_title_instruction");
            //});

            modelBuilder
                    .Entity<GUIScreenText>()
                    .Property(t => t.btn_accept_caption)
                    .HasColumnAnnotation("FK_GUIScreenText_btn_accept_caption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIScreenText>()
                    .HasOptional(t => t.btn_accept_captionNavigation)
                    .WithMany(t => t.GUIScreenTextbtn_accept_captionNavigation)
                    .HasForeignKey(d => d.btn_accept_caption)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<GUIScreenText>()
                    .Property(t => t.btn_back_caption)
                    .HasColumnAnnotation("FK_GUIScreenText_btn_back_caption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIScreenText>()
                    .HasOptional(t => t.btn_back_captionNavigation)
                    .WithMany(t => t.GUIScreenTextbtn_back_captionNavigation)
                    .HasForeignKey(d => d.btn_back_caption)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<GUIScreenText>()
                    .Property(t => t.btn_cancel_caption)
                    .HasColumnAnnotation("FK_GUIScreenText_btn_cancel_caption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIScreenText>()
                    .HasOptional(t => t.btn_cancel_captionNavigation)
                    .WithMany(t => t.GUIScreenTextbtn_cancel_captionNavigation)
                    .HasForeignKey(d => d.btn_cancel_caption)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<GUIScreenText>()
                    .Property(t => t.full_instructions)
                    .HasColumnAnnotation("FK_GUIScreenText_full_instructions", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIScreenText>()
                    .HasOptional(t => t.full_instructionsNavigation)
                    .WithMany(t => t.GUIScreenTextfull_instructionsNavigation)
                    .HasForeignKey(d => d.full_instructions)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<GUIScreenText>()
                    .Property(t => t.guiscreen_id)
                    .HasColumnAnnotation("FK_GUIScreenText_GUIScreen", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIScreenText>()
                    .HasRequired(t => t.GUIScreen)
                    .WithMany(t => t.GUIScreenTexts)
                    .HasForeignKey(d => d.guiscreen_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<GUIScreenText>()
                    .Property(t => t.screen_title)
                    .HasColumnAnnotation("FK_GUIScreenText_screen_title", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIScreenText>()
                    .HasRequired(t => t.screen_titleNavigation)
                    .WithMany(t => t.GUIScreenTextscreen_titleNavigation)
                    .HasForeignKey(d => d.screen_title)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<GUIScreenText>()
                    .Property(t => t.screen_title_instruction)
                    .HasColumnAnnotation("FK_GUIScreenText_screen_title_instruction", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GUIScreenText>()
                    .HasOptional(t => t.screen_title_instructionNavigation)
                    .WithMany(t => t.GUIScreenTextscreen_title_instructionNavigation)
                    .HasForeignKey(d => d.screen_title_instruction)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<GuiScreenListScreen>(entity =>
            //{
            //    entity.Index(e => e.gui_screen_list)
            //        .Name("igui_screen_list_GuiScreenList_Screen");

            //    entity.Index(e => e.guiprepoplist_id)
            //        .Name("iguiprepoplist_id_GuiScreenList_Screen");

            //    entity.Index(e => e.screen)
            //        .Name("iscreen_GuiScreenList_Screen");

            //    entity.Index(e => e.validation_list_id)
            //        .Name("ivalidation_list_id_GuiScreenList_Screen");

            //    entity.Index(e => new { e.gui_screen_list, e.screen })
            //        .Name("UX_GuiScreenList_Screen_ScreenItem")
            //        .Unique();

            //    entity.Index(e => new { e.gui_screen_list, e.screen_order })
            //        .Name("UX_GuiScreenList_Screen_Order")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.GuiScreenListNavigation)
            //        .WithMany(p => p.GuiScreenListScreens)
            //        .ForeignKey(d => d.gui_screen_list)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_GuiScreenList_Screen_GUIScreenList");

            //    entity.HasOne(d => d.GUIPrepopList)
            //        .WithMany(p => p.GuiScreenListScreens)
            //        .ForeignKey(d => d.guiprepoplist_id)
            //        .ConstraintName("FK_GuiScreenList_Screen_GUIPrepopList");

            //    entity.HasOne(d => d.GUIScreenNavigation)
            //        .WithMany(p => p.GuiScreenListScreens)
            //        .ForeignKey(d => d.screen)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_GuiScreenList_Screen_GUIScreen");

            //    entity.HasOne(d => d.ValidationList)
            //        .WithMany(p => p.GuiScreenListScreens)
            //        .ForeignKey(d => d.validation_list_id)
            //        .ConstraintName("FK_GuiScreenList_Screen_ValidationList");
            //});

            modelBuilder
                    .Entity<GuiScreenListScreen>()
                    .Property(t => t.gui_screen_list)
                    .HasColumnAnnotation("FK_GuiScreenList_Screen_GUIScreenList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GuiScreenListScreen>()
                    .HasRequired(t => t.GuiScreenList)
                    .WithMany(t => t.GuiScreenListScreens)
                    .HasForeignKey(d => d.gui_screen_list)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<GuiScreenListScreen>()
                    .Property(t => t.guiprepoplist_id)
                    .HasColumnAnnotation("FK_GuiScreenList_Screen_GUIPrepopList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GuiScreenListScreen>()
                    .HasOptional(t => t.GUIPrepopList)
                    .WithMany(t => t.GuiScreenListScreens)
                    .HasForeignKey(d => d.guiprepoplist_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<GuiScreenListScreen>()
                    .Property(t => t.screen)
                    .HasColumnAnnotation("FK_GuiScreenList_Screen_GUIScreen", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GuiScreenListScreen>()
                    .HasRequired(t => t.GUIScreen)
                    .WithMany(t => t.GuiScreenListScreens)
                    .HasForeignKey(d => d.screen)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<GuiScreenListScreen>()
                    .Property(t => t.validation_list_id)
                    .HasColumnAnnotation("FK_GuiScreenList_Screen_ValidationList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<GuiScreenListScreen>()
                    .HasOptional(t => t.ValidationList)
                    .WithMany(t => t.GuiScreenListScreens)
                    .HasForeignKey(d => d.validation_list_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Language>(entity =>
            //{
            //    entity.HasKey(e => e.code)
            //        .Name("PK_Languages");

            //    entity.Property(e => e.code)
            //        //.Unicode(false)
            //        ;
            //});

            //modelBuilder.Entity<LanguageList>(entity =>
            //{
            //    entity.Index(e => e.default_language)
            //        .Name("idefault_language_LanguageList");

            //    entity.Property(e => e.default_language)
            //        //.Unicode(false)
            //        ;

            //    entity.Property(e => e.enabled).HasDefaultValueSql("((1))");

            //    entity.HasOne(d => d.LanguageNavigation)
            //        .WithMany(p => p.LanguageLists)
            //        .ForeignKey(d => d.default_language)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_LanguageList_Language");
            //});

            modelBuilder
                    .Entity<LanguageList>()
                    .Property(t => t.default_language)
                    .HasColumnAnnotation("FK_LanguageList_Language", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<LanguageList>()
                    .HasRequired(t => t.LanguageNavigation)
                    .WithMany(t => t.LanguageLists)
                    .HasForeignKey(d => d.default_language)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<LanguageListLanguage>(entity =>
            //{
            //    entity.Index(e => e.language_item)
            //        .Name("ilanguage_item_LanguageList_Language");

            //    entity.Index(e => e.language_list)
            //        .Name("ilanguage_list_LanguageList_Language");

            //    entity.Index(e => new { e.language_item, e.language_list })
            //        .Name("UX_LanguageList_Language_LanguageItem")
            //        .Unique();

            //    entity.Index(e => new { e.language_list, e.language_order })
            //        .Name("UX_LanguageList_Language_Order")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.Language)
            //        .WithMany(p => p.LanguageListLanguages)
            //        .ForeignKey(d => d.language_item)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_LanguageList_Language_Language");

            //    entity.HasOne(d => d.LanguageList)
            //        .WithMany(p => p.LanguageListLanguages)
            //        .ForeignKey(d => d.language_list)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_LanguageList_Language_LanguageList");
            //});

            modelBuilder
                    .Entity<LanguageListLanguage>()
                    .Property(t => t.language_item)
                    .HasColumnAnnotation("FK_LanguageList_Language_Language", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<LanguageListLanguage>()
                    .HasRequired(t => t.Language)
                    .WithMany(t => t.LanguageListLanguages)
                    .HasForeignKey(d => d.language_item)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<LanguageListLanguage>()
                    .Property(t => t.language_list)
                    .HasColumnAnnotation("FK_LanguageList_Language_LanguageList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<LanguageListLanguage>()
                    .HasRequired(t => t.LanguageList)
                    .WithMany(t => t.LanguageListLanguages)
                    .HasForeignKey(d => d.language_list)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<PasswordHistory>(entity =>
            //{
            //    entity.Index(e => e.User)
            //        .Name("iUser_PasswordHistory");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.UserNavigation)
            //        .WithMany(p => p.PasswordHistories)
            //        .ForeignKey(d => d.User)
            //        .ConstraintName("FK_PasswordHistory_User");
            //});

            modelBuilder
                    .Entity<PasswordHistory>()
                    .Property(t => t.User)
                    .HasColumnAnnotation("FK_PasswordHistory_User", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<PasswordHistory>()
                    .HasOptional(t => t.UserNavigation)
                    .WithMany(t => t.PasswordHistories)
                    .HasForeignKey(d => d.User)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<PasswordPolicy>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.use_history).HasDefaultValueSql("((1))");
            //});

            ////modelBuilder.Entity<Permission>(entity =>
            ////{
            ////    entity.Index(e => e.activity_id)
            ////        .Name("iactivity_id_Permission");

            ////    entity.Index(e => e.role_id)
            ////        .Name("irole_id_Permission");

            ////    entity.Index(e => new { e.role_id, e.activity_id })
            ////        .Name("UX_Permission")
            ////        .Unique();

            ////    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            ////    entity.HasOne(d => d.activity)
            ////        .WithMany(p => p.Permissions)
            ////        .ForeignKey(d => d.activity_id)
            ////        //.OnDelete(DeleteBehavior.ClientSetNull)
            ////        .ConstraintName("FK_Permission_Activity");

            ////    entity.HasOne(d => d.role)
            ////        .WithMany(p => p.Permissions)
            ////        .ForeignKey(d => d.role_id)
            ////        //.OnDelete(DeleteBehavior.ClientSetNull)
            ////        .ConstraintName("FK_Permission_Role");
            ////});

            modelBuilder
                    .Entity<Permission>()
                    .Property(t => t.activity_id)
                    .HasColumnAnnotation("FK_Permission_Activity", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Permission>()
                    .HasRequired(t => t.activity)
                    .WithMany(t => t.Permissions)
                    .HasForeignKey(d => d.activity_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Permission>()
                    .Property(t => t.role_id)
                    .HasColumnAnnotation("FK_Permission_Role", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Permission>()
                    .HasRequired(t => t.role)
                    .WithMany(t => t.Permissions)
                    .HasForeignKey(d => d.role_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<PrinterStatus>(entity =>
            //{
            //    entity.Index(e => e.printer_id)
            //        .Name("iprinter_id_PrinterStatus");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.modified).HasDefaultValueSql("(getdate())");
            //});

            //modelBuilder.Entity<Printout>(entity =>
            //{
            //    entity.Index(e => e.tx_id)
            //        .Name("itx_id_Printout");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.datetime).HasDefaultValueSql("(getdate())");

            //    entity.HasOne(d => d.tx)
            //        .WithMany(p => p.Printouts)
            //        .ForeignKey(d => d.tx_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_Printout_Transaction");
            //});

            modelBuilder
                    .Entity<Printout>()
                    .Property(t => t.tx_id)
                    .HasColumnAnnotation("FK_Printout_Transaction", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Printout>()
                    .HasRequired(t => t.tx)
                    .WithMany(t => t.Printouts)
                    .HasForeignKey(d => d.tx_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<Role>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");
            //});

            //modelBuilder.Entity<SessionException>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");
            //});

            //modelBuilder.Entity<TextItem>(entity =>
            //{
            //    entity.Index(e => e.Category)
            //        .Name("iCategory_xlns_TextItem_CDE8C5ED");

            //    entity.Index(e => e.TextItemTypeID)
            //        .Name("iTextItemTypeID_xlns_TextItem_2A2E0516");

            //    entity.Index(e => new { e.Name, e.Category })
            //        .Name("UX_UI_TextItem_name")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.CategoryNavigation)
            //        .WithMany(p => p.TextItems)
            //        .ForeignKey(d => d.Category)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_UI_TextItem_TextItemCategory");

            //    entity.HasOne(d => d.TextItemType)
            //        .WithMany(p => p.TextItems)
            //        .ForeignKey(d => d.TextItemTypeID)
            //        //.OnDelete(DeleteBehavior.Cascade)
            //        .ConstraintName("FK_UI_TextItem_TextItemType");
            //});

            modelBuilder
                    .Entity<TextItem>()
                    .Property(t => t.Category)
                    .HasColumnAnnotation("FK_UI_TextItem_TextItemCategory", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TextItem>()
                    .HasRequired(t => t.CategoryNavigation)
                    .WithMany(t => t.TextItems)
                    .HasForeignKey(d => d.Category)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<TextItem>()
                    .Property(t => t.TextItemTypeID)
                    .HasColumnAnnotation("FK_UI_TextItem_TextItemType", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TextItem>()
                    .HasOptional(t => t.TextItemType)
                    .WithMany(t => t.TextItems)
                    .HasForeignKey(d => d.TextItemTypeID)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<TextItemCategory>(entity =>
            //{
            //    entity.Index(e => e.Parent)
            //        .Name("iParent_xlns_TextItemCategory_051D04A6");

            //    entity.Index(e => e.name)
            //        .Name("UX_UI_TextItemCategory_name")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.ParentNavigation)
            //        .WithMany(p => p.InverseParentNavigation)
            //        .ForeignKey(d => d.Parent)
            //        .ConstraintName("FK_UI_TextItemCategory_TextItemCategory");
            //});

            modelBuilder
                    .Entity<TextItemCategory>()
                    .Property(t => t.Parent)
                    .HasColumnAnnotation("FK_UI_TextItemCategory_TextItemCategory", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TextItemCategory>()
                    .HasOptional(t => t.ParentNavigation)
                    .WithMany(t => t.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<TextItemType>(entity =>
            //{
            //    entity.Index(e => e.token)
            //        .Name("UX_UI_TextItemType")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            //});

            //modelBuilder.Entity<TextTranslation>(entity =>
            //{
            //    entity.Index(e => e.LanguageCode)
            //        .Name("iLanguageCode_xlns_TextTranslation_0EDE47B6");

            //    entity.Index(e => e.TextItemID)
            //        .Name("iTextItemID_xlns_TextTranslation_00B6C5AC");

            //    entity.Index(e => new { e.LanguageCode, e.TextItemID })
            //        .Name("UX_UI_Translation_Language_Pair")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.LanguageCode)
            //        //.Unicode(false)
            //        ;

            //    entity.HasOne(d => d.LanguageCodeNavigation)
            //        .WithMany(p => p.TextTranslations)
            //        .ForeignKey(d => d.LanguageCode)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_UI_Translation_Language");

            //    entity.HasOne(d => d.TextItem)
            //        .WithMany(p => p.TextTranslations)
            //        .ForeignKey(d => d.TextItemID)
            //        .ConstraintName("FK_UI_Translation_TextItem");
            //});

            modelBuilder
                    .Entity<TextTranslation>()
                    .Property(t => t.LanguageCode)
                    .HasColumnAnnotation("FK_UI_Translation_Language", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TextTranslation>()
                    .HasRequired(t => t.LanguageCodeNavigation)
                    .WithMany(t => t.TextTranslations)
                    .HasForeignKey(d => d.LanguageCode)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<TextTranslation>()
                    .Property(t => t.TextItemID)
                    .HasColumnAnnotation("FK_UI_Translation_TextItem", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TextTranslation>()
                    .HasRequired(t => t.TextItem)
                    .WithMany(t => t.TextTranslations)
                    .HasForeignKey(d => d.TextItemID)
                    .WillCascadeOnDelete(true);


            //modelBuilder.Entity<Transaction>(entity =>
            //{
            //    entity.Index(e => e.cit_id)
            //        .Name("icit_id_Transaction");

            //    entity.Index(e => e.device_id)
            //        .Name("idevice_Transaction");

            //    entity.Index(e => e.session_id)
            //        .Name("isession_id_Transaction");

            //    entity.Index(e => e.tx_currency)
            //        .Name("itx_currency_Transaction");

            //    entity.Index(e => e.tx_type)
            //        .Name("itx_type_Transaction");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.tx_currency)
            //        //.Unicode(false)
            //        ;

            //    entity.HasOne(d => d.AuthUserNavigation)
            //        .WithMany(p => p.TransactionAuthUsers)
            //        .ForeignKey(d => d.auth_user)
            //        .ConstraintName("FK_Transaction_auth_user");

            //    entity.HasOne(d => d.CIT)
            //        .WithMany(p => p.Transactions)
            //        .ForeignKey(d => d.cit_id)
            //        .ConstraintName("FK_Transaction_CIT");

            //    entity.HasOne(d => d.Device)
            //        .WithMany(p => p.Transactions)
            //        .ForeignKey(d => d.device_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_Transaction_DeviceList");

            //    entity.HasOne(d => d.InitUserNavigation)
            //        .WithMany(p => p.TransactionInitUsers)
            //        .ForeignKey(d => d.init_user)
            //        .ConstraintName("FK_Transaction_init_user");

            //    entity.HasOne(d => d.Session)
            //        .WithMany(p => p.Transactions)
            //        .ForeignKey(d => d.session_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_Transaction_DepositorSession");

            //    entity.HasOne(d => d.Currency)
            //        .WithMany(p => p.Transactions)
            //        .ForeignKey(d => d.tx_currency)
            //        .ConstraintName("FK_Transaction_Currency_Transaction");

            //    entity.HasOne(d => d.TransactionTypeListItemNavigation)
            //        .WithMany(p => p.Transactions)
            //        .ForeignKey(d => d.tx_type)
            //        .ConstraintName("FK_Transaction_TransactionTypeListItem");
            //});
            modelBuilder
                    .Entity<Transaction>()
                    .Property(t => t.auth_user)
                    .HasColumnAnnotation("FK_Transaction_auth_user", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Transaction>()
                    .HasOptional(t => t.AuthUserNavigation)
                    .WithMany(t => t.TransactionAuthUsers)
                    .HasForeignKey(d => d.auth_user)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Transaction>()
                    .Property(t => t.cit_id)
                    .HasColumnAnnotation("FK_Transaction_CIT", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Transaction>()
                    .HasOptional(t => t.CIT)
                    .WithMany(t => t.Transactions)
                    .HasForeignKey(d => d.cit_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Transaction>()
                    .Property(t => t.device_id)
                    .HasColumnAnnotation("FK_Transaction_DeviceList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Transaction>()
                    .HasRequired(t => t.Device)
                    .WithMany(t => t.Transactions)
                    .HasForeignKey(d => d.device_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Transaction>()
                    .Property(t => t.init_user)
                    .HasColumnAnnotation("FK_Transaction_init_user", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Transaction>()
                    .HasOptional(t => t.InitUserNavigation)
                    .WithMany(t => t.TransactionInitUsers)
                    .HasForeignKey(d => d.init_user)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Transaction>()
                    .Property(t => t.session_id)
                    .HasColumnAnnotation("FK_Transaction_DepositorSession", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Transaction>()
                    .HasRequired(t => t.Session)
                    .WithMany(t => t.Transactions)
                    .HasForeignKey(d => d.session_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Transaction>()
                    .Property(t => t.tx_currency)
                    .HasColumnAnnotation("FK_Transaction_Currency_Transaction", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Transaction>()
                    .HasRequired(t => t.Currency)
                    .WithMany(t => t.Transactions)
                    .HasForeignKey(d => d.tx_currency)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<Transaction>()
                    .Property(t => t.tx_type)
                    .HasColumnAnnotation("FK_Transaction_TransactionTypeListItem", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<Transaction>()
                    .HasOptional(t => t.TransactionTypeListItem)
                    .WithMany(t => t.Transactions)
                    .HasForeignKey(d => d.tx_type)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<TransactionException>(entity =>
            //{

            //    entity.Index(e => e.transaction_id)
            //        .Name("itransaction_id_exp_TransactionException_0990CC78");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");
            //});

            //modelBuilder.Entity<TransactionLimitList>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");
            //});

            //modelBuilder.Entity<TransactionLimitListItem>(entity =>
            //{
            //    entity.Index(e => e.currency_code)
            //        .Name("icurrency_code_TransactionLimitListItem");

            //    entity.Index(e => e.transactionitemlist_id)
            //        .Name("itransactionitemlist_id_TransactionLimitListItem");

            //    entity.Index(e => new { e.transactionitemlist_id, e.currency_code })
            //        .Name("UX_TransactionLimitListItem")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.currency_code)
            //        //.Unicode(false)
            //        ;

            //    entity.Property(e => e.prevent_underdeposit).HasDefaultValueSql("((1))");

            //    entity.HasOne(d => d.CurrencyNavigation)
            //        .WithMany(p => p.TransactionLimitListItems)
            //        .ForeignKey(d => d.currency_code)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_TransactionLimitListItem_Currency");

            //    entity.HasOne(d => d.TransactionItemlist)
            //        .WithMany(p => p.TransactionLimitListItems)
            //        .ForeignKey(d => d.transactionitemlist_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_TransactionLimitListItem_TransactionLimitList");
            //});

            modelBuilder
                    .Entity<TransactionLimitListItem>()
                    .Property(t => t.currency_code)
                    .HasColumnAnnotation("FK_TransactionLimitListItem_Currency", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionLimitListItem>()
                    .HasRequired(t => t.CurrencyNavigation)
                    .WithMany(t => t.TransactionLimitListItems)
                    .HasForeignKey(d => d.currency_code)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<TransactionLimitListItem>()
                    .Property(t => t.transactionitemlist_id)
                    .HasColumnAnnotation("FK_TransactionLimitListItem_TransactionLimitList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionLimitListItem>()
                    .HasRequired(t => t.TransactionItemlist)
                    .WithMany(t => t.TransactionLimitListItems)
                    .HasForeignKey(d => d.transactionitemlist_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<TransactionText>(entity =>
            //{
            //    entity.Index(e => e.FundsSource_caption)
            //        .Name("iFundsSource_caption_TransactionText");

            //    entity.Index(e => e.account_name_caption)
            //        .Name("iaccount_name_caption_TransactionText");

            //    entity.Index(e => e.account_number_caption)
            //        .Name("iaccount_number_caption_TransactionText");

            //    entity.Index(e => e.alias_account_name_caption)
            //        .Name("ialias_account_name_caption_TransactionText");

            //    entity.Index(e => e.alias_account_number_caption)
            //        .Name("ialias_account_number_caption_TransactionText");

            //    entity.Index(e => e.depositor_name_caption)
            //        .Name("idepositor_name_caption_TransactionText");

            //    entity.Index(e => e.disclaimer)
            //        .Name("idisclaimer_TransactionText");

            //    entity.Index(e => e.full_instructions)
            //        .Name("ifull_instructions_TransactionText");

            //    entity.Index(e => e.id_number_caption)
            //        .Name("iid_number_caption_TransactionText");

            //    entity.Index(e => e.listItem_caption)
            //        .Name("ilistItem_caption_TransactionText");

            //    entity.Index(e => e.narration_caption)
            //        .Name("inarration_caption_TransactionText");

            //    entity.Index(e => e.phone_number_caption)
            //        .Name("iphone_number_caption_TransactionText");

            //    entity.Index(e => e.receipt_template)
            //        .Name("ireceipt_template_TransactionText");

            //    entity.Index(e => e.reference_account_name_caption)
            //        .Name("ireference_account_name_caption_TransactionText");

            //    entity.Index(e => e.reference_account_number_caption)
            //        .Name("ireference_account_number_caption_TransactionText");

            //    entity.Index(e => e.terms)
            //        .Name("iterms_TransactionText");

            //    entity.Index(e => e.tx_item)
            //        .Name("itx_item_TransactionText");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.full_instructionsNavigation)
            //        .WithMany(p => p.TransactionTextfull_instructionsNavigation)
            //        .ForeignKey(d => d.full_instructions)
            //        .ConstraintName("FK_TransactionText_full_instructions");

            //    entity.HasOne(d => d.tx_itemNavigation)
            //        .WithMany(p => p.TransactionTexts)
            //        .ForeignKey(d => d.tx_item)
            //        .ConstraintName("FK_TransactionText_TransactionTypeListItem");
            //});






            modelBuilder.Entity<TransactionText>()
                .Property(t => t.full_instructions)
                .HasColumnAnnotation("FK_TransactionText_full_instructions", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.full_instructionsNavigation)
                .WithMany(p => p.TransactionTextfull_instructionsNavigation)
                .HasForeignKey(d => d.full_instructions);

            // modelBuilder.Entity<TransactionText>()
            //.Property(d => d.tx_item)
            //     .HasColumnAnnotation("FK_TransactionText_TransactionTypeListItem", new IndexAnnotation(new IndexAttribute()));

            // modelBuilder.Entity<TransactionText>()
            //.HasOptional(d => d.TransactionTypeListItemNavigation)
            //     .WithMany(p => p.TransactionTexts)
            //     .HasForeignKey(d => d.tx_item)
            //     .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.alias_account_number_caption)
                .HasColumnAnnotation("FK_TransactionText_Alias_Account_Number_Caption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.alias_account_number_captionNavigation)
                .WithMany(p => p.TransactionTextalias_account_number_captionNavigation)
                .HasForeignKey(d => d.alias_account_number_caption)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.alias_account_name_caption)
                .HasColumnAnnotation("FK_TransactionText_Alias_Account_Name_Caption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.alias_account_name_captionNavigation)
                .WithMany(p => p.TransactionTextalias_account_name_captionNavigation)
                .HasForeignKey(d => d.alias_account_name_caption)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.account_number_caption)
                .HasColumnAnnotation("FK_TransactionText_Account_Number_Caption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.account_number_captionNavigation)
                .WithMany(p => p.TransactionTextaccount_number_captionNavigation)
                .HasForeignKey(d => d.account_number_caption)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.account_name_caption)
                .HasColumnAnnotation("FK_TransactionText_Account_Name_Caption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.account_name_captionNavigation)
                .WithMany(p => p.TransactionTextaccount_name_captionNavigation)
                .HasForeignKey(d => d.account_name_caption)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.depositor_name_caption)
                .HasColumnAnnotation("FK_TransactionText_Depositor_Name_Caption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.depositor_name_captionNavigation)
                .WithMany(p => p.TransactionTextdepositor_name_captionNavigation)
                .HasForeignKey(d => d.depositor_name_caption)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.disclaimer)
                .HasColumnAnnotation("FK_TransactionText_Disclaimers", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.disclaimerNavigation)
                .WithMany(p => p.TransactionTextdisclaimerNavigations)
                .HasForeignKey(d => d.disclaimer)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.FundsSource_caption)
                .HasColumnAnnotation("FK_TransactionText_Funds_Source_Caption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.FundsSource_captionNavigation)
                .WithMany(p => p.TransactionTextFundsSource_captionNavigation)
                .HasForeignKey(d => d.FundsSource_caption)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.id_number_caption)
                .HasColumnAnnotation("FK_TransactionText_IdNumberCaption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.id_number_captionNavigation)
                .WithMany(p => p.TransactionTextid_number_captionNavigation)
                .HasForeignKey(d => d.id_number_caption);

            modelBuilder.Entity<TransactionText>()
                .Property(d => d.listItem_caption)
                .HasColumnAnnotation("FK_TransactionText_ListItemCaption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.listItem_captionNavigation)
                .WithMany(p => p.TransactionTextlistItem_captionNavigation)
                .HasForeignKey(d => d.listItem_caption)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
                .Property(d => d.narration_caption)
                .HasColumnAnnotation("FK_TransactionText_NarrationCaption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
                .HasOptional(d => d.narration_captionNavigation)
                .WithMany(p => p.TransactionTextnarration_captionNavigation)
                .HasForeignKey(d => d.narration_caption)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
                .Property(d => d.receipt_template)
                .HasColumnAnnotation("FK_TransactionText_ReceiptTemplate", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.receipt_templateNavigation)
                .WithMany(p => p.TransactionTextreceipt_templateNavigation)
                .HasForeignKey(d => d.receipt_template)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.phone_number_caption)
                .HasColumnAnnotation("FK_TransactionText_PhoneNumberCaption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.phone_number_captionNavigation)
                .WithMany(p => p.TransactionTextphone_number_captionNavigation)
                .HasForeignKey(d => d.phone_number_caption)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.reference_account_name_caption)
                .HasColumnAnnotation("FK_TransactionText_ReferenceAccountNameCaption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.reference_account_name_captionNavigation)
                .WithMany(p => p.TransactionTextreference_account_name_captionNavigation)
                .HasForeignKey(d => d.reference_account_name_caption)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.reference_account_number_caption)
                .HasColumnAnnotation("FK_TransactionText_ReferenceAccountNumberCaption", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.reference_account_number_captionNavigation)
                .WithMany(p => p.TransactionTextreference_account_number_captionNavigation)
                .HasForeignKey(d => d.reference_account_number_caption)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<TransactionText>()
           .Property(d => d.terms)
                .HasColumnAnnotation("FK_TransactionText_Terms", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
           .HasOptional(d => d.termsNavigation)
                .WithMany(p => p.TransactionTexttermsNavigations)
                .HasForeignKey(d => d.terms)
                .WillCascadeOnDelete(true);

            // modelBuilder.Entity<TransactionText>()
            //.Property(d => d.alias_account_number_caption)
            //     .HasColumnAnnotation("FK_TransactionText_ValidationTextErrorMessages", new IndexAnnotation(new IndexAttribute()));

            // modelBuilder.Entity<TransactionText>()
            //.HasOptional(d => d.ValidationTextErrorMessageNavigation)
            //     .WithMany(p => p.ValidationTextErrorMessages)
            //     .HasForeignKey(d => d.ValidationTextErrorMessage)
            //     .WillCascadeOnDelete(true);

            // modelBuilder.Entity<TransactionText>()
            //.Property(d => d.validation_text_success_message)
            //     .HasColumnAnnotation("FK_TransactionText_ValidationTextSuccessMessages", new IndexAnnotation(new IndexAttribute()));

            // modelBuilder.Entity<TransactionText>()
            //.HasOptional(d => d.validation_text_success_message)
            //     .WithMany(p => p.ValidationTextSuccessMessages)
            //     .HasForeignKey(d => d.validation_text_success_message)
            //     .WillCascadeOnDelete(true);  

            //modelBuilder
            //        .Entity<TransactionText>()
            //        .Property(t => t.full_instructions)
            //        .HasColumnAnnotation("FK_TransactionText_full_instructions", new IndexAnnotation(new IndexAttribute()));

            //modelBuilder.Entity<TransactionText>()
            //        .HasOptional(t => t.full_instructionsNavigation)
            //        .WithMany(t => t.TransactionTextfull_instructionsNavigation)
            //        .HasForeignKey(d => d.full_instructions)
            //        .WillCascadeOnDelete(true);

            //modelBuilder.Entity<TransactionText>()
            //            .HasRequired<TransactionTypeListItem>(s => s.TransactionTypeListItemNavigation)
            //            .WithMany(g => g.TransactionTexts)
            //            .HasForeignKey<int>(s => s.tx_item);

            modelBuilder
                    .Entity<TransactionText>()
                    .Property(t => t.tx_item)
                    .HasColumnAnnotation("FK_TransactionText_TransactionTypeListItem", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionText>()
                    .HasRequired(t => t.TransactionTypeListItemNavigation)
                    .WithMany(t => t.TransactionTexts)
                    .HasForeignKey(d => d.tx_item)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<TransactionTypeListItem>(entity =>
            //{
            //    entity.Index(e => e.default_account_currency)
            //        .Name("idefault_account_currency_TransactionTypeListItem");

            //    entity.Index(e => e.tx_limit_list)
            //        .Name("itx_limit_list_TransactionTypeListItem");

            //    entity.Index(e => e.tx_text)
            //        .Name("itx_text_TransactionTypeListItem");

            //    entity.Index(e => e.tx_type)
            //        .Name("itx_type_TransactionTypeListItem");

            //    entity.Index(e => e.tx_type_guiscreenlist)
            //        .Name("itx_type_guiscreenlist_TransactionTypeListItem");

            //    entity.Property(e => e.default_account_currency)
            //        //.Unicode(false)

            //        .HasDefaultValueSql("('KES')");

            //    entity.Property(e => e.enabled).HasDefaultValueSql("((1))");

            //    entity.Property(e => e.tx_type_guiscreenlist).HasDefaultValueSql("((1))");

            //    entity.HasOne(d => d.DefaultAccountCurrencyNavigation)
            //        .WithMany(p => p.TransactionTypeListItems)
            //        .ForeignKey(d => d.default_account_currency)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_TransactionTypeListItem_Currency");

            //    entity.HasOne(d => d.TransactionLimitListNavigation)
            //        .WithMany(p => p.TransactionTypeListItems)
            //        .ForeignKey(d => d.tx_limit_list)
            //        .ConstraintName("FK_TransactionTypeListItem_TransactionLimitList");

            //    entity.HasOne(d => d.TransactionTextNavigation)
            //        .WithMany(p => p.TransactionTypeListItems)
            //        .ForeignKey(d => d.tx_text)
            //        // .OnDelete(DeleteBehavior.SetNull)
            //        .ConstraintName("FK_TransactionTypeListItem_TransactionText");

            //    entity.HasOne(d => d.TransactionTypeNavigation)
            //        .WithMany(p => p.TransactionTypeListItems)
            //        .ForeignKey(d => d.tx_type)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_TransactionListItem_TransactionType");

            //    entity.HasOne(d => d.GUIScreenListNavigation)
            //        .WithMany(p => p.TransactionTypeListItems)
            //        .ForeignKey(d => d.tx_type_guiscreenlist)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_TransactionTypeListItem_GUIScreenList");
            //});

            modelBuilder
                    .Entity<TransactionTypeListItem>()
                    .Property(t => t.default_account_currency)
                    .HasColumnAnnotation("FK_TransactionTypeListItem_Currency", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionTypeListItem>()
                    .HasRequired(t => t.DefaultAccountCurrency)
                    .WithMany(t => t.TransactionTypeListItems)
                    .HasForeignKey(d => d.default_account_currency)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<TransactionTypeListItem>()
                    .Property(t => t.tx_limit_list)
                    .HasColumnAnnotation("FK_TransactionTypeListItem_TransactionLimitList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionTypeListItem>()
                    .HasOptional(t => t.TransactionLimitList)
                    .WithMany(t => t.TransactionTypeListItems)
                    .HasForeignKey(d => d.tx_limit_list)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<TransactionTypeListItem>()
                    .Property(t => t.tx_text)
                    .HasColumnAnnotation("FK_TransactionTypeListItem_TransactionText", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionTypeListItem>()
                    .HasOptional(t => t.TransactionText)
                    .WithMany(t => t.TransactionTypeListItems)
                    .HasForeignKey(d => d.tx_text)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<TransactionTypeListItem>()
                    .Property(t => t.tx_type)
                    .HasColumnAnnotation("FK_TransactionListItem_TransactionType", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionTypeListItem>()
                    .HasRequired(t => t.TransactionType)
                    .WithMany(t => t.TransactionTypeListItems)
                    .HasForeignKey(d => d.tx_type)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<TransactionTypeListItem>()
                    .Property(t => t.tx_type_guiscreenlist)
                    .HasColumnAnnotation("FK_TransactionTypeListItem_GUIScreenList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionTypeListItem>()
                    .HasRequired(t => t.GUIScreenList)
                    .WithMany(t => t.TransactionTypeListItems)
                    .HasForeignKey(d => d.tx_type_guiscreenlist)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<TransactionTypeListTransactionTypeListItem>(entity =>
            //{
            //    entity.Index(e => e.txtype_list)
            //        .Name("itxtype_list_TransactionTypeList_TransactionTypeListItem");

            //    entity.Index(e => e.txtype_list_item)
            //        .Name("itxtype_list_item_TransactionTypeList_TransactionTypeListItem");

            //    entity.Index(e => new { e.txtype_list, e.list_order })
            //        .Name("UX_TransactionTypeList_TransactionTypeListItem_Order")
            //        .Unique();

            //    entity.Index(e => new { e.txtype_list, e.txtype_list_item })
            //        .Name("UX_TransactionTypeList_TransactionTypeListItem_Item")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.TransactionTypeList)
            //        .WithMany(p => p.TransactionTypeListTransactionTypeListItems)
            //        .ForeignKey(d => d.txtype_list)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_TransactionTypeList_TransactionTypeListItem_TransactionTypeList");

            //    entity.HasOne(d => d.TransactionTypeListItem)
            //        .WithMany(p => p.TransactionTypeListTransactionTypeListItems)
            //        .ForeignKey(d => d.txtype_list_item)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_TransactionTypeList_TransactionTypeListItem_TransactionTypeListItem");
            //});

            modelBuilder
                    .Entity<TransactionTypeListTransactionTypeListItem>()
                    .Property(t => t.txtype_list)
                    .HasColumnAnnotation("FK_TransactionTypeList_TransactionTypeListItem_TransactionTypeList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionTypeListTransactionTypeListItem>()
                    .HasRequired(t => t.TransactionTypeList)
                    .WithMany(t => t.TransactionTypeListTransactionTypeListItems)
                    .HasForeignKey(d => d.txtype_list)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<TransactionTypeListTransactionTypeListItem>()
                    .Property(t => t.txtype_list_item)
                    .HasColumnAnnotation("FK_TransactionTypeList_TransactionTypeListItem_TransactionTypeListItem", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TransactionTypeListTransactionTypeListItem>()
                    .HasRequired(t => t.TransactionTypeListItem)
                    .WithMany(t => t.TransactionTypeListTransactionTypeListItems)
                    .HasForeignKey(d => d.txtype_list_item)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<UptimeComponentState>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.created).HasDefaultValueSql("(getdate())");
            //});

            //modelBuilder.Entity<UptimeMode>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.created).HasDefaultValueSql("(getdate())");
            //});

            //modelBuilder.Entity<UserGroup>(entity =>
            //{
            //    entity.Index(e => e.parent_group)
            //        .Name("iparent_group_UserGroup");

            //    entity.HasOne(d => d.parent_groupNavigation)
            //        .WithMany(p => p.Inverseparent_groupNavigation)
            //        .ForeignKey(d => d.parent_group)
            //        .ConstraintName("FK_UserGroup_UserGroup");
            //});

            modelBuilder
                    .Entity<UserGroup>()
                    .Property(t => t.parent_group)
                    .HasColumnAnnotation("FK_UserGroup_UserGroup", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<UserGroup>()
                    .HasOptional(t => t.parent_groupNavigation)
                    .WithMany(t => t.Inverseparent_groupNavigation)
                    .HasForeignKey(d => d.parent_group)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<UserLock>(entity =>
            //{
            //    entity.Index(e => e.ApplicationUserLoginDetail)
            //        .Name("iApplicationUserLoginDetail_UserLock");

            //    entity.Index(e => e.InitiatingUser)
            //        .Name("iInitiatingUser_UserLock");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.InitiatingUserNavigation)
            //        .WithMany(p => p.UserLocks)
            //        .ForeignKey(d => d.InitiatingUser)
            //        .ConstraintName("FK_UserLock_InitiatingUser");
            //});

            modelBuilder
                    .Entity<UserLock>()
                    .Property(t => t.InitiatingUser)
                    .HasColumnAnnotation("FK_UserLock_InitiatingUser", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<UserLock>()
                    .HasOptional(t => t.InitiatingUserNavigation)
                    .WithMany(t => t.UserLocks)
                    .HasForeignKey(d => d.InitiatingUser)
                    .WillCascadeOnDelete(true);


            //modelBuilder.Entity<ValidationItem>(entity =>
            //{
            //    entity.Index(e => e.type_id)
            //        .Name("itype_id_ValidationItem");

            //    entity.Index(e => e.validation_text_id)
            //        .Name("ivalidation_text_id_ValidationItem");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.ValidationType)
            //        .WithMany(p => p.ValidationItems)
            //        .ForeignKey(d => d.type_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_ValidationItem_ValidationType");

            //    entity.HasOne(d => d.ValidationText)
            //        .WithMany(p => p.ValidationItems)
            //        .ForeignKey(d => d.validation_text_id)
            //        .ConstraintName("FK_ValidationItem_ValidationText");
            //});

            modelBuilder
                    .Entity<ValidationItem>()
                    .Property(t => t.type_id)
                    .HasColumnAnnotation("FK_ValidationItem_ValidationType", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ValidationItem>()
                    .HasRequired(t => t.ValidationType)
                    .WithMany(t => t.ValidationItems)
                    .HasForeignKey(d => d.type_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<ValidationItem>()
                    .Property(t => t.validation_text_id)
                    .HasColumnAnnotation("FK_ValidationItem_ValidationText", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ValidationItem>()
                    .HasOptional(t => t.ValidationText)
                    .WithMany(t => t.ValidationItems)
                    .HasForeignKey(d => d.validation_text_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<ValidationItemValue>(entity =>
            //{
            //    entity.Index(e => e.validation_item_id)
            //        .Name("ivalidation_item_id_ValidationItemValue");

            //    entity.Index(e => new { e.validation_item_id, e.order })
            //        .Name("UX_ValidationItemValue")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.validation_item)
            //        .WithMany(p => p.ValidationItemValues)
            //        .ForeignKey(d => d.validation_item_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_ValidationItemValue_ValidationItem");
            //});

            //modelBuilder
            //        .Entity<ValidationItemValue>()
            //        .Property(t => t.validation_item_id)
            //        .IsUnicode(true);

            modelBuilder
                    .Entity<ValidationItemValue>()
                    .Property(t => t.validation_item_id)
                    .HasColumnAnnotation("FK_ValidationItemValue_ValidationItem", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ValidationItemValue>()
                    .HasRequired(t => t.validation_item)
                    .WithMany(t => t.ValidationItemValues)
                    .HasForeignKey(d => d.validation_item_id)
                    .WillCascadeOnDelete(true);
            //modelBuilder.Entity<ValidationList>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");
            //});

            //modelBuilder.Entity<ValidationListValidationItem>(entity =>
            //{
            //    entity.Index(e => e.validation_item_id)
            //        .Name("ivalidation_item_id_ValidationList_ValidationItem");

            //    entity.Index(e => e.validation_list_id)
            //        .Name("ivalidation_list_id_ValidationList_ValidationItem");

            //    entity.Index(e => new { e.validation_item_id, e.validation_list_id })
            //        .Name("UX_ValidationList_ValidationItem_UniqueItem")
            //        .Unique();

            //    entity.Index(e => new { e.validation_list_id, e.order })
            //        .Name("UX_ValidationList_ValidationItem_UniqueOrder")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.enabled).HasDefaultValueSql("((1))");

            //    entity.HasOne(d => d.ValidationItem)
            //        .WithMany(p => p.ValidationListValidationItems)
            //        .ForeignKey(d => d.validation_item_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_ValidationList_ValidationItem_ValidationItem");

            //    entity.HasOne(d => d.ValidationList)
            //        .WithMany(p => p.ValidationListValidationItems)
            //        .ForeignKey(d => d.validation_list_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_ValidationList_ValidationItem_ValidationList");
            //});

            modelBuilder
                    .Entity<ValidationListValidationItem>()
                    .Property(t => t.validation_item_id)
                    .HasColumnAnnotation("FK_ValidationList_ValidationItem_ValidationItem", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ValidationListValidationItem>()
                    .HasRequired(t => t.ValidationItem)
                    .WithMany(t => t.ValidationListValidationItems)
                    .HasForeignKey(d => d.validation_item_id)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<ValidationListValidationItem>()
                    .Property(t => t.validation_list_id)
                    .HasColumnAnnotation("FK_ValidationList_ValidationItem_ValidationList", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ValidationListValidationItem>()
                    .HasRequired(t => t.ValidationList)
                    .WithMany(t => t.ValidationListValidationItems)
                    .HasForeignKey(d => d.validation_list_id)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<ValidationText>(entity =>
            //{
            //    entity.Index(e => e.error_message)
            //        .Name("ierror_message_ValidationText");

            //    entity.Index(e => e.success_message)
            //        .Name("isuccess_message_ValidationText");

            //    entity.Index(e => e.validation_item_id)
            //        .Name("ivalidation_item_id_ValidationText");

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.validation_item)
            //        .WithMany(p => p.ValidationTexts)
            //        .ForeignKey(d => d.validation_item_id)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_ValidationText_ValidationItem");
            //});


            modelBuilder
                    .Entity<ValidationText>()
                    .Property(t => t.validation_item_id)
                    .HasColumnAnnotation("FK_ValidationText_ValidationItem", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ValidationText>()
                    .HasRequired(t => t.validation_item)
                    .WithMany(t => t.ValidationTexts)
                    .HasForeignKey(d => d.validation_item_id)
                    .WillCascadeOnDelete(true);



            modelBuilder
                    .Entity<ValidationText>()
                    .Property(t => t.error_message)
                    .HasColumnAnnotation("FK_ValidationText_ErrorMessage", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ValidationText>()
                    .HasRequired(t => t.error_messageNavigation)
                    .WithMany(t => t.ValidationTexterror_messageNavigation)
                    .HasForeignKey(d => d.error_message)
                    .WillCascadeOnDelete(true);


            modelBuilder
                    .Entity<ValidationText>()
                    .Property(t => t.success_message)
                    .HasColumnAnnotation("FK_ValidationText_SuccessMessage", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ValidationText>()
                    .HasRequired(t => t.success_messageNavigation)
                    .WithMany(t => t.ValidationTextsuccess_messageNavigation)
                    .HasForeignKey(d => d.success_message)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<ValidationType>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");
            //});

            //modelBuilder.Entity<sysTextItem>(entity =>
            //{
            //    entity.Index(e => e.Category)
            //        .Name("iCategory_xlns_sysTextItem_A264365A");

            //    entity.Index(e => e.TextItemTypeID)
            //        .Name("iTextItemTypeID_xlns_sysTextItem_BD18CE82");

            //    entity.Index(e => e.Token)
            //        .Name("UX_SysTextItem_name")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.CategoryNavigation)
            //        .WithMany(p => p.sysTextItems)
            //        .ForeignKey(d => d.Category)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_SysTextItem_SysTextItemCategory");

            //    entity.HasOne(d => d.TextItemType)
            //        .WithMany(p => p.sysTextItems)
            //        .ForeignKey(d => d.TextItemTypeID)
            //        .ConstraintName("FK_sysTextItem_sysTextItemType");
            //});

            modelBuilder
                    .Entity<sysTextItem>()
                    .Property(t => t.Token)
                    .IsUnicode(true);

            modelBuilder
                    .Entity<sysTextItem>()
                    .Property(t => t.Category)
                    .HasColumnAnnotation("FK_SysTextItem_SysTextItemCategory", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<sysTextItem>()
                    .HasRequired(t => t.CategoryNavigation)
                    .WithMany(t => t.sysTextItems)
                    .HasForeignKey(d => d.Category)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<sysTextItem>()
                    .Property(t => t.TextItemTypeID)
                    .HasColumnAnnotation("FK_sysTextItem_sysTextItemType", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<sysTextItem>()
                    .HasOptional(t => t.TextItemType)
                    .WithMany(t => t.sysTextItems)
                    .HasForeignKey(d => d.TextItemTypeID)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<sysTextItemCategory>(entity =>
            //{
            //    entity.Index(e => e.Parent)
            //        .Name("iParent_xlns_sysTextItemCategory_51488F7B");

            //    entity.Index(e => e.name)
            //        .Name("UX_TextItemCategory_name")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.HasOne(d => d.ParentNavigation)
            //        .WithMany(p => p.InverseParentNavigation)
            //        .ForeignKey(d => d.Parent)
            //        .ConstraintName("FK_TextItemCategory_TextItemCategory");
            //});

            modelBuilder
                    .Entity<sysTextItemCategory>()
                    .Property(t => t.name)
                    .IsUnicode(true);

            modelBuilder
                    .Entity<sysTextItemCategory>()
                    .Property(t => t.Parent)
                    .HasColumnAnnotation("FK_TextItemCategory_TextItemCategory", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<sysTextItemCategory>()
                    .HasOptional(t => t.ParentNavigation)
                    .WithMany(t => t.InverseParentNavigation)
                    .HasForeignKey(d => d.Parent)
                    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<sysTextItemType>(entity =>
            //{
            //    entity.Property(e => e.id).HasDefaultValueSql("(newid())");
            //});

            //modelBuilder.Entity<sysTextTranslation>(entity =>
            //{
            //    entity.Index(e => e.LanguageCode)
            //        .Name("iLanguageCode_xlns_sysTextTranslation_03BB080F");

            //    entity.Index(e => e.SysTextItemID)
            //        .Name("iSysTextItemID_xlns_sysTextTranslation_7FDC4652");

            //    entity.Index(e => new { e.LanguageCode, e.SysTextItemID })
            //        .Name("UX_Translation_Language_Pair")
            //        .Unique();

            //    entity.Property(e => e.id).HasDefaultValueSql("(newsequentialid())");

            //    entity.Property(e => e.LanguageCode)
            //        //.Unicode(false)
            //        ;

            //    entity.HasOne(d => d.LanguageCodeNavigation)
            //        .WithMany(p => p.sysTextTranslations)
            //        .ForeignKey(d => d.LanguageCode)
            //        //.OnDelete(DeleteBehavior.ClientSetNull)
            //        .ConstraintName("FK_sysTextTranslation_Language");

            //    entity.HasOne(d => d.SysTextItem)
            //        .WithMany(p => p.sysTextTranslations)
            //        .ForeignKey(d => d.SysTextItemID)
            //        .ConstraintName("FK_sysTextTranslation_sysTextItem");
            //});

            modelBuilder
                    .Entity<sysTextTranslation>()
                    .Property(t => t.LanguageCode)
                    .HasColumnAnnotation("FK_sysTextTranslation_Language", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<sysTextTranslation>()
                    .HasRequired(t => t.LanguageCodeNavigation)
                    .WithMany(t => t.sysTextTranslations)
                    .HasForeignKey(d => d.LanguageCode)
                    .WillCascadeOnDelete(true);

            modelBuilder
                    .Entity<sysTextTranslation>()
                    .Property(t => t.SysTextItemID)
                    .HasColumnAnnotation("FK_sysTextTranslation_sysTextItem", new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<sysTextTranslation>()
                    .HasRequired(t => t.SysTextItem)
                    .WithMany(t => t.sysTextTranslations)
                    .HasForeignKey(d => d.SysTextItemID)
                    .WillCascadeOnDelete(true);


        }


        public virtual ObjectResult<GetCITDenominationByDates_Result> GetCITDenominationByDates(
          DateTime? startDate,
          DateTime? endDate)
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCITDenominationByDates_Result>(nameof(GetCITDenominationByDates), startDate.HasValue ? new ObjectParameter(nameof(startDate), startDate) : new ObjectParameter(nameof(startDate), typeof(DateTime)), endDate.HasValue ? new ObjectParameter(nameof(endDate), endDate) : new ObjectParameter(nameof(endDate), typeof(DateTime)));
        }
    }
}