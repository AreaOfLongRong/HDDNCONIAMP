using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HDDNCONIAMP.DB;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.Utils;
using log4net;
using SharpPcap;

namespace HDDNCONIAMP.Mesh
{
    /// <summary>
    /// Mesh设备操作接口类。
    /// 1、程序启动时，启动新的任务对网络内Mesh设备监测；
    /// 2、对每一个Mesh设备启动新的任务进行状态监控、视频信息获取等操作。
    /// </summary>
    public class MeshDeviceManage
    {
        #region 私有变量

        /// <summary>
        /// 日志记录器
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(FormMain));

        /// <summary>
        /// 获取已知MAC地址的IP地址
        /// </summary>
        private static byte[] macbyte = new byte[6];
        /// <summary>
        /// 是否已获取到MAC地址
        /// </summary>
        private static bool HaveGetMAC = false;
        /// <summary>
        /// 锁对象
        /// </summary>
        private static bool Lock = true;

        /// <summary>
        /// ARP列表
        /// </summary>
        private ARPList mARPList = new ARPList();

        /// <summary>
        /// 根节点
        /// </summary>
        private MeshNode mRootNode = new MeshNode();

        /// <summary>
        /// 根节点MAC地址
        /// </summary>
        private string mRootMAC;

        /// <summary>
        /// 根节点IP地址
        /// </summary>
        private string mRootIP;

        /// <summary>
        /// 实时Mesh节点组
        /// </summary>
        private MeshBlockNodes mRealtimeMBN = new MeshBlockNodes();

        /// <summary>
        /// 用于显示的Mesh节点组
        /// </summary>
        private MeshBlockNodes ShowBlockNodes = new MeshBlockNodes();

        /// <summary>
        /// 用于当MYBlockNodes收集完数据时，将ShowBlockNodes传入StoreBlockNodes并进行比对
        /// </summary>
        private MeshBlockNodes StoreBlockNodes = new MeshBlockNodes();

        /// <summary>
        /// MAC地址和Mesh节点的哈希表
        /// </summary>
        Hashtable hashTable = new Hashtable();

        /// <summary>
        /// 需要搜索的Mesh设备MAC地址
        /// </summary>
        List<string> NeedResearchMac = new List<string>();

        int RepeatTime = 0;

        int RepeatTimeForRead = 0;

        List<int> EveryColumnNodeCount = new List<int>();

        List<int> EveryColumnNodeCountForRead = new List<int>();

        private ReaderWriterLock _rReaderWriterLockwlock = new ReaderWriterLock();

        /// <summary>
        /// 读写锁
        /// </summary>
        private ReaderWriterLock _rwlock = new ReaderWriterLock();

        /// <summary>
        /// 扫描频率
        /// </summary>
        private int mScanRate = 10;

        private int ShowRate = 10;

        #endregion

        #region 委托事件

        /// <summary>
        /// 添加Mesh设备委托
        /// </summary>
        /// <param name="meshDeviceInfo">Mesh设备信息</param>
        public delegate void MeshDeviceAdd(MeshDeviceInfo meshDeviceInfo);

        /// <summary>
        /// 添加新的Mesh设备事件
        /// </summary>
        public event MeshDeviceAdd OnMeshDeviceAdded;

        /// <summary>
        /// 刷新Mesh设备委托
        /// </summary>
        /// <param name="meshDeviceInfo">Mesh设备信息</param>
        public delegate void MeshDeviceUpdate(MeshDeviceInfo meshDeviceInfo);

        /// <summary>
        /// 刷新Mesh设备信息事件
        /// </summary>
        public event MeshDeviceUpdate OnMeshDeviceUpdate;

        #endregion

        #region 属性

        /// <summary>
        /// 是否处于扫描中
        /// </summary>
        public bool IsScanning { get; set; }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public MeshDeviceManage()
        {
            mRealtimeMBN.MeshNodeList = new List<MeshNode>();
            mRealtimeMBN.MeshRelationList = new List<MeshRelation>();
            ShowBlockNodes.MeshNodeList = new List<MeshNode>();
            ShowBlockNodes.MeshRelationList = new List<MeshRelation>();
            StoreBlockNodes.MeshNodeList = new List<MeshNode>();
            StoreBlockNodes.MeshRelationList = new List<MeshRelation>();
        }

