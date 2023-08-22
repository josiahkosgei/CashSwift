using System.Threading.Tasks;

namespace CashSwift.API.Messaging.Communication.Monitoring
{
    public interface IMonitoringController
    {
        Task<CommunicationServerPingResponse> ServerPingAsync(
          CommunicationServerPingRequest request);
    }
}
