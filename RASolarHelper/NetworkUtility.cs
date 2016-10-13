using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.NetworkInformation;

namespace RASolarHelper
{
    public static class NetworkUtility
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        static extern int SendARP(int destinationIp, int sourceIp, byte[] macAddress, ref int PhyAddrLen);

        /// <summary>
        /// Gets the MAC address (<see cref="PhysicalAddress"/>) associated with the specified IP.
        /// </summary>
        /// <param name="ipAddress">The remote IP address.</param>
        /// <returns>The remote machine's MAC address.</returns>
        public static PhysicalAddress GetMacAddress(IPAddress ipAddress)
        {
            const int MacAddressLength = 6;
            int length = MacAddressLength;
            var macBytes = new byte[MacAddressLength];
            SendARP(BitConverter.ToInt32(ipAddress.GetAddressBytes(), 0), 0, macBytes, ref length);
            return new PhysicalAddress(macBytes);
        }

        /// <summary>
        /// Gets the MAC address (<see cref="PhysicalAddress"/>) associated with the specified IP.
        /// </summary>
        /// <param name="ipAddress">The remote IP address.</param>
        /// <returns>The remote machine's MAC address.</returns>
        public static PhysicalAddress GetMacAddress(string ipAddress)
        {
            IPAddress ipAddressGroup = IPAddress.Parse(ipAddress);

            const int MacAddressLength = 6;
            int length = MacAddressLength;
            var macBytes = new byte[MacAddressLength];
            SendARP(BitConverter.ToInt32(ipAddressGroup.GetAddressBytes(), 0), 0, macBytes, ref length);
            return new PhysicalAddress(macBytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="physicalAddress"></param>
        /// <returns></returns>
        public static string GetMacAddress(PhysicalAddress physicalAddress)
        {
            byte[] bytes = physicalAddress.GetAddressBytes();
            string sMacAddress = string.Empty;

            for (int i = 0; i < bytes.Length; i++)
            {
                // Display the physical address in hexadecimal.
                sMacAddress += bytes[i].ToString("X2");
                // Insert a hyphen after each byte, unless we are at the end of the address. 
                if (i != bytes.Length - 1)
                {
                    sMacAddress += "-";
                }
            }

            return sMacAddress;
        }
    }
}
