using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMap.NET.WindowsForm.Utils
{
    public class GPS2BD09
    {
        /*
         WGS-84：是国际标准，GPS坐标（Google Earth使用、或者GPS模块）
         GCJ-02：中国坐标偏移标准，Google Map、高德、腾讯使用
         BD-09：百度坐标偏移标准，Baidu Map使用

        代码参考：http://bbs.lbsyun.baidu.com/forum.php?mod=viewthread&tid=10923
         */

        static double pi = 3.14159265358979324;
        static double a = 6378245.0;
        static double ee = 0.00669342162296594323;
        public const double x_pi = 3.14159265358979324 * 3000.0 / 180.0;

        /// <summary>
        /// gps坐标转换成百度坐标，小数点前4位为准确坐标
        /// </summary>
        /// <param name="lat">纬度</param>
        /// <param name="lon">经度</param>
        /// <returns></returns>
        public static double[] wgs2bd(double lat, double lon)
        {
            double[] dwgs2gcj = wgs2gcj(lat, lon);
            double[] dgcj2bd = gcj2bd(dwgs2gcj[0], dwgs2gcj[1]);
            return dgcj2bd;
        }

        /// <summary>
        /// GCJ-02坐标转换为BD-09坐标
        /// </summary>
        /// <param name="lat">纬度（十进制）</param>
        /// <param name="lon">经度（十进制）</param>
        /// <returns>一维数组，第0个元素为BD-09坐标纬度，第1个元素为BD-09坐标经度</returns>
        public static double[] gcj2bd(double lat, double lon)
        {
            double x = lon, y = lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            double bd_lon = z * Math.Cos(theta) + 0.0065;
            double bd_lat = z * Math.Sin(theta) + 0.006;
            return new double[] { bd_lat, bd_lon };
        }

        /// <summary>
        /// BD-09坐标转换为GCJ-02坐标
        /// </summary>
        /// <param name="lat">纬度（十进制）</param>
        /// <param name="lon">经度（十进制）</param>
        /// <returns>一维数组，第0个元素为GCJ-02坐标纬度，第1个元素为GCJ-02坐标经度</returns>
        public static double[] bd2gcj(double lat, double lon)
        {
            double x = lon - 0.0065, y = lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            double gg_lon = z * Math.Cos(theta);
            double gg_lat = z * Math.Sin(theta);
            return new double[] { gg_lat, gg_lon };
        }

        /// <summary>
        /// WGS84坐标转GCJ-02坐标
        /// </summary>
        /// <param name="lat">纬度（十进制）</param>
        /// <param name="lon">经度（十进制）</param>
        /// <returns>一维数组，第0个元素为GCJ坐标纬度，第1个元素为GCJ坐标经度</returns>
        public static double[] wgs2gcj(double lat, double lon)
        {
            double dLat = transformLat(lon - 105.0, lat - 35.0);
            double dLon = transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            double mgLat = lat + dLat;
            double mgLon = lon + dLon;
            double[] loc = { mgLat, mgLon };
            return loc;
        }

        /// <summary>
        /// 转换纬度
        /// </summary>
        /// <param name="lat">纬度（十进制）</param>
        /// <param name="lon">经度（十进制）</param>
        /// <returns>转换后的纬度值</returns>
        private static double transformLat(double lat, double lon)
        {
            double ret = -100.0 + 2.0 * lat + 3.0 * lon + 0.2 * lon * lon + 0.1 * lat * lon + 0.2 * Math.Sqrt(Math.Abs(lat));
            ret += (20.0 * Math.Sin(6.0 * lat * pi) + 20.0 * Math.Sin(2.0 * lat * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lon * pi) + 40.0 * Math.Sin(lon / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(lon / 12.0 * pi) + 320 * Math.Sin(lon * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        /// <summary>
        /// 转换经度
        /// </summary>
        /// <param name="lat">纬度（十进制）</param>
        /// <param name="lon">经度（十进制）</param>
        /// <returns>转换后的经度值</returns>
        private static double transformLon(double lat, double lon)
        {
            double ret = 300.0 + lat + 2.0 * lon + 0.1 * lat * lat + 0.1 * lat * lon + 0.1 * Math.Sqrt(Math.Abs(lat));
            ret += (20.0 * Math.Sin(6.0 * lat * pi) + 20.0 * Math.Sin(2.0 * lat * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lat * pi) + 40.0 * Math.Sin(lat / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(lat / 12.0 * pi) + 300.0 * Math.Sin(lat / 30.0 * pi)) * 2.0 / 3.0;
            return ret;
        }
    }
}
