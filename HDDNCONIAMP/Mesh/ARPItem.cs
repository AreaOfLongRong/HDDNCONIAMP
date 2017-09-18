using System;

namespace HDDNCONIAMP.Mesh
{
    /// <summary>
    /// ARP项
    /// </summary>
    [Serializable]
    public class ARPItem
    {
        /// <summary>
        /// 获取或设置网络地址，即IP地址
        /// </summary>
        public string InternetAddress { get; set; }

        /// <summary>
        /// 获取或设置物理地址，即MAC地址
        /// </summary>
        public string PhysicalAddress { get; set; }

        /// <summary>
        /// 获取或设置类型
        /// </summary>
        public string Type { get;  set; }

        /// <summary>
        /// 字符串转换
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ARP:" + InternetAddress + " " + PhysicalAddress + " " + Type;
        }
    }
}
