// CashSwift.API.DapperDataAccessLibrary.Models.IAPIUserDataAccess
using CashSwift.Finacle.Integration.DataAccess.Entities;

namespace CashSwift.Finacle.Integration.DataAccess.Dapper
{
    public interface IAPIUserDataAccess
    {
        Task<APIUser> GetAPIUserAsync(Guid AppID);
    }
}