// CashSwift.API.Integration.Server.Modules.AccountManager
using CashSwift.Finacle.Integration.DataAccess.Dapper;
using CashSwift.Finacle.Integration.DataAccess.Entities;
using CashSwift.Finacle.Integration.Models.AccountValidation;
using CashSwift.Library.Standard.Statuses;

namespace CashSwift.Finacle.Integration.Modules
{
    public class AccountManager
    {
        private IIntegrationDataAccess DBContext { get; set; }

        public AccountManager(IIntegrationDataAccess dataAccess)
        {
            DBContext = dataAccess;
        }

        public async Task<CheckAccountPermission_Result> CheckAccountPermissionAsync(int transactionListItemId, string account_number, string language)
        {
            _ = 2;
            try
            {
                TransactionTypeListItem txType = await DBContext.GetTransactionTypeListItemAsync(transactionListItemId) ?? throw new CashSwiftException($"Transaction Type ID '{transactionListItemId}' is invalid.")
                {
                    PublicErrorMessage = "Transaction type not supported. Contact administrator."
                };
                if (!(txType.enabled.HasValue && txType.enabled.Value))
                {
                    throw new Exception("Transaction Type " + txType.name + " is disabled");
                }
                AccountPermission accountPermission = await DBContext.GetAccountPermissionAsync(txType.account_permission ?? throw new Exception("Transaction Type " + txType.name + " has no account_permission"));
                if (!accountPermission.enabled)
                {
                    throw new Exception("account permission for txtype " + txType.name + " is Disabled");
                }
                CheckAccountAgainstAccountPermission_Result checkAccountAgainstAccountPermission_Result = await DBContext.CheckAccountAgainstAccountPermissionAsync(transactionListItemId, account_number, language);
                return accountPermission.list_type == 0 ? checkAccountAgainstAccountPermission_Result != null ? new CheckAccountPermission_Result
                {
                    IsSuccess = false,
                    PublicErrorMessage = string.IsNullOrWhiteSpace(checkAccountAgainstAccountPermission_Result.error_message) ? "Account cannot transact on this machine. Contact administrator." : checkAccountAgainstAccountPermission_Result.error_message,
                    ServerErrorMessage = "Failed. Account '" + account_number + "' In Blacklist"
                } : new CheckAccountPermission_Result
                {
                    IsSuccess = true,
                    PublicErrorMessage = null
                } : checkAccountAgainstAccountPermission_Result != null ? new CheckAccountPermission_Result
                {
                    IsSuccess = true,
                    PublicErrorMessage = null
                } : new CheckAccountPermission_Result
                {
                    IsSuccess = false,
                    PublicErrorMessage = "Account cannot transact on this machine. Contact administrator.",
                    ServerErrorMessage = "Failed. Account '" + account_number + "' not in Whitelist"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
