using CashSwift.API.Messaging.CDM.GUIControl.AccountsLists;
using System.Threading.Tasks;

namespace CashSwift.API.Messaging.CDM.GUIControl.Clients
{
    public interface IGUIControlClient
    {
        Task<AccountsListResponse> GetAccountsListAsync(
          AccountsListRequest request);

        Task<AccountsListResponse> SearchAccountAsync(
          AccountsListRequest request);
    }
}
