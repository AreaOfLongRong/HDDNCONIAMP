using System;
using System.Drawing;
using System.Windows.Forms;
using HDDNCONIAMP.UI.Common;

namespace HDDNCONIAMP.UI.AudioVideoProcess
{
    /// <summary>
    /// 音视频综合处理控件
    /// </summary>
    public partial class UCAudioVideoProcess : UserControl
    {

        
        private UCMeshList2 ucMeshDeviceListMain;

        public UCAudioVideoProcess(FormMain main)
        {
            InitializeComponent();
            
            initUCMeshList(main);
        }

        /// <summary>
        /// 获取视频网格所在位置的X值
        /// </summary>
        /// <returns></returns>
        public int GetGridLocationX()
        {
            return collapsibleSplitContainer1.SplitterDistance + collapsibleSplitContainer1.SplitterWidth;
        }

        /// <summary>
        /// 初始化Mesh设备列表
        /// </summary>
        /// <param name="main"></param>
        private void initUCMeshList(FormMain main)
        {
            ucMeshDeviceListMain = new UCMeshList2(main);
            ucMeshDeviceListMain.BuddyBMapControl = null;
            ucMeshDeviceListMain.BuddyGrid = ucGrid9Main;
            ucMeshDeviceListMain.Dock = DockStyle.Fill;
            ucMeshDeviceListMain.Location = new Point(0, 0);
            ucMeshDeviceListMain.Name = "ucMeshListmain";
            ucMeshDeviceListMain.Size = new Size(150, 439);
            ucMeshDeviceListMain.TabIndex = 0;
            // 
            // collapsibleSplitContainerMain.Panel1
            // 
            this.collapsibleSplitContainer1.Panel1.Controls.Add(ucMeshDeviceListMain);
            this.collapsibleSplitContainer1.Panel1MinSize = 5;

        }
        
        private void ucGrid9Main_Load(object sender, EventArgs e)
        {
        }
    }
}
