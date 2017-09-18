using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BMap.NET.WindowsForm.Utils;
using HDDNCONIAMP.DB.Model;
using log4net;

namespace HDDNCONIAMP.Network
{
    /// <summary>
    /// GPS信号UDP监听器
    /// </summary>
    public class GPSUDPListener
    {

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(GPSUDPListener));

        /// <summary>
        /// 接收到GPS消息委托
        /// </summary>
        /// <param name="device"></param>
        public delegate void OnReceiveGPSDelegate(AudioAndVideoDevice device);

        /// <summary>
        /// 接收到GPS信号事件
        /// </summary>
        public event OnReceiveGPSDelegate OnReceiveGPS;

        /// <summary>
        /// UDP监听端口
        /// </summary>
        private const int Port = 8340;

        private const int GPS_MESSAGE_HEADER_LENGTH = 16;

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

        /// <summary>
        /// 开启接收UDP消息
        /// </summary>
        public void StartReceive()
        {
            udpcRecv = new UdpClient(Port);
            thrRecv = new Thread(ReceiveMessage);
            thrRecv.Start();
            logger.Info("GPS UDP监听器已成功启动");
        }

        /// <summary>
        /// 停止接收UDP消息
        /// </summary>
        public void StopReceive()
        {
            if (thrRecv != null)
                thrRecv.Abort();  //先关闭线程
            if (udpcRecv != null)
                udpcRecv.Close();
            logger.Info("GPS UDP监听器已关闭");
        }

        /// <summary>
        /// 接收消息处理
        /// </summary>
        private void ReceiveMessage()
        {
            //监听所有设备发来的UDP消息
            IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Any, Port);
            while (true)
            {
                try
                {
                    //UDP接收GPS数据格式：
                    //4字节：Type = 5;
                    //4字节：设备ID；
                    //8字节：时间戳；
                    //后续数据：标准的NMEA 0183协议GPS数据
                    byte[] bytes = udpcRecv.Receive(ref ipendpoint);

                    int deviceId = bytes[4] << 24 | bytes[5] << 16 | bytes[6] << 8 | bytes[7];

                    byte[] gpsdata = new byte[bytes.Length - GPS_MESSAGE_HEADER_LENGTH];
                    for (int i = GPS_MESSAGE_HEADER_LENGTH; i < bytes.Length; i++)
                    {
                        gpsdata[i - GPS_MESSAGE_HEADER_LENGTH] = bytes[i];
                    }
                    string ip = ipendpoint.Address.ToString();
                    string message = Encoding.Default.GetString(gpsdata, 0, gpsdata.Length);
                    logger.Info("收到来自“" + ip +
                        "”的UDP消息：“" + message + "”。");

                    if (message.StartsWith("$GPRMC"))
                    {
                        //切割字符串
                        string[] temp = message.Split(',');
                        AudioAndVideoDevice device = new AudioAndVideoDevice();
                        //device.Name = ip;
                        device.Name = (bytes[4] << 24 | bytes[5] << 16 | bytes[6] << 8 | bytes[7]).ToString();  //设备的ID
                        try
                        {
                            double[] latLon = GPS2BD09.wgs2bd(Double.Parse(temp[3].Substring(0, 2))
                                                + Double.Parse(temp[3].Substring(2)) / 60.0,
                                                Double.Parse(temp[5].Substring(0, 3))
                                                + Double.Parse(temp[5].Substring(3)) / 60.0);
                            device.Lat = latLon[0];
                            device.Lon = latLon[1];
                            device.Alias = device.Name;
                            RaiseReceiveGPS(device);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("接收到的GPS位置信号有问题!", ex);
                            device.Lat = 0;
                            device.Lon = 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("接收数据出现异常", ex);
                }
            }
        }

        /// <summary>
        /// 上报接收到GPS信号事件
        /// </summary>
        /// <param name="device"></param>
        private void RaiseReceiveGPS(AudioAndVideoDevice device)
        {
            OnReceiveGPS?.Invoke(device);
        }

        #endregion

        #region 测试代码

        public void TestReceiveMessage()
        {
            for (;;)
            {
                AudioAndVideoDevice device = new AudioAndVideoDevice();
                device.Name = "26089";
                device.Lat = 40.8;
                device.Lon = 116.3;
                device.Alias = device.Name;
                RaiseReceiveGPS(device);
                Thread.Sleep(10 * 1000);
            }
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
