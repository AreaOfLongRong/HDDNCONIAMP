using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HDDNCONIAMP.UI.Common;
using BMap.NET.WindowsForm.Video;
using HDDNCONIAMP.Utils;

namespace HDDNCONIAMP.UI.AudioVideoProcess
{
    /// <summary>
    /// 音视频综合处理控件
    /// </summary>
    public partial class UCAudioVideoProcess : UserControl
    {


        private UCDeviceList ucDeviceListMain;

        private UCMeshList ucMeshDeviceListMain;

        public UCAudioVideoProcess(FormMain main)
        {
            InitializeComponent();

            //initUCDeviceList(main);

            initUCMeshList(main);
        }

        /// <summary>
        /// 初始化Mesh设备列表
        /// </summary>
        /// <param name="main"></param>
        private void initUCMeshList(FormMain main)
        {
            ucMeshDeviceListMain = new UCMeshList(main);
            ucMeshDeviceListMain.BuddyBMapControl = null;
            ucMeshDeviceListMain.BuddyGrid = ucGrid9Main;
            ucMeshDeviceListMain.MDManage = main.MDManage;
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
            ucMeshDeviceListMain.StartScanMeshDevice();

        }

        /// <summary>
        /// 初始化GPS设备列表，被废弃
        /// </summary>
        /// <param name="main"></param>
        private void initUCDeviceList(FormMain main)
        {
            this.ucDeviceListMain = new HDDNCONIAMP.UI.Common.UCDeviceList(main);
            this.ucDeviceListMain.BuddyBMapControl = null;
            this.ucDeviceListMain.BuddyGrid = ucGrid9Main;
            this.ucDeviceListMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDeviceListMain.Location = new System.Drawing.Point(0, 0);
            this.ucDeviceListMain.Name = "ucDeviceList1";
            this.ucDeviceListMain.Size = new System.Drawing.Size(150, 420);
            this.ucDeviceListMain.TabIndex = 0;
            // 
            // collapsibleSplitContainer1.Panel1
            // 
            this.collapsibleSplitContainer1.Panel1.Controls.Add(this.ucDeviceListMain);
            this.collapsibleSplitContainer1.Panel1MinSize = 5;
        }

        private void ucGrid9Main_Load(object sender, EventArgs e)
        {
            //VideoInject vi = new VideoInject();
            //vi.injectPanel(this.ucGrid9Main.GetNextAvailablePanel(), this.ucGrid9Main.GetMainPanel(), "26908", "0", PathUtils.VIDEO_DATA_DEFAULT_PATH);
            //VideoInject vi2 = new VideoInject();
            //vi2.injectPanel(this.ucGrid9Main.GetNextAvailablePanel(), this.ucGrid9Main.GetMainPanel(), "27022", "0", PathUtils.VIDEO_DATA_DEFAULT_PATH);
        }
    }
}
