using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ArtNet.Utils
{
    public static class AddressFinder
    {
        public static IPAddress FindIPAddressFromHostName(string hostname)
        {
            IPAddress address = IPAddress.None;
            try
            {
                if (IPAddress.TryParse(hostname, out address))
                    return address;

                IPAddress[] addresses = Dns.GetHostAddresses(hostname);
                foreach (IPAddress ad in addresses)
                {
                    if (ad.AddressFamily == AddressFamily.InterNetwork)
                    {
                        address = ad;
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERR] Failed to find IP for :\n host name = {0}\n exception={1}", hostname, e);
            }

            return address;
        }

        public static IPAddress GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static PhysicalAddress GetMacAddress()
        {
            return NetworkInterface.GetAllNetworkInterfaces().
                Where(nic => nic.OperationalStatus == OperationalStatus.Up).
                Select(nic => nic.GetPhysicalAddress()).FirstOrDefault();
        }
    }
}
