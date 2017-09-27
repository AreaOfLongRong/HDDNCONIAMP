using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BMap.NET.WindowsForm.Utils;
using HDDNCONIAMP.Utils;
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
        /// <param name="gpsInfo"></param>
        public delegate void ReceiveGPSInfoDelegate(GPSInfo gpsInfo);

        /// <summary>
        /// 接收到GPS信号事件
        /// </summary>
        public event ReceiveGPSInfoDelegate OnReceiveGPSInfo;

        /// <summary>
        /// UDP监听端口
        /// </summary>
        private const int Port = 8340;

        /// <summary>
        /// GSP消息头的长度
        /// </summary>
        private const int GPS_MESSAGE_HEADER_LENGTH = 16;

        /// <summary>
        /// 用于UDP接收的网络服务类
        /// </summary>
        private UdpClient udpcRecv;

        /// <summary>
        /// 线程：不断监听UDP报文
        /// </summary>
        Thread thrRecv;

        /// <summary>
        /// 监听UDP报文任务
        /// </summary>
        private Task mReceiveTask;

        public GPSUDPListener()
        {

        }

        #region 接收数据

        /// <summary>
        /// 开启接收UDP消息
        /// </summary>
        public void StartReceive()
        {
            try
            {
                udpcRecv = new UdpClient(Port);
                thrRecv = new Thread(ReceiveMessage);
                thrRecv.Start();
                logger.Info("GPS UDP监听器已成功启动!");
            }
            catch (SocketException se)
            {
                logger.Error("GPS UDP监听启动失败！", se);
                MessageBox.Show("GPS UDP监听端口" +
                    Port + "被占用！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 开启接收UDP消息
        /// </summary>
        public void StartReceive(CancellationToken ct)
        {
            try
            {
                udpcRecv = new UdpClient(Port);
                mReceiveTask = Task.Factory.StartNew(() => ReceiveMessage(ct), ct);
                logger.Info("GPS UDP监听器已成功启动!");
            }
            catch (SocketException se)
            {
                logger.Error("GPS UDP监听启动失败！", se);
                MessageBox.Show("GPS UDP监听端口" +
                    Port + "被占用！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
        /// 停止接收UDP消息
        /// </summary>
        public void StopReceive(CancellationToken ct)
        {
            if (udpcRecv != null)
                udpcRecv.Close();
            //if (mReceiveTask != null)
            //    mReceiveTask.Dispose();
            logger.Info("GPS UDP监听器已关闭");
        }

        /// <summary>
        /// 接收消息处理
        /// </summary>
        private void ReceiveMessage()
        {
            //监听所有设备发来的UDP消息
            IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Any, Port);
            while (!LifeTimeControl.closing)
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
                    string message = Encoding.Default.GetString(gpsdata, 0, gpsdata.Length);
                    logger.Info("收到来自ID为“" + deviceId +
                        "”的UDP消息：“" + message + "”。");

                    if (message.StartsWith("$GPRMC"))
                    {
                        //切割字符串
                        string[] temp = message.Split(',');
                        GPSInfo info = new GPSInfo();
                        info.ID = (bytes[4] << 24 | bytes[5] << 16 | bytes[6] << 8 | bytes[7]).ToString();  //设备的ID
                        try
                        {
                            double[] latLon = GPS2BD09.wgs2bd(Double.Parse(temp[3].Substring(0, 2))
                                                + Double.Parse(temp[3].Substring(2)) / 60.0,
                                                Double.Parse(temp[5].Substring(0, 3))
                                                + Double.Parse(temp[5].Substring(3)) / 60.0);
                            info.Lat = latLon[0];
                            info.Lon = latLon[1];
                        }
                        catch (Exception ex)
                        {
                            //经纬度解析异常，继续下次数据接收
                            logger.Error("接收到的GPS位置信号有问题!", ex);
                            continue;
                        }
                        info.Time = DateTime.Now.ToString();
                        //保存GPS信息到相应文件中
                        FileUtils.AppendGPSInfoToFile(info);
                        //上报GPS更新信息
                        RaiseReceiveGPS(info);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("接收数据出现异常", ex);
                }
            }
        }

        /// <summary>
        /// 接收消息处理
        /// </summary>
        private void ReceiveMessage(CancellationToken ct)
        {
            //监听所有设备发来的UDP消息
            IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Any, Port);
            while (!LifeTimeControl.closing)
            {
                if (ct.IsCancellationRequested)
                {
                    udpcRecv.Close();
                    logger.Info("GPS UDP监听器已关闭");
                    return;
                }

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
                    string message = Encoding.Default.GetString(gpsdata, 0, gpsdata.Length);
                    logger.Info("收到来自ID为“" + deviceId +
                        "”的UDP消息：“" + message + "”。");

                    if (message.StartsWith("$GPRMC"))
                    {
                        //切割字符串
                        string[] temp = message.Split(',');
                        GPSInfo info = new GPSInfo();
                        info.ID = (bytes[4] << 24 | bytes[5] << 16 | bytes[6] << 8 | bytes[7]).ToString();  //设备的ID
                        try
                        {
                            double[] latLon = GPS2BD09.wgs2bd(Double.Parse(temp[3].Substring(0, 2))
                                                + Double.Parse(temp[3].Substring(2)) / 60.0,
                                                Double.Parse(temp[5].Substring(0, 3))
                                                + Double.Parse(temp[5].Substring(3)) / 60.0);
                            info.Lat = latLon[0];
                            info.Lon = latLon[1];
                        }
                        catch (Exception ex)
                        {
                            //经纬度解析异常，继续下次数据接收
                            logger.Error("接收到的GPS位置信号有问题!", ex);
                            continue;
                        }
                        info.Time = DateTime.Now.ToString();
                        //保存GPS信息到相应文件中
                        FileUtils.AppendGPSInfoToFile(info);
                        //上报GPS更新信息
                        RaiseReceiveGPS(info);
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
        /// <param name="info">GPS信息</param>
        private void RaiseReceiveGPS(GPSInfo info)
        {
            OnReceiveGPSInfo?.Invoke(info);
        }

        #endregion

        #region 测试代码

        public void TestReceiveMessage()
        {
            for (;;)
            {
                GPSInfo device = new GPSInfo();
                device.ID = "26908";
                device.Lat = 40.04933;
                device.Lon = 116.31224;
                device.Time = DateTime.Now.ToString();
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
