using CashSwift.Finacle.Integration.Models.AccountValidation;

namespace CashSwift.Finacle.Integration.CQRS.Services.IServices
{
    public interface IAccountManagerService
    {
        public Task<CheckAccountPermission_Result> CheckAccountPermissionAsync(int transactionListItemId, string account_number, string language);
    }
}
