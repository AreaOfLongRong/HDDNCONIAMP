using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDDNCONIAMP.Network
{
    /// <summary>
    /// GPS信息
    /// </summary>
    public struct GPSInfo
    {

        /// <summary>
        /// 获取或设置GPS的ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 获取或设置GPS信号的时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 获取或设置GPS纬度
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// 获取或设置GPS经度
        /// </summary>
        public double Lon { get; set; }

    }
}
