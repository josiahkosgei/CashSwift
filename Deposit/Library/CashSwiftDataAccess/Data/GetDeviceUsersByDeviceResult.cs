
using System;

namespace CashSwiftDataAccess.Data
{
    public partial class GetDeviceUsersByDevice_Result
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public Guid role_id { get; set; }
        public string email { get; set; }
        public bool email_enabled { get; set; }
        public string phone { get; set; }
        public bool phone_enabled { get; set; }
        public bool password_reset_required { get; set; }
        public int login_attempts { get; set; }
        public int user_group { get; set; }
        public bool? depositor_enabled { get; set; }
        public bool? UserDeleted { get; set; }
        public bool? IsActive { get; set; }
        public Guid? ApplicationUserLoginDetail { get; set; }
        public bool is_ad_user { get; set; }
    }
}
