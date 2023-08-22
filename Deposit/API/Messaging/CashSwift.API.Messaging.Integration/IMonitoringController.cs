
// Type: CashSwift.API.Messaging.Integration.IMonitoringController
// Assembly: CashSwift.API.Messaging.Integration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAB85D66-8EA2-4ACC-B89A-1F5088BA6930
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwift.API.Messaging.Integration.dll

using CashSwift.API.Messaging.Integration.ServerPing;
using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Integration
{
    public interface IMonitoringController
    {
        Task<IntegrationServerPingResponse> ServerPingAsync(
          IntegrationServerPingRequest request);
    }
}
