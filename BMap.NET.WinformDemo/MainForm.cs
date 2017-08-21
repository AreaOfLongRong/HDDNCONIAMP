using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using BMap.NET.HTTPService;

namespace BMap.NET.WinformDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 点击搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            bPlaceBox1.StartSearch();
        }

        private void buttonDownloadJJJData_Click(object sender, EventArgs e)
        {
            FormDownloadBDMap fdbd = new FormDownloadBDMap();
            fdbd.ShowDialog();
        }
    }
}
