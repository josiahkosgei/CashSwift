
// Type: CashSwiftUtil.Licensing.LicenseMechanism
// Assembly: CashSwiftUtil, Version=3.2.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 885F1C6C-21D2-4135-B89E-154B0975A233
// Assembly location: C:\DEV\maniwa\Coop\Coop\CashSwiftDeposit\App\UI\6.0\CashSwiftUtil.dll

using CashSwift.Library.Standard.Utilities;
using System;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace CashSwiftUtil.Licensing
{
    public class LicenseMechanism
    {
        private static string LicenseKeyFile = AppDomain.CurrentDomain.BaseDirectory + "License.txt";
        private static string ActivationKeyFile = AppDomain.CurrentDomain.BaseDirectory + "ActivationKey.txt";
        private CDMLicense _license;

        public CDMLicense License
        {
            get => _license;
            protected set => _license = value;
        }

        public DateTime ExpiryDate { get; }

        private string ActivationKey { get; }

        public LicenseMechanism()
        {
            try
            {
                ActivationKey = GenerateActivationKey();
                File.WriteAllText(ActivationKeyFile, ActivationKey);
                processLicense();
            }
            catch (Exception ex)
            {
                throw new Exception("License validation failed", ex);
            }
        }

        private void processLicense()
        {
            License = decryptLicense();
            if (ActivationKey != License.ActivationKey)
                throw new Exception("InvalidLicense");
            if (DateTime.Now > License.ExpiryDate)
                throw new Exception(string.Format("License expired on {0:yyyy-MM-dd} and is invalid", License.ExpiryDate));
        }

        private CDMLicense decryptLicense()
        {
            LicenseFile licenseFile = (LicenseFile)LicenseFile.Serializer.Deserialize(File.ReadAllText(LicenseKeyFile).ToStream());
            SymmetricKey symmetricKey = (SymmetricKey)SymmetricKey.Serializer.Deserialize(new LicenseEncryption().Decrypt(licenseFile.SymmetricKeyCypherText).ToStream());
            return (CDMLicense)CDMLicense.Serializer.Deserialize(LicenseEncryption.DecryptStringFromBytes(Convert.FromBase64String(licenseFile.CDMLicenseCypherText), symmetricKey.Key, symmetricKey.IV).ToStream());
        }

        private string GenerateActivationKey()
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Format("MACADDRESS={0}", GetMacAddress()));
                stringBuilder.AppendLine(string.Format("CPUID={0}", GetCpuId()));
                return Convert.ToBase64String(Hash(stringBuilder.ToString()));
            }
            catch (Exception ex)
            {
                throw new Exception("License Key Creation Failed", ex);
            }
        }

        public static string GetMacAddress()
        {
            string macAddress = "";
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            int index = 0;
            if (index < networkInterfaces.Length)
                macAddress = networkInterfaces[index].GetPhysicalAddress().ToString();
            return macAddress;
        }

        public static string GetCpuId()
        {
            string cpuId = null;
            try
            {
                foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("select * from Win32_Processor").Get())
                    cpuId = managementBaseObject["ProcessorId"].ToString();
                return cpuId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static byte[] Hash(string input)
        {
            try
            {
                return new SHA512Managed().ComputeHash(Encoding.UTF32.GetBytes(input));
            }
            catch (Exception ex)
            {
                throw new Exception("Error Generating Hash", ex);
            }
        }
    }
}
