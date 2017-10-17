using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using BMap.NET.WindowsForm;
using BMap.NET.WindowsForm.BMapElements;
using DevComponents.AdvTree;
using HDDNCONIAMP.DB;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.Network;
using HDDNCONIAMP.UI.AudioVideoProcess;
using HDDNCONIAMP.Utils;
using log4net;
using NodeTopology;
using System.Threading;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Text;

namespace HDDNCONIAMP.UI.Common
{
    public partial class UCMeshList2 : UserControl
    {

        #region 私有变量

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(UCMeshList2));

        /// <summary>
        /// 主界面引用
        /// </summary>
        private FormMain mFormMain;

        /// <summary>
        /// Mesh设备所有信息列表
        /// </summary>
        private List<MeshAllInfo> mMeshAllInfo = new List<MeshAllInfo>();

        /// <summary>
        /// Mesh设备标注点列表
        /// </summary>
        private List<BMeshPoint> mBMeshPoints = new List<BMeshPoint>();

        /// <summary>
        /// 视频转发服务
        /// </summary>
        private Process mVideoTransServerProcess;

        /// <summary>
        /// 是否处于视频转发过程中
        /// </summary>
        private bool mIsVideoTransferring = false;

        #endregion

        #region 结构体

        /// <summary>
        /// Mesh设备所有信息结构体
        /// </summary>
        private class MeshAllInfo
        {
            /// <summary>
            /// 获取或设置设备基本信息
            /// </summary>
            public MeshDeviceInfo DeviceInfo { get; set; }
            /// <summary>
            /// 获取或设置预案信息
            /// </summary>
            public MeshPlanManage PlanInfo { get; set; }
            /// <summary>
            /// 获取或设置GPS信息
            /// </summary>
            public GPSInfo MeshGPSInfo { get; set; }
            /// <summary>
            /// 获取或设置与该Mesh设备绑定的树节点
            /// </summary>
            public Node BuddyNode { get; set; }
            /// <summary>
            /// 获取或设置与该Mesh设备绑定的百度地图点
            /// </summary>
            public BMeshPoint BuddyBMeshPoint { get; set; }
            /// <summary>
            /// 设备离线次数，如果大于3次，则将设备置为离线状态
            /// </summary>
            public int OfflineCount { get; set; }
            /// <summary>
            /// 设备是否在线过
            /// </summary>
            public bool WasOnline { get; set; }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置与本控件绑定的百度地图控件
        /// </summary>
        public BMapControl2 BuddyBMapControl { get; set; }

        /// <summary>
        /// 获取或设置与本控件绑定的视频网格
        /// </summary>
        public IGrid BuddyGrid { get; set; }

        #endregion

        public UCMeshList2(FormMain main)
        {
            InitializeComponent();
            mFormMain = main;
            //双缓冲设置，防止界面闪烁
            setTableLayoutPanelDoubleBufferd();
        }

        /// <summary>
        /// 界面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCMeshList2_Load(object sender, EventArgs e)
        {
            loadMeshDeviceGroupFromDB();
            loadMeshDeviceFromDB();
            buttonItemExpandAll_Click(null, null);

            mFormMain.NLM.PGPSUDPListener.OnReceiveGPSInfo += PGPSUDPListener_OnReceiveGPSInfo;

            //视频转发按钮启用状态设置
            buttonItemVideoTransfer.Visible = (BuddyGrid != null);            
        }

        #region 公共方法

        /// <summary>
        /// 注册相关事件
        /// </summary>
        public void RegisterEvent()
        {

            //注册地图Mesh设备点击打开视频事件
            if (BuddyBMapControl != null)
            {
                BuddyBMapControl.OnOpenVideo += BuddyBMapControl_OnOpenVideo;
            }

        }

