using CashSwift.API.Messaging.APIClients;
using CashSwift.API.Messaging.CDM.GUIControl.AccountsLists;
using CashSwift.Library.Standard.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CashSwift.API.Messaging.CDM.GUIControl.Clients
{
    public class GUIControlServiceClient : APIClient, IGUIControlClient
    {
        public GUIControlServiceClient(
          string apiBaseAddress,
          Guid appID,
          byte[] appKey,
          IConfiguration configuration)
          : base(new CashSwiftAPILogger(nameof(GUIControlServiceClient), configuration), apiBaseAddress, appID, appKey, configuration)
        {
        }

        public async Task<AccountsListResponse> GetAccountsListAsync(
          AccountsListRequest request)
        {
            return await SendAsync<AccountsListResponse>("api/AccountsList/GetAccountsList", request);
        }

        public async Task<AccountsListResponse> SearchAccountAsync(
          AccountsListRequest request)
        {
            return await SendAsync<AccountsListResponse>("api/AccountsList/SearchAccountsList", request);
        }
    }
}
