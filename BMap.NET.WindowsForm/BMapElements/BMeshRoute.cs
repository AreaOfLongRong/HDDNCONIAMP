using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BMap.NET.WindowsForm.BMapElements
{
    /// <summary>
    /// 设备路线
    /// </summary>
    public class BMeshRoute : BMapElement
    {

        /// <summary>
        /// 获取或设置设备名称
        /// </summary>
        public string DeviceName
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置设备位置列表
        /// </summary>
        public List<LatLngPoint> DeviceLocationList
        {
            get;
            set;
        }

        /// <summary>
        /// 路径颜色
        /// </summary>
        public Color RouteColor
        {
            get;
            set;
        }

        /// <summary>
        /// 路径颜色数组
        /// </summary>
        private static Color[] _RouteColors = {
            Color.Red, Color.Orange,
            Color.Yellow, Color.Green,
            Color.Cyan, Color.Blue };

        /// <summary>
        /// 当前路径颜色索引
        /// </summary>
        private static int _CurrentColorIndex = -1;

        /// <summary>
        /// 设备路线绘制
        /// </summary>
        /// <param name="g"></param>
        /// <param name="center"></param>
        /// <param name="zoom"></param>
        /// <param name="screen_size"></param>
        public override void Draw(Graphics g, LatLngPoint center, int zoom, Size screen_size)
        {
            LatLngPoint first, second;
            Point s_first = new Point(), s_second = new Point();

            using (Pen p = new Pen(RouteColor, 2))
            {
                p.StartCap = System.Drawing.Drawing2D.LineCap.Round; //连接圆滑
                p.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                p.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                for (int i = DeviceLocationList.Count - 1; i > 0; --i)
                {
                    first = DeviceLocationList[i - 1];
                    second = DeviceLocationList[i];
                    s_first = MapHelper.GetScreenLocationByLatLng(first, center, zoom, screen_size);
                    s_second = MapHelper.GetScreenLocationByLatLng(second, center, zoom, screen_size);
                    //if (new Rectangle(new Point(0, 0), screen_size).Contains(s_first) || new Rectangle(new Point(0, 0), screen_size).Contains(s_second)) //在屏幕范围内
                    g.DrawLine(p, s_first, s_second);
                }
            }
        }

        /// <summary>
        /// 获取下一个可用的颜色
        /// </summary>
        /// <returns></returns>
        public Color getNextColor()
        {
            _CurrentColorIndex++;
            if (_CurrentColorIndex > (_RouteColors.Length - 1))
                _CurrentColorIndex = 0;
            Console.WriteLine("当前颜色：" + _RouteColors[_CurrentColorIndex]);
            return _RouteColors[_CurrentColorIndex];
        }
    }
}
