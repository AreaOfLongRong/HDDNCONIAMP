using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace HDDNCONIAMP
{
    public partial class FormMain : Office2007Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_Load(object sender, EventArgs e)
        {
            //启动计时器，更新主界面中的当前系统时间
            timerUpdateTime.Start();
        }

        /// <summary>
        /// 主界面关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //停止计时器
            timerUpdateTime.Stop();
        }

        /// <summary>
        /// 退出应用程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBoxExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 更新主界面中的当前时间信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerUpdateTime_Tick(object sender, EventArgs e)
        {
            labelXSystemTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
