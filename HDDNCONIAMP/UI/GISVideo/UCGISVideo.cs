using System.Drawing;
using System.Windows.Forms;
using HDDNCONIAMP.UI.Common;
using HDDNCONIAMP.Utils;

namespace HDDNCONIAMP.UI.GISVideo
{
    public partial class UCGISVideo : UserControl
    {
        private UCMeshList2 ucMeshDeviceListMain;

        public UCGISVideo(FormMain main)
        {
            InitializeComponent();

            initUCMeshList(main);

            this.bMapControl2Main.VideoServerIP = 
                main.AllApplicationSetting[ApplicationSettingKey.VideoServerIPV4];
            this.bMapControl2Main.VideoServerUserName =
                main.AllApplicationSetting[ApplicationSettingKey.VideoServerUserName];
            this.bMapControl2Main.VideoServerPassword =
                main.AllApplicationSetting[ApplicationSettingKey.VideoServerPassword];
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
        
    }
}
