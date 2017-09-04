using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NetworkTest
{
    class GPSUDPListener
    {
        
        /// <summary>
        /// UDP监听端口
        /// </summary>
        private const int Port = 8340;

        /// <summary>
        /// 用于UDP发送的网络服务类
        /// </summary>
        private UdpClient udpcSend;

        /// <summary>
        /// 用于UDP接收的网络服务类
        /// </summary>
        private UdpClient udpcRecv;

        /// <summary>
        /// 线程：不断监听UDP报文
        /// </summary>
        Thread thrRecv;

        public GPSUDPListener()
        {

        }

        #region 接收数据

        public void StartReceive2()
        {
            UdpClient udpclient = new UdpClient(Port);
            IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Any, Port);
            try
            {
                while (true)
                {
                    byte[] bytes = udpclient.Receive(ref ipendpoint);
                    string strIP = "信息来自" + ipendpoint.Address.ToString();
                    string strInfo = Encoding.Default.GetString(bytes, 0, bytes.Length);
                    Console.WriteLine(strInfo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                udpclient.Close();
            }
        }

        /// <summary>
        /// 开启接收UDP消息
        /// </summary>
        public void StartReceive()
        {
            IPAddress ipAddress = Dns.Resolve(Dns.GetHostName()).AddressList[0];
            //IPEndPoint localIpep = new IPEndPoint(
            //        IPAddress.Parse("127.0.0.1"), Port); // 本机IP和监听端口号
            IPEndPoint localIpep = new IPEndPoint(
                    ipAddress, Port); // 本机IP和监听端口号
            udpcRecv = new UdpClient(localIpep);
            thrRecv = new Thread(ReceiveMessage);
            thrRecv.Start();
        }

        /// <summary>
        /// 停止接收UDP消息
        /// </summary>
        public void StopReceive()
        {
            thrRecv.Abort();  //先关闭线程
            udpcRecv.Close();
        }

        /// <summary>
        /// 接收消息处理
        /// </summary>
        private void ReceiveMessage()
        {
            //监听所有设备发来的UDP消息
            IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Any, Port);
            while (true)
            {
                try
                {
                    byte[] bytRecv = udpcRecv.Receive(ref remoteIpep);
                    if (bytRecv == null)
                        continue;
                    string message = Encoding.Unicode.GetString(
                        bytRecv, 0, bytRecv.Length);
                    string[] temp = message.Split(':');
                    string[] temp2 = temp[1].Split(',');
                    double Lat = ConvertDegreesToDigital(string.Format("{0}°{1}′{2}″",
                        temp2[3].Substring(0, 2),
                        temp2[3].Substring(2, 2),
                        Double.Parse(string.Format("0.{1}", temp2[3].Substring(5))) * 60));
                    double Lon = ConvertDegreesToDigital(string.Format("{0}°{1}′{2}″",
                        temp2[5].Substring(0, 2),
                        temp2[5].Substring(2, 2),
                        Double.Parse(string.Format("0.{1}", temp2[5].Substring(5))) * 60));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        
        #endregion

        #region 发送数据

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="message"></param>
        public void StartSendMessage(string message)
        {
            // 匿名发送
            //udpcSend = new UdpClient(0);             // 自动分配本地IPv4地址

            // 实名发送
            IPEndPoint localIpep = new IPEndPoint(
                IPAddress.Parse("127.0.0.1"), 12345); // 本机IP，指定的端口号
            udpcSend = new UdpClient(localIpep);

            Thread thrSend = new Thread(SendMessage);
            thrSend.Start(message);
        }

        private void SendMessage(object obj)
        {
            string message = (string)obj;
            byte[] sendbytes = Encoding.Unicode.GetBytes(message);
            IPEndPoint remoteIpep = new IPEndPoint(
                IPAddress.Parse("127.0.0.1"), Port); // 发送到的IP地址和端口号

            udpcSend.Send(sendbytes, sendbytes.Length, remoteIpep);
            udpcSend.Close();
        }

        #endregion


        #region 方法

        /// <summary>
        /// 数字经纬度和度分秒经纬度转换 (Digital degree of latitude and longitude and vehicle to latitude and longitude conversion)
        /// </summary>
        /// <param name="digitalLati_Longi">数字经纬度</param>
        /// <return>度分秒经纬度</return>
        static public string ConvertDigitalToDegrees(string digitalLati_Longi)
        {
            double digitalDegree = Convert.ToDouble(digitalLati_Longi);
            return ConvertDigitalToDegrees(digitalDegree);
        }

        /// <summary>
        /// 数字经纬度和度分秒经纬度转换 (Digital degree of latitude and longitude and vehicle to latitude and longitude conversion)
        /// </summary>
        /// <param name="digitalDegree">数字经纬度</param>
        /// <return>度分秒经纬度</return>
        static public string ConvertDigitalToDegrees(double digitalDegree)
        {
            const double num = 60;
            int degree = (int)digitalDegree;
            double tmp = (digitalDegree - degree) * num;
            int minute = (int)tmp;
            double second = (tmp - minute) * num;
            string degrees = "" + degree + "°" + minute + "′" + second + "″";
            return degrees;
        }


        /// <summary>
        /// 度分秒经纬度(必须含有'°')和数字经纬度转换
        /// </summary>
        /// <param name="digitalDegree">度分秒经纬度</param>
        /// <return>数字经纬度</return>
        static public double ConvertDegreesToDigital(string degrees)
        {
            const double num = 60;
            double digitalDegree = 0.0;
            int d = degrees.IndexOf('°');           //度的符号对应的 Unicode 代码为：00B0[1]（六十进制），显示为°。
            if (d < 0)
            {
                return digitalDegree;
            }
            string degree = degrees.Substring(0, d);
            digitalDegree += Convert.ToDouble(degree);

            int m = degrees.IndexOf('′');           //分的符号对应的 Unicode 代码为：2032[1]（六十进制），显示为′。
            if (m < 0)
            {
                return digitalDegree;
            }
            string minute = degrees.Substring(d + 1, m - d - 1);
            digitalDegree += ((Convert.ToDouble(minute)) / num);

            int s = degrees.IndexOf('″');           //秒的符号对应的 Unicode 代码为：2033[1]（六十进制），显示为″。
            if (s < 0)
            {
                return digitalDegree;
            }
            string second = degrees.Substring(m + 1, s - m - 1);
            digitalDegree += (Convert.ToDouble(second) / (num * num));

            return digitalDegree;
        }


        /// <summary>
        /// 度分秒经纬度(必须含有'/')和数字经纬度转换
        /// </summary>
        /// <param name="digitalDegree">度分秒经纬度</param>
        /// <param name="cflag">分隔符</param>
        /// <return>数字经纬度</return>
        static public double ConvertDegreesToDigital_default(string degrees)
        {
            char ch = '/';
            return ConvertDegreesToDigital(degrees, ch);
        }

        /// <summary>
        /// 度分秒经纬度和数字经纬度转换
        /// </summary>
        /// <param name="digitalDegree">度分秒经纬度</param>
        /// <param name="cflag">分隔符</param>
        /// <return>数字经纬度</return>
        static public double ConvertDegreesToDigital(string degrees, char cflag)
        {
            const double num = 60;
            double digitalDegree = 0.0;
            int d = degrees.IndexOf(cflag);
            if (d < 0)
            {
                return digitalDegree;
            }
            string degree = degrees.Substring(0, d);
            digitalDegree += Convert.ToDouble(degree);

            int m = degrees.IndexOf(cflag, d + 1);
            if (m < 0)
            {
                return digitalDegree;
            }
            string minute = degrees.Substring(d + 1, m - d - 1);
            digitalDegree += ((Convert.ToDouble(minute)) / num);

            int s = degrees.Length;
            if (s < 0)
            {
                return digitalDegree;
            }
            string second = degrees.Substring(m + 1, s - m - 1);
            digitalDegree += (Convert.ToDouble(second) / (num * num));

            return digitalDegree;
        }

        #endregion
    }
}
