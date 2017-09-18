using System;

namespace HDDNCONIAMP.Mesh
{
    /// <summary>
    /// Mesh设备相互关系
    /// </summary>
    [Serializable]
    public class MeshRelation
    {

        /// <summary>
        /// 获取或设置本地节点
        /// </summary>
        public MeshNode LocalNode { get; set; }

        /// <summary>
        /// 获取或设置本地端口
        /// </summary>
        public int LocalPort { get; set; }

        /// <summary>
        /// 获取或设置远端节点
        /// </summary>
        public MeshNode RemoteNode { get; set; }

        /// <summary>
        /// 获取或设置远端端口
        /// </summary>
        public int RemotePort { get; set; }

        /// <summary>
        /// 获取或设置TX Speed (Mbps) 发送速度
        /// </summary>
        public int TxSpeed { get; set; }

        /// <summary>
        /// 获取或设置TX SNR (dB) 发送功率
        /// </summary>
        public int TxSnr { get; set; }

        /// <summary>
        /// 获取或设置RX Speed (Mbps) 接收速度
        /// </summary>
        public int RxSpeed { get; set; }

        /// <summary>
        /// 获取或设置RX SNR (dB) 接收功率
        /// </summary>
        public int RxSnr { get; set; }

        /// <summary>
        /// 获取或设置发现时间
        /// </summary>
        public int FindTimes { get; set; }

        public MeshRelation(MeshNode fromNode, MeshNode toNode)
        {
            this.LocalNode = fromNode;
            this.RemoteNode = toNode;
        }

    }
}
