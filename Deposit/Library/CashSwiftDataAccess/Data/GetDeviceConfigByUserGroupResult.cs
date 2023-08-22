
using System;

namespace CashSwiftDataAccess.Data
{
    public partial class GetDeviceConfigByUserGroup_Result
    {
        public Guid id { get; set; }
        public int group_id { get; set; }
        public string config_id { get; set; }
        public string config_value { get; set; }
    }
}
