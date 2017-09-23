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

    public static class OperateNode
    {
        //获取已知MAC地址的IP地址
        static byte[] macbyte = new byte[6];
        static bool HaveGetMAC = false;

        static bool Lock = true;

        public static string getStringMAC()
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


        public static string getIPaddress(string MacAddress, ARPLIST ArpList)
        {
            // NeedIPNode.IpAddress = "AAAAAAAAAAAA";
            //  ArpList.Where(x => x. == i.Key)
            var a = ArpList.UsingArpList.Where(x => x.PhysicalAddress.Replace("-", "").ToUpper() == MacAddress).ToList().FirstOrDefault();
            return a == null ? null : a.InternetAddress;
        }


        public static string[] NetworkInterfaceCard()
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
        public static string GetIniMAC(string NICdescription)
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

                byte[] bytes = StringToByte(MyDiscoverTel);

                device.SendPacket(bytes);

                device.StartCapture();


                Console.WriteLine("-- Packet MacDiscoverTel sent successfuly.");


                while (Lock)
                {
                    Thread.Sleep(30);
                }


                device.StopCapture();

                //byte[] Topology = StringToByte(MyTopologyTel);
                //device.SendPacket(Topology);

                //Console.WriteLine("-- Packet 2 sent successfuly.");



            }
            catch (Exception e)
            {
                Console.WriteLine("-- " + e.Message);
            }

            //Close the pcap device
            device.Close();
            //Console.WriteLine("-- Device closed.");
            //Console.Write("Hit 'Enter' to exit...");
            //Console.ReadLine();

            return BytetoString(macbyte);
        }


        /// <summary>
        /// Prints the time and length of each received packet
        /// </summary>
        private static void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            var time = e.Packet.Timeval.Date;
            var len = e.Packet.Data.Length;

            if (len == 60)
            {
                string stringOut = string.Empty;


                Console.WriteLine("{0}:{1}:{2},{3} Len={4}",
                    time.Hour, time.Minute, time.Second, time.Millisecond, len);
                // Console.WriteLine(e.Packet.ToString());



                if (len == 60)
                {

                    //string s1 = string.Format("{0:X2}", e.Packet.Data[12]);
                    //string s2 = string.Format("{0:X2}", e.Packet.Data[13]);
                    //string s3 = string.Format("{0:X2}", e.Packet.Data[14]);
                    //string s4 = string.Format("{0:X2}", e.Packet.Data[15]);
                    //string s5 = string.Format("{0:X2}", e.Packet.Data[16]);


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
                        //foreach (byte bt in e.Packet.Data)
                        //{
                        //    stringOut = stringOut + " " + string.Format("{0:X2}", bt);

                        //}
                        //Console.WriteLine(stringOut);
                        //byte[] cmdData = { 85, 85, 83, 83, 255, 123, 99, 33, 55, 1, 1 };

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


        private static string BytetoString(byte[] Pbyte)
        {
            string s = string.Empty;
            foreach (byte bt in Pbyte)
            {
                //s = s + " " + string.Format("{0:X2}", bt);
                s = s + string.Format("{0:X2}", bt);

            }
            return s;
        }


        private static byte[] StringToByte(string InString)
        {
            string[] ByteStrings;

            ByteStrings = InString.Split(" ".ToCharArray());

            byte[] ByteOut;

            ByteOut = new byte[ByteStrings.Length];

            for (int i = 0; i < (ByteStrings.Length - 1); i++)
            {
                //ByteOut[i] = Convert.ToByte(("Ox"+ ByteStrings[i]));
                ByteOut[i] = (byte)Convert.ToByte(ByteStrings[i], 16);
            }
            return ByteOut;

        }
    }
}
