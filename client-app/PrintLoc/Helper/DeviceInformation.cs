using System;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Linq;

namespace PrintLoc.Helper
{
    public class DeviceInformation
    {
        public string GetLocalIPAddress()
        {
            try
            {
                string ipAddress = NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(n => n.OperationalStatus == OperationalStatus.Up)
                    .SelectMany(n => n.GetIPProperties()?.UnicastAddresses)
                    .FirstOrDefault(a => a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !IPAddress.IsLoopback(a.Address))?
                    .Address?
                    .ToString();

                return ipAddress;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving local IP address: " + ex.Message);
                return null;
            }
        }

        //public static string GetLocalIPAddress()
        //{
        //    try
        //    {
        //        string localIp = string.Empty;
        //        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

        //        foreach (IPAddress ip in host.AddressList)
        //        {
        //            if (ip.AddressFamily == AddressFamily.InterNetwork)
        //            {
        //                localIp = ip.ToString();
        //                break;
        //            }
        //        }

        //        return localIp;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error retrieving local IP address: " + ex.Message);
        //        return null;
        //    }
        //}

        public string GetProcessorName()
        {
            string processorName = string.Empty;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (ManagementObject obj in searcher.Get())
                {
                    processorName = obj["Name"].ToString();
                    break; // Assuming there's only one processor
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving processor information: " + ex.Message);
            }
            return processorName;
        }

        public string GetMachineName()
        {
            string machineName = Environment.MachineName;
            return machineName;
        }

        public string GetOperatingSystemInfo()
        {
            string osInfo = string.Empty;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject obj in searcher.Get())
                {
                    osInfo = obj["Caption"] + " - Version: " + obj["Version"];
                    break; // Assuming there's only one operating system
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving operating system information: " + ex.Message);
            }
            return osInfo;
        }


        public string GetUniqueDeviceIdentifier()
        {
            string macAddress = GetMacAddress();

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(macAddress));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < 4; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString().Substring(0, 8);
            }
        }

        private string GetMacAddress()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string macAddress = "";

            foreach (ManagementObject mo in moc)
            {
                if ((bool)mo["IPEnabled"])
                {
                    macAddress = mo["MacAddress"].ToString();
                    break;
                }
            }

            return macAddress;
        }
    }
}
