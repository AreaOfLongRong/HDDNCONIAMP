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
        public UCGrid9()
        {
            InitializeComponent();

            setTableLayoutPanelDoubleBufferd();
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
        /// 获取指定屏幕坐标位置处的Panel
        /// </summary>
        /// <param name="point">屏幕坐标位置</param>
        /// <returns>网格面板</returns>
        public Panel GetPanelAtPoint(Point point)
        {
            if (panel1.DisplayRectangle.Contains(point))
            {
                return panel1;
            }
            else if (panel2.DisplayRectangle.Contains(point))
            {
                return panel2;
            }
            else if (panel3.DisplayRectangle.Contains(point))
            {
                return panel3;
            }
            else if (panel4.DisplayRectangle.Contains(point))
            {
                return panel4;
            }
            else if (panel5.DisplayRectangle.Contains(point))
            {
                return panel5;
            }
            else if (panel6.DisplayRectangle.Contains(point))
            {
                return panel6;
            }
            else if (panel6.DisplayRectangle.Contains(point))
            {
                return panel7;
            }
            else if (panel8.DisplayRectangle.Contains(point))
            {
                return panel8;
            }
            else if (panel9.DisplayRectangle.Contains(point))
            {
                return panel9;
            }
            else
            {
                return panel1;
            }
        }

        /// <summary>
        /// 获取指定索引处的面板
        /// </summary>
        /// <param name="index">面板索引，从1开始</param>
        /// <returns>网格面板</returns>
        public Panel GetPanelByIndex(int index)
        {
            switch (index)
            {
                default:
                case 1:
                    return panel1;
                case 2:
                    return panel2;
                case 3:
                    return panel3;
                case 4:
                    return panel4;
                case 5:
                    return panel5;
                case 6:
                    return panel6;
                case 7:
                    return panel7;
                case 8:
                    return panel8;
                case 9:
                    return panel9;
            }
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
    }
}
