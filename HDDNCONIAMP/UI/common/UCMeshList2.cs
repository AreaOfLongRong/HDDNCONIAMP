using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BMap.NET.WindowsForm;
using BMap.NET.WindowsForm.BMapElements;
using BMap.NET.WindowsForm.Video;
using DevComponents.AdvTree;
using HDDNCONIAMP.DB;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.Network;
using HDDNCONIAMP.UI.AudioVideoProcess;
using HDDNCONIAMP.Utils;
using log4net;
using NodeTopology;

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

        #endregion

        #region 结构体

        /// <summary>
        /// Mesh设备所有信息结构体
        /// </summary>
        private struct MeshAllInfo
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
        }

        #endregion

        #region 属性
        
        /// <summary>
        /// 获取或设置与本控件绑定的百度地图控件
        /// </summary>
        public BMapControl2 BuddyBMapControl { get; set; }

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

            startTaskToRefreshMeshList();
        }

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
            Node selectedNode = advTreeMeshList.SelectedNode;
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
        /// 单击节点，跳转到设备所在的位置。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeMeshList_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            Node selectNode = advTreeMeshList.SelectedNode;
            Cell selectCell = selectNode.GetCellAt(e.X, e.Y);
            if (selectNode.Level == 1)
            {
                MeshAllInfo mai = (MeshAllInfo)selectNode.Tag;
                GPSInfo vp = mai.MeshGPSInfo;
                if (selectCell.Images.ImageIndex == 9 && BuddyBMapControl != null)
                {
                    //地图上跳转到设备所在的位置
                    BuddyBMapControl.Center = new LatLngPoint(vp.Lon, vp.Lat);
                    BuddyBMapControl.Locate(false);
                }
                else if (selectCell.Images.ImageIndex == 10 && BuddyGrid != null)
                {
                    VideoInject inject = new VideoInject(mFormMain.AllApplicationSetting[ApplicationSettingKey.VideoServerIPV4],
                        mFormMain.AllApplicationSetting[ApplicationSettingKey.VideoServerUserName],
                        mFormMain.AllApplicationSetting[ApplicationSettingKey.VideoServerPassword]);
                    inject.injectPanel(BuddyGrid.GetNextAvailablePanel(), 
                        mFormMain.GetVideoFullScreenLocation(), 
                        BuddyGrid.GetFullScreenPanel(), 
                        mai.PlanInfo.Model265ID, "0");
                }
            }
        }

        /// <summary>
        /// 子元素编辑完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeMeshList_AfterCellEditComplete(object sender, CellEditEventArgs e)
        {
            Node editNode = e.Cell.Parent;
            switch (editNode.Level)
            {
                case 0:
                    MeshDeviceGroup group = (MeshDeviceGroup)editNode.Tag;
                    //处理设备分组元素编辑事件
                    SQLiteHelper.GetInstance().MeshDeviceGroupUpdate(
                        group.ID, editNode.Text);
                    logger.Info("修改分组“" + group.ID.ToString() + "”的组名为“" + editNode.Text + "”");
                    break;
                case 1:
                    //处理设备节点的元素编辑事件
                    ((BMeshPoint)editNode.Tag).Alias = e.NewText;
                    break;
            }
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeMeshList_MouseDown(object sender, MouseEventArgs e)
        {
            Node selectNode = advTreeMeshList.SelectedNode;
            if (selectNode != null && selectNode.Level == 0)
            {
                //拖动设备
                advTreeMeshList.DoDragDrop(selectNode, DragDropEffects.Copy);
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
            Node targetNode = tree.GetNodeAt(point);
            Node dragNode = (Node)e.Data.GetData("DevComponents.AdvTree.Node");
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
        /// 开始任务刷新Mesh设备在线状态
        /// </summary>
        private void startTaskToRefreshMeshList()
        {
            Task.Factory.StartNew(
                () =>
                {
                    while (!LifeTimeControl.closing)
                    {   //定时刷新设备状态
                        foreach (MeshAllInfo item in mMeshAllInfo)
                        {
                            myTelnet telnet = new myTelnet(item.DeviceInfo.IPV4);
                            string receiveData = telnet.recvDataWaitWord("help", 1);
                            doUpdateAdvTreeMeshList(item, receiveData.Length > 0 ? "在线" : "离线");
                        }
                        Thread.Sleep(int.Parse(mFormMain.AllApplicationSetting[ApplicationSettingKey.MeshListRefreshFrequency]));
                    }
                });
        }

        /// <summary>
        /// 异步更新Mesh设备列表树委托
        /// </summary>
        /// <param name="mai"></param>
        /// <param name="args"></param>
        private delegate void updateAdvTreeMeshList(MeshAllInfo mai, params object[] args);

        /// <summary>
        /// 异步更新Mesh设备列表
        /// </summary>
        /// <param name="mai">Mesh设备所有信息</param>
        /// <param name="args">参数</param>
        private void doUpdateAdvTreeMeshList(MeshAllInfo mai, params object[] args)
        {
            try
            {
                if (this.advTreeMeshList.InvokeRequired)
                {
                    updateAdvTreeMeshList uatml = new updateAdvTreeMeshList(doUpdateAdvTreeMeshList);
                    this.Invoke(uatml, mai, args);
                }
                else
                {
                    mai.BuddyNode.Cells[1].Text = args[0].ToString();
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
                        subNode.Cells.Add(cellState);
                        Cell cellGPS = new Cell();
                        cellGPS.Images.ImageIndex = 9;
                        cellGPS.ImageAlignment = eCellPartAlignment.NearCenter;
                        subNode.Cells.Add(cellGPS);
                        Cell cellVideo = new Cell();
                        cellVideo.Images.ImageIndex = 10;
                        subNode.Cells.Add(cellVideo);

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
                                VideoID = mpm.Model265ID,
                                Power = item.Power,
                                Frequency = item.Frequency,
                                BandWidth = item.BandWidth,
                                Battery = item.Battery,
                                IsOnline = false,
                                Location = new LatLngPoint(0, 0)
                            }
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
                BuddyBMapControl.AddMeshDevicePlaces(mBMeshPoints);
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
                foreach (Node device in group.Nodes)
                {
                    if (searchText.Length > 0)
                        device.Visible = device.Text.Contains(searchText);
                    else
                        device.Visible = true;
                }
            }
        }



        #endregion

    }
}
