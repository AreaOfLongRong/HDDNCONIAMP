using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BMap.NET.WindowsForm;
using BMap.NET.WindowsForm.BMapElements;
using DevComponents.AdvTree;
using HDDNCONIAMP.DB.Model;
using log4net;

namespace HDDNCONIAMP.UI.common
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

        #endregion

        #region 属性

        /// <summary>
        /// 获取或设置与本控件绑定的百度地图控件
        /// </summary>
        public BMapControl2 BuddyBMapControl { get; set; }



        #endregion


        public UCDeviceList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 控件加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCDeviceList_Load(object sender, EventArgs e)
        {
            //临时测试设备添加
            List<BVideoPoint> devices = new List<BVideoPoint>();
            BVideoPoint poi1 = new BVideoPoint();
            poi1.Location = new LatLngPoint(116.391046, 40.014476);
            poi1.Index = 1;
            poi1.IsOnline = true;
            devices.Add(poi1);
            BVideoPoint poi2 = new BVideoPoint();
            poi2.Location = new LatLngPoint(116.549722, 39.972907);
            poi2.Index = 2;
            poi2.IsOnline = false;
            devices.Add(poi2);
            if (BuddyBMapControl != null)
            {
                BuddyBMapControl.AddVideoPlaces(devices);
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

        private void buttonItemAddGroup_Click(object sender, EventArgs e)
        {
            Node node = new Node("新建分组" + sCurrentNewGroupIndex);
            node.ImageIndex = 5;
            node.ImageExpandedIndex = 4;
            advTreeDeviceList.Nodes.Add(node);
            sCurrentNewGroupIndex++;
            logger.Info("添加分组“" + node.Text + "”。");
        }

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

        public void updateDeviceGroup()
        {

        }


        #endregion

    }
}
