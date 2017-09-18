using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDDNCONIAMP.UI.AudioVideoProcess
{
    public partial class UCGrid1 : UserControl, IGrid
    {
        public UCGrid1()
        {
            InitializeComponent();
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
        /// <returns>可用的面板</returns>
        public Panel GetNextAvailablePanel()
        {
            return panel1;
        }


        /// <summary>
        /// 获取指定屏幕坐标位置处的Panel
        /// </summary>
        /// <param name="point">屏幕坐标位置</param>
        /// <returns>网格面板</returns>
        public Panel GetPanelAtPoint(Point point)
        {
            if (this.DisplayRectangle.Contains(point))
            {
                return panel1;
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
            return panel1;
        }

        /// <summary>
        /// 初始化面板字典，键为面板，值为是否可用
        /// </summary>
        /// <returns></returns>
        public void InitPanelDictionary()
        {
            throw new NotImplementedException();
        }
    }
}
