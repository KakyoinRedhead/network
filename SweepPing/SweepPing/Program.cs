using System;
using System.Net.NetworkInformation;

namespace SweepPing 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write IP you want to ping:");
            string baseIP = Console.ReadLine();
            int timeout = 1000;

            for (int i = 1; i <= 255; i++)
            {
                string ip = baseIP + i.ToString();

                Ping ping = new Ping();
                ping.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);
                ping.SendAsync(ip, timeout, ip);
            }

            Console.ReadLine();
        }

        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;

            if (e.Error != null)
            {
                Console.WriteLine($"Ping {ip} failed: {e.Error.Message}");
                return;
            }

            if (e.Reply.Status == IPStatus.Success)
            {
                Console.WriteLine($"Ping {ip} succeeded: Received {e.Reply.Buffer.Length} bytes in {e.Reply.RoundtripTime}ms");
            }
            else
            {
                Console.WriteLine($"Ping {ip} failed: {e.Reply.Status}");
            }
        }
    
    }
}