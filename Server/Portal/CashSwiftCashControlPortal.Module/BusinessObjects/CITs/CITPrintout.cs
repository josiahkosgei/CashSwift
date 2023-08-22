
//BusinessObjects.CITs.CITPrintout


using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using DisplayNameAttribute = DevExpress.Xpo.DisplayNameAttribute;

namespace CashSwiftCashControlPortal.Module.BusinessObjects.CITs
{
    [FriendlyKeyProperty("cit_id.device_id.name")]
    [DefaultProperty("datetime")]
    [MapInheritance(MapInheritanceType.OwnTable)]
    public class CITPrintout : XPLiteObject
    {
        private Guid fid;
        private DateTime fdatetime;
        private CIT fcit_id;
        private Guid fprint_guid;
        private string fprint_content;
        private bool fis_copy;

        [Key(true)]
        [Browsable(false)]
        [ModelDefault("AllowEdit", "False")]
        public Guid id
        {
            get => fid;
            set => SetPropertyValue(nameof(id), ref fid, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [ModelDefault("DisplayFormat", "yyyy-MM-dd HH:mm:ss.fff")]
        public DateTime datetime
        {
            get => fdatetime;
            set => SetPropertyValue<DateTime>(nameof(datetime), ref fdatetime, value);
        }

        [ModelDefault("AllowEdit", "False")]
        [Association("CITPrintoutReferencesCIT")]
        public CIT cit_id
        {
            get => fcit_id;
            set => SetPropertyValue(nameof(cit_id), ref fcit_id, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public Guid print_guid
        {
            get => fprint_guid;
            set => SetPropertyValue(nameof(print_guid), ref fprint_guid, value);
        }

        [Size(-1)]
        [ModelDefault("AllowEdit", "False")]
        public string print_content
        {
            get => fprint_content;
            set => SetPropertyValue(nameof(print_content), ref fprint_content, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool is_copy
        {
            get => fis_copy;
            set => SetPropertyValue(nameof(is_copy), ref fis_copy, value);
        }

        public CITPrintout(Session session)
          : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
    }
}
