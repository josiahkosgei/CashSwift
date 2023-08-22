
//DatabaseUpdate.Updater


using CashSwift.Library.Standard.Security;
using CashSwiftCashControlPortal.Module.BusinessObjects.ApplicationConfiguration;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift;
using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
using CashSwiftCashControlPortal.Module.BusinessObjects.CITs;
using CashSwiftCashControlPortal.Module.BusinessObjects.Devices;
using CashSwiftCashControlPortal.Module.BusinessObjects.Monitoring;
using CashSwiftCashControlPortal.Module.BusinessObjects.Server;
using CashSwiftCashControlPortal.Module.BusinessObjects.Transactions;
using CashSwiftCashControlPortal.Module.Controllers;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using Newtonsoft.Json;
using System;

namespace CashSwiftCashControlPortal.Module.DatabaseUpdate
{
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion)
          : base(objectSpace, currentDBVersion)
        {
        }

        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
            UserTextItemUpdater.Update(ObjectSpace, CurrentDBVersion);
            SystemTextItemUpdate.Update(ObjectSpace, CurrentDBVersion);
            ConfigCategory configCategory = ObjectSpace.FindObject<ConfigCategory>(new BinaryOperator("name", "API"));
            if (configCategory == null)
            {
                configCategory = ObjectSpace.CreateObject<ConfigCategory>();
                configCategory.name = "API";
                configCategory.description = " APIs settings";
                configCategory.Save();
            }
            if (ObjectSpace.FindObject<Config>(new BinaryOperator("name", "API_CDM_GUI_URI")) == null)
            {
                Config config = ObjectSpace.CreateObject<Config>();
                config.name = "API_CDM_GUI_URI";
                config.description = "CDM.GUICONTROL.API Endpoint URI";
                config.category_id = configCategory;
                config.Save();
            }
            if (ObjectSpace.FindObject<Config>(new BinaryOperator("name", "API_AUTH_API_URI")) == null)
            {
                Config config = ObjectSpace.CreateObject<Config>();
                config.name = "API_AUTH_API_URI";
                config.description = "Endpoint URI";
                config.category_id = configCategory;
                config.Save();
            }
            if (ObjectSpace.FindObject<Config>(new BinaryOperator("name", "API_COMMSERV_URI")) == null)
            {
                Config config = ObjectSpace.CreateObject<Config>();
                config.name = "API_COMMSERV_URI";
                config.description = "Endpoint URI";
                config.category_id = configCategory;
                config.Save();
            }
            if (ObjectSpace.FindObject<Config>(new BinaryOperator("name", "API_INTEGRATION_URI")) == null)
            {
                Config config = ObjectSpace.CreateObject<Config>();
                config.name = "API_INTEGRATION_URI";
                config.description = "Endpoint URI";
                config.category_id = configCategory;
                config.Save();
            }
            Guid guid = Guid.NewGuid();
            byte[] random = PasswordGenerator.GenerateRandom(32L);
            if (ObjectSpace.FindObject<APIUser>(new BinaryOperator("Name", "WEB_PORTAL_APP_ID")) == null)
            {
                APIUser apiUser = ObjectSpace.CreateObject<APIUser>();
                apiUser.id = Guid.NewGuid();
                apiUser.Name = "WEB_PORTAL_APP_ID";
                apiUser.Enabled = true;
                apiUser.AppId = guid;
                apiUser.AppKey = random;
                apiUser.Save();
            }
            if (ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "INTEGRATION_URL")) == null)
            {
                ServerConfiguration serverConfiguration = ObjectSpace.CreateObject<ServerConfiguration>();
                serverConfiguration.config_key = "INTEGRATION_URL";
                serverConfiguration.config_value = "";
                serverConfiguration.Save();
            }
            if (ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_URL")) == null)
            {
                ServerConfiguration serverConfiguration = ObjectSpace.CreateObject<ServerConfiguration>();
                serverConfiguration.config_key = "WEB_PORTAL_URL";
                serverConfiguration.config_value = "";
                serverConfiguration.Save();
            }
            if (ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_APP_ID")) == null)
            {
                ServerConfiguration serverConfiguration = ObjectSpace.CreateObject<ServerConfiguration>();
                serverConfiguration.config_key = "WEB_PORTAL_APP_ID";
                serverConfiguration.config_value = guid.ToString();
                serverConfiguration.Save();
            }
            if (ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_APP_KEY")) == null)
            {
                ServerConfiguration serverConfiguration = ObjectSpace.CreateObject<ServerConfiguration>();
                serverConfiguration.config_key = "WEB_PORTAL_APP_KEY";
                serverConfiguration.config_value = Convert.ToBase64String(random);
                serverConfiguration.Save();
            }
            if (ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_LOGIN_CONFIG")) == null)
            {
                ServerConfiguration serverConfiguration = ObjectSpace.CreateObject<ServerConfiguration>();
                serverConfiguration.config_key = "WEB_PORTAL_LOGIN_CONFIG";
                serverConfiguration.config_value = JsonConvert.SerializeObject(new LoginConfiguration(), Formatting.Indented);
                serverConfiguration.Save();
            }
            if (ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_EMAIL_CONFIG")) == null)
            {
                ServerConfiguration serverConfiguration = ObjectSpace.CreateObject<ServerConfiguration>();
                serverConfiguration.config_key = "WEB_PORTAL_EMAIL_CONFIG";
                serverConfiguration.config_value = JsonConvert.SerializeObject(new EmailConfiguration(), Formatting.Indented);
                serverConfiguration.Save();
            }
            if (ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "CDM_REPORT_SAVE_PATH")) == null)
            {
                ServerConfiguration serverConfiguration = ObjectSpace.CreateObject<ServerConfiguration>();
                serverConfiguration.config_key = "CDM_REPORT_SAVE_PATH";
                serverConfiguration.config_value = "c:\\Server\\Reports\\UptimeReport\\{0}\\{1}";
                serverConfiguration.Save();
            }
            if (ObjectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "CDM_REPORT_URL")) == null)
            {
                ServerConfiguration serverConfiguration = ObjectSpace.CreateObject<ServerConfiguration>();
                serverConfiguration.config_key = "CDM_REPORT_URL";
                serverConfiguration.config_value = "https://localhost:44348/";
                serverConfiguration.Save();
            }
            if (ObjectSpace.GetObjects(typeof(PasswordPolicy)).Count == 0)
            {
                PasswordPolicy passwordPolicy = ObjectSpace.CreateObject<PasswordPolicy>();
                passwordPolicy.allowed_special = "";
                passwordPolicy.expiry_days = 90;
                passwordPolicy.history_size = 3;
                passwordPolicy.id = Guid.NewGuid();
                passwordPolicy.min_digits = 1;
                passwordPolicy.min_length = 8;
                passwordPolicy.min_lowercase = 1;
                passwordPolicy.min_special = 1;
                passwordPolicy.min_uppercase = 1;
                passwordPolicy.use_history = true;
                passwordPolicy.Save();
            }
            ObjectSpace.CommitChanges();
            ObjectSpace.CommitChanges();
        }

        private UserGroup CreateManagers(UserGroup ParentGroup)
        {
            WebPortalRole webPortalRole = ObjectSpace.FindObject<WebPortalRole>(new BinaryOperator("Name", "Branch Manager"));
            if (webPortalRole == null)
            {
                webPortalRole = ObjectSpace.CreateObject<WebPortalRole>();
                webPortalRole.Name = "Branch Manager";
                webPortalRole.IsAdministrative = false;
                webPortalRole.AddObjectPermission<ApplicationUser>("Read;Navigate", "[id] = CurrentUserId() && IsAllowedByUserGroup([id])", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddMemberPermission<ApplicationUser>("Write", "ChangePasswordOnFirstLogon", "[id] = CurrentUserId() && IsAllowedByUserGroup([id])", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddMemberPermission<ApplicationUser>("Write", "StoredPassword", "[id] = CurrentUserId() && IsAllowedByUserGroup([id])", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddObjectPermission<ApplicationUser>("Read;Navigate", "IsAllowedByUserGroup([id])", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddObjectPermission<ApplicationUserLoginDetail>("Read;Write;Delete;Navigate", "IsAllowedByUserGroup([User].[id])", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddObjectPermission<WebPortalLoginLogEntry>("Read;Write;Delete;Navigate", "IsAllowedByUserGroup([ApplicationUserLoginDetail].[User].[id])", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddObjectPermission<UserLockLogEntry>("Read;Write;Delete;Navigate", "IsAllowedByUserGroup([ApplicationUserLoginDetail].[User].[id])", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<WebPortalRole>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<ModelDifference>("Read;Write", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<ModelDifferenceAspect>("Read;Write", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddNavigationPermission("CIT", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddNavigationPermission("Device Status", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddNavigationPermission("Transaction", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddNavigationPermission("My Details", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddNavigationPermission("Application Log", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddNavigationPermission(" Communication Service Status", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<Bank>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<Branch>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<BusinessObjects.ApplicationConfiguration.Country>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<Currency>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<Language>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<CITDenominations>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<CITPrintout>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<CIT>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<Device>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<DeviceType>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<CommunicationServiceStatus>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<DeviceStatus>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<PrinterStatus>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<DenominationDetail>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<DepositorSession>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<Printout>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<Transaction>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.AddTypePermissionsRecursively<TransactionTypeListItem>("Read", new SecurityPermissionState?(SecurityPermissionState.Allow));
                webPortalRole.Save();
            }
            UserGroup managers = ObjectSpace.FindObject<UserGroup>(new BinaryOperator("Name", "Branch Manager"));
            if (managers == null)
            {
                managers = ObjectSpace.CreateObject<UserGroup>();
                managers.description = "[Default] IT User UserGroup with all access";
                managers.name = "Branch Manager";
                managers.parent_group = ParentGroup;
                managers.Save();
            }
            DeviceRole deviceRole = ObjectSpace.FindObject<DeviceRole>(new BinaryOperator("name", "Branch Manager"));
            if (deviceRole == null)
            {
                deviceRole = ObjectSpace.CreateObject<DeviceRole>();
                deviceRole.id = Guid.NewGuid();
                deviceRole.name = "Branch Manager";
                deviceRole.description = "[Default] IT Branch Manager CDM Role";
                deviceRole.id = Guid.NewGuid();
                deviceRole.Save();
            }
            ApplicationUser applicationUser1 = ObjectSpace.FindObject<ApplicationUser>(new BinaryOperator("UserName", "Man1"));
            if (applicationUser1 == null)
            {
                applicationUser1 = ObjectSpace.CreateObject<ApplicationUser>();
                applicationUser1.id = Guid.NewGuid();
                applicationUser1.UserName = "Man1";
                applicationUser1.SetPassword("12345678!qQ");
                applicationUser1.DefaultPassword =  null;
                applicationUser1.IsActive = true;
                applicationUser1.fname = "Default IT Manager";
                applicationUser1.lname = "";
                applicationUser1.email = "Man1@example.com";
                applicationUser1.email_enabled = false;
                applicationUser1.ChangePasswordOnFirstLogon = true;
                applicationUser1.user_group = managers;
                applicationUser1.DeviceRole = deviceRole;
            }
            applicationUser1.Roles.Add(webPortalRole);
            ApplicationUser applicationUser2 = ObjectSpace.FindObject<ApplicationUser>(new BinaryOperator("UserName", "Man2"));
            if (applicationUser2 == null)
            {
                applicationUser2 = ObjectSpace.CreateObject<ApplicationUser>();
                applicationUser2.id = Guid.NewGuid();
                applicationUser2.UserName = "Man2";
                applicationUser2.SetPassword("12345678!qQ");
                applicationUser2.DefaultPassword =  null;
                applicationUser2.IsActive = true;
                applicationUser2.fname = "Default Manager 2";
                applicationUser2.lname = "";
                applicationUser2.email = "Man2@example.com";
                applicationUser2.email_enabled = false;
                applicationUser2.ChangePasswordOnFirstLogon = true;
                applicationUser2.user_group = managers;
                applicationUser2.DeviceRole = deviceRole;
            }
            applicationUser2.Roles.Add(webPortalRole);
            return managers;
        }
    }
}
