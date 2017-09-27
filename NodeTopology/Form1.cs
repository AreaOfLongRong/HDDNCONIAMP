using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace NodeTopology
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);
        int GW_CHILD = 5;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        public const int EM_SETREADONLY = 0xcf;
        
        
        ARPList MyARPLIST = new ARPList();

        string RootMAC = string.Empty;

        string RootIp = string.Empty;

        MeshNode RootNode = new MeshNode();

        BlockNodes MYBlockNodes = new BlockNodes();  //用于实时存储

        BlockNodes ShowBlockNodes = new BlockNodes(); //用于显示

        BlockNodes StoreBlockNodes = new BlockNodes(); //用于当MYBlockNodes收集完数据时，将ShowBlockNodes传入StoreBlockNodes并进行比对

        private const int Nmax = 150;

        List<string> NeedResearchMac = new List<string>();

        int RepeatTime = 0;

        int RepeatTimeForRead = 0;

        List<int> EveryColumnNodeCount = new List<int>();

        List<int> EveryColumnNodeCountForRead = new List<int>();


        int ScanRate = 10;

        int ShowRate = 10;


        private ReaderWriterLock _rReaderWriterLockwlock = new ReaderWriterLock();

        //BufferedGraphics graphBuffer; 

        
        public Form1()
        {
           

            
            InitializeComponent();

           // graphBuffer = (new BufferedGraphicsContext()).Allocate(panel2.CreateGraphics(),panel2.DisplayRectangle);

            try
            {
                this.ScanRate = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ScanRate"].ToString());
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog("未在配置文件中找到刷新速率！！！" + ex.Message.ToString());
            }

            try
            {
                this.ShowRate = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ShowRate"].ToString());
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("未在配置文件中找到显示速率！！！" + ex.Message.ToString());
            }



            IntPtr editHandle = GetWindow(this.NIC.Handle, GW_CHILD);
            SendMessage(editHandle, EM_SETREADONLY, 1, 0);


            this.NIC.DataSource = OperateNode.NetworkInterfaceCard();


            LogHelper.WriteLog("程序启动");

            if (string.IsNullOrEmpty(this.NIC.SelectedItem.ToString()))
            {
                MessageBox.Show("未找到网卡，请检查网卡是否正常开启！！！");
            
            }



            
            
        }





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

            MeshNode TheNode = MYBlockNodes.Nodelist.Where(n => n.MacAddress.Equals(needInfoNode)).ToList().First();


                if (TheNode != null)
                {

                    MyTelnet tn = new MyTelnet(TheNode.IpAddress);

                    try
                    {

                        string recvStr = tn.recvDataWaitWord("help", 1);

                        //Thread.Sleep(1);
                        tn.sendData("battery");
                        //Thread.Sleep(1);
                        recvStr = tn.recvDataWaitWord("V", 1);
                        TheNode.Battery = double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("V", ""));

                        Thread.Sleep(1);
                        tn.sendData("frequency");
                        recvStr = tn.recvDataWaitWord("MHz", 1);
                        TheNode.Frequency = double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("MHz", ""));

                        //Thread.Sleep(1);
                        tn.sendData("txpower");
                        recvStr = tn.recvDataWaitWord("dBm", 1);
                        TheNode.TxPower = double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("dBm", ""));
                        //Thread.Sleep(1);
                        tn.sendData("bandwidth");
                        recvStr = tn.recvDataWaitWord("MHz", 1);
                        TheNode.BandWidth = double.Parse(recvStr.Replace("OK\n#user@/>", "").Replace("MHz", ""));

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
                                        MeshNode NewNode = new MeshNode(NeedToCheckMac);

                                                NewNode.IpAddress = NeedToCheckIP;

                                                MeshRelation NewRelation = new MeshRelation(TheNode, NewNode);

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

                                                    //


                                                    var needupdaterelation = MYBlockNodes.Relationlist.Where(x => x.Localnode.MacAddress.Equals(NeedToCheckMac) && x.Localnode.IpAddress.Equals(NeedToCheckIP) && x.Remotenode.MacAddress.Equals(TheNode.MacAddress) && x.Remotenode.IpAddress.Equals(TheNode.IpAddress)).ToList();

                                                    if (needupdaterelation.Count > 0)
                                                    {
                                                        MeshRelation TheRelation = needupdaterelation.FirstOrDefault();
                                                        TheRelation.Remoteport = int.Parse(MeshInfo[0]);

                                                        TheRelation.Findtimes = 2;

                                                        //这里需要确认以下是否需要跟新RX和TX的信息！！！
                                                    }
                                                    else
                                                    {
                                                        var theOtherNode = MYBlockNodes.Nodelist.Where(x => x.MacAddress.Equals(NeedToCheckMac) && x.IpAddress.Equals(NeedToCheckIP)).ToList().First();

                                                        MeshRelation NewRelation = new MeshRelation(TheNode, theOtherNode);

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
                                    MeshNode NewNode = new MeshNode(NeedToCheckMac);

                                            NewNode.IpAddress = NeedToCheckIP;

                                            MeshRelation NewRelation = new MeshRelation(TheNode, NewNode);

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

            //20170301 已经能够获取到全网段IP MAC对照表          

            try
            {
                int i = int.Parse(this.textBox1.Text);

                if (i < 0 || i > 255)
                {
                    MessageBox.Show("请输入0～255之间的数字！");

                    this.textBox1.Focus();
                    this.textBox1.SelectAll();
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入合理IP网段信息！");
                this.textBox1.Focus();
                this.textBox1.SelectAll();
                return;
            }

            try
            {
                int i = int.Parse(this.textBox2.Text);

                if (i < 0 || i > 255)
                {
                    MessageBox.Show("请输入0～255之间的数字！");

                    this.textBox2.Focus();
                    this.textBox2.SelectAll();
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入合理IP网段信息！");
                this.textBox2.Focus();
                this.textBox2.SelectAll();
                return;
            }

            try
            {
                int i = int.Parse(this.textBox3.Text);

                if (i < 0 || i > 255)
                {
                    MessageBox.Show("请输入0～255之间的数字！");

                    this.textBox3.Focus();
                    this.textBox3.SelectAll();
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("请输入合理IP网段信息！");
                this.textBox3.Focus();
                this.textBox3.SelectAll();
                return;
            }




            string SubNet = this.textBox1.Text.Trim() + "." + this.textBox2.Text.Trim() + "." + this.textBox3.Text.Trim() + ".";

            ///之后选择的网卡无效
            ///之后填写的IP地址无效
            //this.NIC.Enabled = false;
            //this.textBox1.Enabled = false;
            //this.textBox2.Enabled = false;
            //this.textBox3.Enabled = false;


            while (true)
            {
                LogHelper.WriteLog("循环扫描开始！！！");

                LogHelper.WriteLog("PingSubNet:" + SubNet);

                MyARPLIST.PingSubNet(SubNet);

                LogHelper.WriteLog("PingSubNet结束!!!");

                LogHelper.WriteLog("ReloadARP开始！！！");

                MyARPLIST.ReloadARP();

                LogHelper.WriteLog("ReloadARP结束！！！");

                Thread.Sleep(1);


                ////以下应该为循环多线程执行?

                LogHelper.WriteLog("TestGetIniMACandIP开始！！！");

                TestGetIniMACandIP();

                LogHelper.WriteLog("TestGetIniMACandIP结束！！！");

                if (RootIp.Equals(string.Empty))
                {
                    MessageBox.Show("未获取到根节点IP信息，请检查网络连接是否正常！");

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

                    




                }



                LogHelper.WriteLog("RealTimeNode全部拓扑信息读取完成");


                this.PrintStatues(this.MYBlockNodes.Nodelist.ToList(),this.MYBlockNodes.Relationlist.ToList(),true);


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


        private delegate void AddNode(MeshNode Pnode);

        

        
        private void AddNodeMethod(MeshNode Pnode)
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
                    TreeNode[] TempNodes = this.treeView1.Nodes.Find(Pnode.IpAddress,false);

                    if (TempNodes.Count()>0)
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


            while (true)
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



                if (this.ShowBlockNodes.Nodelist.Count>0)
                {

                  

                 _rwlock.AcquireReaderLock(100);



                ///首先删除那些已经不存在的关系和节点!!!
                ///urinatedong 20170325
                ///删除上一轮中消失的节点
                ///问题是按照要求需要显示出来，显示在哪里??? 
                 var NoneNodes = from p in this.StoreBlockNodes.Nodelist where !(from q in this.ShowBlockNodes.Nodelist select q.MacAddress).Contains(p.MacAddress) select p;

                 if (NoneNodes.Count() > 0)
                 {
                     foreach (MeshNode n in NoneNodes)
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
                     foreach (MeshRelation r in NoneRelations)
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
                 int ColumnWidth = this.panel2.Width / RepeatTimeForRead;

                 int nodecount = 0;

                 

                 for (int i = 0; i < RepeatTimeForRead; i++)
                 {
                     //x坐标为 ColumnWidth/2 + i*ColumnWidth - 30(图片宽度的一半)

                     int j = EveryColumnNodeCountForRead[i];

                     if (j > 0)
                     {
                         //每行占据的高度
                         int CellHeight = this.panel2.Height / j;

                         for (int k = 0; k < j; k++)
                         {
                                //y坐标为 CellHeight/2 + k*CellHeight - 30(图片高度的一半)

                                //AddGObject(x, y, node);




                                MeshNode TempNode = ShowBlockNodes.Nodelist[nodecount];


                             //urinatedog 向TREEVIEW中加入节点
                             //TreeNode NewNode = this.treeView1.Nodes.Add(TempNode.IpAddress);
                             //TreeNode SubNode = new TreeNode(TempNode.MacAddress);

                             //NewNode.Nodes.Add(SubNode);

                             AddNodeMethod(TempNode);


                             int x = ColumnWidth / 2 + i * ColumnWidth - this.imageList1.ImageSize.Width/2;
                             int y = CellHeight / 2 + k * CellHeight - this.imageList1.ImageSize.Height/2;

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
                     foreach (MeshRelation r in ShowBlockNodes.Relationlist)
                     {
                         GNetwork.FindGObjectByName(r.Localnode.MacAddress, ref TempGObject);
                         int x1 = TempGObject.x1;
                         int y1 = TempGObject.y1;
                         GNetwork.FindGObjectByName(r.Remotenode.MacAddress, ref TempGObject);
                         int x2 = TempGObject.x1;
                         int y2 = TempGObject.y1;

                         int ImageHeight = imageList1.Images[0].Height;
                         int ImageWidth =  imageList1.Images[0].Width;

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


                Thread.Sleep(this.ShowRate *1000);




            }
        
        }


        Thread GetRealInfoThread;
        Thread RefreshPanelContext;
        Thread DrawNodeTestThread;

        private void button1_Click(object sender, EventArgs e)
        {


            //if (DrawNodeTestThread != null)
            //{
            //    DrawNodeTestThread.Abort();
            //}

            //DrawNodeTestThread = new Thread(DrawNodeTest);
            //DrawNodeTestThread.Start();


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

        private int Xdown = 0;
        private int Ydown = 0;
        private DateTime Tdown;
        private int DragTimeMin = 300; //拖动的时间大于300毫秒,则认为是有拖拽的动作!!
        private bool Dragging = false;
        private int CurrObjDragIndx = 0;


        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            //ReDrawAll();
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            string CoordsMsg = "";
            CoordsMsg = "x = " + e.X.ToString() + " : y = " + e.Y.ToString();
            toolStripStatusLabel1.Text = CoordsMsg;
            //this.toolTip1.Hide(this.panel2);
            //this.toolTip1.Hide(this);

        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
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

        private void panel2_MouseDown(object sender, MouseEventArgs e)
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

        private void Form1_Load(object sender, EventArgs e)
        {
            GNetwork = new GScenario(Nmax);
            GNetwork.Clear();
            GNetwork.CurrObjIndx = 0;
        }

        
        
        /// <summary>
        /// 关闭窗口，释放资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogHelper.WriteLog("系统被管理员正常退出!");
            System.Environment.Exit(System.Environment.ExitCode);
            this.Dispose();
            this.Close();
        }




        /// <summary>
        /// 在日志中打印当前扫描到的NODE和RELATION信息
        /// </summary>
        /// <param name="pnodelist">nodelist</param>
        /// <param name="prelationlist">relationlist</param>
        /// <param name="isonline">是否为实时数据，如果为前台显示用，则该项为否</param>
        private void PrintStatues(List<MeshNode> pnodelist, List<MeshRelation> prelationlist , bool isonline)
        {
            if (isonline)
            {
                LogHelper.WriteLog(" Node Discover Report");

                LogHelper.WriteLog("IP".PadLeft(20, ' ') + "MAC".PadLeft(20, ' ') + "BandWidth".PadLeft(10, ' ') + "TxPower".PadLeft(10, ' ') + "Frequency".PadLeft(10, ' ') + "Battery".PadLeft(10, ' '));

                foreach (MeshNode Si in pnodelist)
                {
            LogHelper.WriteLog(Si.IpAddress.PadLeft(20, ' ')  + Si.MacAddress.PadLeft(20, ' ') + Si.BandWidth.ToString().PadLeft(10, ' ')  + Si.TxPower.ToString().PadLeft(10, ' ')  + Si.Frequency.ToString().PadLeft(10, ' ')  + Si.Battery.ToString().PadLeft(10, ' '));
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

                foreach (MeshRelation R in prelationlist)
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
            }
            else
            { 
            
            }


        }

        /// <summary>
        /// 一键改频按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            
            double newfrequency = 0.0D;
            try
            {
                newfrequency = double.Parse(this.textBox4.Text.Trim());
            }
            catch(Exception ex)
            {
                MessageBox.Show("请检查输入的频率是否正确！！！");
                this.textBox4.Focus();
                this.textBox4.SelectAll();
                return;
            }



            Thread Tupdatefrequency = new Thread(updatefrequency);

            Tupdatefrequency.Start(newfrequency);

            while(true)
            {
                if (Tupdatefrequency.ThreadState.Equals("Running"))
                {
                    Thread.Sleep(5);
                }
                else
                {
                    this.button1.Enabled = true;
                    break;
                }
            }





        }

        #region 一键改频
        /// <summary>
        /// urinatedong 20170312
        /// 新增一键改频后台代码
        /// </summary>
        /// <param name="frequency">新频率</param>
        private void updatefrequency(object frequency)
        {

            GNetwork.Clear();
            GNetwork = new GScenario(Nmax);
            GNetwork.CurrObjIndx = 0;

            //this.panel2.Refresh();
            this.Invoke(new DPanel2Refresh(Panel2Refresh));
            
            
            
            
            List<MeshNode> updatefrequencynodelist = new List<MeshNode>();

            

            _rwlock.AcquireWriterLock(100);

             updatefrequencynodelist = this.ShowBlockNodes.Nodelist.ToArray().ToList();

            _rwlock.ReleaseWriterLock();






            if (updatefrequencynodelist.Count > 0)
            {
                ///尝试关闭进程
                try
                {
                    if (GetRealInfoThread != null)
                    {
                        GetRealInfoThread.Abort();
                        GetRealInfoThread = null;
                    }

                    if (RefreshPanelContext != null)
                    {
                        RefreshPanelContext.Abort();
                        RefreshPanelContext = null;
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("关闭前后台现成时异常：" + ex.Message.ToString());
                    MessageBox.Show("无法关闭前后台现成，不能进行一键改频操作");
                    return;
                }

                double param = (double)frequency;

                int nodecount = updatefrequencynodelist.Count;


                for (int i = nodecount-1; i >= 0; i--)
                {
                    
                    LogHelper.WriteLog("开始修改IP为" + updatefrequencynodelist[i].IpAddress + " 节点的frequency 信息!");
                    LogHelper.WriteLog("当前频率: " + updatefrequencynodelist[i].Frequency.ToString() + " 计划修改频率: " + param.ToString());
                    try
                    {
                        if (updatefrequencynodelist[i].Frequency.Equals(param))
                        {
                            LogHelper.WriteLog("频率与原来频率相同无需修改!!!");
                        }
                        else
                        {
                            UpdateFrequencyTelnetTelegram(updatefrequencynodelist[i].IpAddress, param);
                        }
                    }
                    catch(Exception ex)
                    {
                        LogHelper.WriteLog("该IP修改频率未成功,不再修改其他节点频率!!!");
                        MessageBox.Show("IP为" + updatefrequencynodelist[i].IpAddress + " 节点的频率信息未修改成功!");
                        break;
                    }

                }
                this.MYBlockNodes.Nodelist.Clear();
                this.MYBlockNodes.Relationlist.Clear();
                this.ShowBlockNodes.Nodelist.Clear();
                this.ShowBlockNodes.Relationlist.Clear();
                this.StoreBlockNodes.Nodelist.Clear();
                this.StoreBlockNodes.Relationlist.Clear();
                MessageBox.Show("更改频率完成，请重新扫描拓扑检查信息!!!");
                
            }
            else
            {
                MessageBox.Show("没有发现节点，请检查拓扑结构！");
            
            }










        }
        #endregion




        #region 离线测试
        /// <summary>
        /// urinatedong 20170314
        /// 新增离线测试功能，用来测试绘图部分的优化
        /// </summary>
        private void DrawNodeTest() 
        {

            while (true)
            {

                ///随机增加层数和NODE数量，关系
                MeshNode TestNode1 = new MeshNode("AAAAAAAAAAAA");

                TestNode1.IpAddress = "192.168.0.9";

                TestNode1.Battery = 1900;

                MeshNode TestNode2 = new MeshNode("AAAAAAAAAAAB");

                TestNode2.IpAddress = "192.168.0.10";

                TestNode2.Battery = 1900;

                MeshNode TestNode3 = new MeshNode("AAAAAAAAAAAC");

                TestNode3.IpAddress = "192.168.0.11";

                TestNode3.Battery = 1900;

                MeshNode TestNode4 = new MeshNode("AAAAAAAAAAAD");

                TestNode4.IpAddress = "192.168.0.18";


                this.MYBlockNodes.Nodelist.Add(TestNode1);
                this.MYBlockNodes.Nodelist.Add(TestNode2);
                this.MYBlockNodes.Nodelist.Add(TestNode3);
                this.MYBlockNodes.Nodelist.Add(TestNode4);


                MeshRelation relation1 = new MeshRelation(TestNode1,TestNode2);

                relation1.Findtimes = 2;

                MeshRelation relation2 = new MeshRelation(TestNode2, TestNode3);

                relation2.Findtimes = 2;

                MeshRelation relation3 = new MeshRelation(TestNode2,TestNode4);

                relation3.Findtimes = 1;


                 this.MYBlockNodes.Relationlist.Add(relation1);
                 this.MYBlockNodes.Relationlist.Add(relation2);
                 this.MYBlockNodes.Relationlist.Add(relation3);

                 this.RepeatTime = 3;

                 this.EveryColumnNodeCount =new List<int>() { 1 ,1 ,2};



                 LogHelper.WriteLog("DrawNodeTest全部拓扑信息读取完成");


               // this.PrintStatues(this.MYBlockNodes.Nodelist.ToList(), this.MYBlockNodes.Relationlist.ToList(), true);




                ///锁定信息并进行复制
                _rwlock.AcquireWriterLock(100);

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

        #endregion

        private void panel2_MouseDoubleClick(object sender, MouseEventArgs e)
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

                this.toolTip1.SetToolTip(this.panel2, GContainer.AddInfo);
                //this.toolTip1.ShowAlways = true;
                //this.toolTip1.Show(this);

            }
            else
            {
                // Click out of all objects
            }
            
            
        }

        private void panel2_Leave(object sender, EventArgs e)
        {

            //this.toolTip1.RemoveAll();
        }

        private void panel2_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.RemoveAll();
        }




    }
}
