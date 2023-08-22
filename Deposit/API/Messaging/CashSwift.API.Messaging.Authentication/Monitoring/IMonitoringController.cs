
// Type: CashSwift.API.Messaging.Authentication.Monitoring.IMonitoringController
// Assembly: CashSwift.API.Messaging.Authentication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 670AB76D-75EA-4F80-9B4D-25B6824AF8A6
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwift.API.Messaging.Authentication.dll

using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Authentication.Monitoring
{
    public interface IMonitoringController
    {
        Task<AuthenticationServerPingResponse> ServerPingAsync(
          AuthenticationServerPingRequest request);
    }
}
