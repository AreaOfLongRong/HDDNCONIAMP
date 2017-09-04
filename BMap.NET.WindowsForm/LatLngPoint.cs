using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMap.NET.WindowsForm
{
    /// <summary>
    /// 经纬度坐标
    /// </summary>
    public class LatLngPoint
    {
        public double Lng
        {
            set;
            get;
        }
        public double Lat
        {
            set;
            get;
        }
        public LatLngPoint(double _lng, double _lat)
        {
            Lng = _lng;
            Lat = _lat;
        }

        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("经度：{0:F4}°\n纬度：{1:F4}°", Lng, Lat);
        }
    }
}
