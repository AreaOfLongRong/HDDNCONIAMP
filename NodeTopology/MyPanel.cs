using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NodeTopology
{
    public class MyPanel:Panel
    {
        public MyPanel()
        {
          this.SetStyle(ControlStyles.AllPaintingInWmPaint | //不擦除背景 ,减少闪烁
                          ControlStyles.OptimizedDoubleBuffer | //双缓冲
                          ControlStyles.UserPaint , //使用自定义的重绘事件,减少闪烁
                          true);
          this.UpdateStyles();
        }
    }
}
