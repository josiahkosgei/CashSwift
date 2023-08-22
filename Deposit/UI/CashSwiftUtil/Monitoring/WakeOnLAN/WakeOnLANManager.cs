
// Type: CashSwiftUtil.Monitoring.WakeOnLAN.WakeOnLANManager
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CashSwiftUtil.Monitoring.WakeOnLAN
{
    internal class WakeOnLANManager
    {
        public static async Task WakeOnLan(string macAddress)
        {
            byte[] magicPacket = BuildMagicPacket(macAddress);
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces().Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback && n.OperationalStatus == OperationalStatus.Up))
            {
                IPInterfaceProperties iPInterfaceProperties = networkInterface.GetIPProperties();
                foreach (IPAddressInformation multicastAddress in iPInterfaceProperties.MulticastAddresses)
                {
                    IPAddress address = multicastAddress.Address;
                    if (address.ToString().StartsWith("ff02::1%", StringComparison.OrdinalIgnoreCase))
                    {
                        UnicastIPAddressInformation addressInformation = iPInterfaceProperties.UnicastAddresses.Where(u => u.Address.AddressFamily == AddressFamily.InterNetworkV6 && !u.Address.IsIPv6LinkLocal).FirstOrDefault();
                        if (addressInformation != null)
                        {
                            await SendWakeOnLan(addressInformation.Address, address, magicPacket);
                            break;
                        }
                    }
                    else if (address.ToString().Equals("224.0.0.1"))
                    {
                        UnicastIPAddressInformation addressInformation = iPInterfaceProperties.UnicastAddresses.Where(u => u.Address.AddressFamily == AddressFamily.InterNetwork && !iPInterfaceProperties.GetIPv4Properties().IsAutomaticPrivateAddressingActive).FirstOrDefault();
                        if (addressInformation != null)
                        {
                            await SendWakeOnLan(addressInformation.Address, address, magicPacket);
                            break;
                        }
                    }
                }
            }
            magicPacket = null;
        }

        private static byte[] BuildMagicPacket(string macAddress)
        {
            macAddress = Regex.Replace(macAddress, "[: -]", "");
            byte[] buffer = new byte[6];
            for (int index = 0; index < 6; ++index)
                buffer[index] = Convert.ToByte(macAddress.Substring(index * 2, 2), 16);
            using (MemoryStream output = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(output))
                {
                    for (int index = 0; index < 6; ++index)
                        binaryWriter.Write(byte.MaxValue);
                    for (int index = 0; index < 16; ++index)
                        binaryWriter.Write(buffer);
                }
                return output.ToArray();
            }
        }

        private static async Task SendWakeOnLan(
          IPAddress localIpAddress,
          IPAddress multicastIpAddress,
          byte[] magicPacket)
        {
            using (UdpClient client = new UdpClient(new IPEndPoint(localIpAddress, 0)))
            {
                int num = await client.SendAsync(magicPacket, magicPacket.Length, multicastIpAddress.ToString(), 9);
            }
        }
    }
}
