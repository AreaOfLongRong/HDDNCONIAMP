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

        /// <summary>
        /// Mesh设备列表控件
        /// </summary>
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
            return 300;
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
            panel1.Controls.Add(ucMeshDeviceListMain);

        }
        
        /// <summary>
        /// 重载Windows消息处理
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            int WM_MOUSEWHEEL = 0x020A;
            if (m.Msg == WM_MOUSEWHEEL)
            {//禁用鼠标滚轮事件的处理
                return;
            }
            base.WndProc(ref m);
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

    }
}
