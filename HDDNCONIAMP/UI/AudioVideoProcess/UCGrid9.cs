using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.AdvTree;

namespace HDDNCONIAMP.UI.AudioVideoProcess
{
    public partial class UCGrid9 : UserControl, IGrid
    {

        /// <summary>
        /// 面板字典
        /// </summary>
        private Dictionary<Panel, bool> mPanelDictionary;

        public UCGrid9()
        {
            InitializeComponent();

            setTableLayoutPanelDoubleBufferd();
            InitPanelDictionary();
        }


        /// <summary>
        /// 获取网格数量
        /// </summary>
        public int GridCount
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// 获取下一个可用的面板
        /// </summary>
        /// <returns>下一个可用的面板</returns>
        public Panel GetNextAvailablePanel()
        {
            foreach(KeyValuePair<Panel, bool> kvp in mPanelDictionary)
            {
                if(kvp.Value == false)
                {
                    mPanelDictionary[kvp.Key] = true;
                    return kvp.Key;
                }
            }
            return null;
        }


        /// <summary>
        /// 获取指定屏幕坐标位置处的Panel
        /// </summary>
        /// <param name="point">屏幕坐标位置</param>
        /// <returns>网格面板</returns>
        public Panel GetPanelAtPoint(Point point)
        {
            return null;
        }

        /// <summary>
        /// 获取指定索引处的面板
        /// </summary>
        /// <param name="index">面板索引，从1开始</param>
        /// <returns>网格面板</returns>
        public Panel GetPanelByIndex(int index)
        {
            return mPanelDictionary.ElementAt(index).Key;
        }

        /// <summary>
        /// 初始化面板字典
        /// </summary>
        public void InitPanelDictionary()
        {
            mPanelDictionary = new Dictionary<Panel, bool>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    PictureBox pb = new PictureBox();
                    pb.Dock = DockStyle.Fill;
                    pb.Image = Properties.Resources.video_no_signal_512;
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;

                    Panel panel = new Panel();
                    panel.SuspendLayout();
                    panel.Dock = DockStyle.Fill;
                    panel.Controls.Add(pb);
                    panel.ResumeLayout(false);
                    this.tableLayoutPanelMain.Controls.Add(panel, j, i);                    
                    this.mPanelDictionary.Add(panel, false);
                }
            }
            this.tableLayoutPanelMain.ResumeLayout(false);
        }

        /// <summary>
        /// 获取包含九宫格的主面板
        /// </summary>
        /// <returns></returns>
        public Panel GetMainPanel()
        {
            return panelMain;
        }

        /// <summary>
        /// 启用TableLayoutPanel双缓冲，防止界面闪烁
        /// </summary>
        private void setTableLayoutPanelDoubleBufferd()
        {
            tableLayoutPanelMain.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanelMain, true, null);
        }

        private void tableLayoutPanelMain_DragDrop(object sender, DragEventArgs e)
        {
            Node node = (Node)e.Data;
            if(node != null)
            {
                Panel targetPanel = GetPanelAtPoint(new Point(e.X, e.Y));
                if (targetPanel != null)
                {
                    TextBox temp = new TextBox();
                    temp.Text = node.Text;
                    temp.Dock = DockStyle.Fill;
                    targetPanel.Controls.Add(temp);
                }
            }
        }

        /// <summary>
        /// 获取全屏面板
        /// </summary>
        /// <returns>全屏面板</returns>
        public Panel GetFullScreenPanel()
        {
            return panelMain;
        }
    }
}
