using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.Utils;
using log4net;
using HDDNCONIAMP.Utils;

namespace HDDNCONIAMP.Network.UDP
{
    /// <summary>
    /// UDP监听器，处理UDP消息。
    /// </summary>
    public class UDPListener
    {

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(UDPListener));

        /// <summary>
        /// UDP监听端口
        /// </summary>
        private int mPortReceive = 8340;

        /// <summary>
        /// UDP接收网络服务数据对象
        /// </summary>
        private UdpClient mUDPReceive;

        /// <summary>
        /// 监听UDP接收报文线程
        /// </summary>
        private Thread mThreadReceive;

        #region 委托和实现

        /// <summary>
        /// 接收到GPS消息委托
        /// </summary>
        /// <param name="device"></param>
        public delegate void OnReceiveGPSDelegate(AudioAndVideoDevice device);

        /// <summary>
        /// 接收到GPS信号事件
        /// </summary>
        public event OnReceiveGPSDelegate OnReceiveGPS;


        #endregion


        public UDPListener()
        {

        }

        #region 接收消息

        /// <summary>
        /// 开启接收UDP消息
        /// </summary>
        public void StartReceive()
        {
            mUDPReceive = new UdpClient(mPortReceive);
            mThreadReceive = new Thread(ReceiveMessage);
            mThreadReceive.Start();
            logger.Info("UDP监听器已成功启动");
        }

        /// <summary>
        /// 停止接收UDP消息
        /// </summary>
        public void StopReceive()
        {
            mThreadReceive.Abort();  //先关闭线程
            mUDPReceive.Close();
            logger.Info("UDP监听器已关闭");
        }

        /// <summary>
        /// 处理接收的UDP消息
        /// </summary>
        private void ReceiveMessage()
        {
            //监听所有设备发来的UDP消息
            IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Any, mPortReceive);
            while (!LifeTimeControl.closing)
            {
                try
                {
                    //UDP接收GPS数据格式：
                    //4字节：Type = 5;
                    //4字节：设备ID；
                    //8字节：时间戳；
                    //后续数据：标准的NMEA 0183协议GPS数据
                    byte[] buffer = mUDPReceive.Receive(ref ipendpoint);
                    int type = BitConverter.ToInt16(buffer, 0);  //消息类型
                    switch (type)
                    {
                        case 8:  //透明串口数据

                            break;
                        case 5:  //GPS数据

                            break;
                        case 6:  //调度命令推送

                            break;
                        case 21:  //调度命令返回的序列号

                            break;
                        case 10: //报警信息

                            break;
                        case 11:  //云台控制

                            break;
                        case 12:  //获取列表

                            break;
                        case 13:  //上下线通知

                            break;
                        case 14:  //流量通知

                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("接收数据出现异常", ex);
                }
            }
        }

        /// <summary>
        /// UDP接收GPS数据格式：
        /// 4字节：Type = 5;
        /// 4字节：设备ID；
        /// 8字节：时间戳；
        /// 后续数据：标准的NMEA 0183协议GPS数据
        /// </summary>
        /// <param name="receiveData">接收到的数据</param>
        private void receiveGPSMessage(byte[] receiveData)
        {
            string message = Encoding.Default.GetString(receiveData, 16, receiveData.Length - 16);
            //切割字符串
            string[] temp = message.Split(',');
            AudioAndVideoDevice device = new AudioAndVideoDevice();
            device.Name = BitConverter.ToInt32(receiveData, 4).ToString();  //设备的ID
            device.Lat = Double.Parse(temp[3].Substring(0, 2))
                + Double.Parse(temp[3].Substring(2)) / 60.0;  //设备纬度
            device.Lon = Double.Parse(temp[5].Substring(0, 3))
                + Double.Parse(temp[5].Substring(3)) / 60.0;  //设备经度

            logger.Info("收到来自“" + device.Name +
                "”的UDP消息：“" + message + "”。");

            RaiseReceiveGPS(device);
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

    }
}
