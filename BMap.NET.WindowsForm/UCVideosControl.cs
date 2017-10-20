using System;
using System.Windows.Forms;
using BMap.NET.WindowsForm.BMapElements;
using BMap.NET.WindowsForm.Video;

namespace BMap.NET.WindowsForm
{
    public partial class UCVideosControl : UserControl
    {

        #region 事件

        /// <summary>
        /// 打开视频委托
        /// </summary>
        /// <param name="p">地图上的Mesh设备点</param>
        public delegate void UCVCOpenVideoDeleget(BMeshPoint p);

        /// <summary>
        /// 打开视频事件
        /// </summary>
        public event UCVCOpenVideoDeleget OnUCVCOpenVideo;

        #endregion

        public UCVideosControl()
        {
            InitializeComponent();
            buttonX265Video.Image = Properties.BMap.video_camera_shoulder_64;
            //buttonXHK.Image = Properties.BMap.video_camera_ball_64;
        }

        /// <summary>
        /// 获取或设置与之绑定的地图上的Mesh设备点
        /// </summary>
        public BMeshPoint UCBMeshPoint { get; set; }

        /// <summary>
        /// 获取或设置265模块的ID
        /// </summary>
        public string Model265ID { get; set; }

        /// <summary>
        /// 获取或设置视频服务器的IP
        /// </summary>
        public string VideoServerIP { get; set; }

        /// <summary>
        /// 获取或设置视频服务器的用户名
        /// </summary>
        public string VideoServerUserName { get; set; }

        /// <summary>
        /// 获取或设置视频服务器的密码
        /// </summary>
        public string VideoServerPassword { get; set; }

        /// <summary>
        /// 打开265视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonX265Video_Click(object sender, EventArgs e)
        {
            if (UCBMeshPoint != null)
                OnUCVCOpenVideo?.Invoke(UCBMeshPoint);
        }

        /// <summary>
        /// 打开海康球机视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXHK_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