        public void StartScanMeshDevice()
        {
            //启动Mesh设备扫描任务
            Task.Factory.StartNew(() =>
            {
                //获取子网网段
                string subNet = SQLiteHelper.GetInstance()
                    .ApplicationSettingValueBykey(ApplicationSettingKey.SubNetworkIP).Value;

                IsScanning = true;
                while (IsScanning)
                {
                    //ping子网内所有IP，看是否能ping通
                    mARPList.PingSubNet(subNet);
                    //重新载入子网内ARP列表
                    mARPList.ReloadARP();

                    logger.Info("获取根节点MAC地址...");
                    mRootMAC = GetIniMAC(GetWiredNetworkCard()[0]);
                    logger.Info("根节点MAC地址为：" + mRootMAC);

                    logger.Info("获取根节点IP地址...");
                    mRootIP = getIPAddress(mRootMAC, mARPList);
                    logger.Info("根节点IP地址为：" + mRootIP);
                    if (mRootIP.Equals(string.Empty))
                    {
                        MessageBox.Show("未获取到根节点IP信息，请检查网络连接是否正常！");
                        return;
                    }

                    logger.Info("添加根节点到实时节点列表中...");
                    mRootNode.MACAddress = mRootMAC;
                    mRootNode.IPAddress = mRootIP;
                    mRealtimeMBN.MeshNodeList.Add(mRootNode);

                    logger.Info("使用Telnet命令扫描子网内的Mesh设备...");
                    ScanMeshDeviceBySendTelnet(mRootMAC);

                    RepeatTime = 1;

                    EveryColumnNodeCount.Clear();

                    EveryColumnNodeCount.Add(1);

                    bool search = false;

                    if (mRealtimeMBN.MeshNodeList.Count > 1)
                    {
                        search = true;
                        logger.Info("发现其他节点,开始循环读取NODE信息");
                    }

                    while (search)
                    {
                        ///用NeedtoClear来控制这一轮要找的NODE,用NeedResearchMac来控制下一轮要找的NODE
                        //复制NeedResearchMac到NeedtoClear
                        List<string> NeedtoClear = new List<string>(NeedResearchMac.ToArray());
                        //清空NeedResearchMac
                        NeedResearchMac.Clear();
                        if (NeedtoClear.Count == 0)
                        {
                            search = false;
                        }
                        else
                        {
                            ///有这一轮要填充信息的node
                            RepeatTime++;

                            EveryColumnNodeCount.Add(NeedtoClear.Count);

                            foreach (string s in NeedtoClear)
                            {
                                ///考虑使用线程并发加快拓扑访问速度!!!
                                ///给全部线程一个时间，要求在标准时间内in完成6秒?
                                ScanMeshDeviceBySendTelnet(s);
                            }
                        }
                        logger.Info("RealTimeNode全部拓扑信息读取完成");


                        this.PrintStatues(mRealtimeMBN.MeshNodeList.ToList(), mRealtimeMBN.MeshRelationList.ToList(), true);

                        
                        ///锁定信息并进行复制
                        _rwlock.AcquireWriterLock(100);


                        ///urinatedong 20170325 在将实时状态复制到ShowBlockNodes之前，先复制ShowBlockNodes到StoreBlockNodes
                        this.StoreBlockNodes.MeshNodeList = this.ShowBlockNodes.MeshNodeList.ToArray().ToList();
                        this.StoreBlockNodes.MeshRelationList = this.ShowBlockNodes.MeshRelationList.ToArray().ToList();

                        ///全部复制
                        this.RepeatTimeForRead = this.RepeatTime;
                        this.EveryColumnNodeCountForRead = this.EveryColumnNodeCount.ToArray().ToList();
                        this.ShowBlockNodes.MeshNodeList = this.mRealtimeMBN.MeshNodeList.ToArray().ToList();
                        this.ShowBlockNodes.MeshRelationList = this.mRealtimeMBN.MeshRelationList.ToArray().ToList();

                        // = new List<string>(NeedResearchMac.ToArray());

                        _rwlock.ReleaseWriterLock();
                        
                        ///清除所有内容
                        mRealtimeMBN.MeshNodeList.Clear();
                        mRealtimeMBN.MeshRelationList.Clear();
                        
                        ///将所有实时的拓扑节点复制到MYBlockNodes用于显示
                        ///清空实时拓扑用于显示
                        
                        //初步定为每隔1分钟总体扫描1次
                        Thread.Sleep(this.mScanRate * 1000);
                    }
                }

            });
        }

