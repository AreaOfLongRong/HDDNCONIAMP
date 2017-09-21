using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BMap.NET.WindowsForm;
using BMap.NET.WindowsForm.BMapElements;
using BMap.NET.WindowsForm.Video;
using DevComponents.AdvTree;
using HDDNCONIAMP.DB;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.Mesh;
using HDDNCONIAMP.UI.AudioVideoProcess;
using log4net;

namespace HDDNCONIAMP.UI.Common
{
    public partial class UCMeshList : UserControl
    {


        #region 私有变量

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(UCMeshList));

        /// <summary>
        /// 主界面引用
        /// </summary>
        private FormMain mFormMain;

        /// <summary>
        /// 当前新建分组索引编号
        /// </summary>
        private static int sCurrentNewGroupIndex = 0;

        /// <summary>
        /// Mesh设备分组列表
        /// </summary>
        private List<MeshDeviceGroup> mMeshDeviceGroups = new List<MeshDeviceGroup>();

        /// <summary>
        /// Mesh设备节点字典
        /// </summary>
        private Dictionary<string, Node> mMeshDeviceDictionary = new Dictionary<string, Node>();

        /// <summary>
        /// 设备标注点列表
        /// </summary>
        private List<BVideoPoint> mBVideoPoints = new List<BVideoPoint>();

        /// <summary>
        /// 视频封装
        /// </summary>
        private VideoInject inject = new VideoInject();

        #endregion

        #region 属性

        /// <summary>
        /// Mesh设备管理器
        /// </summary>
        public MeshDeviceManage MDManage { get; set; }

        /// <summary>
        /// 获取或设置与本控件绑定的百度地图控件
        /// </summary>
        public BMapControl2 BuddyBMapControl { get; set; }

        public IGrid BuddyGrid { get; set; }

        #endregion

        public UCMeshList(FormMain main)
        {
            InitializeComponent();
            mFormMain = main;
            //mFormMain.NLM.PGPSUDPListener.OnReceiveGPS += PGPSUDPListener_OnReceiveGPS;
        }

        /// <summary>
        /// 界面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCMeshList_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 开始扫描Mesh设备，并注册事件
        /// </summary>
        public void StartScanMeshDevice()
        {
            if (MDManage != null)
            {
                if (!MDManage.IsScanning)
                {
                    MDManage.StartScanMeshDevice();
                }
                MDManage.OnMeshDeviceAdded += MDManage_OnMeshDeviceAdded;
                MDManage.OnMeshDeviceUpdate += MDManage_OnMeshDeviceUpdate;
            }
        }

        /// <summary>
        /// 添加Mesh设备事件
        /// </summary>
        /// <param name="meshDeviceInfo"></param>
        private void MDManage_OnMeshDeviceAdded(MeshDeviceInfo meshDeviceInfo)
        {
            Node meshNode = new Node();
            meshNode.Text = meshDeviceInfo.Alias;
            meshNode.Tag = meshDeviceInfo;
            mMeshDeviceDictionary.Add(meshDeviceInfo.MAC, meshNode);
            nodeDefaultGroup.Nodes.Add(meshNode);
        }

        /// <summary>
        /// 更新Mesh设备事件
        /// </summary>
        /// <param name="meshDeviceInfo"></param>
        private void MDManage_OnMeshDeviceUpdate(MeshDeviceInfo meshDeviceInfo)
        {
            if (mMeshDeviceDictionary.ContainsKey(meshDeviceInfo.MAC))
            {
                mMeshDeviceDictionary[meshDeviceInfo.MAC].Tag = meshDeviceInfo;
            }
            else
            {
                Node meshNode = new Node();
                meshNode.Text = meshDeviceInfo.Alias;
                meshNode.Tag = meshDeviceInfo;
                mMeshDeviceDictionary.Add(meshDeviceInfo.MAC, meshNode);
                nodeDefaultGroup.Nodes.Add(meshNode);
            }
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
            Node node = new Node("新建分组" + sCurrentNewGroupIndex);
            node.ImageIndex = 5;
            node.ImageExpandedIndex = 4;
            advTreeMeshList.Nodes.Add(node);
            sCurrentNewGroupIndex++;
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
            if (selectedNode == nodeDefaultGroup)
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
            if (selectNode.Level == 1 && BuddyBMapControl != null)
            {
                //地图上跳转到设备所在的位置
                BMeshPoint vp = (BMeshPoint)selectNode.Tag;
                BuddyBMapControl.Center = vp.Location;
                BuddyBMapControl.Locate(false);
            }

            if (selectNode.Level == 1 && BuddyGrid != null)
            {
                //TODO：目前只支持一路信号输入，后续需修改
                BMeshPoint vp = (BMeshPoint)selectNode.Tag;
                //inject.injectPanel(BuddyGrid.GetPanelByIndex(1), mMeshPlanManageDictionary[vp.MACAddress].AudioVideoID, "0");
            }
        }

        /// <summary>
        /// 元素编辑完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeMeshList_AfterCellEdit(object sender, CellEditEventArgs e)
        {
            Node editNode = e.Cell.Parent;
            if (editNode.Level == 1)
            {//处理设备节点的元素编辑事件
                ((BMeshPoint)editNode.Tag).Alias = e.NewText;
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

        /// <summary>
        /// 接收到GPS信号的处理
        /// </summary>
        /// <param name="device"></param>
        private void PGPSUDPListener_OnReceiveGPS(AudioAndVideoDevice device)
        {
            BVideoPoint currentPoint = mBVideoPoints.Find(delegate (BVideoPoint p) {
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
                mBVideoPoints.Add(p);
            }

            try
            {
                //advTreeDeviceList.BeginInvoke(new updateDeviceListDelegate(updateDeviceList));
            }
            catch (InvalidOperationException ex)
            {
                logger.Error("更新列表错误", ex);
            }

            //更新地图窗体
            if (BuddyBMapControl != null)
            {
                BuddyBMapControl.AddVideoPlaces(mBVideoPoints);
            }
        }

        #endregion

    }
}
