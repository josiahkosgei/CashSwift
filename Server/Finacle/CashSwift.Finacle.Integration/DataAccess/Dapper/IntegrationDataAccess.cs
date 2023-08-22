
using CashSwift.Finacle.Integration.DataAccess.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CashSwift.Finacle.Integration.DataAccess.Dapper
{
    public class IntegrationDataAccess : IIntegrationDataAccess
    {
        private string connectionString;

        public IntegrationDataAccess(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Default") ?? throw new ArgumentNullException("connectionString");
        }

        public async Task<bool> InsertPingResponseAsync(PingResponse pingResponse)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                await db.QueryAsync("INSERT INTO [cb].[PingResponse]\r\n           ([ServerName]\r\n           ,[UpdateTime]\r\n           ,[IsSuccess]\r\n           ,[ServerOnline]\r\n           ,[ErrorCode]\r\n           ,[ErrorMessage])\r\n     VALUES\r\n           (@ServerName\r\n           ,@UpdateTime\r\n           ,@IsSuccess\r\n           ,@ServerOnline\r\n           ,@ErrorCode\r\n           ,@ErrorMessage)", pingResponse);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdatePingResponseAsync(PingResponse pingResponse)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                await db.QueryAsync("UPDATE [cb].[PingResponse]\r\n   SET [ServerName] = @ServerName\r\n      ,[UpdateTime] = @UpdateTime\r\n      ,[IsSuccess] = @IsSuccess\r\n      ,[ServerOnline] = @ServerOnline\r\n      ,[ErrorCode] = @ErrorCode\r\n      ,[ErrorMessage] = @ErrorMessage", pingResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Currency> GetCurrencyAsync(string currency)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<Currency>("SELECT * FROM [Currency] WHERE [code] = @currency", new { currency });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PingResponse> GetLatestPingRequestAsync(string ServerName)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<PingResponse>("SELECT [ServerName] ,[UpdateTime] ,[IsSuccess] ,[ServerOnline] ,[ErrorCode] ,[ErrorMessage] FROM [cb].[PingResponse] WHERE [ServerName] = @ServerName ORDER BY [UpdateTime] Desc", new { ServerName });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ValidateAccountResponse> GetValidateAccountResponseAsync(string messageID)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<ValidateAccountResponse>("SELECT * FROM cb.ValidateAccountResponse WHERE RequestID = @requestID", new
                {
                    requestID = messageID
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertPostRequestAsync(PostRequest postRequest)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                await db.QueryAsync("INSERT INTO [cb].[PostRequest]\r\n           ([id]\r\n           ,[Session]\r\n           ,[Created]\r\n           ,[RequestID]\r\n           ,[Raw])\r\n     VALUES\r\n           (@id\r\n           ,@Session\r\n           ,@Created\r\n           ,@RequestID\r\n           ,@Raw)", postRequest);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertPostResponseAsync(PostResponse postResponse)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                await db.QueryAsync("INSERT INTO [cb].[PostResponse]\r\n           ([id]\r\n           ,[Session]\r\n           ,[Created]\r\n           ,[RequestID]\r\n           ,[Raw])\r\n     VALUES\r\n           (@id\r\n           ,@Session\r\n           ,@Created\r\n           ,@RequestID\r\n           ,@Raw)", postResponse);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertValidateAccountRequestAsync(ValidateAccountRequest validateAccountRequest)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                await db.QueryAsync("INSERT INTO [cb].[ValidateAccountRequest]\r\n           ([id]\r\n           ,[Session]\r\n           ,[Created]\r\n           ,[RequestID]\r\n           ,[Raw])\r\n     VALUES\r\n           (@id\r\n           ,@Session\r\n           ,@Created\r\n           ,@RequestID\r\n           ,@Raw)", validateAccountRequest);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertValidateAccountResponseAsync(ValidateAccountResponse validateAccountResponse)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                await db.QueryAsync("INSERT INTO [cb].[ValidateAccountResponse]\r\n           ([id]\r\n           ,[Session]\r\n           ,[Created]\r\n           ,[RequestID]\r\n           ,[Raw])\r\n     VALUES\r\n           (@id\r\n           ,@Session\r\n           ,@Created\r\n           ,@RequestID\r\n           ,@Raw)", validateAccountResponse);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertValidateReferenceAccountResponseAsync(ValidateReferenceAccountResponse validateReferenceAccountResponse)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                await db.QueryAsync("INSERT INTO [cb].[ValidateReferenceAccountResponse]\r\n           ([id]\r\n           ,[Session]\r\n           ,[Created]\r\n           ,[RequestID]\r\n           ,[Raw])\r\n     VALUES\r\n           (@id\r\n           ,@Session\r\n           ,@Created\r\n           ,@RequestID\r\n           ,@Raw)", validateReferenceAccountResponse);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ValidateAccountResponse> GetLatestValidateAccountResponseAsync(string sessionID)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<ValidateAccountResponse>("SELECT TOP 1 * FROM cb.ValidateAccountResponse WHERE Session = @sessionID ORDER BY Created DESC", new { sessionID });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertValidateReferenceAccountRequestAsync(ValidateReferenceAccountRequest validateReferenceAccountRequest)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                await db.QueryAsync("INSERT INTO [cb].[ValidateReferenceAccountRequest]\r\n           ([id]\r\n           ,[Session]\r\n           ,[Created]\r\n           ,[RequestID]\r\n           ,[Raw])\r\n     VALUES\r\n           (@id\r\n           ,@Session\r\n           ,@Created\r\n           ,@RequestID\r\n           ,@Raw)", validateReferenceAccountRequest);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Device> GetDevice(Guid id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<Device>("SELECT * FROM Device WHERE id = @id", new { id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Branch> GetBranch(Guid id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<Branch>("SELECT * FROM Branch WHERE id = @id", new { id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Bank> GetBankByDeviceIDAsync(Guid deviceID)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<Bank>("SELECT TOP 1 k.* FROM Bank k INNER JOIN Branch b on b.bank_id = k.id INNER JOIN Device d on d.branch_id = b.id WHERE d.id = @DeviceID", new
                {
                    DeviceID = deviceID
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SearchAccountInTransactionPermissionList_Result> SearchAccountInTransactionPermissionList(int TxTypeID, string AccountNumber)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<SearchAccountInTransactionPermissionList_Result>("EXEC SearchAccountInTransactionPermissionList @TxTypeID @AccountNumber", new { TxTypeID, AccountNumber });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountPermission> GetAccountPermissionAsync(Guid id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<AccountPermission>("SELECT * FROM cb.AccountPermission WHERE id = @id", new { id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Account> GetAccountAsync(Guid id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<Account>("SELECT * FROM cb.Account WHERE id = @id", new { id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AccountPermissionItem> GetAccountPermissionItemAsync(Guid id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<AccountPermissionItem>("SELECT * FROM cb.AccountPermissionItem WHERE id = @id", new { id });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CheckAccountAgainstAccountPermission_Result> CheckAccountAgainstAccountPermissionAsync(int transactionTypeListITemID, string account_number, string language)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<CheckAccountAgainstAccountPermission_Result>("EXEC CheckAccountAgainstAccountPermission @transactionTypeListITemID, @account_number, @language", new { transactionTypeListITemID, account_number, language });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<TransactionTypeListItem> GetTransactionTypeListItemAsync(int id)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QueryFirstOrDefaultAsync<TransactionTypeListItem>("SELECT * FROM TransactionTypeListItem WHERE id = @id", new { id });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
