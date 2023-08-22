
//Controllers.LoginConfiguration


namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class LoginConfiguration
    {
        public string AD_USERNAME { get; set; }

        public string AD_PASSWORD { get; set; }

        public string AD_USERDOMAIN { get; set; }

        public string AD_BASEDN { get; set; }

        public bool AD_USESSL { get; set; }

        public bool AD_IGNORE_CERT { get; set; } = true;

        public bool AD_ALLOW { get; set; }

        public bool AD_ALLOW_USER_REG { get; set; }

        public int LOGIN_MAX_COUNT { get; set; }

        public string[] AD_SERVERS { get; set; }
    }
}
