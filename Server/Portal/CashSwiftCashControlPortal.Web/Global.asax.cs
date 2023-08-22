namespace CashSwiftCashControlPortal.Web
{
    using  CashSwift.Library.Standard.Logging;
    using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF;
    using CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF.Permissions;
    using CashSwiftCashControlPortal.Module.Controllers;
    using CashSwiftCashControlPortal.Web.Properties;
    using DevExpress.ExpressApp;
    using DevExpress.ExpressApp.Security;
    using DevExpress.ExpressApp.Web;
    using DevExpress.ExpressApp.Web.Templates;
    using DevExpress.Persistent.AuditTrail;
    using DevExpress.Persistent.Base;
    using DevExpress.Persistent.BaseImpl.PermissionPolicy;
    using DevExpress.Web;
    using DevExpress.Xpo.Metadata;
    using NLog;
    using NLog.Fluent;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;

    public class Global : HttpApplication
    {
        public static readonly Logger AuditLog = LogManager.GetLogger("PortalAuditLog");
        public static readonly ICashSwiftLogger Log = Module.Util.Logger.Log;

        public Global()
        {
            InitializeComponent();
        }

        private void Application_AcquireRequestState(object sender, EventArgs e)
        {
            try
            {
                if ((HttpContext.Current != null) && (HttpContext.Current.Session != null))
                {
                    double logoffTimeout = Settings.Default.LogoffTimeout;
                    logoffTimeout = (logoffTimeout >= 0.0) ? logoffTimeout : 0.0;
                    DateTime? nullable = Session["LastRequestTime"] as DateTime?;
                    if (((nullable != null) && (logoffTimeout > 0.0)) && ((DateTime.Now.Subtract(nullable.Value).TotalSeconds > logoffTimeout) && ((WebApplication.Instance != null) && WebApplication.Instance.IsLoggedOn)))
                    {
                        WebApplication.LogOff(Session);
                    }
                    Session["LastRequestTime"] = DateTime.Now;
                    if ((Session["SFBegone"] == null) || (Request.Cookies["SFBegone"] == null))
                    {
                        if ((WebApplication.Instance != null) && WebApplication.Instance.IsLoggedOn)
                        {
                            WebApplication.LogOff(Session);
                        }
                    }
                    else if (!WebApplication.Instance.ObjectSpaceProvider.CreateObjectSpace().GetObject<ApplicationUser>(SecuritySystem.CurrentUser as ApplicationUser).ApplicationUserLoginDetail.IsValidLogin(Session.SessionID, Session["SFBegone"].ToString()) || !Session["SFBegone"].ToString().Equals(Request.Cookies["SFBegone"].Value))
                    {
                        if ((WebApplication.Instance != null) && WebApplication.Instance.IsLoggedOn)
                        {
                            WebApplication.LogOff(Session);
                        }
                        else
                        {
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
                            WebApplication.DisposeInstance(Session);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastError = HttpContext.Current.Server.GetLastError();
            ErrorHandling.Instance.SetPageError(CustomErrorController.HandleException(lastError));
            ErrorHandling.Instance.ProcessApplicationError();
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            SecurityAdapterHelper.Enable();
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
            ErrorInfo.FormatText += new EventHandler<ErrorInfoFormatTextEventArgs>(ErrorInfo_FormatText);
        }

        private static void CustomizeRequestProcessors(
         object sender,
         CustomizeRequestProcessorsEventArgs e)
        {
            List<IOperationPermission> permissions = new List<IOperationPermission>();
            if (sender is SecurityStrategyComplex securityStrategyComplex && securityStrategyComplex.User is ApplicationUser user)
            {
                foreach (PermissionPolicyRoleBase role in user.Roles)
                {
                    foreach (PermissionPolicyTypePermissionObject typePermission in role.TypePermissions)
                    {
                        if (typePermission is MakerTypePermissionObject permissionObject)
                        {
                            SecurityPermissionState? nullable;
                            if (permissionObject.MakerState.HasValue)
                            {
                                List<IOperationPermission> operationPermissionList = permissions;
                                Type targetType = permissionObject.TargetType;
                                nullable = permissionObject.MakerState;
                                int state = (int)nullable.Value;
                                MakerPermission makerPermission = new MakerPermission(targetType, (SecurityPermissionState)state);
                                operationPermissionList.Add(makerPermission);
                            }
                            nullable = permissionObject.CheckerState;
                            if (nullable.HasValue)
                            {
                                List<IOperationPermission> operationPermissionList = permissions;
                                Type targetType = permissionObject.TargetType;
                                nullable = permissionObject.CheckerState;
                                int state = (int)nullable.Value;
                                CheckerPermission checkerPermission = new CheckerPermission(targetType, (SecurityPermissionState)state);
                                operationPermissionList.Add(checkerPermission);
                            }
                        }
                    }
                }
            }
            IPermissionDictionary permissionDictionary = new PermissionDictionary(permissions);
            e.Processors.Add(typeof(MakerPermissionRequest), new MakerPermissionRequestProcessor(permissionDictionary));
            e.Processors.Add(typeof(CheckerPermissionRequest), new CheckerPermissionRequestProcessor(permissionDictionary));
        }
        private void ErrorInfo_FormatText(object sender, ErrorInfoFormatTextEventArgs e)
        {
            Exception exception = CustomErrorController.HandleException(e?.Exception);
            e.Text = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] " + exception.Message;
        }

        private void InitializeComponent()
        {
        }

        private void Instance_SaveAuditTrailData1(object sender, SaveAuditTrailDataEventArgs e)
        {
            foreach (AuditDataItem item in e.AuditTrailDataItems)
            {
                bool? nullable2;
                bool? nullable1;
                if (item == null)
                {
                    nullable2 = null;
                    nullable1 = nullable2;
                }
                else
                {
                    XPMemberInfo memberInfo = item.MemberInfo;
                    if (memberInfo == null)
                    {
                        XPMemberInfo local1 = memberInfo;
                        nullable2 = null;
                        nullable1 = nullable2;
                    }
                    else
                    {
                        string name = memberInfo.Name;
                        if (name == null)
                        {
                            string local2 = name;
                            nullable2 = null;
                            nullable1 = nullable2;
                        }
                        else
                        {
                            string text2 = name.ToUpperInvariant();
                            if (text2 != null)
                            {
                                nullable1 = new bool?(text2.Contains("PASSWORD"));
                            }
                            else
                            {
                                string local3 = text2;
                                nullable2 = null;
                                nullable1 = nullable2;
                            }
                        }
                    }
                }
                bool? nullable = nullable1;
                if ((nullable != null) ? nullable.GetValueOrDefault() : false)
                {
                    item.OldValue = "****";
                    item.NewValue = "****";
                }
                string message = $"{item.ModifiedOn:yyyy-MM-dd HH:mm:ss.fff}|{SecuritySystem.CurrentUserName}|{item.AuditObject}|{item.OperationType}|{item.MemberInfo}|{item.OldValue}|{item.NewValue}";
                AuditLog.Info(message);
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            WebApplication.SetInstance(Session, new CashSwiftCashControlPortalAspNetApplication());
            AuditTrailService.Instance.SaveAuditTrailData += new SaveAuditTrailDataEventHandler(Instance_SaveAuditTrailData1);
            ((SecurityStrategy)WebApplication.Instance.Security).CustomizeRequestProcessors += new EventHandler<CustomizeRequestProcessorsEventArgs>(CustomizeRequestProcessors);
            DefaultVerticalTemplateContentNew.ClearSizeLimit();
            WebApplication.Instance.SwitchToNewStyle();
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            if (Debugger.IsAttached && (WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema))
            {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
        }
    }
}

