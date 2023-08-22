
//BusinessObjects.Authentication.XAF.CashSwiftAuthentication


using CashSwiftCashControlPortal.Module.BusinessObjects.Server;
using CashSwiftCashControlPortal.Module.Controllers;
using CashSwiftCashControlPortal.Module.Util;
using CashSwiftUtil.Security.Authentication.LDAP;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Authentication.XAF
{
    public class CashSwiftAuthentication : AuthenticationBase, IAuthenticationStandard
    {
        private CashSwiftLogonParameters cashSwiftLogonParameters;
        private LDAPAuthentication ADAuth;
        private LoginConfiguration loginConfiguration;

        public CashSwiftAuthentication() => cashSwiftLogonParameters = new CashSwiftLogonParameters();

        public override void Logoff()
        {
            base.Logoff();
            cashSwiftLogonParameters = new CashSwiftLogonParameters();
        }

        public override void ClearSecuredLogonParameters()
        {
            cashSwiftLogonParameters.Password = "";
            base.ClearSecuredLogonParameters();
        }

        public override object Authenticate(IObjectSpace objectSpace)
        {
            try
            {
                loginConfiguration = JsonConvert.DeserializeObject<LoginConfiguration>((objectSpace.FindObject<ServerConfiguration>(new BinaryOperator("config_key", "WEB_PORTAL_LOGIN_CONFIG")) ?? throw new NullReferenceException("AD_CONFIG not found")).config_value);
                NetworkCredential networkCredential = new NetworkCredential();
                if (!string.IsNullOrWhiteSpace(loginConfiguration.AD_USERNAME) || !string.IsNullOrWhiteSpace(loginConfiguration.AD_PASSWORD))
                {
                    networkCredential.UserName = loginConfiguration.AD_USERNAME;
                    networkCredential.Password = loginConfiguration.AD_PASSWORD;
                }
                ADAuth = new LDAPAuthentication(null, loginConfiguration.AD_USERDOMAIN, loginConfiguration.AD_USESSL, loginConfiguration.AD_IGNORE_CERT, networkCredential);
                if (objectSpace == null)
                    throw new Exception("Login Error. Contact your administrator.");
                if (cashSwiftLogonParameters == null)
                    throw new Exception("Login Error. Contact your administrator.");
                if (string.IsNullOrWhiteSpace(cashSwiftLogonParameters.UserName))
                    throw new AuthenticationException(cashSwiftLogonParameters.UserName, "Enter a Username");
                if (string.IsNullOrWhiteSpace(cashSwiftLogonParameters.Password))
                    throw new AuthenticationException(cashSwiftLogonParameters.Password, "Enter a password");
                ApplicationUser user = objectSpace.FindObject<ApplicationUser>(new BinaryOperator("UserName", cashSwiftLogonParameters.UserName));
                if (user == null)
                {
                    if (!loginConfiguration.AD_ALLOW || !loginConfiguration.AD_ALLOW_USER_REG)
                        throw new AuthenticationException(cashSwiftLogonParameters.UserName, "Username and/or Password Invalid");
                    if (!ActiveDirectoryAuthentication(cashSwiftLogonParameters.UserName, cashSwiftLogonParameters.Password))
                        HandleFailedLogin(user);
                }
                else if (user.IsActiveDirectoryUser && loginConfiguration.AD_ALLOW)
                {
                    if (!ActiveDirectoryAuthentication(cashSwiftLogonParameters.UserName, cashSwiftLogonParameters.Password))
                        HandleFailedLogin(user);
                }
                else
                    StandardAuthentication(user);
                if (user.UserDeleted)
                    throw new AuthenticationException(cashSwiftLogonParameters.UserName, "Username and/or Password Invalid");
                if (!user.IsActive)
                    throw new AuthenticationException(cashSwiftLogonParameters.UserName, "User is locked. Contact your administrator.");
                objectSpace.CommitChanges();
                return user;
            }
            catch (Exception ex1)
            {
                try
                {
                    objectSpace.CommitChanges();
                }
                catch (Exception ex2)
                {
                    throw;
                }
                throw;
            }
        }

        public override void SetLogonParameters(object logonParameters) => cashSwiftLogonParameters = (CashSwiftLogonParameters)LogonParameters;

        public override IList<Type> GetBusinessClasses() => new Type[1]
        {
      typeof (CashSwiftLogonParameters)
        };

        public override object LogonParameters => cashSwiftLogonParameters;

        public override bool IsLogoffEnabled => true;

        public override bool AskLogonParametersViaUI => true;

        private bool ActiveDirectoryAuthentication(string username, string password)
        {
            foreach (string str in loginConfiguration.AD_SERVERS)
            {
                try
                {
                    ADAuth.Host = str;
                    string username1 = username;
                    if (!username.Contains("\\"))
                        username1 = string.Join(".", loginConfiguration.AD_BASEDN.Split(new string[1]
                        {
              ","
                        }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Substring(x.LastIndexOf('=') + 1))) + "\\" + username;
                    return ADAuth.ActiveDirectoryAuthentication(username1, password);
                }
                catch (LdapException ex)
                {
                    if (ex.ErrorCode != 81)
                        return false;
                    Logger.Log.Error(nameof(CashSwiftAuthentication), ex.GetType().Name, "Eror", "LDAP server {0} auth failed with error code {1} and message {2}.", ex.Message, ex.ErrorCode, ex.Data);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            throw new AuthenticationException("Could not connect to AD server(s). Contact administrator");
        }

        private bool StandardAuthentication(ApplicationUser user)
        {
            if (!user.ComparePassword(cashSwiftLogonParameters.Password))
                HandleFailedLogin(user);
            if (user.IsPasswordExpired())
                user.ChangePasswordOnFirstLogon = true;
            return true;
        }

        public void HandleFailedLogin(ApplicationUser user)
        {
            if (user != null)
            {
                ++user.ApplicationUserLoginDetail.FailedLoginCount;
                int num = Math.Max(loginConfiguration.LOGIN_MAX_COUNT, 0);
                if (num != 0 && user.ApplicationUserLoginDetail.FailedLoginCount >= num)
                {
                    user.LockUser(true);
                    user.Save();
                    throw new AuthenticationException(cashSwiftLogonParameters.UserName, "User is locked. Contact your administrator.");
                }
            }
            throw new AuthenticationException(cashSwiftLogonParameters.UserName, "Username and/or Password Invalid");
        }
    }
}