        public void StopScanMeshDevice()
        {
            IsScanning = false;
        }

        /// <summary>
        /// 获取MAC地址
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <param name="MacAddress">MAC地址</param>
        /// <param name="ArpList">ARP列表</param>
        /// <returns>IP地址</returns>
        public static string getIPAddress(string MacAddress, ARPList ArpList)
        {
            var a = ArpList.UsingArpList.Where(
                x => x.PhysicalAddress.Replace("-", "").ToUpper() == MacAddress)
                .ToList().FirstOrDefault();
            return a == null ? null : a.InternetAddress;
        }

        /// <summary>
        /// 获取网卡列表
        /// </summary>
        /// <returns>网卡列表</returns>
        public static string[] GetWiredNetworkCard()
        {
            List<string> nics = new List<string>();
            foreach (var device in CaptureDeviceList.Instance)
            {
                nics.Add(device.Description);
            }
            return nics.ToArray();

        }

        /// <summary>
        /// 获取初始MAC
        /// </summary>
        /// <param name="NICdescription"></param>
        /// <returns></returns>
        public static string GetIniMAC(string NICdescription)
        {
            string mac = "";
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["IPEnabled"].ToString() == "True")
                {
                    mac = mo["MacAddress"].ToString();//只获取第一个？
                    break;
                }
            }

            // Retrieve the device list
            var device = CaptureDeviceList.Instance.Where(x => x.Description.Equals(NICdescription)).First();
            

            device.OnPacketArrival +=
                new PacketArrivalEventHandler(device_OnPacketArrival);
            
            //Open the device
            device.Open();

            //使用本机MAC地址发包!!!
            try
            {
                //Send the packet out the network device
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
            }
            catch (Exception e)
            {
                Console.WriteLine("-- " + e.Message);
            }

            //Close the pcap device
            device.Close();

            return BytetoString(macbyte);
        }


        /// <summary>
        /// Prints the time and length of each received packet
        /// 每当收到数据包时打印时间和长度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        logger.Info("device_OnPacketArrival经通过监听截取到目标MAC地址:" + BytetoString(macbyte));
                    }
                }
            }
        }

        /// <summary>
        /// 通过发送Telnet命令，扫描Mesh设备
        /// </summary>
        /// <param name="mac">需要的扫描设备MAC地址</param>
        public void ScanMeshDeviceBySendTelnet(string mac)
        {

            //需要在这里try catch finally 以关闭连接
            //必须关闭连接!!!!!! tn.close!!!!

            logger.Info("开始获取MAC为:" + mac + " 的NODE的信息！！！ ");

            MeshNode mNode = null;
            if (mRealtimeMBN.MeshNodeList != null && mRealtimeMBN.MeshNodeList.Count > 0)
                mNode = mRealtimeMBN.MeshNodeList.Where(n => n.MACAddress.Equals(mac)).ToList().First();


            if (mNode != null)
            {

                MeshTelnet tn = new MeshTelnet(mNode.IPAddress);

                try
                {
                    MeshNode cacheNode = null;
                    if (hashTable.ContainsKey(mac))
                    {
                        cacheNode = (MeshNode)hashTable[mac];
                    }
                    string recvStr = tn.recvDataWaitWord("help", 1);

                    //电池电压
                    tn.sendData("battery");
                    recvStr = tn.recvDataWaitWord("V", 1);
                    mNode.Battery = (cacheNode != null) ? cacheNode.Battery : double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("V", ""));

                    //频率
                    Thread.Sleep(1);
                    tn.sendData("frequency");
                    recvStr = tn.recvDataWaitWord("MHz", 1);
                    mNode.Frequency = (cacheNode != null) ? cacheNode.Frequency : 616;
                    
                    //主发送功率
                    Thread.Sleep(1);
                    tn.sendData("txpower");
                    recvStr = tn.recvDataWaitWord("dBm", 1);
                    mNode.TxPower = (cacheNode != null) ? cacheNode.TxPower : double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("dBm", ""));
                    
                    //带宽
                    tn.sendData("bandwidth");
                    recvStr = tn.recvDataWaitWord("MHz", 1);
                    mNode.BandWidth = (cacheNode != null) ? cacheNode.BandWidth : double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("MHz", ""));

                    MeshNode cacheNod = new MeshNode();
                    cacheNod.MACAddress = mac;
                    cacheNod.IPAddress = mNode.IPAddress;
                    cacheNod.Frequency = mNode.Frequency;
                    cacheNod.BandWidth = mNode.BandWidth;
                    cacheNod.TxPower = mNode.TxPower;
                    cacheNod.Battery = mNode.Battery;
                    if (hashTable.ContainsKey(mac))
                    {
                        hashTable[mac] = cacheNod;
                        //上报更新事件
                        RaiseMeshDeviceUpdate(cacheNod);
                    }
                    else
                    {
                        hashTable.Add(mac, cacheNod);
                        //上报添加事件
                        RaiseMeshDeviceAdd(cacheNod);
                    }
                    
                    tn.sendData("mesh");
                    recvStr = tn.recvDataWaitWord("OK", 1);
                    string[] tempMesh = recvStr.Replace("OK\n#user@/>", "").Split(":".ToArray());

                    if (tempMesh.Length > 1)
                    {
                        int i = int.Parse(tempMesh[0]);
                        if (i > 0)
                        {
                            string[] meshs = tempMesh[1].Split(";".ToArray());

                            mNode.HasChild = true;
                            
                            for (int j = 0; j < meshs.Length; j++)
                            {
                                if (meshs[j] == "\r\n\n")
                                {
                                    break;
                                }

                                string SplitNewNode = meshs[j].Replace("\r", "").Replace("\n", "").Replace(".", "");
                                string[] MeshInfo = SplitNewNode.Split(",".ToCharArray());

                                ///如果用子节点登录?不再访问其根节点
                                string NeedToCheckMac = MeshInfo[6].Substring(0, 12);
                                string NeedToCheckIP = getIPAddress(NeedToCheckMac, mARPList);

                                if (mRealtimeMBN.MeshNodeList.Count > 1)
                                {
                                    int samenode = mRealtimeMBN.MeshNodeList.Where(x => x.MACAddress.Equals(NeedToCheckMac) && x.IPAddress.Equals(NeedToCheckIP)).ToList().Count;

                                    if (samenode == 0) //这是首次发现的NODE
                                    {
                                        MeshNode NewNode = new MeshNode(NeedToCheckMac);

                                        NewNode.IPAddress = NeedToCheckIP;

                                        MeshRelation relation = new MeshRelation(mNode, NewNode);

                                        relation.LocalPort = int.Parse(MeshInfo[0]);
                                        relation.TxSpeed = int.Parse(MeshInfo[2]);
                                        relation.TxSnr = int.Parse(MeshInfo[3]);
                                        relation.RxSpeed = int.Parse(MeshInfo[4]);
                                        relation.RxSnr = int.Parse(MeshInfo[5]);
                                        relation.FindTimes = 1;

                                        mRealtimeMBN.MeshNodeList.Add(NewNode);

                                        mRealtimeMBN.MeshRelationList.Add(relation);

                                        NeedResearchMac.Add(NeedToCheckMac);
                                    }
                                    else //找到了根节点，这时候跟新与根节点的端口关系
                                    {
                                        //对关系是否需要增加则需要判断一下是否有该关系
                                        if (mRealtimeMBN.MeshRelationList.Count > 0)
                                        {
                                            var needUpdateRelation = mRealtimeMBN.MeshRelationList.Where(
                                                x => x.LocalNode.MACAddress.Equals(NeedToCheckMac) && 
                                                x.LocalNode.IPAddress.Equals(NeedToCheckIP) &&
                                                x.RemoteNode.MACAddress.Equals(mNode.MACAddress) && 
                                                x.RemoteNode.IPAddress.Equals(mNode.IPAddress)).ToList();

                                            if (needUpdateRelation.Count > 0)
                                            {
                                                MeshRelation tempRelation = needUpdateRelation.FirstOrDefault();
                                                tempRelation.RemotePort = int.Parse(MeshInfo[0]);
                                                tempRelation.FindTimes = 2;
                                                //这里需要确认以下是否需要跟新RX和TX的信息！！！
                                            }
                                            else
                                            {
                                                var theOtherNode = mRealtimeMBN.MeshNodeList.Where(
                                                    x => x.MACAddress.Equals(NeedToCheckMac) && 
                                                    x.IPAddress.Equals(NeedToCheckIP)).ToList().First();

                                                MeshRelation NewRelation = new MeshRelation(mNode, theOtherNode);

                                                NewRelation.LocalPort = int.Parse(MeshInfo[0]);
                                                NewRelation.TxSpeed = int.Parse(MeshInfo[2]);
                                                NewRelation.TxSnr = int.Parse(MeshInfo[3]);
                                                NewRelation.RxSpeed = int.Parse(MeshInfo[4]);
                                                NewRelation.RxSnr = int.Parse(MeshInfo[5]);
                                                NewRelation.FindTimes = 1;

                                                this.mRealtimeMBN.MeshRelationList.Add(NewRelation);
                                            }
                                        }
                                    }
                                }
                                else //刚开始从根目录查找
                                {
                                    MeshNode NewNode = new MeshNode(NeedToCheckMac);
                                    NewNode.IPAddress = NeedToCheckIP;

                                    MeshRelation NewRelation = new MeshRelation(mNode, NewNode);

                                    NewRelation.LocalPort = int.Parse(MeshInfo[0]);
                                    NewRelation.TxSpeed = int.Parse(MeshInfo[2]);
                                    NewRelation.TxSnr = int.Parse(MeshInfo[3]);
                                    NewRelation.RxSpeed = int.Parse(MeshInfo[4]);
                                    NewRelation.RxSnr = int.Parse(MeshInfo[5]);
                                    NewRelation.FindTimes = 1;

                                    mRealtimeMBN.MeshNodeList.Add(NewNode);
                                    mRealtimeMBN.MeshRelationList.Add(NewRelation);
                                    this.NeedResearchMac.Add(NeedToCheckMac);

                                }
                                //0 ->  端口号
                                //1 ->  本地MAC
                                //2 ->  TX Speed
                                //3 ->  TX SNR
                                //4 ->  RX Speed
                                //5 ->  RX SNR
                                //6 ->  REMOTE MAC ADDRESS 子节点MAC地址                                
                            }
                        }
                        else
                        {
                            //     //此时只有根节点
                            //     //需要告知用户
                            logger.Info("此拓扑只发现单一节点，未发现子节点！！！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("在获取NODE信息时异常: " + ex.Message);
                }
                finally
                {
                    tn.close();
                }
            }
        }

        /// <summary>
        /// 在日志中打印当前扫描到的NODE和RELATION信息
        /// </summary>
        /// <param name="pnodelist">nodelist</param>
        /// <param name="prelationlist">relationlist</param>
        /// <param name="isonline">是否为实时数据，如果为前台显示用，则该项为否</param>
        private void PrintStatues(List<MeshNode> pnodelist, List<MeshRelation> prelationlist, bool isonline)
        {
            if (isonline)
            {
                logger.Info("Mesh Node Discover Report");

                logger.Info("IP".PadLeft(20, ' ') + "MAC".PadLeft(20, ' ') + "BandWidth".PadLeft(10, ' ') + "TxPower".PadLeft(10, ' ') + "Frequency".PadLeft(10, ' ') + "Battery".PadLeft(10, ' '));

                foreach (MeshNode mn in pnodelist)
                {
                    logger.Info(mn.IPAddress.PadLeft(20, ' ') + 
                        mn.MACAddress.PadLeft(20, ' ') + 
                        mn.BandWidth.ToString().PadLeft(10, ' ') + 
                        mn.TxPower.ToString().PadLeft(10, ' ') + 
                        mn.Frequency.ToString().PadLeft(10, ' ') + 
                        mn.Battery.ToString().PadLeft(10, ' '));
                }

                logger.Info(" Relation Discover Report");

                logger.Info("LoaclNodeIP".PadLeft(20, ' ') +
                                   "LoaclNodeMAC".PadLeft(20, ' ') +
                                   "RemoteNodeIP".PadLeft(20, ' ') +
                                   "RemoteNodeMAC".PadLeft(20, ' ') +
                                   "LocalPort".PadLeft(10, ' ') +
                                   "RemotePort".PadLeft(10, ' ') +
                                   "Tspeed".PadLeft(10, ' ') +
                                   "Txsnr".PadLeft(10, ' ') +
                                   "Rxspeed".PadLeft(10, ' ') +
                                   "Rxsnr".PadLeft(10, ' ') +
                                   "Findtimes".PadLeft(10, ' '));

                foreach (MeshRelation R in prelationlist)
                {
                    logger.Info(R.LocalNode.IPAddress.PadLeft(20, ' ') +
                                              R.LocalNode.MACAddress.PadLeft(20, ' ') +
                                              R.RemoteNode.IPAddress.PadLeft(20, ' ') +
                                              R.RemoteNode.MACAddress.ToString().PadLeft(20, ' ') +
                                              R.LocalNode.ToString().PadLeft(10, ' ') +
                                              R.RemotePort.ToString().PadLeft(10, ' ') +
                                              R.TxSpeed.ToString().PadLeft(10, ' ') +
                                              R.TxSnr.ToString().PadLeft(10, ' ') +
                                              R.RxSpeed.ToString().PadLeft(10, ' ') +
                                              R.RxSnr.ToString().PadLeft(10, ' ') +
                                              R.FindTimes.ToString().PadLeft(10, ' ')
                                              );
                }
            }
            else
            {

            }
        }

        /// <summary>
        /// 上报添加新的Mesh设备事件
        /// </summary>
        /// <param name="meshNode">Mesh节点</param>
        private void RaiseMeshDeviceAdd(MeshNode meshNode)
        {
            if(OnMeshDeviceAdded != null)
            {
                MeshDeviceInfo mdi = new MeshDeviceInfo();
                mdi.IPV4 = meshNode.IPAddress;
                mdi.Alias = meshNode.IPAddress;
                //mdi.Frequency = meshNode.Frequency + "";
                //mdi.Power = meshNode.TxPower + "";
                OnMeshDeviceAdded(mdi);
            }
        }

        /// <summary>
        /// 上报添加新的Mesh设备事件
        /// </summary>
        /// <param name="meshNode">Mesh节点</param>
        private void RaiseMeshDeviceUpdate(MeshNode meshNode)
        {
            if (OnMeshDeviceUpdate != null)
            {
                MeshDeviceInfo mdi = new MeshDeviceInfo();
                mdi.IPV4 = meshNode.IPAddress;
                //mdi.Frequency = meshNode.Frequency + "";
                //mdi.Power = meshNode.TxPower + "";
                OnMeshDeviceUpdate(mdi);
            }
        }

        /// <summary>
        /// 将MAC地址字节数组转换为字符串
        /// </summary>
        /// <param name="macBytes">MAC地址数组</param>
        /// <returns></returns>
        private static string BytetoString(byte[] macBytes)
        {
            string s = string.Empty;
            foreach (byte bt in macBytes)
            {
                s = s + string.Format("{0:X2}", bt);
            }
            return s;
        }

        /// <summary>
        /// 字符串转换为字节数组
        /// </summary>
        /// <param name="InString"></param>
        /// <returns></returns>
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
