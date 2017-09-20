using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BMap.NET.WindowsForm.Video;

namespace BMap.NET.WindowsForm
{
    public partial class UCVideosControl : UserControl
    {
        public UCVideosControl()
        {
            InitializeComponent();
            buttonX265Video.Image = Properties.BMap.video_camera_shoulder_64;
            //buttonXHK.Image = Properties.BMap.video_camera_ball_64;
        }

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
            VideoInject vi = new VideoInject(VideoServerIP, VideoServerUserName, VideoServerPassword);
            vi.injectWindow(Model265ID);
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
