
using CashSwift.Finacle.Integration.DataAccess.Entities;
namespace CashSwift.Finacle.Integration.DataAccess.Dapper
{
    public interface IIntegrationDataAccess
    {
        Task<Bank> GetBankByDeviceIDAsync(Guid deviceID);

        Task<PingResponse> GetLatestPingRequestAsync(string serverName);

        Task<ValidateAccountResponse> GetValidateAccountResponseAsync(string messageID);

        Task<bool> InsertPingResponseAsync(PingResponse pingResponse);

        Task<bool> InsertPostRequestAsync(PostRequest postRequest);

        Task<bool> InsertPostResponseAsync(PostResponse postResponse);

        Task<bool> InsertValidateAccountRequestAsync(ValidateAccountRequest validateAccountRequest);

        Task<bool> InsertValidateAccountResponseAsync(ValidateAccountResponse validateAccountResponse);

        Task<bool> InsertValidateReferenceAccountRequestAsync(ValidateReferenceAccountRequest validateReferenceAccountRequest);

        Task<bool> InsertValidateReferenceAccountResponseAsync(ValidateReferenceAccountResponse validateReferenceAccountResponse);

        Task<SearchAccountInTransactionPermissionList_Result> SearchAccountInTransactionPermissionList(int TxTypeID, string AccountNumber);

        Task UpdatePingResponseAsync(PingResponse pingResponse);

        Task<AccountPermission> GetAccountPermissionAsync(Guid id);

        Task<Account> GetAccountAsync(Guid id);

        Task<AccountPermissionItem> GetAccountPermissionItemAsync(Guid id);

        Task<CheckAccountAgainstAccountPermission_Result> CheckAccountAgainstAccountPermissionAsync(int transactionTypeListITemID, string account_number, string language);

        Task<TransactionTypeListItem> GetTransactionTypeListItemAsync(int id);

        Task<Device> GetDevice(Guid id);

        Task<Branch> GetBranch(Guid id);

        Task<Currency> GetCurrencyAsync(string currency);
    }

}
