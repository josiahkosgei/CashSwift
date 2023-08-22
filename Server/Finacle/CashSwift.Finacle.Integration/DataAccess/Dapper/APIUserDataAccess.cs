// CashSwift.API.DapperDataAccessLibrary.Models.APIUserDataAccess
using System.Data;
using CashSwift.Finacle.Integration.DataAccess.Entities;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CashSwift.Finacle.Integration.DataAccess.Dapper
{
    public class APIUserDataAccess : IAPIUserDataAccess
    {
        private string connectionString;

        public APIUserDataAccess(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Default") ?? throw new ArgumentNullException("connectionString");
        }

        public async Task<APIUser> GetAPIUserAsync(Guid AppID)
        {
            try
            {
                using IDbConnection db = new SqlConnection(connectionString);
                return await db.QuerySingleOrDefaultAsync<APIUser>("SELECT TOP 1 * FROM [api].[APIUser] WHERE AppId = @AppID", new { AppID });
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}