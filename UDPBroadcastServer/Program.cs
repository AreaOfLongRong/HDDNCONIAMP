using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UDPBroadcastServer
{
    class Program
    {
        static void Main(string[] args)
        {  
            UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 0));  
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 58000);  
            byte[] buf = Encoding.UTF8.GetBytes(Dns.GetHostEntry(Dns.GetHostName()).AddressList.Last().ToString());  
            Thread t = new Thread(new ThreadStart(RecvThread));  
            t.IsBackground = true;  
            t.Start();  
            while (true)  
            {  
                client.Send(buf, buf.Length, endpoint);  
                Thread.Sleep(1000);  
            }  
        }  
  
        static void RecvThread()
        {  
            UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 58000));  
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);  
            while (true)
            {  
                byte[] buf = client.Receive(ref endpoint);  
                string msg = Encoding.UTF8.GetString(buf);  
                Console.WriteLine(msg);  
            }  
        }  
    }
}
