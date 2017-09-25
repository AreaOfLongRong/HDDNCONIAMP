using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using HDDNCONIAMP.DB;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.Utils;
using log4net;
using NodeTopology;

namespace HDDNCONIAMP.UI.MeshManagement
{
    /// <summary>
    /// Mesh设备管理界面
    /// </summary>
    public partial class UCMeshManagement2 : UserControl
    {

        #region 私有变量

        MeshTcpConfigManager meshTcpManager = MeshTcpConfigManager.GetInstance();

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(UCMeshManagement2));

        /// <summary>
        /// 主窗体引用
        /// </summary>
        private FormMain mFormMain;

        /// <summary>
        /// 当前Mesh设备操作类型
        /// </summary>
        private MeshPlanOperatorType mCurrentMPOType;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);
        int GW_CHILD = 5;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        public const int EM_SETREADONLY = 0xcf;


        ARPLIST MyARPLIST = new ARPLIST();

        string RootMAC = string.Empty;

        string RootIp = string.Empty;

        node RootNode = new node();

        BlockNodes MYBlockNodes = new BlockNodes();  //用于实时存储

        BlockNodes ShowBlockNodes = new BlockNodes(); //用于显示

        BlockNodes StoreBlockNodes = new BlockNodes(); //用于当MYBlockNodes收集完数据时，将ShowBlockNodes传入StoreBlockNodes并进行比对

        private const int Nmax = 150;

        List<string> NeedResearchMac = new List<string>();

        int RepeatTime = 0;

        int RepeatTimeForRead = 0;

        List<int> EveryColumnNodeCount = new List<int>();

        List<int> EveryColumnNodeCountForRead = new List<int>();


        Hashtable hashTable = new Hashtable();

        int ScanRate = 10;

        int ShowRate = 10;

        Thread GetRealInfoThread;
        Thread RefreshPanelContext;

        public NodeTopology.GScenario GNetwork;
        private const int XTextPixelOffset = -10;
        private const int YTextPixelOffset = 30;//80;


        private int Xdown = 0;
        private int Ydown = 0;
        private DateTime Tdown;
        private int DragTimeMin = 300; //拖动的时间大于300毫秒,则认为是有拖拽的动作!!
        private bool Dragging = false;
        private int CurrObjDragIndx = 0;


        private ReaderWriterLock _rReaderWriterLockwlock = new ReaderWriterLock();


        #endregion

        public UCMeshManagement2(FormMain formMain)
        {
            InitializeComponent();

            mFormMain = formMain;

            initMeshPlanControlsDefaultValue();

            setTableLayoutPanelDoubleBufferd();

            this.ScanRate = 3;
            this.ShowRate = 3;

            IntPtr editHandle = GetWindow(this.NIC.Handle, GW_CHILD);
            SendMessage(editHandle, EM_SETREADONLY, 1, 0);

            string[] cards = OperateNode.NetworkInterfaceCard();
            if (cards != null)
            {
                this.NIC.DataSource = cards;
                //this.comboBoxNetworkCard.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("未找到网卡，请检查网卡是否正常开启！！！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCMeshManagement2_Load(object sender, EventArgs e)
        {
            initMeshPlanTable();
            initComboxExMPMGroupName();
            initMeshBaseParamConfit();

            GNetwork = new NodeTopology.GScenario(Nmax);
            GNetwork.Clear();
            GNetwork.CurrObjIndx = 0;

        }

        #region 网络拓扑事件处理

        private void buttonXRefreshTopology_Click(object sender, EventArgs e)
        {
            StartTopology();
            buttonXRefreshTopology.Enabled = false;
            buttonXRefreshTopology.Text = "网络拓扑刷新中...";
            buttonXStopRefresh.Enabled = true;
        }

        /// <summary>
        /// 停止刷新网络拓扑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXStopRefresh_Click(object sender, EventArgs e)
        {
            StopTopology();
            buttonXRefreshTopology.Enabled = true;
            buttonXRefreshTopology.Text = "刷新网络拓扑";
            buttonXStopRefresh.Enabled = false;
        }

        private void StartTopology()
        {
            if (GetRealInfoThread != null)
            {
                GetRealInfoThread.Abort();
            }

            GetRealInfoThread = new Thread(GetRealInfo);
            GetRealInfoThread.Start();

            if (RefreshPanelContext != null)
            {
                RefreshPanelContext.Abort();
            }

            RefreshPanelContext = new Thread(RefreshPanelInfo);
            RefreshPanelContext.Start();
        }

        public void StopTopology()
        {
            if (GetRealInfoThread != null)
            {
                GetRealInfoThread.Abort();
            }
            if (RefreshPanelContext != null)
            {
                RefreshPanelContext.Abort();
            }
        }

        #region 拓扑发现


        /// <summary>
        /// 1)获取ARP列表(一个单独线程)==>完成（暂时写在主线程里）       
        /// 2)发送初始化报文，获取连接到PC的MAC地址（自动连接单点或者操作者手工输入）
        /// 3)尝试用TELNET访问该IP的40000端口
        /// 4)报文切割
        /// 5)将报文对应到单独的NODE
        /// 6)逐一操作这些NODE
        /// 7)日志和异常处理
        /// </summary>
        /// 

        private delegate void DGetIniMAC();

        private void RunGetIniMAC()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.NIC.InvokeRequired)
            {
                DGetIniMAC d = new DGetIniMAC(RunGetIniMAC);
                this.Invoke(d);
            }
            else
            {
                //return OperateNode.GetIniMAC(this.NIC.SelectedItem.ToString());
                RootMAC = OperateNode.GetIniMAC(this.NIC.SelectedItem.ToString());
            }

            //return OperateNode.GetIniMAC(this.NIC.SelectedItem.ToString());
        }

        public void TestGetIniMACandIP()
        {

            //测试能够获取到猫的MAC
            LogHelper.WriteLog("获取ROOT的MAC地址");

            //RootMAC = OperateNode.GetIniMAC(this.NIC.SelectedItem.ToString());

            RunGetIniMAC();//this.Invoke(new DGetIniMAC(RunGetIniMAC),this.NIC.SelectedItem.ToString()).ToString();

            LogHelper.WriteLog("获取ROOT的MAC地址结束");


            LogHelper.WriteLog("获取ROOT的IP地址");

            RootIp = OperateNode.getIPaddress(RootMAC, MyARPLIST);

            ///未获取到IP的时候需要结束！！！

            LogHelper.WriteLog("获取ROOT的IP地址结束");

        }

        public void AddNodes()
        {
            //在这里增加根节点Node???

            RootNode.MacAddress = RootMAC;
            RootNode.IpAddress = RootIp;

            //看看这里是否需要其他条件
            //urinatedong 2017.3.5
            this.MYBlockNodes.Nodelist.Add(RootNode);
        }

        public void TestSendTelnetTelegram(string needInfoNode)
        {
            //需要在这里try catch finally 以关闭连接
            //必须关闭连接!!!!!! tn.close!!!!

            LogHelper.WriteLog("开始获取MAC为:" + needInfoNode + " 的NODE的信息！！！ ");

            node TheNode = null;
            if (MYBlockNodes.Nodelist != null && MYBlockNodes.Nodelist.Count > 0)
                TheNode = MYBlockNodes.Nodelist.Where(n => n.MacAddress.Equals(needInfoNode)).ToList().First();


            if (TheNode != null)
            {

                myTelnet tn = new myTelnet(TheNode.IpAddress);

                try
                {
                    node cacheNode = null;
                    if (hashTable.ContainsKey(needInfoNode))
                    {
                        cacheNode = (node)hashTable[needInfoNode];
                    }
                    string recvStr = tn.recvDataWaitWord("help", 1);

                    //Thread.Sleep(1);
                    tn.sendData("battery");
                    //Thread.Sleep(1);
                    recvStr = tn.recvDataWaitWord("V", 1);
                    TheNode.Battery = (cacheNode != null) ? cacheNode.Battery : double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("V", ""));

                    Thread.Sleep(1);
                    tn.sendData("frequency");
                    recvStr = tn.recvDataWaitWord("MHz", 1);
                    TheNode.Frequency = (cacheNode != null) ? cacheNode.Frequency : 616;
                    //TheNode.Frequency = double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("MHz", ""));

                    //Thread.Sleep(1);
                    tn.sendData("txpower");
                    recvStr = tn.recvDataWaitWord("dBm", 1);
                    TheNode.TxPower = (cacheNode != null) ? cacheNode.TxPower : double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("dBm", ""));
                    //Thread.Sleep(1);
                    tn.sendData("bandwidth");
                    recvStr = tn.recvDataWaitWord("MHz", 1);
                    TheNode.BandWidth = (cacheNode != null) ? cacheNode.BandWidth : double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("MHz", ""));

                    node cacheNod = new node();
                    cacheNod.IpAddress = TheNode.IpAddress;
                    cacheNod.Frequency = TheNode.Frequency;
                    cacheNod.BandWidth = TheNode.BandWidth;
                    cacheNod.TxPower = TheNode.TxPower;
                    cacheNod.Battery = TheNode.Battery;
                    if (hashTable.ContainsKey(needInfoNode))
                    {
                        hashTable[needInfoNode] = cacheNod;
                    }
                    else
                    {
                        hashTable.Add(needInfoNode, cacheNod);
                    }
                    //Thread.Sleep(1);
                    tn.sendData("mesh");
                    recvStr = tn.recvDataWaitWord("OK", 1);
                    string[] tempmesh = recvStr.Replace("OK\n#user@/>", "").Split(":".ToArray());

                    if (tempmesh.Length > 1)
                    {
                        int i = int.Parse(tempmesh[0]);
                        if (i > 0)
                        {
                            string[] mymesh = tempmesh[1].Split(";".ToArray());

                            TheNode.Haschild = true;

                            //3:
                            //9,6C6126100328,14,12,45,19,6C6126100327
                            //
                            //11,6C6126100328,13,12,57,21,6C612610035C.

                            for (int j = 0; j < mymesh.Length; j++)
                            {
                                /////在这里增加新节点
                                //1
                                //\r\n9,6C612610034D,82,26,85,27,6C612610031D.\n
                                ///在这里建立新的Relation
                                //if (j % 2 == 1)
                                //{
                                if (mymesh[j] == "\r\n\n")
                                {
                                    break;
                                }

                                string SplitNewNode = mymesh[j].Replace("\r", "").Replace("\n", "").Replace(".", "");
                                string[] MeshInfo = SplitNewNode.Split(",".ToCharArray());

                                ///如果用子节点登录?不再访问其根节点
                                string NeedToCheckMac = MeshInfo[6].Substring(0, 12);
                                string NeedToCheckIP = OperateNode.getIPaddress(NeedToCheckMac, MyARPLIST);

                                if (MYBlockNodes.Nodelist.Count > 1)
                                {
                                    int samenode = MYBlockNodes.Nodelist.Where(x => x.MacAddress.Equals(NeedToCheckMac) && x.IpAddress.Equals(NeedToCheckIP)).ToList().Count;

                                    if (samenode == 0) //这是首次发现的NODE
                                    {
                                        node NewNode = new node(NeedToCheckMac);

                                        NewNode.IpAddress = NeedToCheckIP;

                                        relation NewRelation = new relation(TheNode, NewNode);

                                        NewRelation.Localport = int.Parse(MeshInfo[0]);
                                        NewRelation.Txspeed = int.Parse(MeshInfo[2]);

                                        NewRelation.Txsnr = int.Parse(MeshInfo[3]);

                                        NewRelation.Rxspeed = int.Parse(MeshInfo[4]);

                                        NewRelation.Rxsnr = int.Parse(MeshInfo[5]);


                                        NewRelation.Findtimes = 1;

                                        this.MYBlockNodes.Nodelist.Add(NewNode);

                                        this.MYBlockNodes.Relationlist.Add(NewRelation);

                                        this.NeedResearchMac.Add(NeedToCheckMac);
                                    }
                                    else //找到了根节点，这时候跟新与根节点的端口关系
                                    {
                                        //对关系是否需要增加则需要判断一下是否有该关系
                                        if (MYBlockNodes.Relationlist.Count > 0)
                                        {
                                            var needupdaterelation = MYBlockNodes.Relationlist.Where(x => x.Localnode.MacAddress.Equals(NeedToCheckMac) && x.Localnode.IpAddress.Equals(NeedToCheckIP) && x.Remotenode.MacAddress.Equals(TheNode.MacAddress) && x.Remotenode.IpAddress.Equals(TheNode.IpAddress)).ToList();

                                            if (needupdaterelation.Count > 0)
                                            {
                                                relation TheRelation = needupdaterelation.FirstOrDefault();
                                                TheRelation.Remoteport = int.Parse(MeshInfo[0]);

                                                TheRelation.Findtimes = 2;

                                                //这里需要确认以下是否需要跟新RX和TX的信息！！！
                                            }
                                            else
                                            {
                                                var theOtherNode = MYBlockNodes.Nodelist.Where(x => x.MacAddress.Equals(NeedToCheckMac) && x.IpAddress.Equals(NeedToCheckIP)).ToList().First();

                                                relation NewRelation = new relation(TheNode, theOtherNode);

                                                NewRelation.Localport = int.Parse(MeshInfo[0]);
                                                NewRelation.Txspeed = int.Parse(MeshInfo[2]);

                                                NewRelation.Txsnr = int.Parse(MeshInfo[3]);

                                                NewRelation.Rxspeed = int.Parse(MeshInfo[4]);

                                                NewRelation.Rxsnr = int.Parse(MeshInfo[5]);

                                                NewRelation.Findtimes = 1;

                                                this.MYBlockNodes.Relationlist.Add(NewRelation);
                                            }
                                        }
                                    }
                                }
                                else //刚开始从根目录查找
                                {
                                    node NewNode = new node(NeedToCheckMac);

                                    NewNode.IpAddress = NeedToCheckIP;

                                    relation NewRelation = new relation(TheNode, NewNode);

                                    NewRelation.Localport = int.Parse(MeshInfo[0]);
                                    NewRelation.Txspeed = int.Parse(MeshInfo[2]);

                                    NewRelation.Txsnr = int.Parse(MeshInfo[3]);

                                    NewRelation.Rxspeed = int.Parse(MeshInfo[4]);

                                    NewRelation.Rxsnr = int.Parse(MeshInfo[5]);

                                    NewRelation.Findtimes = 1;

                                    this.MYBlockNodes.Nodelist.Add(NewNode);

                                    this.MYBlockNodes.Relationlist.Add(NewRelation);

                                    this.NeedResearchMac.Add(NeedToCheckMac);

                                }
                                //0 ->  端口号
                                //1 ->  本地MAC
                                //2 ->  TX Speed
                                //3 ->  TX SNR
                                //4 ->  RX Speed
                                //5 ->  RX SNR
                                //6 ->  REMOTE MAC ADDRESS 子节点MAC地址

                                // }

                            }
                        }
                        else
                        {
                            //     //此时只有根节点
                            //     //需要告知用户
                            LogHelper.WriteLog("此拓扑只发现单一节点，未发现子节点！！！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("在获取NODE信息时异常: " + ex.Message);
                }
                finally
                {
                    tn.close();
                }



            }//if end



        }


        private delegate void DPanel2Refresh();
        private void Panel2Refresh()
        {
            //this.panel2.Refresh();
            //Invalidate(new Rectangle(0, 0, 1, 1));
            //this.treeView1.Nodes.Clear();

            ReDrawAll();
        }



        private ReaderWriterLock _rwlock = new ReaderWriterLock();

        private void GetRealInfo()
        {

            while (!LifeTimeControl.closing)
            {
                LogHelper.WriteLog("循环扫描开始！！！");

                LogHelper.WriteLog("ReloadARP开始！！！");

                MyARPLIST.ReloadARP();

                LogHelper.WriteLog("ReloadARP结束！！！");

                Thread.Sleep(1);


                ////以下应该为循环多线程执行?

                LogHelper.WriteLog("TestGetIniMACandIP开始！！！");

                TestGetIniMACandIP();

                LogHelper.WriteLog("TestGetIniMACandIP结束！！！");

                if (RootIp == null || RootIp.Equals(string.Empty))
                {
                    //MessageBox.Show("未获取到根节点IP信息，请检查网络连接是否正常！");

                    buttonXStopRefresh_Click(null, null);

                    return;
                }

                LogHelper.WriteLog("加入根节点AddNodes()开始！！！");

                AddNodes();

                LogHelper.WriteLog("加入根节点AddNodes()结束！！！");

                LogHelper.WriteLog("读取根节点信息TestSendTelnetTelegram(this.RootMAC)！！！");

                TestSendTelnetTelegram(this.RootMAC);

                LogHelper.WriteLog("根节点信息读取结束！！！");

                RepeatTime = 1;

                EveryColumnNodeCount.Clear();

                EveryColumnNodeCount.Add(1);



                bool search = false;

                if (MYBlockNodes.Nodelist.Count > 1)
                {
                    search = true;
                    LogHelper.WriteLog("发现其他节点,开始循环读取NODE信息");
                }



                while (!LifeTimeControl.closing && search)
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

                        //int RepeatTime = 0;

                        //int RepeatTimeForRead = 0;

                        //List<int> EveryColumnNodeCount = new List<int>();

                        //List<int> EveryColumnNodeCountForRead = new List<int>();

                        ///有这一轮要填充信息的node
                        RepeatTime++;

                        EveryColumnNodeCount.Add(NeedtoClear.Count);

                        foreach (string s in NeedtoClear)
                        {
                            ///考虑使用线程并发加快拓扑访问速度!!!
                            ///给全部线程一个时间，要求在标准时间内in完成6秒?
                            TestSendTelnetTelegram(s);

                        }

                    }

                    LogHelper.WriteLog("RealTimeNode全部拓扑信息读取完成");


                    this.PrintStatues(this.MYBlockNodes.Nodelist.ToList(), this.MYBlockNodes.Relationlist.ToList(), true);


                    //int RepeatTime = 0;

                    //int RepeatTimeForRead = 0;

                    //List<int> EveryColumnNodeCount = new List<int>();

                    //List<int> EveryColumnNodeCountForRead = new List<int>();

                    ///锁定信息并进行复制
                    _rwlock.AcquireWriterLock(100);


                    ///urinatedong 20170325 在将实时状态复制到ShowBlockNodes之前，先复制ShowBlockNodes到StoreBlockNodes
                    this.StoreBlockNodes.Nodelist = this.ShowBlockNodes.Nodelist.ToArray().ToList();
                    this.StoreBlockNodes.Relationlist = this.ShowBlockNodes.Relationlist.ToArray().ToList();

                    ///全部复制
                    this.RepeatTimeForRead = this.RepeatTime;
                    this.EveryColumnNodeCountForRead = this.EveryColumnNodeCount.ToArray().ToList();
                    this.ShowBlockNodes.Nodelist = this.MYBlockNodes.Nodelist.ToArray().ToList();
                    this.ShowBlockNodes.Relationlist = this.MYBlockNodes.Relationlist.ToArray().ToList();

                    // = new List<string>(NeedResearchMac.ToArray());

                    _rwlock.ReleaseWriterLock();

                    ///打印信息


                    ///清除所有内容
                    this.MYBlockNodes.Nodelist.Clear();
                    this.MYBlockNodes.Relationlist.Clear();


                    ///将所有实时的拓扑节点复制到MYBlockNodes用于显示
                    ///清空实时拓扑用于显示



                    //初步定为每隔1分钟总体扫描1次
                    Thread.Sleep(this.ScanRate * 1000);
                }
            }
        }


        private delegate void AddNode(node Pnode);

        private void AddNodeMethod(node Pnode)
        {
            if (this.treeView1.InvokeRequired)
            {
                AddNode d = new AddNode(AddNodeMethod);
                this.Invoke(d, (object)Pnode);

            }
            else
            {

                if (this.treeView1.Nodes.Count > 0)
                {
                    TreeNode[] TempNodes = treeView1.Nodes.Find(Pnode.IpAddress, false);

                    if (TempNodes.Count() > 0)
                    {
                        if (!Pnode.MacAddress.Equals(TempNodes[0].Nodes[0].ToString()))
                        {
                            TreeNode UpdateNode = new TreeNode(Pnode.MacAddress);
                            TempNodes[0].Nodes.Clear();
                            TempNodes[0].Nodes.Add(UpdateNode);
                        }

                    }
                    else
                    {
                        TreeNode NewTreenode = this.treeView1.Nodes.Add(Pnode.IpAddress, Pnode.IpAddress);
                        TreeNode SubNode = NewTreenode.Nodes.Add(Pnode.MacAddress, Pnode.MacAddress);

                    }
                }
                else
                {
                    TreeNode NewTreenode = this.treeView1.Nodes.Add(Pnode.IpAddress, Pnode.IpAddress);
                    TreeNode SubNode = NewTreenode.Nodes.Add(Pnode.MacAddress, Pnode.MacAddress);

                }


            }


            // myListBox.Items.Add(myStr1 + myStr2);
            //AddNode myAddNodeDelegate = new AddNode(AddNodeMethod);
        }


        private delegate void DeleteNode(string Key);

        private void DeleteNodeMethod(string Key)
        {
            if (this.treeView1.InvokeRequired)
            {
                DeleteNode d = new DeleteNode(DeleteNodeMethod);
                this.Invoke(d, (object)Key);

            }
            else
            {

                TreeNode[] TempNodes = this.treeView1.Nodes.Find(Key, true);

                if (TempNodes.Count() > 0)
                {
                    TempNodes[0].Parent.Remove();
                }
            }


        }

        /// <summary>
        /// 首先判断ShowBlockNodes和StoreBlockNodes的比对关系
        /// </summary>
        private void RefreshPanelInfo()
        {
            while (!LifeTimeControl.closing)
            {
                //争取在左上角加一个时间显示！！！

                ///争取画面不闪烁的关键是一次性画出所有控件！！！

                //每一次都从GNetwork读取信息，不再删除该数组
                //GNetwork.Clear();
                //GNetwork = new GScenario(Nmax);
                //GNetwork.CurrObjIndx = 0;

                //this.panel2.Refresh();



                //urinatedong
                //RepeatTime 代表最多有几层
                //FindNodeNumber 代表每层最多有几个，这两个数据需要在显示之前传进来

                //int RepeatTime = 6;

                //int[] FindNodeNumber = new int[5] { 1, 2, 3, 4, 6 };

                ///在读端使用

                //int RepeatTimeForRead = 0;

                //List<int> EveryColumnNodeCountForRead = new List<int>();

                if (this.ShowBlockNodes.Nodelist.Count > 0)
                {

                    _rwlock.AcquireReaderLock(100);

                    ///首先删除那些已经不存在的关系和节点!!!
                    ///urinatedong 20170325
                    ///删除上一轮中消失的节点
                    ///问题是按照要求需要显示出来，显示在哪里??? 
                    var NoneNodes = from p in this.StoreBlockNodes.Nodelist where !(from q in this.ShowBlockNodes.Nodelist select q.MacAddress).Contains(p.MacAddress) select p;

                    if (NoneNodes.Count() > 0)
                    {
                        foreach (node n in NoneNodes)
                        {
                            GObject obj = new GObject();
                            GNetwork.FindGObjectByName(n.MacAddress, ref obj);

                            if (obj.Type != "")
                            {
                                GNetwork.DeleteGObject(obj);
                                //在TreeView中删除该MAC对应的父节点!!!
                                //this.treeView1.Nodes.Find(n.MacAddress, true);
                                DeleteNodeMethod(n.MacAddress);
                            }
                        }
                    }

                    var NoneRelations = from p in this.ShowBlockNodes.Relationlist where !this.StoreBlockNodes.Relationlist.Any(x => x.Localnode.IpAddress == p.Localnode.IpAddress & x.Remotenode.IpAddress == p.Remotenode.IpAddress) select p;

                    if (NoneRelations.Count() > 0)
                    {
                        foreach (relation r in NoneRelations)
                        {
                            GObject obj = new GObject();
                            GNetwork.FindGObjectByName(r.Localnode.MacAddress + r.Remotenode.MacAddress, ref obj);

                            if (obj.Type != "")
                            {
                                GNetwork.DeleteGObject(obj);
                            }
                        }
                    }

                    //var NoneNodes = this.ShowBlockNodes.Nodelist.Where(x=>x.IpAddress)

                    ///需要在这里判断出来哪些是在这个循环中没有出现的节点???

                    ///首先分割界面！！！
                    ///获取区间宽度
                    int ColumnWidth = this.drawPanel.Width / RepeatTimeForRead;

                    int nodecount = 0;

                    for (int i = 0; i < RepeatTimeForRead; i++)
                    {
                        //x坐标为 ColumnWidth/2 + i*ColumnWidth - 30(图片宽度的一半)

                        int j = EveryColumnNodeCountForRead[i];

                        if (j > 0)
                        {
                            //每行占据的高度
                            int CellHeight = this.drawPanel.Height / j;

                            for (int k = 0; k < j; k++)
                            {
                                //y坐标为 CellHeight/2 + k*CellHeight - 30(图片高度的一半)

                                //AddGObject(x, y, node);

                                node TempNode = ShowBlockNodes.Nodelist[nodecount];

                                //urinatedog 向TREEVIEW中加入节点
                                //TreeNode NewNode = this.treeView1.Nodes.Add(TempNode.IpAddress);
                                //TreeNode SubNode = new TreeNode(TempNode.MacAddress);

                                //NewNode.Nodes.Add(SubNode);

                                AddNodeMethod(TempNode);


                                int x = ColumnWidth / 2 + i * ColumnWidth - this.imageList1.ImageSize.Width / 2;
                                int y = CellHeight / 2 + k * CellHeight - this.imageList1.ImageSize.Height / 2;

                                nodecount++;

                                //AddGobject(ColumnWidth / 2 + i * ColumnWidth - 30, CellHeight / 2 + k * CellHeight - 30, TempNode);

                                ///使用双缓存后，这里就很重要，必须每个循环后判断哪些变化了，哪些没有变化！！！

                                AddGObject(ColumnWidth / 2 + i * ColumnWidth - this.imageList1.ImageSize.Width / 2, CellHeight / 2 + k * CellHeight - this.imageList1.ImageSize.Height / 2, TempNode);
                            }

                        }
                    }


                    ///开始画线
                    ///public void FindGObjectByName(string ObjLnkName, ref GObject GObj)
                    ///urinatedong 20170311
                    ///关系不对暂时取消！

                    GObject TempGObject = new GObject();

                    if (ShowBlockNodes.Relationlist.Count > 0)
                    {
                        foreach (relation r in ShowBlockNodes.Relationlist)
                        {
                            GNetwork.FindGObjectByName(r.Localnode.MacAddress, ref TempGObject);
                            int x1 = TempGObject.x1;
                            int y1 = TempGObject.y1;
                            GNetwork.FindGObjectByName(r.Remotenode.MacAddress, ref TempGObject);
                            int x2 = TempGObject.x1;
                            int y2 = TempGObject.y1;

                            int ImageHeight = imageList1.Images[0].Height;
                            int ImageWidth = imageList1.Images[0].Width;

                            AddGobject(x1 + ImageWidth / 2, y1 + ImageHeight / 2, x2 + ImageWidth / 2, y2 + ImageHeight / 2, r);
                        }

                    }

                    #region urinatedong 20170322 取消对于连线位置的判断
                    //if (ShowBlockNodes.Relationlist.Count > 0)
                    //{
                    //    foreach (relation r in ShowBlockNodes.Relationlist)
                    //    {
                    //      GNetwork.FindGObjectByName(r.Localnode.MacAddress, ref TempGObject);
                    //      int x1 = TempGObject.x1;
                    //      int y1 = TempGObject.y1;
                    //      GNetwork.FindGObjectByName(r.Remotenode.MacAddress, ref TempGObject);
                    //      int x2 = TempGObject.x1;
                    //      int y2 = TempGObject.y1;

                    //      ///重新定义画线的方式（如果新节点在上，采用老节点右上，新节点坐下的方式画）
                    //      if (y1 > y2)
                    //      {
                    //          if (x1.Equals(x2)) //画竖线
                    //          {
                    //              x1 = x1 + 30;
                    //              y2 = y2 + 60;

                    //              x2 = x2 + 30;


                    //          }
                    //          else
                    //          {
                    //              x1 = x1 + 60;
                    //              //x2 = x2 - 60;
                    //              y1 = y1 + 30;
                    //              y2 = y2 + 30;

                    //          }                           
                    //      }
                    //      else if (y1 < y2)
                    //      {

                    //          if (x1.Equals(x2)) //画竖线
                    //          {
                    //              x1 = x1 + 30;
                    //              y1 = y1 + 60;

                    //              x2 = x2 + 30;

                    //          }
                    //          else
                    //          {
                    //              x1 = x1 + 60;
                    //              y1 = y1 + 30;
                    //              y2 = y2 + 30;
                    //          }
                    //      }
                    //      else  //画横线
                    //      {

                    //          x1 = x1 + 60;
                    //          y1 = y1 + 30;
                    //          y2 = y2 + 30;

                    //      }

                    //      AddGobject(x1, y1, x2, y2, r);

                    //    }
                    //}
                    #endregion

                    _rwlock.ReleaseReaderLock();

                    this.StoreBlockNodes.Nodelist = this.ShowBlockNodes.Nodelist.ToArray().ToList();
                    this.StoreBlockNodes.Relationlist = this.ShowBlockNodes.Relationlist.ToArray().ToList();

                }


                this.Invoke(new DPanel2Refresh(Panel2Refresh));


                Thread.Sleep(this.ShowRate * 1000);

            }

        }

        private void drawPanel_MouseUp(object sender, MouseEventArgs e)
        {
            int H = 0;
            int W = 0;
            GObject GContainer = new GObject();
            GObject GToDrag = new GObject();
            GToDrag = GNetwork.GObjects[CurrObjDragIndx];
            double d1 = 0;
            double d2 = 0;
            TimeSpan DTDrag = new TimeSpan();
            DTDrag = DateTime.Now.Subtract(Tdown);
            if ((Dragging == true) && (DTDrag.Milliseconds > DragTimeMin))
            {
                //如果拖动的是线，并且有容器
                if ((GNetwork.GObjects[CurrObjDragIndx].Type == "Line")
                   && (GNetwork.FindContainerObject(e.X, e.Y, ref GContainer, true) > -1))
                {
                    //
                    //    What is the point of the line to link ? 
                    //    The nearest to (Xdown,Ydown)
                    //
                    d1 = CommFnc.distance(Xdown, Ydown, GToDrag.x1, GToDrag.y1);
                    d2 = CommFnc.distance(Xdown, Ydown, GToDrag.x2, GToDrag.y2);
                    if (d1 <= d2)
                    {
                        GToDrag.x1 = (GContainer.x1 + GContainer.x2) / 2;
                        GToDrag.y1 = (GContainer.y1 + GContainer.y2) / 2;
                        GToDrag.Lnk1 = GContainer.Name;
                    }
                    else
                    {
                        GToDrag.x2 = (GContainer.x1 + GContainer.x2) / 2;
                        GToDrag.y2 = (GContainer.y1 + GContainer.y2) / 2;
                        GToDrag.Lnk2 = GContainer.Name;
                    }
                }
                else
                {
                    W = GToDrag.x2 - GToDrag.x1;
                    H = GToDrag.y2 - GToDrag.y1;
                    GToDrag.x1 = e.X;
                    GToDrag.y1 = e.Y;
                    GToDrag.x2 = e.X + W;
                    GToDrag.y2 = e.Y + H;
                    GNetwork.AdjustLinkedTo(GToDrag.Name);
                }
                Cursor.Current = Cursors.Default;
                Dragging = false;

                //this.panel2.Refresh();

                ReDrawAll();

                //this.Refresh();
            }
        }

        private void drawPanel_MouseMove(object sender, MouseEventArgs e)
        {

        }



        private void drawPanel_MouseDown(object sender, MouseEventArgs e)
        {
            Xdown = e.X;
            Ydown = e.Y;
            Tdown = DateTime.Now;
            GObject GContainer = new GObject();
            int Container = GNetwork.FindContainerObject(Xdown, Ydown, ref GContainer, false);

            //如果有容器，就允许拖动！！
            if (Container > -1)
            {
                Dragging = true;
                Cursor.Current = Cursors.Hand;
                CurrObjDragIndx = Container;
            }
            else
            {
                // Click out of all objects
            }
        }

        private void drawPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Xdown = e.X;
            Ydown = e.Y;
            //Tdown = DateTime.Now;
            GObject GContainer = new GObject();
            int Container = GNetwork.FindContainerObject(Xdown, Ydown, ref GContainer, false);

            //如果有容器，就允许拖动！！
            if (Container > -1)
            {
                //Dragging = true;
                //Cursor.Current = Cursors.Hand;
                //CurrObjDragIndx = Container;

                this.toolTip1.SetToolTip(this.drawPanel, GContainer.AddInfo);
                //this.toolTip1.ShowAlways = true;
                //this.toolTip1.Show(this);

            }
            else
            {
                // Click out of all objects
            }
        }

        private void drawPanel_Leave(object sender, EventArgs e)
        {
        }
        private void drawPanel_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.RemoveAll();
        }

        private void drawPanel_Paint(object sender, PaintEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 在日志中打印当前扫描到的NODE和RELATION信息
        /// </summary>
        /// <param name="pnodelist">nodelist</param>
        /// <param name="prelationlist">relationlist</param>
        /// <param name="isonline">是否为实时数据，如果为前台显示用，则该项为否</param>
        private void PrintStatues(List<node> pnodelist, List<relation> prelationlist, bool isonline)
        {
            if (isonline)
            {
                LogHelper.WriteLog(" Node Discover Report");

                LogHelper.WriteLog("IP".PadLeft(20, ' ') + "MAC".PadLeft(20, ' ') + "BandWidth".PadLeft(10, ' ') + "TxPower".PadLeft(10, ' ') + "Frequency".PadLeft(10, ' ') + "Battery".PadLeft(10, ' '));

                foreach (node Si in pnodelist)
                {
                    try
                    {
                        LogHelper.WriteLog(Si.IpAddress.PadLeft(20, ' ') + Si.MacAddress.PadLeft(20, ' ') + Si.BandWidth.ToString().PadLeft(10, ' ') + Si.TxPower.ToString().PadLeft(10, ' ') + Si.Frequency.ToString().PadLeft(10, ' ') + Si.Battery.ToString().PadLeft(10, ' '));
                    }
                    catch (Exception e)
                    {

                    }
                }

                LogHelper.WriteLog(" Relation Discover Report");

                LogHelper.WriteLog("LoaclNodeIP".PadLeft(20, ' ') +
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

                foreach (relation R in prelationlist)
                {
                    try
                    {
                        LogHelper.WriteLog(R.Localnode.IpAddress.PadLeft(20, ' ') +
                                                              R.Localnode.MacAddress.PadLeft(20, ' ') +
                                                              R.Remotenode.IpAddress.PadLeft(20, ' ') +
                                                              R.Remotenode.MacAddress.ToString().PadLeft(20, ' ') +
                                                              R.Localport.ToString().PadLeft(10, ' ') +
                                                              R.Remoteport.ToString().PadLeft(10, ' ') +
                                                              R.Txspeed.ToString().PadLeft(10, ' ') +
                                                              R.Txsnr.ToString().PadLeft(10, ' ') +
                                                              R.Rxspeed.ToString().PadLeft(10, ' ') +
                                                              R.Rxsnr.ToString().PadLeft(10, ' ') +
                                                              R.Findtimes.ToString().PadLeft(10, ' ')
                                                              );
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            else
            {

            }
        }


        #endregion

        #region 画布处理

        //这里是创建一个画布 

        private void ReDrawAll()
        {

            Bitmap bmp = new Bitmap(this.drawPanel.Width, this.drawPanel.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(this.drawPanel.BackColor);

            //Graphics g = this.CreateGraphics();
            //Graphics g = this.drawPanel.CreateGraphics();
            g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            GObject CurrObj = new GObject();
            Rectangle Rct = new Rectangle();

            //Pen p = new Pen(Color.Blue);
            Image ObjImg;
            int xm = 0;
            int ym = 0;
            string IsLine = "";
            //for (int i = 0; i < GNetwork.Nobj; i++)
            //urinatedong 只显示需要显示的
            for (int i = 0; i < GNetwork.Nobj; i++)
            {
                CurrObj = GNetwork.GObjects[i];
                //
                if (CurrObj.Type == "") IsLine = "N/D";
                if (CurrObj.Type == "Line") IsLine = "Y";
                if ((CurrObj.Type != "Line") && (CurrObj.Type != "")) IsLine = "N";
                //
                switch (IsLine)
                {
                    case "Y":
                        // g.DrawLine(p, CurrObj.x1, CurrObj.y1, CurrObj.x2, CurrObj.y2);

                        AdjustableArrowCap lineCap = new AdjustableArrowCap(5, 6, true);

                        Pen p;

                        string[] MyRelationInfo = CurrObj.AddInfo.Split("\n".ToCharArray());

                        if (MyRelationInfo[6].IndexOf("2") > 0)
                        {
                            p = new Pen(Color.Blue, 3);
                        }
                        else
                        {
                            p = new Pen(Color.Orange, 3);
                            p.DashStyle = DashStyle.Custom;
                            p.DashPattern = new float[] { 6, 3 };
                        }

                        p.CustomEndCap = lineCap;
                        p.CustomStartCap = lineCap;

                        g.DrawLine(p, CurrObj.x1, CurrObj.y1, CurrObj.x2, CurrObj.y2);

                        xm = (CurrObj.x1 + CurrObj.x2) / 2;
                        ym = (CurrObj.y1 + CurrObj.y2) / 2;
                        //AddText(xm, ym, CurrObj.Name, false,g);

                        int x1 = (CurrObj.x1 + xm) / 2;
                        int y1 = (CurrObj.y1 + ym) / 2;

                        int x2 = (CurrObj.x2 + xm) / 2;
                        int y2 = (CurrObj.y2 + ym) / 2;

                        AddText(x1, y1, MyRelationInfo[0] + "\n" + MyRelationInfo[1] + "\n" + MyRelationInfo[2], false, g);

                        AddText(x2, y2, MyRelationInfo[3] + "\n" + MyRelationInfo[4] + "\n" + MyRelationInfo[5], false, g);

                        p.Dispose();
                        break;
                    case "N":

                        string[] MyNodeInfo = CurrObj.AddInfo.Split("\n".ToCharArray());

                        Rct.X = CurrObj.x1;
                        Rct.Y = CurrObj.y1;
                        Rct.Width = CurrObj.x2 - CurrObj.x1;
                        Rct.Height = CurrObj.y2 - CurrObj.y1;
                        if (CurrObj.Type != String.Empty)
                        {
                            if (double.Parse(MyNodeInfo[4].Replace("Battery", "")) > 0)
                            {
                                ObjImg = FindGObjectTypeImage("Router");
                            }
                            else
                            {
                                ObjImg = FindGObjectTypeImage("NotOnline");
                            }

                            g.DrawImage(ObjImg, Rct);

                            //使用IP地址显示
                            //AddText(CurrObj.x1, CurrObj.y1, CurrObj.Name, true,g);

                            AddText(CurrObj.x1, CurrObj.y1, MyNodeInfo[0], true, g);

                            GNetwork.AdjustLinkedTo(CurrObj.Name);
                        }
                        break;
                }
            }

            //g1.DrawEllipse(new Pen(System.Drawing.Color.Red), 10, 10, 100, 100); 
            //g1.DrawImage(Image.FromFile("E:/down.png"), x, 10);//这是在画布上绘制图形 
            this.drawPanel.CreateGraphics().DrawImage(bmp, 0, 0);//这句是将图形显示到窗口上

            bmp.Dispose();
            g.Dispose();
        }


        private void AddText(int Xbase, int Ybase, string Msg, bool UseOffset, Graphics UsingGraphics)
        {
            //Graphics g = this.CreateGraphics();
            //Graphics g = this.drawPanel.CreateGraphics();
            //g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.CompositingQuality = CompositingQuality.HighQuality;

            Font CurrFont = new Font("Arial", 8);
            int x = 0;
            int y = 0;
            if (UseOffset == true)
            {
                x = Xbase + XTextPixelOffset;
                y = Ybase + YTextPixelOffset;
            }
            else
            {
                x = Xbase;
                y = Ybase;
            }
            UsingGraphics.DrawString(Msg, CurrFont, new SolidBrush(Color.Black), x, y);
        }
        private Image FindGObjectTypeImage(string ObjType)
        {
            Image RetImg = null;
            switch (ObjType)
            {
                case "Network":
                    RetImg = imageList1.Images[0];
                    break;
                case "Router":
                    RetImg = imageList1.Images[3];
                    break;
                case "Emitter":
                    RetImg = imageList1.Images[2];
                    break;
                case "Receiver":
                    RetImg = imageList1.Images[1];
                    break;
                case "NotOnline":
                    RetImg = imageList1.Images[4];
                    break;
            }
            return RetImg;
        }

        public void AddGobject(int x1, int y1, int x2, int y2, relation prelation)
        {

            #region 为了防止刷新，不在此时画线!!! urinatedong  20170322

            //Graphics g = this.drawPanel.CreateGraphics();//this.CreateGraphics();
            //g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.CompositingQuality = CompositingQuality.HighQuality;

            //AdjustableArrowCap lineCap = new AdjustableArrowCap(5, 6, true);

            //Pen p;

            //if (prelation.Findtimes == 2)
            //{
            //     p = new Pen(Color.Blue,3);
            //}
            //else
            //{
            //     p = new Pen(Color.Orange,3);
            //     p.DashStyle = DashStyle.Custom;
            //     p.DashPattern = new float[] { 6, 3 };
            //}

            //p.CustomEndCap = lineCap;

            //g.DrawLine(p, x1, y1, x2, y2);

            #endregion

            ///新需求中不再强调这些信息
            ///urinatedong 20170314
            //int xm = (x1 + x2) / 2;
            //int ym = (y1 + y2) / 2;
            string relationinfo = "Localport " + prelation.Localport +
                       "\nTxsnr" + prelation.Txsnr +
                       "\nTxspeed " + prelation.Txspeed +
                       "\nRemoteport " + prelation.Remoteport +
                       "\nRxsnr " + prelation.Rxsnr +
                       "\nRxspeed " + prelation.Rxspeed +
                       "\nFindTimes " + prelation.Findtimes;
            //AddText(xm, ym, relationinfo, false);

            //AddText(xm, ym, "Rx Tx", false);

            GObject TempGObject = new GObject();

            //在数组中查找是否已经有该关系了
            GNetwork.FindGObjectByName(prelation.Localnode.MacAddress + prelation.Remotenode.MacAddress, ref TempGObject);

            if (TempGObject.Type == "")
            {
                GNetwork.AddGObject(prelation.Localnode.MacAddress + prelation.Remotenode.MacAddress, "Line", x1, y1, x2, y2, relationinfo);
            }
            else
            {
                ///更新关系信息！！！
                TempGObject.AddInfo = relationinfo;
            }

        }

        public void AddGObject(int x1, int y1, node pnode)
        {

            #region 为了防止刷新，不在此时画图形!!! urinatedong 20170322

            //Graphics g = this.drawPanel.CreateGraphics();//

            ////Graphics g = this.graphBuffer.Graphics;


            ////Graphics g = Graphics.FromHwnd(_panel.Handle);

            //g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle ObjRct = new Rectangle();
            //Pen p = new Pen(Color.Blue);
            Image ObjImg;


            ///新需求中不在要求更多的节点信息

            string AddInfo = pnode.IpAddress +
            "\nTxPower " + pnode.TxPower +
            "\nFrequency " + pnode.Frequency +
            "\nBandWidth " + pnode.BandWidth +
            "\nBattery " + pnode.Battery;

            string ObjName = pnode.MacAddress;


            ObjImg = FindGObjectTypeImage("Router");
            ObjRct.X = x1;
            ObjRct.Y = y1;
            ObjRct.Height = ObjImg.Height;
            ObjRct.Width = ObjImg.Width;
            //g.DrawImage(ObjImg, ObjRct);
            //AddText(x1, y1, ObjName, true);
            //AddText(x1, y1, AddInfo, true);
            int x2 = x1 + ObjRct.Width;
            int y2 = y1 + ObjRct.Height;

            #endregion

            GObject TempGObject = new GObject();

            //在数组中查找是否已经有该子节点了！！！
            GNetwork.FindGObjectByName(ObjName, ref TempGObject);

            if (TempGObject.Type == "")
            {
                GNetwork.AddGObject(ObjName, "Router", x1, y1, x2, y2, AddInfo);
            }
            else
            {
                string[] NodeInfo = TempGObject.AddInfo.Split("\n".ToCharArray());
                if (!NodeInfo[0].Equals(pnode.IpAddress))
                {
                    LogHelper.WriteLog("MAC地址为:" + pnode.MacAddress + " 的终端 IP地址由:" + NodeInfo[0] + " 改变为:" + pnode.IpAddress);
                }

                TempGObject.AddInfo = AddInfo;
            }

            //string ObjName = ObjType + "_" + GNetwork.LastIndexOfGObject(ObjType).ToString();
            ////
            //if (ObjType == "Line")
            //{
            //    g.DrawLine(p, x1, y1, x2, y2);
            //    int xm = (x1 + x2) / 2;
            //    int ym = (y1 + y2) / 2;
            //    AddText(xm, ym, ObjName, false);
            //}
            //else
            //{
            //    ObjImg = FindGObjectTypeImage(ObjType);
            //    ObjRct.X = x1;
            //    ObjRct.Y = y1;
            //    ObjRct.Height = ObjImg.Height;
            //    ObjRct.Width = ObjImg.Width;
            //    g.DrawImage(ObjImg, ObjRct);
            //    AddText(x1, y1, ObjName, true);
            //    x2 = x1 + ObjRct.Width;
            //    y2 = y1 + ObjRct.Height;
            //}
            ////
            //GNetwork.AddGObject(ObjName, ObjType, x1, y1, x2, y2);

            //p.Dispose(); 
            //g.Dispose();

        }

        public void AddGObject(int x1, int y1, int x2, int y2, string ObjType, string addinfo)
        {

            Graphics g = this.drawPanel.CreateGraphics();//this.CreateGraphics();
            g.SmoothingMode = SmoothingMode.AntiAlias;  //使绘图质量最高，即消除锯齿
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            Rectangle ObjRct = new Rectangle();


            Pen p = new Pen(Color.Blue);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;//恢复实线  
            p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            Image ObjImg;
            string ObjName = ObjType + "_" + GNetwork.LastIndexOfGObject(ObjType).ToString();
            //
            if (ObjType == "Line")
            {
                g.DrawLine(p, x1, y1, x2, y2);
                int xm = (x1 + x2) / 3;
                int ym = (y1 + y2) / 3;
                //AddText(xm, ym, ObjName, false,g);

                AddText(xm, ym, addinfo, false, g);
            }
            else
            {
                ObjImg = FindGObjectTypeImage(ObjType);
                ObjRct.X = x1;
                ObjRct.Y = y1;
                ObjRct.Height = ObjImg.Height;
                ObjRct.Width = ObjImg.Width;
                g.DrawImage(ObjImg, ObjRct);
                AddText(x1, y1, ObjName, true, g);
                x2 = x1 + ObjRct.Width;
                y2 = y1 + ObjRct.Height;
            }
            //
            GNetwork.AddGObject(ObjName, ObjType, x1, y1, x2, y2, addinfo);
            p.Dispose();
            g.Dispose();
        }


        public void UpdateFrequencyTelnetTelegram(string needupdateNodeIP, double newfrequency)
        {

            //需要在这里try catch finally 以关闭连接
            //必须关闭连接!!!!!! tn.close!!!!

            //LogHelper.WriteLog("开始获取MAC为:" + needInfoNode + " 的NODE的信息！！！ ");

            //node TheNode = MYBlockNodes.Nodelist.Where(n => n.MacAddress.Equals(needInfoNode)).ToList().First();


            if (!string.IsNullOrEmpty(needupdateNodeIP))
            {

                myTelnet tn = new myTelnet(needupdateNodeIP);

                try
                {

                    string recvStr = tn.recvDataWaitWord("help", 1);

                    LogHelper.WriteLog(recvStr);

                    Thread.Sleep(1);
                    string sendnewfrequency = "frequency " + newfrequency.ToString();

                    LogHelper.WriteLog(recvStr);

                    tn.sendData(sendnewfrequency);
                    Thread.Sleep(1);
                    recvStr = tn.recvDataWaitWord("MHz", 1);

                    LogHelper.WriteLog(recvStr);

                    string receive = recvStr.Replace("OK\n#user@/>", "").Replace("MHz", "");

                    double result = double.Parse(receive);

                    if (newfrequency.Equals(result))
                    {
                        LogHelper.WriteLog("IP 为" + needupdateNodeIP + "的设备Frequency 修改为 " + result.ToString() + "成功!!!");
                    }
                    else
                    {
                        LogHelper.WriteLog("IP 为" + needupdateNodeIP + "的设备Frequency 修改失败!!!" + " newfrequency : " + newfrequency.ToString() + " result :" + result.ToString());
                    }

                    //Thread.Sleep(1);
                    //tn.sendData("frequency");
                    //recvStr = tn.recvDataWaitWord("MHz", 1);
                    //TheNode.Frequency = double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("MHz", ""));

                }

                catch (Exception ex)
                {
                    LogHelper.WriteLog("在修改频率时出现异常: " + ex.Message);
                    throw ex;
                }
                finally
                {
                    tn.close();
                }
            }//if end
        }

        #endregion

        #endregion

        #region 预案管理事件处理

        /// <summary>
        /// 添加预案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemMPMAddPlan_Click(object sender, EventArgs e)
        {
            updateMeshPlanControlsState(true);
            initMeshPlanControlsDefaultValue();
            mCurrentMPOType = MeshPlanOperatorType.Add;
            buttonXMeshPlanAdd.Text = "添加预案";
        }

        /// <summary>
        /// 预案管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXGroupManage_Click(object sender, EventArgs e)
        {
            FGroupManage fgm = new FGroupManage();
            if (DialogResult.OK == fgm.ShowDialog())
            {
                comboBoxExMPMGroupName.BeginUpdate();
                comboBoxExMPMGroupName.Items.Clear();
                initComboxExMPMGroupName();
                comboBoxExMPMGroupName.EndUpdate();
            }
        }

        private void buttonItemMPMDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewXMeshPlan.SelectedRows != null)
            {
                updateMeshPlanControlsState(false);
                DialogResult result = MessageBox.Show("确定删除该预案?", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    //删除选中的预案
                    string meshIP = dataGridViewXMeshPlan.SelectedRows[0].Cells[2].Value.ToString();
                    int count = SQLiteHelper.GetInstance().MeshPlanDelete(meshIP);
                    if (count == 1)
                    {
                        logger.Info("成功删除Mesh设备IP为“" + meshIP + "”的预案");
                        MessageBox.Show("删除成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        updateMeshPlanTable();
                    }
                    else
                    {
                        MessageBox.Show("删除失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要删除的预案!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 添加或修改预案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXMeshPlanAdd_Click(object sender, EventArgs e)
        {
            switch (mCurrentMPOType)
            {
                case MeshPlanOperatorType.Add:
                    MeshPlanManage mpm = new MeshPlanManage();
                    mpm.GroupName = comboBoxExMPMGroupName.SelectedItem.ToString();
                    mpm.Alias = textBoxXMeshPlanAlias.Text;
                    mpm.MeshIP = ipAddressInputMeshPlanMeshIP.Value;
                    mpm.Model265IP = ipAddressInputMeshPlanModel265IP.Value;
                    mpm.Model265ID = textBoxXMeshPlanModel265ID.Text;
                    mpm.TCPToCOMIP = ipAddressInputMPMTCPToCOM.Value;
                    //mpm.HKVideoIP = ipAddressInputMeshPlanHKIP.Value;
                    SQLiteHelper.GetInstance().MeshPlanInsert(mpm);

                    MeshDeviceInfo mdi = new MeshDeviceInfo();
                    mdi.GroupName = mpm.GroupName;
                    mdi.Alias = mpm.Alias;
                    mdi.IPV4 = mpm.MeshIP;
                    mdi.Power = 15;
                    mdi.Frequency = 616;
                    mdi.BandWidth = 20;
                    mdi.Battery = 0;
                    SQLiteHelper.GetInstance().MeshDeviceInfoInsert(mdi);

                    logger.Info("插入新的预案：" + mpm.ToString());
                    break;
                case MeshPlanOperatorType.Edit:
                    MeshPlanManage mpm2 = new MeshPlanManage();
                    mpm2.ID = int.Parse(textBoxXMeshPlanAlias.Tag.ToString());
                    mpm2.GroupName = comboBoxExMPMGroupName.SelectedItem.ToString();
                    mpm2.Alias = textBoxXMeshPlanAlias.Text;
                    mpm2.MeshIP = ipAddressInputMeshPlanMeshIP.Value;
                    mpm2.Model265ID = textBoxXMeshPlanModel265ID.Text;
                    mpm2.Model265IP = ipAddressInputMeshPlanModel265IP.Value;
                    mpm2.TCPToCOMIP = ipAddressInputMPMTCPToCOM.Value;
                    //mpm.HKVideoIP = ipAddressInputMeshPlanHKIP.Value;
                    int count = SQLiteHelper.GetInstance().MeshPlanUpdate(mpm2);
                    if (count == 1)
                    {
                        MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        logger.Info("修改预案：" + mpm2.ToString());
                    }
                    break;
            }
            updateMeshPlanTable();
            updateMeshPlanControlsState(false);
            initMeshPlanControlsDefaultValue();
            buttonXMeshPlanAdd.Text = "添加预案";
        }

        /// <summary>
        /// Mesh设备预案表元素单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridViewXMeshPlan_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //选中单行
            if (e.RowIndex >= 0 && dataGridViewXMeshPlan.SelectedRows.Count == 1)
            {
                DataGridViewRow row = dataGridViewXMeshPlan.SelectedRows[0];
                textBoxXMeshPlanAlias.Tag = row.Cells[0].Value.ToString();
                comboBoxExMPMGroupName.SelectedItem = row.Cells[1].Value.ToString();
                textBoxXMeshPlanAlias.Text = row.Cells[2].Value.ToString();
                ipAddressInputMeshPlanMeshIP.Value = row.Cells[3].Value.ToString();
                ipAddressInputMeshPlanModel265IP.Value = row.Cells[4].Value.ToString();
                textBoxXMeshPlanModel265ID.Text = row.Cells[5].Value.ToString();
                ipAddressInputMPMTCPToCOM.Value = row.Cells[6].Value.ToString();
                mCurrentMPOType = MeshPlanOperatorType.Edit;
                buttonXMeshPlanAdd.Text = "修改预案";
                updateMeshPlanControlsState(true);
            }
        }

        /// <summary>
        /// 初始化Mesh设备预案管理表格
        /// </summary>
        private void initMeshPlanTable()
        {
            dataGridViewXMeshPlan.Rows.Clear();
            foreach (var item in mFormMain.MeshPlanManageDictionary)
            {
                MeshPlanManage mpm = item.Value;
                DataGridViewRow row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = mpm.ID });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = mpm.GroupName });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = mpm.Alias });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = mpm.MeshIP });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = mpm.Model265IP });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = mpm.Model265ID });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = mpm.TCPToCOMIP });
                row.Cells.Add(new DataGridViewTextBoxCell() { Value = mpm.HKVideoIP });
                dataGridViewXMeshPlan.Rows.Add(row);
            }
        }

        /// <summary>
        /// 初始化分组名称下拉框
        /// </summary>
        private void initComboxExMPMGroupName()
        {
            string[] groupNames = SQLiteHelper.GetInstance().MeshDeviceGroupNameAllQuery().ToArray();
            comboBoxExMPMGroupName.Items.AddRange(groupNames);
            comboBoxExMPMGroupName.SelectedIndex = 0;
        }

        /// <summary>
        /// 更新Mesh设备预案表
        /// </summary>
        private void updateMeshPlanTable()
        {
            mFormMain.MeshPlanManageDictionary = SQLiteHelper.GetInstance().MeshPlanAllQueryAsDictionary();
            initMeshPlanTable();
        }

        /// <summary>
        /// 更新Mesh预案中的控件状态
        /// </summary>
        /// <param name="enabled"></param>
        private void updateMeshPlanControlsState(bool enabled)
        {
            comboBoxExMPMGroupName.Enabled = enabled;
            textBoxXMeshPlanAlias.Enabled = enabled;
            ipAddressInputMeshPlanMeshIP.Enabled = enabled;
            ipAddressInputMeshPlanModel265IP.Enabled = enabled;
            textBoxXMeshPlanModel265ID.Enabled = enabled;
            ipAddressInputMPMTCPToCOM.Enabled = enabled;
            ipAddressInputMeshPlanHKIP.Enabled = false;  //始终隐藏
        }

        /// <summary>
        /// 初始化Mesh预案配置默认值
        /// </summary>
        private void initMeshPlanControlsDefaultValue()
        {
            string prefix = textBoxXBSIP1.Text + "." + textBoxXBSIP2.Text + "." + textBoxXBSIP3.Text + ".";
            textBoxXMeshPlanAlias.Text = "分组";
            ipAddressInputMeshPlanMeshIP.Value = prefix + "1";
            ipAddressInputMeshPlanModel265IP.Value = prefix + "2";
            textBoxXMeshPlanModel265ID.Text = "20000";
            ipAddressInputMPMTCPToCOM.Value = prefix + "3";
            ipAddressInputMeshPlanHKIP.Value = prefix + "4";
        }

        private enum MeshPlanOperatorType
        {
            /// <summary>
            /// 添加预案
            /// </summary>
            Add,
            /// <summary>
            /// 编辑预案
            /// </summary>
            Edit,
            /// <summary>
            /// 删除预案
            /// </summary>
            Delete
        }

        #endregion

        #region 基本配置事件处理

        /// <summary>
        /// 修改配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXMBSSave_Click(object sender, EventArgs e)
        {
            modifyLocalIP();
        }

        /// <summary>
        /// 修改指定网卡的IP地址
        /// </summary>
        private void modifyLocalIP()
        {
            if (NetUtils.ConfigNetworkCardIPAddress(NIC.SelectedItem.ToString(), ipAddressInputLocal.Text))
            {
                MessageBox.Show("IP地址配置成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("IP地址配置失败!", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion

        #region 私有方法


        /// <summary>
        /// 启用TableLayoutPanel双缓冲，防止界面闪烁
        /// </summary>
        private void setTableLayoutPanelDoubleBufferd()
        {
            tableLayoutPanelMeshNodeTopology.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshNodeTopology, true, null);
            tableLayoutPanelMeshParameters.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshParameters, true, null);
            tableLayoutPanelMeshPlan.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshPlan, true, null);
            tableLayoutPanelMeshBasicSetting.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshBasicSetting, true, null);
            tableLayoutPanelMeshLocalhostSetting.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshLocalhostSetting, true, null);
            tableLayoutPanelMeshTCP.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshTCP, true, null);
        }

        /// <summary>
        /// 初始化“Mesh基本参数配置”界面控件
        /// </summary>
        private void initMeshBaseParamConfit()
        {
            this.comboBoxExLocalhostNetwordCard.Items.AddRange(
                NetUtils.GetEthernetNetworkCardName());
            this.comboBoxExLocalhostNetwordCard.SelectedIndex = 0;
            ipAddressInputLocal.Value =
                NetUtils.GetIPv4ByNetworkCardName(
                    this.comboBoxExLocalhostNetwordCard.SelectedItem.ToString());
        }


        #endregion

        string currentNodeName = null;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (currentNodeName == null)
            {
                MessageBox.Show("请先选择节点MAC地址");
                return;
            }
            string name = currentNodeName;
            try
            {
                //数字校验
                //功率 10-30
                int itx = slider1.Value;
                //频率 616-656
                int irate = slider2.Value;
                //带宽 5-20
                int ibindwidth = slider3.Value;

                if (itx < 10 || itx > 30)
                    throw new Exception();
                if (irate < 616 || irate > 656)
                    throw new Exception();
                if (ibindwidth < 5 || ibindwidth > 20)
                    throw new Exception();

                if (hashTable.Contains(name))
                {
                    node info = (node)hashTable[name];
                    info.TxPower = itx;
                    info.Frequency = irate;
                    info.BandWidth = ibindwidth;

                    //todo database
                    MeshDeviceInfo meshInfo = SQLiteHelper.GetInstance().MeshDeviceInfoQueryByIP(info.IpAddress);
                    meshInfo.BandWidth = (decimal)info.BandWidth;
                    meshInfo.Frequency = (decimal)info.Frequency;
                    meshInfo.Power = (decimal)info.TxPower;

                    SQLiteHelper.GetInstance().MeshDeviceInfoUpdate(meshInfo);

                    MeshPlanManage meshPlan = SQLiteHelper.GetInstance().MeshPlanQueryByMeshIP(info.IpAddress);
                    if (meshPlan != null)
                    {
                        meshTcpManager.SendMessageTo(meshPlan.TCPToCOMIP, MeshTcpConfigManager.GetChangePowerCommand(itx));
                        meshTcpManager.SendMessageTo(meshPlan.TCPToCOMIP, MeshTcpConfigManager.GetChangeRateCommand(irate));
                    }
                    MessageBox.Show("设置成功");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("数据输入有误，请检查输入数据");
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string name = e.Node.Text;
            if (hashTable.Contains(name))
            {
                currentNodeName = name;
                node info = (node)hashTable[name];
                ipAddressInputMeshIP.Value = info.IpAddress;
                slider1.Value = (int)info.TxPower;
                slider2.Value = (int)info.Frequency;
                slider3.Value = (int)info.BandWidth;
                sliderUpdateText(slider1);
                sliderUpdateText(slider2);
                sliderUpdateText(slider3);
                progressBarXMeshPower.Value = (int)(info.Battery * 100);
            }
            else
            {
                currentNodeName = null;
                ipAddressInputMeshIP.Value = "";
            }
        }

        private void sliderValueChanged(object sender, EventArgs e)
        {
            Slider slider = (Slider)sender;
            sliderUpdateText(slider);
        }

        private void sliderUpdateText(Slider slider)
        {
            slider.Text = slider.Value.ToString();
        }

    }
}
