using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading;

namespace NodeTopology
{
    public class ARPLIST
    {
        private List<ARPitem> usingarplist = new List<ARPitem>();

        [DllImport("iphlpapi.dll")]

        extern static int GetIpNetTable(IntPtr pTcpTable, ref int pdwSize, bool bOrder);

        public List<ARPitem> UsingArpList
        {
            get { return usingarplist; }
            set { usingarplist = value; }
        }

        //初始化ARP列表
        public void ReloadARP()
        {
            if (this.usingarplist != null)
            {
                this.usingarplist.Clear();
            }            
            listarp(); 
        }

        //刷新ARP列表
        public void RefreshList()
        {            
        }

        /// <summary>
        /// 用户需要在这里定义一个网段
        /// </summary>
        public void PingSubNet(string SubNet)
        {
            try
            {
                for (int i = 1; i <= 255; i++)
                {
                    Ping myPing;
                    myPing = new Ping();

                    Thread.Sleep(3);

                   // myPing.PingCompleted += new PingCompletedEventHandler(_myPing_PingCompleted);

                    string pingIP = SubNet + i.ToString();
                    myPing.SendAsync(pingIP, 100, null);
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog(e.Message);
            }           
        }

        private void _myPing_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Reply.Status == IPStatus.Success)
            {
               // Console.WriteLine(e.Reply.Address.ToString() + "|" + Dns.GetHostEntry(IPAddress.Parse(e.Reply.Address.ToString())).HostName);
                ///urinatedong 将其写在日志中
            }
        }


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
        static string[] typeStr = { "", "其它", "无效", "动态", "静态" }; 
 
       // private static void listarp(ListBox l) 
         private void listarp()
        {
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

                    //l.Items.Add("ID:" + i.Key.ToString()); 

                    foreach (var n in ArpList.Where(x => x.Index == i.Key))
                    {
                        // l.Items.Add( ipstr(n.Addr) +"\t"+macstr(n.PhysAddr)+"\t"+ typeStr[n.Type]);
                        if (!typeStr[n.Type].Equals("无效"))
                        {

                            ARPitem TempARP = new ARPitem();

                            TempARP.InternetAddress = ipstr(n.Addr);
                            TempARP.PhysicalAddress = macstr(n.PhysAddr);
                            TempARP.Type = typeStr[n.Type];

                            usingarplist.Add(TempARP);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteLog("listarp error:" + e.Message);
            }

            LogHelper.WriteLog("listarp()结束!!!");
        } 
        private static string macstr(byte[] m) 
        { 
            return string.Format("{0:x2}-{1:x2}-{2:x2}-{3:x2}-{4:x2}-{5:x2}", m[0], m[1], m[2], m[3], m[4], m[5]); 
        } 
 
        private static string ipstr(int addr) 
        { 
            var b = BitConverter.GetBytes(addr); 
            return string.Format("{0}.{1}.{2}.{3}", b[0], b[1], b[2], b[3]); 
        }
        




    }
}
