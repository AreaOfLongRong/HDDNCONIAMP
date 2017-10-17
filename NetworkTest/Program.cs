using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            //GPSUDPListener li = new GPSUDPListener();
            //li.StartReceive2();

            DateTime t1 = new DateTime(2017, 10, 11, 10, 20, 30);
            DateTime t2 = new DateTime(2017, 10, 11, 10, 21, 30);
            Console.WriteLine((t2 - t1).TotalMilliseconds);


            PingTest();

            Console.ReadLine();
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

        private static void PingTest()
        {
            string[] ipList = new string[200];
            for (int i = 0; i < 200; i++)
            {
                ipList[i] = "192.168.1." + (i + 1);
            }
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    foreach (var ip in ipList)
                    {
                        Ping p = new Ping();
                        string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                        byte[] buffer = Encoding.ASCII.GetBytes(data);
                        PingOptions options = new PingOptions(64, true);

                        try
                        {
                            p.PingCompleted += P_PingCompleted;
                            p.SendAsync(ip, 5000, buffer, options, ip);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                        finally
                        {
                            p.Dispose();
                        }
                    }
                    Thread.Sleep(10 * 1000);
                }
            });
        }

        private static void P_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            Console.WriteLine(string.Format("Ping {0} 的结果为：{1}", e.UserState.ToString(), e.Reply.Status.ToString()));
        }
    }
}
