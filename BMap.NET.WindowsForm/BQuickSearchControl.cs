using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BMap.NET.WindowsForm
{
    /// <summary>
    /// 快速搜索控件
    /// </summary>
    partial class BQuickSearchControl : UserControl
    {
        public event QuickSearchEventHandler QuickSearch;
        public BQuickSearchControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Visible = false;
        }
        /// <summary>
        /// 点击搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel2_Click(object sender, EventArgs e)
        {
            string search_name = (sender as LinkLabel).Text;
            if (QuickSearch != null)
            {
                QuickSearch(search_name);
            }
        }
    }
    /// <summary>
    /// 表示处理快速搜索事件的方法
    /// </summary>
    /// <param name="searchName"></param>
    delegate void QuickSearchEventHandler(string searchName);
}
