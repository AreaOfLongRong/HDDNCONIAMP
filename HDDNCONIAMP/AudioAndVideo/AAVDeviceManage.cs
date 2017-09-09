using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BMap.NET.WindowsForm;
using BMap.NET.WindowsForm.BMapElements;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.Network;

namespace HDDNCONIAMP.AudioAndVideo
{
    /// <summary>
    /// 音视频设备管理类
    /// </summary>
    public class AAVDeviceManage
    {

        /// <summary>
        /// 获取或设置音视频设备列表
        /// </summary>
        public List<AudioAndVideoDevice> AAVDeviceList { get; set; }

        /// <summary>
        /// 设备标注点列表
        /// </summary>
        public List<BVideoPoint> BVideoPointList { get; set; }

        /// <summary>
        /// 获取当前更新的音视频设备
        /// </summary>
        public AudioAndVideoDevice CurrentUpdateAAVDevice { get; private set; }

        public AAVDeviceManage(GPSUDPListener listener)
        {
            AAVDeviceList = new List<AudioAndVideoDevice>();
            BVideoPointList = new List<BVideoPoint>();
            listener.OnReceiveGPS += Listener_OnReceiveGPS;
        }

        /// <summary>
        /// 监听接收到GPS消息事件。
        /// </summary>
        /// <param name="device"></param>
        private void Listener_OnReceiveGPS(AudioAndVideoDevice device)
        {
            AudioAndVideoDevice temp =
                AAVDeviceList.Find( a => a.Name == device.Name);
            if(temp == null)
            {
                device.Alias = device.Name;
                //设备不存在，添加到列表中
                AAVDeviceList.Add(device);
            }
            else
            {
                //设备存在，更新列表内容
                temp.Lat = device.Lat;
                temp.Lon = device.Lon;
            }

            BVideoPoint currentPoint = BVideoPointList.Find(delegate (BVideoPoint p) {
                return p.Name == device.Name;
            });
            if (currentPoint != null)
            {
                currentPoint.Location = new LatLngPoint(device.Lon, device.Lat);
            }
            else
            {
                BVideoPoint p = new BVideoPoint();
                p.Location = new LatLngPoint(device.Lon, device.Lat);
                p.Name = device.Name;
                p.Alias = device.Alias;
                p.IsOnline = true;
                BVideoPointList.Add(p);
            }

            CurrentUpdateAAVDevice = device;
        }
    }
}
