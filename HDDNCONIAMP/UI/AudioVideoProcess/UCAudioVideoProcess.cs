using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDDNCONIAMP.UI.AudioVideoProcess
{
    /// <summary>
    /// 音视频综合处理控件
    /// </summary>
    public partial class UCAudioVideoProcess : UserControl
    {


        private Common.UCDeviceList ucDeviceListMain;

        public UCAudioVideoProcess(FormMain main)
        {
            InitializeComponent();

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
    }
}
