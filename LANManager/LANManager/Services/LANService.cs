using System;
using System.Net;
using System.Net.NetworkInformation;

namespace LANManager.Services
{
    public class LANService
    {
        public static void EnumDevice(string networkSegment = "192.168.0.")
        {
            try
            {
                Ping myPing = new Ping();
                myPing.PingCompleted += MyPing_PingCompleted;
                for (int i = 1; i <= 255; i++)
                {
                    string pingIP = networkSegment + i.ToString();
                    myPing.SendAsync(pingIP, 1000, null);
                }
                myPing.PingCompleted -= MyPing_PingCompleted;
            }
            catch (Exception)
            {

            }
        }

        private static void MyPing_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if(e.Reply.Status == IPStatus.Success)
            {
                Console.WriteLine(e.Reply.Address.ToString() + "|" + Dns.GetHostByAddress(IPAddress.Parse(e.Reply.Address.ToString())).HostName);
                Console.WriteLine(e.Reply.Address.ToString() + "|" + Dns.GetHostEntry(e.Reply.Address).HostName);
            }
        }
    }
}
