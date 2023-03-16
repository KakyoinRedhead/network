using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace Network
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NetworkInterface[] adaptery = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adaptery)
            {
                if (adapter.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    foreach (UnicastIPAddressInformation address in properties.UnicastAddresses)
                    {
                        if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            Console.WriteLine("IP Address: {0}", address.Address);
                            Console.WriteLine("Subnet Mask: {0}", address.IPv4Mask);
                            break;
                        }
                    }
                    foreach (GatewayIPAddressInformation gateway in properties.GatewayAddresses)
                    {
                        Console.WriteLine("Default Gateway: {0}", gateway.Address);
                    }
                }
            }

            //SerialPort serialPort = new SerialPort("COM7", 115200);
            TcpClient client = new TcpClient("192.168.100.222", 80);
            NetworkStream stream = client.GetStream();

            // poslat zpravu
            string message = "Hello, ESP8266!";
            byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);

            // přijmout zpravu
            data = new byte[256];
            int bytes = stream.Read(data, 0, data.Length);
            string response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", response);

            // vyčistit
            stream.Close();
            client.Close();
        }
    }
}