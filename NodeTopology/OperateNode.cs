using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpPcap;
using SharpPcap.LibPcap;
using SharpPcap.AirPcap;
using SharpPcap.WinPcap;
using System.Threading;
using System.Management;



namespace NodeTopology
{

    public class OperateNode
    {
        //获取已知MAC地址的IP地址
        byte[] macbyte = new byte[6];
        bool HaveGetMAC = false;

        bool Lock = true;

        public string getStringMAC()
        {
            if (HaveGetMAC)
            {
                return BytetoString(macbyte);
            }
            else
            {
                return "未获取到MAC地址,可能需要手工输入或确认网卡可以正确通讯！";
            }

        }


        public string getIPaddress(string MacAddress, ARPList ArpList)
        {
            // NeedIPNode.IpAddress = "AAAAAAAAAAAA";
            //  ArpList.Where(x => x. == i.Key)

            //Z-20170927：如果MAC地址为null，则直接返回
            if (MacAddress == null)
                return null;

            var a = ArpList.UsingArpList.Where(x => x.PhysicalAddress.Replace("-", "").ToUpper() == MacAddress).ToList().FirstOrDefault();
            return a == null ? null : a.InternetAddress;
        }


        public string[] NetworkInterfaceCard()
        {
            var devices = CaptureDeviceList.Instance;
            // Where(x=>x.LinkType);

            int i = devices.Count;

            string[] MyNIC = new string[i];

            int j = 0;

            foreach (var device in devices)
            {
                MyNIC[j] = device.Description;
                j++;
            }

            return MyNIC;

        }

        //获取初始MAC
        public string GetIniMAC(string NICdescription)
        {
            //string ver = SharpPcap.Version.VersionString;
            //Console.WriteLine("SharpPcap {0}, Example9.SendPacket.cs\n", ver);
            string mac = "";

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (mo["IPEnabled"].ToString() == "True")
                {
                    mac = mo["MacAddress"].ToString();//只获取第一个？

                    //string MyIP = mo["IPAddress"].ToString();

                    break;
                }
            }

            // Retrieve the device list
            var device = CaptureDeviceList.Instance.Where(x => x.Description.Equals(NICdescription)).First();

            //mac= device.MacAddress.ToString();

            // If no devices were found print an error

            // if (devices.Count < 1)
            // {
            //     //需要在此处抛出异常
            //     Console.WriteLine("No devices were found on this machine");
            //     return  "未找到网卡";
            // }

            // //Console.WriteLine("The following devices are available on this machine:");
            // //Console.WriteLine("----------------------------------------------------");
            // //Console.WriteLine();

            // //int i = 0;

            //var device = devices[0];

            //if (devices.Count == 1)
            //{
            //    device = devices[0];
            //}
            //else
            //{
            //    //选择一个网卡
            //    device = devices[1];

            //}

            device.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival);

            device.OnCaptureStopped += Device_OnCaptureStopped;

            // Open the device for capturing
            ///应该判断设备必须为WinPcapDevice
            int readTimeoutMilliseconds = 1000;
            //if (device is AirPcapDevice)
            //{
            //    // NOTE: AirPcap devices cannot disable local capture
            //    var airPcap = device as AirPcapDevice;
            //    airPcap.Open(SharpPcap.WinPcap.OpenFlags.DataTransferUdp, readTimeoutMilliseconds);
            //}
            //else if (device is WinPcapDevice)
            //{
            //    var winPcap = device as WinPcapDevice;
            //    winPcap.Open(SharpPcap.WinPcap.OpenFlags.DataTransferUdp | SharpPcap.WinPcap.OpenFlags.NoCaptureLocal, readTimeoutMilliseconds);
            //}
            //else if (device is LibPcapLiveDevice)
            //{
            //    var livePcapDevice = device as LibPcapLiveDevice;
            //    livePcapDevice.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
            //}
            //else
            //{
            //    //需要在日志中抛出异常！！！
            //    //throw new System.InvalidOperationException("unknown device type of " + device.GetType().ToString());
            //}


            //Open the device
            device.Open();

            //使用本机MAC地址发包!!!

            //Generate a random packet

