using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HDDNCONIAMP.DB.Model;

namespace HDDNCONIAMP.Mesh
{
    /// <summary>
    /// Mesh设备操作接口类。
    /// 1、程序启动时，启动新的任务对网络内Mesh设备监测；
    /// 2、对每一个Mesh设备启动新的任务进行状态监控、视频信息获取等操作。
    /// </summary>
    public class MeshOperator
    {

        #region 委托事件

        /// <summary>
        /// Mesh设备状态更新委托
        /// </summary>
        /// <param name="groups">Mesh设备分组列表</param>
        public delegate void OnMeshDeviceStateUpdate(List<MeshDeviceGroup> groups);

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public MeshOperator()
        {

        }

    }
}
