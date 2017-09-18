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
            server = new TcpServer(8080);
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
    }
}
