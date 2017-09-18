using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using log4net;

namespace HDDNCONIAMP.Utils
{
    /// <summary>
    /// 网络工具类
    /// </summary>
    public class NetUtils
    {
        #region 私有变量

        /// <summary>
        /// 日志记录器
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(NetUtils));

        /// <summary>
        /// 网卡接口数组
        /// </summary>
        private static NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

        #endregion

        /// <summary>
        /// 私有构造函数，防止被实例化
        /// </summary>
        private NetUtils() { }

        /// <summary>
        /// 获取本机以太网卡名称列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetEthernetNetworkCardName()
        {
            List<string> ethernetNames = new List<string>();
            //判断是否为以太网卡
            //Ethernet              以太网卡  
            //Wireless80211         无线网卡
            NetworkInterface[] ethernets = nics.Where(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet).ToArray();
            foreach (NetworkInterface adapter in ethernets)
            {
                ethernetNames.Add(adapter.Name);
            }
            return ethernetNames.ToArray();
        }

        /// <summary>
        /// 获取指定网卡名称的IPv4地址
        /// </summary>
        /// <param name="networkCardName">网卡名称</param>
        /// <returns>手动配置的IP地址，否则返回null</returns>
        public static string GetIPv4ByNetworkCardName(string networkCardName)
        {
            foreach (NetworkInterface nic in nics)
            {
                if (networkCardName.Equals(nic.Name))
                {
                    foreach (UnicastIPAddressInformation uipai in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (uipai.Address.AddressFamily == AddressFamily.InterNetwork
                            && uipai.PrefixOrigin == PrefixOrigin.Manual)
                        {
                            return uipai.Address.ToString();
                        }
                    }
                    break;
                }
            }
            return null;
        }

        /// <summary>
        /// 配置网卡IP地址
        /// </summary>
        /// <param name="networkCardName">网卡名称</param>
        /// <param name="ipAddress">IP地址</param>
        /// <returns>配置成功返回true，否则返回false</returns>
        public static bool ConfigNetworkCardIPAddress(string networkCardName, string ipAddress)
        {
            Process p = new Process();
            try
            {
                p.StartInfo.FileName = "cmd.exe";  //设定程序名
                p.StartInfo.UseShellExecute = false;        //关闭Shell的使用  
                p.StartInfo.RedirectStandardInput = true;   //重定向标准输入
                p.StartInfo.RedirectStandardOutput = true;  //重定向标准输出
                p.StartInfo.RedirectStandardError = true;   //重定向错误输出
                p.StartInfo.CreateNoWindow = true;          //设置不显示命令行窗口 
                p.Start();                                  //启动
                p.StandardInput.WriteLine(string.Format(
                    "netsh interface ipv4 set address name=\"{0}\" source=static addr={1} mask=255.255.255.0 gwmetric=0 >nul&exit",
                    networkCardName, ipAddress));
                p.WaitForExit();
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("将本机网卡“{0}”IP地址设置为“{1}”时发生异常：\n", networkCardName, ipAddress), ex);
                return false;
            }
            finally
            {
                p.Dispose();                                //释放资源
                logger.Info(string.Format("设置本机网卡“{0}”的IP地址为“{1}”", networkCardName, ipAddress));
            }
            return true;
        }

    }
}
