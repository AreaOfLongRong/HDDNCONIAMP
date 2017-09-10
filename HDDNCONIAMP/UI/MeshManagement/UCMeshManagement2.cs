using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using HDDNCONIAMP.Utils;
using log4net;

namespace HDDNCONIAMP.UI.MeshManagement
{
    public partial class UCMeshManagement2 : UserControl
    {

        #region 私有变量

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(UCMeshManagement2));
        
        #endregion

        public UCMeshManagement2()
        {
            InitializeComponent();
            setTableLayoutPanelDoubleBufferd();
        }

        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCMeshManagement2_Load(object sender, EventArgs e)
        {
            initMeshBaseParamConfit();
        }

        #region 网络拓扑事件处理



        #endregion

        #region 预案管理事件处理



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
            if (NetUtils.ConfigNetworkCardIPAddress(comboBoxExLocalhostNetwordCard.SelectedItem.ToString(), ipAddressInputLocal.Text))
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
            tableLayoutPanelMeshTopology.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshTopology, true, null);
            tableLayoutPanelMeshParameters.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMeshParameters, true, null);
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

    }
}
