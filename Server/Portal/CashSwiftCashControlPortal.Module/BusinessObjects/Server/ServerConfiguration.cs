
//BusinessObjects.Server.ServerConfiguration


using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Server
{
    [NavigationItem("Server Management")]
    [FriendlyKeyProperty("key")]
    [DefaultProperty("key")]
    [Persistent("sec.KeyStore")]
    public class ServerConfiguration : XPLiteObject
    {
        private string fkey;
        private string fstore_value;

        [Key]
        [Size(50)]
        [Persistent("config_key")]
        [DisplayName("Key")]
        public string config_key
        {
            get => fkey;
            set => SetPropertyValue(nameof(config_key), ref fkey, value);
        }

        [Persistent("config_value")]
        [DisplayName("Value")]
        [Size(-1)]
        public string config_value
        {
            get => fstore_value;
            set => SetPropertyValue(nameof(config_value), ref fstore_value, value);
        }

        public ServerConfiguration(Session session)
          : base(session)
        {
        }
    }
}
