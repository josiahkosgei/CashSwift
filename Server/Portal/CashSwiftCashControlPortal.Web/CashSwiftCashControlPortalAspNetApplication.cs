namespace CashSwiftCashControlPortal.Web
{
    using CashSwiftCashControlPortal.Module;
    using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
    using CashSwiftCashControlPortal.Module.Web;
    using DevExpress.Data.Filtering;
    using DevExpress.ExpressApp;
    using DevExpress.ExpressApp.Actions;
    using DevExpress.ExpressApp.AuditTrail;
    using DevExpress.ExpressApp.ConditionalAppearance;
    using DevExpress.ExpressApp.HtmlPropertyEditor.Web;
    using DevExpress.ExpressApp.Security;
    using DevExpress.ExpressApp.SystemModule;
    using DevExpress.ExpressApp.TreeListEditors;
    using DevExpress.ExpressApp.TreeListEditors.Web;
    using DevExpress.ExpressApp.Validation;
    using DevExpress.ExpressApp.Validation.Web;
    using DevExpress.ExpressApp.Web;
    using DevExpress.ExpressApp.Web.Editors.ASPx;
    using DevExpress.ExpressApp.Web.SystemModule;
    using DevExpress.ExpressApp.Xpo;
    using DevExpress.Persistent.Base;
    using DevExpress.Persistent.BaseImpl;
    using DevExpress.Xpo;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Web;
    using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.CashSwift;

    public class CashSwiftCashControlPortalAspNetApplication : WebApplication
    {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private SystemAspNetModule module2;
        private CashControlPortalModule module3;
        private CashControlPortalAspNetModule module4;
        private SecurityModule securityModule1;
        private SecurityStrategyComplex securityStrategyComplex1;
        private HtmlPropertyEditorAspNetModule htmlPropertyEditorAspNetModule;
        private ValidationModule validationModule;
        private CashSwiftAuthentication cashSwiftAuthentication1;
        private ConditionalAppearanceModule conditionalAppearanceModule1;
        private TreeListEditorsModuleBase treeListEditorsModuleBase1;
        private TreeListEditorsAspNetModule treeListEditorsAspNetModule1;
        private AuditTrailModule auditTrailModule1;
        private ValidationAspNetModule validationAspNetModule;
        private List<Type> CustodianDetailTypes;

        static CashSwiftCashControlPortalAspNetApplication()
        {
            EnableMultipleBrowserTabsSupport = true;
            ASPxGridListEditor.AllowFilterControlHierarchy = true;
            ASPxGridListEditor.MaxFilterControlHierarchyDepth = 3;
            ASPxCriteriaPropertyEditor.AllowFilterControlHierarchyDefault = true;
            ASPxCriteriaPropertyEditor.MaxHierarchyDepthDefault = 3;
            PasswordCryptographer.EnableRfc2898 = true;
            PasswordCryptographer.SupportLegacySha512 = false;
        }

        public CashSwiftCashControlPortalAspNetApplication()
        {
            List<Type> list1 = new List<Type>();
            list1.Add(typeof(ChangePasswordOnLogonParameters));
            list1.Add(typeof(ChangePasswordParameters));
            CustodianDetailTypes = list1;
            InitializeComponent();
            InitializeDefaults();
        }

        private void CashControlPortalAspNetApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                e.Updater.Update();
                e.Handled = true;
            }
            else
            {
                string message = "The application cannot connect to the specified database, because the database doesn't exist,\x00a0its version is older than that of the application or its schema does not match the ORM data model structure. To avoid this error, use one of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";
                if ((e.CompatibilityError != null) && (e.CompatibilityError.Exception != null))
                {
                    message = message + "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
                }
                throw new InvalidOperationException(message);
            }
        }

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        {
            args.ObjectSpaceProvider = new XPObjectSpaceProvider(GetDataStoreProvider(args.ConnectionString, args.Connection), true);
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }

        private IXpoDataStoreProvider GetDataStoreProvider(string connectionString, IDbConnection connection)
        {
            HttpApplicationState application = HttpContext.Current?.Application;
            IXpoDataStoreProvider provider = null;
            if ((application != null) && (application["DataStoreProvider"] != null))
            {
                provider = application["DataStoreProvider"] as IXpoDataStoreProvider;
            }
            else
            {
                provider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, true);
                if (application != null)
                {
                    application["DataStoreProvider"] = provider;
                }
            }
            return provider;
        }

        private void InitializeComponent()
        {
            module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            module2 = new SystemAspNetModule();
            securityModule1 = new SecurityModule();
            securityStrategyComplex1 = new SecurityStrategyComplex();
            cashSwiftAuthentication1 = new CashSwiftAuthentication();
            htmlPropertyEditorAspNetModule = new HtmlPropertyEditorAspNetModule();
            validationModule = new ValidationModule();
            validationAspNetModule = new ValidationAspNetModule();
            module3 = new CashControlPortalModule();
            module4 = new CashControlPortalAspNetModule();
            conditionalAppearanceModule1 = new ConditionalAppearanceModule();
            treeListEditorsModuleBase1 = new TreeListEditorsModuleBase();
            treeListEditorsAspNetModule1 = new TreeListEditorsAspNetModule();
            auditTrailModule1 = new AuditTrailModule();
            BeginInit();
            securityStrategyComplex1.Authentication = cashSwiftAuthentication1;
            securityStrategyComplex1.RoleType = typeof(WebPortalRole);
            securityStrategyComplex1.SupportNavigationPermissionsForTypes = false;
            securityStrategyComplex1.UsePermissionRequestProcessor = false;
            securityStrategyComplex1.UserType = typeof(ApplicationUser);
            validationModule.AllowValidationDetailsAccess = true;
            validationModule.IgnoreWarningAndInformationRules = false;
            auditTrailModule1.AuditDataItemPersistentType = typeof(AuditDataItemPersistent);
            ApplicationName = "CashSwiftCashControlPortal";
            CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
            Modules.Add(module1);
            Modules.Add(module2);
            Modules.Add(validationModule);
            Modules.Add(conditionalAppearanceModule1);
            Modules.Add(treeListEditorsModuleBase1);
            Modules.Add(treeListEditorsAspNetModule1);
            Modules.Add(auditTrailModule1);
            Modules.Add(module3);
            Modules.Add(htmlPropertyEditorAspNetModule);
            Modules.Add(validationAspNetModule);
            Modules.Add(module4);
            Modules.Add(securityModule1);
            Security = securityStrategyComplex1;
            DatabaseVersionMismatch += new EventHandler<DatabaseVersionMismatchEventArgs>(CashControlPortalAspNetApplication_DatabaseVersionMismatch);
            EndInit();
        }

        private void InitializeDefaults()
        {
            LinkNewObjectToParentImmediately = false;
            OptimizedControllersCreation = true;
        }

        protected override void OnDetailViewCreated(DetailView view)
        {
            base.OnDetailViewCreated(view);
            if (!Properties.Settings.Default.NS_NAVIGATION)
            {
                IObjectSpace objectSpace = ObjectSpaceProvider.CreateObjectSpace();
                ApplicationUser user = objectSpace.GetObject<ApplicationUser>(SecuritySystem.CurrentUser as ApplicationUser);
                Type item = view.ObjectTypeInfo.Type;
                if (!CustodianDetailTypes.Contains(item) && (user != null))
                {
                    object obj1;
                    if (user == null)
                    {
                        obj1 = null;
                    }
                    else
                    {
                        XPCollection<WebPortalRole> roles = user.Roles;
                        if (roles != null)
                        {
                            obj1 = roles.FirstOrDefault<WebPortalRole>(x => (x.Name == "Administrator") || (x.Name == "Root"));
                        }
                        else
                        {
                            XPCollection<WebPortalRole> local1 = roles;
                            obj1 = null;
                        }
                    }
                    if (obj1 == null)
                    {
                        try
                        {
                            if (item == typeof(ApplicationUser))
                            {
                                ApplicationUser currentObject = view.CurrentObject as ApplicationUser;
                                if ((currentObject != null) && (currentObject.id == user.id))
                                {
                                    return;
                                }
                            }

                            if (!(view.ObjectTypeInfo.Attributes.FirstOrDefault<Attribute>(x => x is NavigationItemAttribute) is NavigationItemAttribute))
                            {
                                throw new NullReferenceException("navAttribute");
                            }
                            ChoiceActionItem selectedItem = MainWindow.GetController<ShowNavigationItemController>().ShowNavigationItemAction.SelectedItem;
                            if (!SecuritySystem.IsGranted(objectSpace, item, "Read", view.CurrentObject, string.Empty) && !CustodianDetailTypes.Contains(item))
                            {
                                throw new Exception("Illegal navigation to " + item.Name + " detail view");
                            }
                        }
                        catch (Exception)
                        {
                            object[] messageFormatObjects = new object[] { view.Caption, item };
                            Global.Log.Warning(GetType().Name + ".OnDetailViewCreated", "DetailView Blocked", "Authorization", "Permission denied to DetailView {0} for Role {1}", messageFormatObjects);
                            LogOff();
                        }
                    }
                }
            }
        }

        protected override void OnListViewCreated(ListView listView)
        {
            base.OnListViewCreated(listView);
            if (!Properties.Settings.Default.NS_NAVIGATION)
            {
                IObjectSpace objectSpace = ObjectSpaceProvider.CreateObjectSpace();
                ApplicationUser user = objectSpace.GetObject<ApplicationUser>(SecuritySystem.CurrentUser as ApplicationUser);
                if (user == null)
                {
                    throw new Exception("OnListViewCreated(): current user cannot be null");
                }
                if (user != null)
                {
                    object obj1;
                    if (user == null)
                    {
                        obj1 = null;
                    }
                    else
                    {
                        XPCollection<WebPortalRole> roles = user.Roles;
                        if (roles != null)
                        {
                            obj1 = roles.FirstOrDefault<WebPortalRole>(x => (x.Name == "Administrator") || (x.Name == "Root"));
                        }
                        else
                        {
                            XPCollection<WebPortalRole> local1 = roles;
                            obj1 = null;
                        }
                    }
                    if ((obj1 == null) && listView.IsRoot)
                    {
                        Type objectType = listView.ObjectTypeInfo.Type;
                        try
                        {
                            NavigationItemAttribute attribute = listView.ObjectTypeInfo.Attributes.FirstOrDefault<Attribute>(x => x is NavigationItemAttribute) as NavigationItemAttribute;
                            if (attribute == null)
                            {
                                throw new NullReferenceException("navAttribute");
                            }
                            ChoiceActionItem selectedItem = MainWindow.GetController<ShowNavigationItemController>().ShowNavigationItemAction.SelectedItem;
                            if (!SecuritySystem.IsGrantedNavigate("Application/NavigationItems/Items/" + attribute.GroupName + "/Items/" + listView.Id, objectType, null, objectSpace) && !CustodianDetailTypes.Contains(objectType))
                            {
                                throw new Exception("Illegal navigation to " + objectType.Name + " ListView");
                            }
                        }
                        catch (Exception)
                        {
                            object[] messageFormatObjects = new object[] { listView.Caption, objectType };
                            Global.Log.Warning(GetType().Name + ".OnListViewCreated", "ListView Blocked", "Authorization", "Permission denied to ListView {0} for Role {1}", messageFormatObjects);
                            LogOff();
                        }
                    }
                }
            }
        }

        protected override void OnLoggedOff()
        {
            base.OnLoggedOff();
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.RemoveAll();
            if (HttpContext.Current.Request.Cookies["ASP.NET_SessionId"] != null)
            {
                HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                HttpContext.Current.Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (HttpContext.Current.Request.Cookies["CashControlPortalUserName"] != null)
            {
                HttpContext.Current.Response.Cookies["CashControlPortalUserName"].Value = string.Empty;
                HttpContext.Current.Response.Cookies["CashControlPortalUserName"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (HttpContext.Current.Request.Cookies["SFBegone"] != null)
            {
                HttpContext.Current.Response.Cookies["SFBegone"].Value = string.Empty;
                HttpContext.Current.Response.Cookies["SFBegone"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

        protected override void OnLoggedOn(LogonEventArgs args)
        {
            try
            {
                base.OnLoggedOn(args);
                IObjectSpace space = ObjectSpaceProvider.CreateObjectSpace();
                ApplicationUser user = space.GetObject<ApplicationUser>(SecuritySystem.CurrentUser as ApplicationUser);
                if (space.FindObject<WebPortalLoginLogEntry>(CriteriaOperator.Parse("[SessionID]='" + HttpContext.Current.Session.SessionID + "'", Array.Empty<object>())) != null)
                {
                    LogOff();
                }
                else
                {
                    user.Login();
                    HttpContext.Current.Session["SFBegone"] = user.ApplicationUserLoginDetail.LastLoginLogEntry.SFBegone;
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("SFBegone", user.ApplicationUserLoginDetail.LastLoginLogEntry.SFBegone));
                    space.CommitChanges();
                    space.Refresh();
                }
            }
            catch (Exception)
            {
            }
        }

        protected override void OnLoggingOff(LoggingOffEventArgs args)
        {
            base.OnLoggingOff(args);
            try
            {
                IObjectSpace space1 = ObjectSpaceProvider.CreateObjectSpace();
                ApplicationUser local1 = space1.GetObject<ApplicationUser>(SecuritySystem.CurrentUser as ApplicationUser);
                if (local1 == null)
                {
                    ApplicationUser local2 = local1;
                }
                else
                {
                    local1.LogOff();
                }
                IObjectSpace local3 = space1;
                local3.CommitChanges();
                local3.Refresh();
            }
            catch (Exception)
            {
            }
        }
    }
}

