// CashSwift.API.Integration.Server.Models.SearchAccountInTransactionPermissionList_Result

namespace CashSwift.Finacle.Integration.DataAccess.Dapper
{
    public class SearchAccountInTransactionPermissionList_Result
    {
        public Guid id { get; set; }

        public byte[] Icon { get; set; }

        public string currency { get; set; }

        public string account_number { get; set; }

        public string account_name { get; set; }

        public bool enabed { get; set; } = true;


        public int list_type { get; set; }

        public Guid? ap_error_message { get; set; }

        public Guid? api_error_message { get; set; }
    }
}
