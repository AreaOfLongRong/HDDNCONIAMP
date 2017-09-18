namespace HDDNCONIAMP.Utils
{
    /// <summary>
    /// 应用程序配置关键字
    /// </summary>
    public class ApplicationSettingKey
    {

        /// <summary>
        /// 百度地图缓存路径
        /// </summary>
        public static string BDMapCachePath = "BDMapCachePath";

        /// <summary>
        /// 视频数据缓存路径
        /// </summary>
        public static string VideoCachePath = "VideoCachePath";

        /// <summary>
        /// 本机网卡
        /// </summary>
        public static string LocalNetworkCard = "LocalNetworkCard";

        /// <summary>
        /// 子网IP
        /// </summary>
        public static string SubNetworkIP = "SubNetworkIP";

        /// <summary>
        /// 本机IPV4地址
        /// </summary>
        public static string LocalIPV4 = "LocalIPV4";

        /// <summary>
        /// UDP监听端口
        /// </summary>
        public static string UDPListenerPort = "UDPListenerPort";


        /// <summary>
        /// 私有构造函数，防止被实例化
        /// </summary>
        private ApplicationSettingKey() { }

    }
}
