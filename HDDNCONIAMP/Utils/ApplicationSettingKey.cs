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
        /// 本机IPV4地址
        /// </summary>
        public static string LocalIPV4 = "LocalIPV4";

        /// <summary>
        /// UDP监听端口
        /// </summary>
        public static string UDPListenerPort = "UDPListenerPort";

        /// <summary>
        /// 子网IP
        /// </summary>
        public static string SubNetworkIP = "SubNetworkIP";

        /// <summary>
        /// Mesh设备默认功率
        /// </summary>
        public static string MeshDefaultPower = "MeshDefaultPower";

        /// <summary>
        /// Mesh设备默认频率
        /// </summary>
        public static string MeshDefaultFrequency = "MeshDefaultFrequency";

        /// <summary>
        /// Mesh设备列表刷新频率（单位：毫秒）
        /// </summary>
        public static string MeshListRefreshFrequency = "MeshListRefreshFrequency";

        /// <summary>
        /// 视频服务器IP地址
        /// </summary>
        public static string VideoServerIPV4 = "VideoServerIPV4";

        /// <summary>
        /// 视频服务器用户名
        /// </summary>
        public static string VideoServerUserName = "VideoServerUserName";

        /// <summary>
        /// 视频服务器用户密码
        /// </summary>
        public static string VideoServerPassword = "VideoServerPassword";

        /// <summary>
        /// GPS信号超时时间（毫秒）
        /// </summary>
        public static string GPSExpirationTime = "GPSExpirationTime";

        /// <summary>
        /// 私有构造函数，防止被实例化
        /// </summary>
        private ApplicationSettingKey() { }

    }
}
