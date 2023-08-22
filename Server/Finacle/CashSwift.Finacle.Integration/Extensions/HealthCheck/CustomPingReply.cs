using System.Net.NetworkInformation;

namespace CashSwift.Finacle.Integration.Extensions.HealthCheck
{
    internal class CustomPingReply
    {
        public IPStatus Status { get; set; }
    }
}
