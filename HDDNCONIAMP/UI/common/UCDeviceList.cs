using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BMap.NET.WindowsForm;
using BMap.NET.WindowsForm.BMapElements;
using BMap.NET.WindowsForm.Video;
using DevComponents.AdvTree;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.UI.AudioVideoProcess;
using log4net;

namespace HDDNCONIAMP.UI.Common
{
    public partial class UCDeviceList : UserControl
    {

        #region 私有字段

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(UCDeviceList));

        /// <summary>
        /// 当前新建分组索引编号
        /// </summary>
        private static int sCurrentNewGroupIndex = 0;

        /// <summary>
        /// Mesh设备分组列表
        /// </summary>
        private List<MeshDeviceGroup> mMeshDeviceGroups = new List<MeshDeviceGroup>();

        /// <summary>
        /// 设备标注点列表
        /// </summary>
        private List<BVideoPoint> mBVideoPoints = new List<BVideoPoint>();

        /// <summary>
        /// 主窗口引用
        /// </summary>
        private FormMain mFormMain;
        
        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置与本控件绑定的百度地图控件
        /// </summary>
        public BMapControl2 BuddyBMapControl { get; set; }

        public IGrid BuddyGrid { get; set; }

        #endregion


        public UCDeviceList(FormMain main)
        {
            InitializeComponent();
            mFormMain = main;
            mFormMain.NLM.PGPSUDPListener.OnReceiveGPS += PGPSUDPListener_OnReceiveGPS;
        }

        /// <summary>
        /// 控件加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCDeviceList_Load(object sender, EventArgs e)
        {
            

            //List<BDeviceRoute> routes = new List<BDeviceRoute>();
            //BDeviceRoute r1 = new BDeviceRoute();
            //r1.DeviceName = "device1";
            //List<LatLngPoint> ps1 = new List<LatLngPoint>();
            //ps1.Add(new LatLngPoint(115.20, 39));
            //ps1.Add(new LatLngPoint(116.30, 40));
            //ps1.Add(new LatLngPoint(116.391046, 40.014476));
            //r1.DeviceLocationList = ps1;
            //routes.Add(r1);
            //BDeviceRoute r2 = new BDeviceRoute();
            //r2.DeviceName = "device2";
            //List<LatLngPoint> ps2 = new List<LatLngPoint>();
            //ps2.Add(new LatLngPoint(117.20, 39));
            //ps2.Add(new LatLngPoint(116.80, 40.1));
            //ps2.Add(new LatLngPoint(116.549722, 39.972907));
            //r2.DeviceLocationList = ps2;
            //routes.Add(r2);
            //if (BuddyBMapControl != null)
            //{
            //    BuddyBMapControl.AddDeviceRoutes(routes);
            //}

            
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
                onSearchDevice();
        }

        /// <summary>
        /// 搜索按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXSearch_Click(object sender, EventArgs e)
        {
            onSearchDevice();
        }

        /// <summary>
        /// 展开所有树节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonItemExpandAll_Click(object sender, EventArgs e)
        {
            advTreeDeviceList.ExpandAll();
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
            advTreeDeviceList.CollapseAll();
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
            advTreeDeviceList.Nodes.Add(node);
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
            Node selectedNode = advTreeDeviceList.SelectedNode;
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
                        advTreeDeviceList.Nodes.Remove(selectedNode);
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
        private void advTreeDeviceList_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            Node selectNode = advTreeDeviceList.SelectedNode;
            if(selectNode.Level == 1 && BuddyBMapControl != null)
            {
                //地图上跳转到设备所在的位置
                BVideoPoint vp = (BVideoPoint)selectNode.Tag;
                BuddyBMapControl.Center = vp.Location;
                BuddyBMapControl.Locate(false);
            }

            if (selectNode.Level == 1 && BuddyGrid != null)
            {
                //TODO：添加视频信号
            }
        }

        /// <summary>
        /// 元素编辑完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeDeviceList_AfterCellEdit(object sender, CellEditEventArgs e)
        {
            Node editNode = e.Cell.Parent;
            if (editNode.Level == 1)
            {//处理设备节点的元素编辑事件
                ((BVideoPoint)editNode.Tag).Alias = e.NewText;
            }
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void advTreeDeviceList_MouseDown(object sender, MouseEventArgs e)
        {
            Node selectNode = advTreeDeviceList.SelectedNode;
            if (selectNode != null && selectNode.Level == 0)
            {
                //拖动设备
                advTreeDeviceList.DoDragDrop(selectNode, DragDropEffects.Copy);
            }
        }

        #region 拖拽事件处理


        private void advTreeDeviceList_NodeDragStart(object sender, EventArgs e)
        {
            Node node = (Node)sender;
            if (node != null && node.Level == 1)
            {
                DoDragDrop(node, DragDropEffects.Move);
            }
        }

        private void advTreeDeviceList_DragEnter(object sender, DragEventArgs e)
        {
            // 拖动效果设成移动
            e.Effect = DragDropEffects.Move;
        }

        private void advTreeDeviceList_DragDrop(object sender, DragEventArgs e)
        {
            AdvTree tree = (AdvTree)sender;
            Point point = tree.PointToClient(new Point(e.X, e.Y));
            Node targetNode = tree.GetNodeAt(point);
            Node dragNode = (Node)e.Data.GetData("DevComponents.AdvTree.Node");
            if (targetNode != null)
            {
                targetNode.Nodes.Insert(targetNode.Nodes.Count, dragNode);
                advTreeDeviceList.SelectedNode = dragNode;
                targetNode.Expand();
            }
        }

        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// 设备检索
        /// </summary>
        private void onSearchDevice()
        {
            string searchText = textBoxXSearch.Text.Trim();
            foreach (Node group in advTreeDeviceList.Nodes)
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
        /// 更新设备列表委托
        /// </summary>
        private delegate void updateDeviceListDelegate();

        /// <summary>
        /// 更新设备列表
        /// </summary>
        public void updateDeviceList()
        {
            advTreeDeviceList.BeginUpdate();
            foreach (BVideoPoint vp in mBVideoPoints)
            {
                bool exist = false;
                foreach (Node group in advTreeDeviceList.Nodes)
                {
                    foreach (Node n in group.Nodes)
                    {
                        BVideoPoint temp = (BVideoPoint)n.Tag;
                        //如果节点已存在则更新节点内容
                        if (temp.Name.Equals(vp.Name))
                        {
                            n.Tag = vp;
                            exist = true;
                            break;
                        }
                    }
                    if (exist)
                        break;
                }
                
                if (!exist)
                {
                    Node node = new Node();
                    node.Tag = vp;
                    node.Text = vp.Alias;
                    nodeDefaultGroup.Nodes.Add(node);
                }
            }
            advTreeDeviceList.EndUpdate();
        }

        /// <summary>
        /// 接收到GPS信号时，实时更新设备位置
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
                advTreeDeviceList.BeginInvoke(new updateDeviceListDelegate(updateDeviceList));
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
