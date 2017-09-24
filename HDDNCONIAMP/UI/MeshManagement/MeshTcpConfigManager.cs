using HDDNCONIAMP.DB;
using HDDNCONIAMP.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCPServer;

namespace HDDNCONIAMP.UI.MeshManagement
{
    class MeshTcpConfigManager
    {
        // 定义一个静态变量来保存类的实例
        private static MeshTcpConfigManager uniqueInstance;

        private TcpServer server;
        // 定义私有构造函数，使外界不能创建该类实例
        private MeshTcpConfigManager()
        {
            server = new TcpServer(9200);
            server.OnWelcomeMessage += Server_OnWelcomeMessage;
        }

        private void Server_OnWelcomeMessage(TcpConnention conn)
        {
            MeshDeviceInfo meshInfo = null;
            string toCOMip = conn.ipAddr;
            MeshPlanManage plan = SQLiteHelper.GetInstance().MeshPlanQuerByTCPToCOMIP(toCOMip);
            if (plan != null)
            {
                meshInfo = SQLiteHelper.GetInstance().MeshDeviceInfoQueryByIP(toCOMip);
            }

            if ((meshInfo != null))
            {
                SendMessageTo(toCOMip, MeshTcpConfigManager.GetChangePowerCommand((int)meshInfo.Power));
                SendMessageTo(toCOMip, MeshTcpConfigManager.GetChangeRateCommand((int)meshInfo.Frequency));
            }
            SendMessageTo(toCOMip, MeshTcpConfigManager.GetChangeRateCommand(656));

        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static MeshTcpConfigManager GetInstance()
        {
            // 如果类的实例不存在则创建，否则直接返回
            if (uniqueInstance == null)
            {
                uniqueInstance = new MeshTcpConfigManager();
            }
            return uniqueInstance;
        }

        public static bool HasInstance()
        {
            return uniqueInstance != null;
        }

        public void SendMessageTo(String ipAddr, String message)
        {
            server.SendMessageTo(ipAddr, message);
        }

        public void DisconnectClientWithIPAddr(String ipAddr)
        {
            server.DisconnectClientWithIPAddr(ipAddr);
        }

        public void CloseServer()
        {
            server.Close();
            server = null;
        }

        /// <summary>
        /// 获取修改频率命令
        /// </summary>
        /// <param name="rate">要设置的频率</param>
        /// <param name="bindWidth"></param>
        /// <returns>修改频率命令</returns>
        public static string GetChangeRateCommand(int rate,int bindWidth = 20)
        {
            int value = 0;
            
            if (rate > 636)
                value = 10 * rate - 5 * bindWidth - 1728;
            else
                value = 10 * rate + 5 * bindWidth - 1728;
            if (value > 0)
                return string.Format("meshf{0}\r\n", ConvertToStringValue(value));
            return null;
        }

        /// <summary>
        /// 获取修改功率命令
        /// </summary>
        /// <param name="power">要设置的功率</param>
        /// <returns>修改功率命令</returns>
        public static String GetChangePowerCommand(int power)
        {
            int value = power * 2;
            if (value > 0)
                return string.Format("meshl{0}\r\n", ConvertToStringValue(value));
            return null;
        }

        public static string ConvertToStringValue(int value)
        {
            string convertString = "";
            if (value > 255 && value <65536)
            {
                int value_h = value / 256;
                int value_l = value % 256;
                byte[] bytes = new byte[2];
                bytes[0] = (byte)value_l;
                bytes[1] = (byte)value_h;
                convertString = System.Text.Encoding.ASCII.GetString(bytes);
            }
            else if (value >= 0 && value<= 255)
            {
                byte[] bytes = new byte[1];
                bytes[0] = (byte)value;
                convertString = System.Text.Encoding.ASCII.GetString(bytes);
            }
            return convertString;
        }
    }
}