        /// <summary>
        /// 更新Mesh设备状态
        /// </summary>
        /// <param name="meshIp">Mesh设备IP</param>
        /// <param name="status">状态</param>
        public void UpdateMeshStatus(string meshIp, string status)
        {
            MeshAllInfo mai = mMeshAllInfo.Find(m => m.DeviceInfo.IPV4 == meshIp);
            if (mai != null)
            {
                if (status.Equals("离线"))
                {
                    //如果之前有在线过，3次以内如果没有ping到该设备，仍然认为该设备在线，否则不在线
                    if (mai.WasOnline && mai.OfflineCount < 3)
                    {
                        status = "在线";
                        mai.OfflineCount++;
                        mai.WasOnline = true;
                    }
                    else
                    {
                        mai.OfflineCount = 0;
                        mai.WasOnline = false;
                    }
                }
                else
                {
                    mai.OfflineCount = 0;
                    mai.WasOnline = true;
                }
                doUpdateAdvTreeMeshList(meshIp, status);
            }
        }

        /// <summary>
        /// 更新Mesh设备信息
        /// </summary>
        /// <param name="mdi">Mesh设备信息</param>
        public void UpdateMeshDeviceInfo(MeshDeviceInfo mdi)
        {
            MeshAllInfo mai = mMeshAllInfo.Find(m => m.DeviceInfo.IPV4.Equals(mdi.IPV4));
            if(mai != null)
            {
                mai.DeviceInfo.Frequency = mdi.Frequency;
                mai.DeviceInfo.Power = mdi.Power;
                mai.DeviceInfo.BandWidth = mdi.BandWidth;
                mai.BuddyBMeshPoint.Frequency = mdi.Frequency;
                mai.BuddyBMeshPoint.Power = mdi.Power;
                mai.BuddyBMeshPoint.BandWidth = mdi.BandWidth;
            }
        }
        
        #endregion

        #region 设备列表事件


        /// <summary>
        /// 设备搜索框按键按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxXSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals((char)Keys.Return))
                onSearchMeshDevice();
        }

        /// <summary>
        /// 搜索按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXSearch_Click(object sender, EventArgs e)
        {
            onSearchMeshDevice();
        }

        /// <summary>
        /// 展开所有树节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemExpandAll_Click(object sender, EventArgs e)
        {
            advTreeMeshList.ExpandAll();
            buttonItemExpandAll.Enabled = false;
            buttonItemFoldAll.Enabled = true;
        }

        /// <summary>
        /// 折叠所有树节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemFoldAll_Click(object sender, EventArgs e)
        {
            advTreeMeshList.CollapseAll();
            buttonItemExpandAll.Enabled = true;
            buttonItemFoldAll.Enabled = false;

        }

