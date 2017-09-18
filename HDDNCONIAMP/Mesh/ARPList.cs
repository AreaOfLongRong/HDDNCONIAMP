using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using log4net;

namespace HDDNCONIAMP.Mesh
{
    public class ARPList
    {

        #region 私有变量

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(ARPList));

        /// <summary>
        /// 类型字符串数组
        /// </summary>
        private static string[] typeStr = { "", "其它", "无效", "动态", "静态" };

        #endregion

        #region 外部DLL导入


        [DllImport("iphlpapi.dll")]
        extern static int GetIpNetTable(IntPtr pTcpTable, ref int pdwSize, bool bOrder);

        #endregion

        #region 结构体

        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_IPNETROW
        {
            public int Index;
            public int PhysAddrLen;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] PhysAddr;
            public int Addr;
            public int Type;
        }

        #endregion

        /// <summary>
        /// 获取或设置正在使用的ARP列表
        /// </summary>
        public List<ARPItem> UsingArpList { get; set; }

        /// <summary>
        /// 重新加载初始化ARP列表
        /// </summary>
        public void ReloadARP()
        {
            if(UsingArpList == null)
                UsingArpList = new List<ARPItem>();
            
            if (UsingArpList != null)
                UsingArpList.Clear();

            listARP();
        }

        /// <summary>
        /// Ping子网的所有IP
        /// </summary>
        /// <param name="subNet"></param>
        public void PingSubNet(string subNet)
        {
            logger.Info("开始ping网段“" + subNet + "”内的所有IP：");
            try
            {
                for (int i = 0; i <= 255; i++)
                {
                    Ping myPing = new Ping();
                    Thread.Sleep(3);
                    string targetIP = subNet + (subNet.EndsWith(".") ? "" : ".") + i.ToString();
                    myPing.SendAsync(targetIP, 100, null);
                }
            }
            catch (Exception e)
            {
                logger.Error("Ping地址出错：", e);
            }
        }

        #region 私有方法

        /// <summary>
        /// 列出网络中的ARP列表
        /// </summary>
        private void listARP()
        {
            logger.Info("开始扫描ARP列表...");
            try
            {
                List<MIB_IPNETROW> ArpList = new List<MIB_IPNETROW>();
                int size = 0;
                GetIpNetTable(IntPtr.Zero, ref size, true);
                var p = Marshal.AllocHGlobal(size);
                if (GetIpNetTable(p, ref size, true) == 0)
                {
                    var num = Marshal.ReadInt32(p);
                    var ptr = IntPtr.Add(p, 4);
                    for (int i = 0; i < num; i++)
                    {
                        ArpList.Add((MIB_IPNETROW)Marshal.PtrToStructure(ptr, typeof(MIB_IPNETROW)));
                        ptr = IntPtr.Add(ptr, Marshal.SizeOf(typeof(MIB_IPNETROW)));
                    }
                    Marshal.FreeHGlobal(p);
                }
                foreach (var i in ArpList.GroupBy(x => x.Index))
                {
                    foreach (var n in ArpList.Where(x => x.Index == i.Key))
                    {
                        if (!typeStr[n.Type].Equals("无效"))
                        {
                            ARPItem arp = new ARPItem();
                            arp.InternetAddress = formatIPString(n.Addr);
                            arp.PhysicalAddress = formatMACString(n.PhysAddr);
                            arp.Type = typeStr[n.Type];
                            UsingArpList.Add(arp);
                            logger.Info("搜索到新的" + arp.ToString());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("扫描ARP列表出错：", e);
            }
            logger.Info("结束扫描ARP列表。");
        }

        /// <summary>
        /// 格式化MAC地址字符串
        /// </summary>
        /// <param name="m">MAC地址数组</param>
        /// <returns>MAC地址字符串</returns>
        private static string formatMACString(byte[] m)
        {
            return string.Format("{0:x2}-{1:x2}-{2:x2}-{3:x2}-{4:x2}-{5:x2}", m[0], m[1], m[2], m[3], m[4], m[5]);
        }

        /// <summary>
        /// 将输入的int型IP地址格式化为常规IP地址字符串
        /// </summary>
        /// <param name="addr">int型IP地址</param>
        /// <returns>常规IP地址字符串</returns>
        private static string formatIPString(int addr)
        {
            var b = BitConverter.GetBytes(addr);
            return string.Format("{0}.{1}.{2}.{3}", b[0], b[1], b[2], b[3]);
        }

        #endregion

    }
}
