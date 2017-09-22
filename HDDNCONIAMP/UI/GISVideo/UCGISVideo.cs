using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HDDNCONIAMP.UI.Common;

namespace HDDNCONIAMP.UI.GISVideo
{
    public partial class UCGISVideo : UserControl
    {
        private UCDeviceList ucDeviceListmain;

        private UCMeshList2 ucMeshDeviceListMain;

        public UCGISVideo(FormMain main)
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
            ucMeshDeviceListMain = new UCMeshList2(main);
            ucMeshDeviceListMain.BuddyBMapControl = this.bMapControl2Main;
            ucMeshDeviceListMain.Dock = DockStyle.Fill;
            ucMeshDeviceListMain.Location = new Point(0, 0);
            ucMeshDeviceListMain.Name = "ucMeshListmain";
            ucMeshDeviceListMain.Size = new Size(150, 439);
            ucMeshDeviceListMain.TabIndex = 0;
            // 
            // collapsibleSplitContainerMain.Panel1
            // 
            this.collapsibleSplitContainerMain.Panel1.Controls.Add(ucMeshDeviceListMain);
            this.collapsibleSplitContainerMain.Panel1MinSize = 5;

        }

        /// <summary>
        /// 初始化GPS设备列表，被废弃
        /// </summary>
        /// <param name="main"></param>
        private void initUCDeviceList(FormMain main)
        {
            ucDeviceListmain = new HDDNCONIAMP.UI.Common.UCDeviceList(main);
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
