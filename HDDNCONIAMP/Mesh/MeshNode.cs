using System;

namespace HDDNCONIAMP.Mesh
{
    /// <summary>
    /// Mesh设备节点
    /// </summary>
    [Serializable]
    public class MeshNode
    {

        /// <summary>
        /// 获取或设置是否为根节点
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// 获取或设置是否有子节点
        /// </summary>
        public bool HasChild { get; set; }

        /// <summary>
        /// 获取或设置MAC地址
        /// </summary>
        public string MACAddress { get; set; }

        /// <summary>
        /// 获取或设置IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 获取或设置输入电压V
        /// </summary>
        public double Battery { get; set; }

        /// <summary>
        /// 获取或设置频率分布MHz
        /// </summary>
        public double Frequency { get; set; }

        /// <summary>
        /// 获取或设置主发送功能率dBm （不涉及点对点之间的通讯，点对点之间见 TX Speed 和 TX SNR）
        /// </summary>
        public double TxPower { get; set; }

        /// <summary>
        /// 获取或设置输出带宽
        /// </summary>
        public double BandWidth { get; set; }

        /// <summary>
        /// 初始化一个根节点Mesh节点
        /// </summary>
        public MeshNode()
        {
            IsRoot = true;
        }

        /// <summary>
        /// 根据指定的MAC地址初始化一个Mesh叶子节点
        /// </summary>
        /// <param name="initMacAddress"></param>
        public MeshNode(string initMacAddress)
        {
            this.MACAddress = initMacAddress;
            this.IsRoot = false;
        }

    }
}
