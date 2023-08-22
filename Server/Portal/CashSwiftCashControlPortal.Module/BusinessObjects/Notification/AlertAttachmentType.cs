
//BusinessObjects.Notification.AlertAttachmentType


using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.Notification
{
    [NavigationItem("Notification")]
    public class AlertAttachmentType : XPLiteObject
    {
        private string fcode;
        private string fname;
        private string fdescription;
        private int falert_type_id;
        private bool fenabled;
        private string fmime_type;
        private string fmime_subtype;

        [Key]
        [Size(6)]
        [DisplayName("Code")]
        public string code
        {
            get => fcode;
            set => SetPropertyValue(nameof(code), ref fcode, value);
        }

        [Size(50)]
        [DisplayName("Name")]
        public string name
        {
            get => fname;
            set => SetPropertyValue(nameof(name), ref fname, value);
        }

        [Size(255)]
        [DisplayName("Description")]
        public string description
        {
            get => fdescription;
            set => SetPropertyValue(nameof(description), ref fdescription, value);
        }

        [DisplayName("Alert Type")]
        public int alert_type_id
        {
            get => falert_type_id;
            set => SetPropertyValue<int>(nameof(alert_type_id), ref falert_type_id, value);
        }

        [DisplayName("Enabled")]
        public bool enabled
        {
            get => fenabled;
            set => SetPropertyValue(nameof(enabled), ref fenabled, value);
        }

        [DisplayName("mime_type")]
        [Size(100)]
        public string mime_type
        {
            get => fmime_type;
            set => SetPropertyValue(nameof(mime_type), ref fmime_type, value);
        }

        [DisplayName("mime_subtype")]
        [Size(100)]
        public string mime_subtype
        {
            get => fmime_subtype;
            set => SetPropertyValue(nameof(mime_subtype), ref fmime_subtype, value);
        }

        public AlertAttachmentType(Session session)
          : base(session)
        {
        }
    }
}
