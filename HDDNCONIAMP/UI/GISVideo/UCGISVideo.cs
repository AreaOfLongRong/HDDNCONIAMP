﻿using System.Drawing;
using System.Windows.Forms;
using HDDNCONIAMP.DB.Model;
using HDDNCONIAMP.UI.Common;
using HDDNCONIAMP.Utils;

namespace HDDNCONIAMP.UI.GISVideo
{
    public partial class UCGISVideo : UserControl
    {
        /// <summary>
        /// Mesh设备列表控件
        /// </summary>
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
            ucMeshDeviceListMain.RegisterEvent();
            // 
            // collapsibleSplitContainerMain.Panel1
            // 
            this.collapsibleSplitContainerMain.Panel1.Controls.Add(ucMeshDeviceListMain);
            this.collapsibleSplitContainerMain.Panel1MinSize = 5;

        }
        
        /// <summary>
        /// 更新指定Mesh设备列表
        /// </summary>
        /// <param name="meshIp"></param>
        /// <param name="status"></param>
        public void UpdateMeshStatus(string meshIp, string status)
        {
            ucMeshDeviceListMain.UpdateMeshStatus(meshIp, status);
        }


        /// <summary>
        /// 更新Mesh设备信息
        /// </summary>
        /// <param name="mdi">Mesh设备信息</param>
        public void UpdateMeshDeviceInfo(MeshDeviceInfo mdi)
        {
            ucMeshDeviceListMain.UpdateMeshDeviceInfo(mdi);
        }

    }
}
