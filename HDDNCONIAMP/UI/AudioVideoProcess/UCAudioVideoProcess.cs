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


        private common.UCDeviceList ucDeviceList1;

        public UCAudioVideoProcess(FormMain main)
        {
            InitializeComponent();

            this.ucDeviceList1 = new HDDNCONIAMP.UI.common.UCDeviceList(main);
            this.ucDeviceList1.BuddyBMapControl = null;
            this.ucDeviceList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDeviceList1.Location = new System.Drawing.Point(0, 0);
            this.ucDeviceList1.Name = "ucDeviceList1";
            this.ucDeviceList1.Size = new System.Drawing.Size(150, 420);
            this.ucDeviceList1.TabIndex = 0;
            // 
            // collapsibleSplitContainer1.Panel1
            // 
            this.collapsibleSplitContainer1.Panel1.Controls.Add(this.ucDeviceList1);
            this.collapsibleSplitContainer1.Panel1MinSize = 5;
        }
    }
}
