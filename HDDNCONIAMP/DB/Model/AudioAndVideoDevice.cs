﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDDNCONIAMP.DB.Model
{
    /// <summary>
    /// 视频设备对象
    /// </summary>
    public class AudioAndVideoDevice
    {

        /// <summary>
        /// 获取或设置视频设备名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 设备别名
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 获取或设置视频设备所在纬度
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// 获取或设置视频设备所在经度
        /// </summary>
        public double Lon { get; set; }

    }
}
