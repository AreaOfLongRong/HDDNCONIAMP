﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// 初始化面板字典，键为面板，值为是否可用
        /// </summary>
        void InitPanelDictionary();

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

        /// <summary>
        /// 获取下一个可用的面板
        /// </summary>
        /// <returns>可用的面板</returns>
        Panel GetNextAvailablePanel();

        /// <summary>
        /// 获取全屏面板
        /// </summary>
        /// <returns>全屏面板</returns>
        Panel GetFullScreenPanel();

        /// <summary>
        /// 绑定面板及其进程
        /// </summary>
        /// <param name="panel">面板</param>
        /// <param name="process">面板进程</param>
        void BindPanelProcess(Panel panel, Process process);
    }
}
