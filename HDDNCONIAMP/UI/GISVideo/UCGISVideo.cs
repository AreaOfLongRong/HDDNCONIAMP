using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDDNCONIAMP.UI.GISVideo
{
    public partial class UCGISVideo : UserControl
    {
        private common.UCDeviceList ucDeviceListmain;

        public UCGISVideo(FormMain main)
        {
            InitializeComponent();

            ucDeviceListmain = new HDDNCONIAMP.UI.common.UCDeviceList(main);
            ucDeviceListmain.BuddyBMapControl = this.bMapControl2Main;
            ucDeviceListmain.Dock = System.Windows.Forms.DockStyle.Fill;
            ucDeviceListmain.Location = new System.Drawing.Point(0, 0);
            ucDeviceListmain.Name = "ucDeviceListmain";
            ucDeviceListmain.Size = new System.Drawing.Size(150, 439);
            ucDeviceListmain.TabIndex = 0;
            // 
            // collapsibleSplitContainerMain.Panel1
            // 
            this.collapsibleSplitContainerMain.Panel1.Controls.Add(this.ucDeviceListmain);
            this.collapsibleSplitContainerMain.Panel1MinSize = 5;
        }
    }
}
