using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //while (true)
            //{
            //    string message = "26903:$GPRMC,083559.00,A,4717.11437,N,00833.91522,E,0.004,77.52,091202,,,A*57";
            //    Console.WriteLine(message);
            //    StartSendMessage(message);
            //    Thread.Sleep(10 * 1000);
            //}

            GPSUDPListener li = new GPSUDPListener();
            li.StartReceive2();

        }

        /// <summary>
        /// 用于UDP发送的网络服务类
        /// </summary>
        private static UdpClient udpcSend;


        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="message"></param>
        public static void StartSendMessage(string message)
        {
            // 匿名发送
            //udpcSend = new UdpClient(0);             // 自动分配本地IPv4地址
            IPAddress ipAddress = Dns.Resolve(Dns.GetHostName()).AddressList[0];
            // 实名发送
            IPEndPoint localIpep = new IPEndPoint(
                ipAddress, 12345); // 本机IP，指定的端口号
            udpcSend = new UdpClient(localIpep);

            Thread thrSend = new Thread(SendMessage);
            thrSend.Start(message);
        }

        private static void SendMessage(object obj)
        {
            string message = (string)obj;
            byte[] sendbytes = Encoding.Unicode.GetBytes(message);
            IPAddress ipAddress = Dns.Resolve(Dns.GetHostName()).AddressList[0];
            IPEndPoint remoteIpep = new IPEndPoint(
                ipAddress, 18340); // 发送到的IP地址和端口号

            udpcSend.Send(sendbytes, sendbytes.Length, remoteIpep);
            udpcSend.Close();
        }

    }
}