            //byte[] bytes = GetRandomPacket();
            try
            {
                //Send the packet out the network device

                //获取本机MAC 本机   44-37-E6-94-EC-87
                //urinatedong 20170301

                mac = mac.Replace(":", " ");

                string MyDiscoverTel = "01 13 9D 00 00 00 " + mac + " 00 08 AA AA 03 00 13 9D 0C 01 00 00 00 00 00 "
                           + "00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 "
                           + "00 00 00 00 00 00";

                byte[] bytes = StringToBytes(MyDiscoverTel);

                System.Timers.Timer t = new System.Timers.Timer(30 * 1000);
                t.Elapsed += T_Elapsed;
                t.Start();
                Lock = true;

                device.SendPacket(bytes);
                device.StopCaptureTimeout = new TimeSpan(0, 0, 1);

                device.StartCapture();

                Console.WriteLine("-- Packet MacDiscoverTel sent successfuly.");


                while (Lock)
                {
                    Thread.Sleep(100);
                }


                device.StopCapture();

                //byte[] Topology = StringToByte(MyTopologyTel);
                //device.SendPacket(Topology);

                //Console.WriteLine("-- Packet 2 sent successfuly.");



            }
            catch (Exception e)
            {
                Console.WriteLine("-- " + e.Message);
                LogHelper.WriteLog("根节点MAC地址获取超时或发生异常：" + e.Message);
            }

            //Close the pcap device
            device.Close();
            //Console.WriteLine("-- Device closed.");
            //Console.Write("Hit 'Enter' to exit...");
            //Console.ReadLine();

            //return BytetoString(macbyte);

            //Z-20170927：如果获取到的MAC地址全为0，是不正确的。
            string macStr = BytetoString(macbyte);
            return macStr.Equals("000000000000") ? null : macStr;
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Lock = false;
        }

        private void Device_OnCaptureStopped(object sender, CaptureStoppedEventStatus status)
        {
            //switch (status)
            //{
            //    case CaptureStoppedEventStatus.CompletedWithoutError:
            //        LogHelper.WriteLog("成功获取根节点MAC地址。");
            //        break;
            //    case CaptureStoppedEventStatus.ErrorWhileCapturing:
            //        LogHelper.WriteLog("根节点MAC地址获取超时或发生异常。");
            //        break;
            //}
            //Lock = false;
        }


        /// <summary>
        /// Prints the time and length of each received packet
        /// </summary>
        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var time = e.Packet.Timeval.Date;
            var len = e.Packet.Data.Length;

            Console.WriteLine("{0}:{1}:{2},{3} Len={4}",
                time.Hour, time.Minute, time.Second, time.Millisecond, len);
            if (len == 60)
            {           
                if (len == 60)
                {
                    //确认是MAC请求的回应
                    if (string.Format("{0:X2}", e.Packet.Data[12]) == "00"
                        && string.Format("{0:X2}", e.Packet.Data[13]) == "14"
                        && string.Format("{0:X2}", e.Packet.Data[14]) == "AA"
                        && string.Format("{0:X2}", e.Packet.Data[15]) == "AA"
                        && string.Format("{0:X2}", e.Packet.Data[16]) == "03"
                        //&& string.Format("{0:X2}", e.Packet.Data[17]) == "00"
                        //&& string.Format("{0:X2}", e.Packet.Data[18]) == "13"
                        //&& string.Format("{0:X2}", e.Packet.Data[19]) == "9D"
                        )
                    {
                        //Z-20170927:如果接收不到网络内的Mesh设备响应，此处会一直阻塞整个线程                        
                        Array.Copy(e.Packet.Data, 28, macbyte, 0, 6);
                        if (Lock)
                        {
                            Lock = false;
                        }
                        LogHelper.WriteLog("device_OnPacketArrival经通过监听截取到目标MAC地址:" + BytetoString(macbyte));
                    }
                }
            }
        }

        /// <summary>
        /// 字节数组转字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string BytetoString(byte[] bytes)
        {
            string s = string.Empty;
            foreach (byte bt in bytes)
            {
                s = s + string.Format("{0:X2}", bt);
            }
            return s;
        }

        /// <summary>
        /// 字符串转字节数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private byte[] StringToBytes(string str)
        {
            string[] ByteStrings = str.Split(" ".ToCharArray());
            byte[] ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i < (ByteStrings.Length - 1); i++)
            {
                ByteOut[i] = (byte)Convert.ToByte(ByteStrings[i], 16);
            }
            return ByteOut;
        }
    }
}
