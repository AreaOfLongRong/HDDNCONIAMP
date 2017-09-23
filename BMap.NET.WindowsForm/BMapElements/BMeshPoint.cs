using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace BMap.NET.WindowsForm.BMapElements
{
    /// <summary>
    /// 地图上的Mesh设备点
    /// </summary>
    public class BMeshPoint : BMapElement
    {

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 设备别名
        /// </summary>
        public string Alias
        {
            get;
            set;
        }
        /// <summary>
        /// 设备IPV4地址
        /// </summary>
        public string IPV4 {
            get;
            set;
        }
        /// <summary>
        /// 265模块ID
        /// </summary>
        public string Model265ID
        {
            get;
            set;
        }
        /// <summary>
        /// 设备功率
        /// </summary>
        /// 
        public decimal Power { get; set; }

        /// <summary>
        /// 设备频率，字符串格式
        /// </summary>
        public decimal Frequency { get; set; }

        /// <summary>
        /// 设备带宽，字符串格式
        /// </summary>
        public decimal BandWidth { get; set; }
        /// <summary>
        /// 设备电压，字符串格式
        /// </summary>
        public decimal Battery { get; set; }
        /// <summary>
        /// 位置坐标
        /// </summary>
        public LatLngPoint Location
        {
            get;
            set;
        }
        /// <summary>
        /// POI的数据源
        /// </summary>
        public JObject DataSource
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置设备是否在线
        /// </summary>
        public bool IsOnline
        {
            get;
            set;
        }
        /// <summary>
        /// 当前POI是否被选中
        /// </summary>
        public bool Selected
        {
            get;
            set;
        }
        private Rectangle _rect;
        /// <summary>
        /// POI在屏幕中的范围
        /// </summary>
        public Rectangle Rect
        {
            get
            {
                return _rect;
            }
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="g">画笔</param>
        /// <param name="center">中心点</param>
        /// <param name="zoom">缩放层级</param>
        /// <param name="screen_size">屏幕尺寸</param>
        public override void Draw(Graphics g, LatLngPoint center, int zoom, Size screen_size)
        {
            Point p = MapHelper.GetScreenLocationByLatLng(Location, center, zoom, screen_size);  //屏幕坐标

            Bitmap b;
            if (Selected)
            {
                b = IsOnline ? Properties.BMap.mesh_person_online_64 : Properties.BMap.mesh_person_offline_64;
            }
            else
            {
                b = IsOnline ? Properties.BMap.mesh_person_online_32 : Properties.BMap.mesh_person_offline_32;
            }
            g.DrawImage(b, new Rectangle(p.X - b.Width / 2, p.Y - b.Height, b.Width, b.Height));
            _rect = new Rectangle(p.X - b.Width / 2, p.Y - b.Height, b.Width, b.Height);
        }

        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("分   组：" + (GroupName == null ? "默认分组" : GroupName));
            sb.AppendLine("别   名：" + (Alias == null ? IPV4 : Alias));
            sb.AppendLine("IP 地址：" + IPV4);
            sb.AppendLine("功   率：" + Power + "W");
            sb.AppendLine("频   率：" + Frequency + "MHz");
            sb.AppendLine("带   宽：" + BandWidth + "Mb");
            sb.AppendLine("电   压：" + Battery + "V");
            sb.AppendLine(Location.ToString());
            sb.AppendLine("状   态：" + (IsOnline ? "在线" : "离线"));
            return sb.ToString();
        }
    }
}
