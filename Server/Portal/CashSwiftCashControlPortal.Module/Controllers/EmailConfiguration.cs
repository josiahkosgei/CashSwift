
//Controllers.EmailConfiguration


namespace CashSwiftCashControlPortal.Module.Controllers
{
    public class EmailConfiguration
    {
        public string EMAIL_HOST { get; set; }

        public int EMAIL_PORT { get; set; }

        public string EMAIL_USERNAME { get; set; }

        public string EMAIL_PASSWORD { get; set; }

        public string EMAIL_FROM { get; set; }

        public int EMAIL_TIMEOUT { get; set; }

        public bool EMAIL_ENABLE_SSL { get; set; }

        public string WEB_PORTAL_URI { get; set; }
    }
}
