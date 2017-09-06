using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HDDNCONIAMP.UI.AudioVideoProcess
{
    /// <summary>
    /// 视频网格划分接口
    /// </summary>
    public interface IGrid
    {
        /// <summary>
        /// 获取网格数量
        /// </summary>
        int GridCount { get; }

        /// <summary>
        /// 获取指定索引处的面板
        /// </summary>
        /// <param name="index">面板索引，从1开始</param>
        /// <returns>网格面板</returns>
        Panel GetPanelByIndex(int index);

        /// <summary>
        /// 获取指定屏幕坐标位置处的Panel
        /// </summary>
        /// <param name="point">屏幕坐标位置</param>
        /// <returns>网格面板</returns>
        Panel GetPanelAtPoint(Point point);

    }
}