        /// <summary>
        /// 添加分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemAddGroup_Click(object sender, EventArgs e)
        {
            Node node = new Node("新建分组" + SQLiteHelper.GetInstance().GetNextMeshDeviceGroupID());
            node.ImageIndex = 5;
            node.ImageExpandedIndex = 4;
            advTreeMeshList.Nodes.Add(node);
            int id = SQLiteHelper.GetInstance().MeshDeviceGroupInsert(node.Text);
            node.Tag = new MeshDeviceGroup() { ID = id, GroupName = node.Text };
            logger.Info("添加分组“" + node.Text + "”。");
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemDeleteGroup_Click(object sender, EventArgs e)
        {
            DevComponents.AdvTree.Node selectedNode = advTreeMeshList.SelectedNode;
            if (selectedNode == null)
            {
                MessageBox.Show("未选择任何分组！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (selectedNode.Text == "默认分组")
            {
                MessageBox.Show("默认分组不能删除！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (selectedNode.Level == 0)
            {
                if (DialogResult.OK == MessageBox.Show("确认删除分组“" + selectedNode.Text + "”？", "提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                {
                    try
                    {
                        SQLiteHelper.GetInstance().MeshDeviceGroupDelete(selectedNode.Text);
                        advTreeMeshList.Nodes.Remove(selectedNode);
                        logger.Info("删除分组“" + selectedNode.Text + "”。");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除分组“" + selectedNode.Text + "”失败！", "失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        logger.Error("删除分组“" + selectedNode.Text + "”失败！\n", ex);
                    }
                }
            }

        }

        /// <summary>
        /// 刷新树形列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemRefreshTree_Click(object sender, EventArgs e)
        {
            advTreeMeshList.Nodes.Clear();
            loadMeshDeviceGroupFromDB();
            loadMeshDeviceFromDB();
        }

        /// <summary>
        /// 开启/关闭视频转发服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemVideoTransfer_Click(object sender, EventArgs e)
        {
            if (!mIsVideoTransferring)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = FileUtils.VIDEO_TRANSFER_SERVER_EXE_PATH;
                psi.RedirectStandardOutput = false;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                //psi.CreateNoWindow = false;
                mVideoTransServerProcess = new Process();
                mVideoTransServerProcess.StartInfo = psi;
                mVideoTransServerProcess.Start();
                logger.Info("开启视频转发服务，端口：58000");
                MessageBox.Show("视频转发服务已启动，UDP监听端口：58000.", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                buttonItemVideoTransfer.Text = "停止转发服务";
            }
            else
            {
                if (mVideoTransServerProcess != null && !mVideoTransServerProcess.HasExited)
                {
                    mVideoTransServerProcess.Kill();
                    mVideoTransServerProcess.WaitForExit();
                    logger.Info("关闭视频转发服务。");
                    buttonItemVideoTransfer.Text = "开启转发服务";
                }
            }
            mIsVideoTransferring = !mIsVideoTransferring;
        }

        /// <summary>
        /// 单击节点，跳转到设备所在的位置。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeMeshList_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            DevComponents.AdvTree.Node selectNode = advTreeMeshList.SelectedNode;

            //设备不在线，不执行后续操作
            if (selectNode == null || selectNode.Level != 1)
                return;

            Cell selectCell = selectNode.GetCellAt(e.X, e.Y);
            if (selectCell == null)
                return;

            if (selectNode.Level == 1 && selectCell.Images != null)
            {
                MeshAllInfo mai = (MeshAllInfo)selectNode.Tag;
                GPSInfo vp = mai.MeshGPSInfo;
                //GPS坐标为（0,0），不能执行定位操作
                if (selectCell.Images.ImageIndex == 10 &&
                    vp.Lat != 0 && vp.Lon != 0
                    && BuddyBMapControl != null)
                {
                    //地图上跳转到设备所在的位置
                    BuddyBMapControl.Center = new LatLngPoint(vp.Lon, vp.Lat);
                    BuddyBMapControl.Locate(false);
                }
                else if (selectCell.Images.ImageIndex == 11)
                {
                    MessageBox.Show("设备不在线！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (selectCell.Images.ImageIndex == 12 && selectNode.Cells[1].Text.Equals("在线"))
                {
                    if (BuddyBMapControl != null)
                    {
                        BuddyBMapControl_OnOpenVideo(mai.BuddyBMeshPoint);
                    }
                    else if (BuddyGrid != null)
                    {
                        VideoInject inject = new VideoInject(mFormMain.AllApplicationSetting[ApplicationSettingKey.VideoServerIPV4],
                            mFormMain.AllApplicationSetting[ApplicationSettingKey.VideoServerUserName],
                            mFormMain.AllApplicationSetting[ApplicationSettingKey.VideoServerPassword]);
                        Panel panel = BuddyGrid.GetNextAvailablePanel();
                        Process process = inject.injectPanel(panel,
                            mFormMain.GetVideoFullScreenLocation(),
                            BuddyGrid.GetFullScreenPanel(),
                            mai.PlanInfo.Model265ID, "0");
                        BuddyGrid.BindPanelProcess(panel, process);
                        mFormMain.VideoProcesses.Add(process);
                        logger.Info(string.Format("在第{0}个Panel中打开了视频。", panel.Tag.ToString()));
                    }
                }
                else if (selectCell.Images.ImageIndex == 13 && BuddyBMapControl != null)
                {
                    bool isDrawingRoute = (bool)selectCell.Tag;
                    if (isDrawingRoute)
                    {//已经绘制，再次点击的时候隐藏已绘制的轨迹
                        BuddyBMapControl.DeleteDeviceRoute(mai.PlanInfo.Model265ID);
                        selectCell.Tag = false;
                    }
                    else
                    {
                        //在地图上绘制轨迹记录
                        FGPSTimeSelect fgpsts = new FGPSTimeSelect();
                        fgpsts.StartDateTime = DateTime.Today.Subtract(new TimeSpan(1, 0, 0, 0));
                        fgpsts.StopDateTime = DateTime.Today;
                        if (DialogResult.OK == fgpsts.ShowDialog() && BuddyBMapControl != null)
                        {
                            BMeshRoute bmr = FileUtils.ReadMeshRouteFromGPSLogs(
                                    mai.PlanInfo.Model265ID,
                                    fgpsts.StartDateTime, fgpsts.StopDateTime);
                            if (bmr.DeviceLocationList.Count > 0)
                            {
                                BuddyBMapControl.AddDeviceRoute(bmr);
                                //地图上跳转到设备所在的位置
                                BuddyBMapControl.Center = bmr.DeviceLocationList[0];
                                BuddyBMapControl.Locate(false);
                                BuddyBMapControl.Zoom = 16;
                                selectCell.Tag = true;  //标识已经绘制了路径
                                logger.Info(string.Format("查看{0}设备的GPS轨迹记录，供{1}条GPS记录。",
                                    mai.PlanInfo.Model265ID, bmr.DeviceLocationList.Count));
                            }
                            else
                            {
                                MessageBox.Show("无历史轨迹记录。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// GIS地图上点击打开视频事件
        /// </summary>
        /// <param name="p"></param>
        private void BuddyBMapControl_OnOpenVideo(BMeshPoint p)
        {
            VideoInject inject = new VideoInject(mFormMain.AllApplicationSetting[ApplicationSettingKey.VideoServerIPV4],
                        mFormMain.AllApplicationSetting[ApplicationSettingKey.VideoServerUserName],
                        mFormMain.AllApplicationSetting[ApplicationSettingKey.VideoServerPassword]);
            Process process = mFormMain.VideoWindowProcesses.Find(ps => ps.StartInfo.Arguments.Contains(p.Model265ID));
            if (BuddyBMapControl != null)
            {
                if (process == null)
                {
                    mFormMain.VideoWindowProcesses.Add(inject.injectWindow(p.Model265ID));
                }
                else
                {
                    if (process.HasExited)
                    {
                        mFormMain.VideoWindowProcesses.Remove(process);
                        mFormMain.VideoWindowProcesses.Add(inject.injectWindow(p.Model265ID));
                    }
                    else
                    {
                        //如果已经打开过该视频，则直接将视频窗口置顶
                        VideoInject.SetForegroundWindow(process.MainWindowHandle);
                    }
                }
            }
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeMeshList_MouseDown(object sender, MouseEventArgs e)
        {
            DevComponents.AdvTree.Node selectNode = advTreeMeshList.SelectedNode;
            if (selectNode != null && selectNode.Level == 0)
            {
                //拖动设备
                advTreeMeshList.DoDragDrop(selectNode, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// 接收到GPS信号事件
        /// </summary>
        /// <param name="gpsInfo">GPS信息</param>
        private void PGPSUDPListener_OnReceiveGPSInfo(GPSInfo gpsInfo)
        {
            MeshAllInfo mesh = mMeshAllInfo.Find(m => m.PlanInfo.Model265ID == gpsInfo.ID);

            //如果ID不存在，则退出
            if (mesh == null)
                return;

            if (mesh.PlanInfo != null)
            {
                GPSInfo gi = mesh.MeshGPSInfo;
                gi.Time = gpsInfo.Time;
                gi.Lat = gpsInfo.Lat;
                gi.Lon = gpsInfo.Lon;
                mesh.MeshGPSInfo = gi;

                BMeshPoint bmp = mesh.BuddyBMeshPoint;
                bmp.IsOnline = true;
                bmp.ReceiveGPSDT = DateTime.Now;
                bmp.Location = new LatLngPoint(gpsInfo.Lon, gpsInfo.Lat);
                mesh.BuddyBMeshPoint = bmp;
            }

            if (mesh.BuddyNode != null)
            {
                doUpdateAdvTreeMeshList(mesh.DeviceInfo.IPV4, "在线", "GPS在线");
            }

            //如果之前没有在地图上显示该设备则添加显示
            BMeshPoint p = mBMeshPoints.Find(b => b.IPV4 == mesh.PlanInfo.MeshIP);
            if (p != null)
            {
                p.Location = new LatLngPoint(gpsInfo.Lon, gpsInfo.Lat);
                if (BuddyBMapControl != null)
                {
                    BuddyBMapControl.AddMeshDevicePlaces(mBMeshPoints);
                }
            }

        }

        #region 拖拽事件处理


        private void advTreeMeshList_NodeDragStart(object sender, EventArgs e)
        {
            Node node = (Node)sender;
            if (node != null && node.Level == 1)
            {
                DoDragDrop(node, DragDropEffects.Move);
            }
        }

        private void advTreeMeshList_DragEnter(object sender, DragEventArgs e)
        {
            // 拖动效果设成移动
            e.Effect = DragDropEffects.Move;
        }

        private void advTreeMeshList_DragDrop(object sender, DragEventArgs e)
        {
            AdvTree tree = (AdvTree)sender;
            Point point = tree.PointToClient(new Point(e.X, e.Y));
            DevComponents.AdvTree.Node targetNode = tree.GetNodeAt(point);
            DevComponents.AdvTree.Node dragNode = (DevComponents.AdvTree.Node)e.Data.GetData("DevComponents.AdvTree.Node");
            if (targetNode != null)
            {
                targetNode.Nodes.Insert(targetNode.Nodes.Count, dragNode);
                advTreeMeshList.SelectedNode = dragNode;
                targetNode.Expand();
            }
        }

        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// 启用TableLayoutPanel双缓冲，防止界面闪烁
        /// </summary>
        private void setTableLayoutPanelDoubleBufferd()
        {
            tableLayoutPanelMeshDeviceList.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshDeviceList, true, null);
        }
        
        /// <summary>
        /// 异步更新Mesh设备列表树委托
        /// </summary>
        /// <param name="ip">Mesh设备IP地址</param>
        /// <param name="args">参数</param>
        private delegate void updateAdvTreeMeshList(string ip, params object[] args);

        /// <summary>
        /// 异步更新Mesh设备列表
        /// </summary>
        /// <param name="ip">Mesh设备IP地址</param>
        /// <param name="args">参数</param>
        private void doUpdateAdvTreeMeshList(string ip, params object[] args)
        {
            try
            {
                if (this.advTreeMeshList.InvokeRequired)
                {
                    updateAdvTreeMeshList uatml = new updateAdvTreeMeshList(doUpdateAdvTreeMeshList);
                    advTreeMeshList.BeginInvoke(uatml, ip, args);
                }
                else
                {
                    advTreeMeshList.BeginUpdate();
                    MeshAllInfo mai = mMeshAllInfo.Find(m => m.DeviceInfo.IPV4 == ip);
                    if (mai != null)
                    {
                        mai.BuddyNode.Cells[1].Text = args[0].ToString();
                        mai.BuddyNode.Cells[1].StyleNormal.TextColor = args[0].ToString().Equals("在线") ? Color.Black : Color.Gray;
                        mai.BuddyNode.Cells[1].StyleNormal.Font = args[0].ToString().Equals("在线") ? new Font("宋体", 9, FontStyle.Bold) : new Font("宋体", 9, FontStyle.Regular);
                        mai.BuddyNode.Cells[2].Images.ImageIndex = args[0].ToString().Equals("在线") && args.Length == 2 ? 10 : 9;
                        mai.BuddyNode.Cells[3].Images.ImageIndex = args[0].ToString().Equals("在线") ? 12 : 11;
                    }
                    advTreeMeshList.EndUpdate();
                }
            }
            catch (Exception ex)
            {
                logger.Error("更新Mesh设备列表发生异常：", ex);
            }
        }

        /// <summary>
        /// 从数据库中加载Mesh设备分组信息
        /// </summary>
        private void loadMeshDeviceGroupFromDB()
        {
            advTreeMeshList.BeginUpdate();
            List<MeshDeviceGroup> mdgList = SQLiteHelper.GetInstance().MeshDeviceGroupAllQuery();
            foreach (var item in mdgList)
            {
                //添加分组节点
                Node node = new Node(item.GroupName);
                node.ImageIndex = 5;
                node.ImageExpandedIndex = 4;
                node.Tag = item.ID;
                advTreeMeshList.Nodes.Add(node);
            }
            advTreeMeshList.EndUpdate();
        }

        /// <summary>
        /// 从数据库中读取并添加Mesh设备列表
        /// </summary>
        private void loadMeshDeviceFromDB()
        {
            advTreeMeshList.BeginUpdate();
            List<MeshDeviceInfo> mdiList = SQLiteHelper.GetInstance().MeshDeviceInfoAllQuery();
            foreach (MeshDeviceInfo item in mdiList)
            {
                foreach (Node node in advTreeMeshList.Nodes)
                {
                    if (node.Text.Equals(item.GroupName))
                    {
                        Node subNode = new Node();
                        subNode.Text = item.Alias;
                        subNode.ImageIndex = 8;
                        Cell cellState = new Cell();
                        cellState.Text = "离线";
                        cellState.StyleNormal = new DevComponents.DotNetBar.ElementStyle();
                        cellState.StyleNormal.TextColor = cellState.Text.Equals("离线") ? Color.Gray : Color.DarkGreen;
                        subNode.Cells.Add(cellState);
                        Cell cellGPS = new Cell();
                        cellGPS.Images.ImageIndex = 9;
                        cellGPS.ImageAlignment = eCellPartAlignment.NearCenter;
                        subNode.Cells.Add(cellGPS);
                        Cell cellVideo = new Cell();
                        cellVideo.Images.ImageIndex = 11;
                        subNode.Cells.Add(cellVideo);
                        Cell cellGPSTrack = new Cell();
                        cellGPSTrack.Images.ImageIndex = 13;
                        cellGPSTrack.ImageAlignment = eCellPartAlignment.NearBottom;
                        cellGPSTrack.Tag = false;  //标识是否在界面上绘制了历史轨迹
                        subNode.Cells.Add(cellGPSTrack);

                        MeshPlanManage mpm = SQLiteHelper.GetInstance().MeshPlanQueryByMeshIP(item.IPV4);
                        MeshAllInfo nodeMAI = new MeshAllInfo()
                        {
                            DeviceInfo = item,
                            PlanInfo = mpm,
                            MeshGPSInfo = new GPSInfo(),
                            BuddyNode = subNode,
                            BuddyBMeshPoint = new BMeshPoint()
                            {
                                GroupName = item.GroupName,
                                Alias = item.Alias,
                                IPV4 = item.IPV4,
                                Model265ID = mpm.Model265ID,
                                Power = item.Power,
                                Frequency = item.Frequency,
                                BandWidth = item.BandWidth,
                                Battery = item.Battery,
                                IsOnline = false,
                                Location = new LatLngPoint(0, 0),
                                Expiration = 30 * 1000,
                                ReceiveGPSDT = DateTime.Now
                            },
                            OfflineCount = 0,
                            WasOnline = false
                        };
                        //添加进列表
                        mMeshAllInfo.Add(nodeMAI);
                        mBMeshPoints.Add(nodeMAI.BuddyBMeshPoint);
                        subNode.Tag = nodeMAI;
                        node.Nodes.Add(subNode);
                        break;
                    }
                }
            }
            //添加Mesh设备点到百度地图上
            if (BuddyBMapControl != null)
            {
                //BuddyBMapControl.AddMeshDevicePlaces(mBMeshPoints);
            }
            advTreeMeshList.EndUpdate();
        }

        /// <summary>
        /// 设备检索
        /// </summary>
        private void onSearchMeshDevice()
        {
            string searchText = textBoxXSearch.Text.Trim();
            foreach (Node group in advTreeMeshList.Nodes)
            {
                bool groupVidible = false;
                foreach (Node device in group.Nodes)
                {
                    //设置设备节点的可见性
                    if (searchText.Length > 0)
                        device.Visible = device.Text.Contains(searchText);
                    else
                        device.Visible = true;
                    groupVidible |= device.Visible;  //处理分组节点的可见性
                }
                if (group.Text.Contains(searchText))
                {
                    group.Visible = true;
                    foreach (Node device in group.Nodes)
                    {
                        device.Visible = true;
                    }
                }
                else
                {
                    group.Visible = groupVidible;
                }
            }
        }

        #endregion

    }
}
