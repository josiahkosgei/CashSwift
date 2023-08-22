using CashSwift.Finacle.Integration.CQRS.Services.IServices;
using CashSwift.Finacle.Integration.DataAccess.Dapper;
using CashSwift.Finacle.Integration.DataAccess.Entities;
using CashSwift.Finacle.Integration.Models.AccountValidation;
using CashSwift.Library.Standard.Statuses;
using System.Data;
using SqlParameter = Microsoft.Data.SqlClient.SqlParameter;

namespace CashSwift.Finacle.Integration.CQRS.Services
{
    public class AccountManagerService : IAccountManagerService
    {
        //private readonly DepositorServerContext _context;
        //private readonly DepositorServerContextProcedures _contextProcedures;
        
        private IIntegrationDataAccess _context;
        public AccountManagerService(IConfiguration configuration)
        {
            _context = new IntegrationDataAccess(configuration);
        }

        public async Task<CheckAccountPermission_Result> CheckAccountPermissionAsync(int transactionListItemId, string account_number, string language)
        {
            CheckAccountPermission_Result permissionResult1;
            //CheckAccountAgainstAccountPermission_Result permissionResult2 = new CheckAccountAgainstAccountPermission_Result();
            try
            {
                TransactionTypeListItem txType = await _context.GetTransactionTypeListItemAsync(transactionListItemId) ?? throw new CashSwiftException(string.Format("Transaction Type ID '{0}' is invalid.", transactionListItemId))
                {
                    PublicErrorMessage = "Transaction type not supported. Contact administrator."
                };
                if ((bool)!txType.enabled)
                    throw new Exception("Transaction Type " + txType.name + " is disabled");

                Guid id = txType.account_permission ?? throw new Exception("Transaction Type " + txType.name + " has no account_permission");
                AccountPermission accountPermission = await _context.GetAccountPermissionAsync(id);
                if (!accountPermission.enabled)
                    throw new Exception("account permission for txtype " + txType.name + " is Disabled");
                var @params = new SqlParameter[] {
                            new SqlParameter() {
                            ParameterName = "@TxTypeID",
                            SqlDbType =  SqlDbType.Int,
                            Direction = ParameterDirection.Input,
                            Value = transactionListItemId
                            },
                            new SqlParameter() {
                            ParameterName = "@AccountNumber",
                            SqlDbType =  SqlDbType.NChar,
                            Direction = ParameterDirection.Input,
                            Value = account_number
                            },
                            new SqlParameter() {
                            ParameterName = "@Language",
                            SqlDbType =  SqlDbType.VarChar,
                            Direction = ParameterDirection.Input,
                            Value = language
                            }};
                var permissionResult2 = await _context.CheckAccountAgainstAccountPermissionAsync(transactionListItemId, account_number, language);

                CheckAccountPermission_Result permissionResult3;
                if (accountPermission.list_type == 0)
                {
                    if (permissionResult2 == null)
                        permissionResult3 = new CheckAccountPermission_Result()
                        {
                            IsSuccess = true,
                            PublicErrorMessage = null
                        };
                    else
                        permissionResult3 = new CheckAccountPermission_Result()
                        {
                            IsSuccess = false,
                            PublicErrorMessage = string.IsNullOrWhiteSpace(permissionResult2?.error_message) ? "Account cannot transact on this machine. Contact administrator." : permissionResult2?.error_message,
                            ServerErrorMessage = "Failed. Account '" + account_number + "' In Blacklist"
                        };
                }
                else if (permissionResult2 == null)
                    permissionResult3 = new CheckAccountPermission_Result()
                    {
                        IsSuccess = false,
                        PublicErrorMessage = "Account cannot transact on this machine. Contact administrator.",
                        ServerErrorMessage = "Failed. Account '" + account_number + "' not in Whitelist"
                    };
                else
                    permissionResult3 = new CheckAccountPermission_Result()
                    {
                        IsSuccess = true,
                        PublicErrorMessage = null
                    };
                permissionResult1 = permissionResult3;
            }
            catch (Exception ex)
            {
                throw;
            }
            return permissionResult1;
        }
    }
}
