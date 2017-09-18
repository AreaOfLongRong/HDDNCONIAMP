using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using log4net;

namespace HDDNCONIAMP.Mesh
{
    public class MeshTelnet
    {
        #region 私有变量

        /// <summary>
        /// 日志记录器
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(MeshTelnet));

        /// <summary>
        /// TCP客户端
        /// </summary>
        private TcpClient client;
        /// <summary>
        /// 远端IP
        /// </summary>
        private IPEndPoint remote;
        /// <summary>
        /// 网络数据流
        /// </summary>
        private NetworkStream ns;
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private int buffsize = 256;

        #endregion

        /// <summary>
        /// 回车换行符，linux和windows系统使用不同的符号。
        /// windows使用“\r\n”;linux使用“\n”
        /// </summary>
        public string ENTER
        {
            get
            {
                return "\r\n";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">域名或者ip</param>
        /// <param name="port">端口号</param>
        public MeshTelnet(string host, int port)
        {
            if (port < 1 || port > 65536)
            {
                Exception en = new Exception("端口号不在范围内");
                logger.Error(en.Message);
            }
            try
            {
                IPAddress ip = Dns.GetHostAddresses(host)[0];
                remote = new IPEndPoint(ip, port);
                client = new TcpClient();
                client.Connect(remote);
                ns = client.GetStream();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

        }
        public MeshTelnet(string host)
            : this(host, 40000)
        {
        }
        public void close()
        {
            client.Close();
        }
        ~MeshTelnet()
        {
            client.Close();
        }
        /// <summary>
        /// 向服务器发送命令
        /// </summary>
        /// <param name="command"></param>
        public void sendData(string command)
        {
            if (!command.EndsWith(ENTER))
                command += ENTER;
            byte[] data = System.Text.Encoding.Default.GetBytes(command);
            ns.Write(data, 0, data.Length);
        }

        /// <summary>
        /// 接收来自远程端的的数据。直到匹配该正则表达式或者接收时间超过设定值，才返回结果。
        /// </summary>
        /// <param name="msg">直到匹配即返回</param>
        /// <param name="second">等待最长时间，秒</param>
        /// <returns></returns>
        public string recvDataWaitWord(Regex msg, int second)
        {
            StringBuilder result = new StringBuilder(buffsize);
            string temp = string.Empty;
            DateTime current = DateTime.Now.AddSeconds(second);
            do
            {
                temp = recvData();
                result.Append(temp);
                if (msg.Match(temp).Success)
                {
                    return result.ToString();
                }

            } while (DateTime.Now < current);
            return result.ToString();
        }
        /// <summary>
        /// 接收来自远程端的数据，直到匹配结束字符串或者超时，才返回数据。
        /// </summary>
        /// <param name="msg">匹配结束符</param>
        /// <param name="second">超时时间（秒）</param>
        /// <returns></returns>
        public string recvDataWaitWord(string msg, int second)
        {
            StringBuilder result = new StringBuilder(buffsize);
            string temp = string.Empty;
            DateTime current = DateTime.Now.AddSeconds(second);
            do
            {
                temp = recvData();
                result.Append(temp);
                if (temp.TrimEnd().EndsWith(msg))
                {
                    return result.ToString();
                }

            } while (DateTime.Now < current);
            return result.ToString();
        }
        public string recvData()
        {
            byte[] tempData = new byte[buffsize];
            List<byte> data = new List<byte>();
            int count = 0;
            do
            {
                if (!ns.DataAvailable)
                {
                    return "";
                }
                count = ns.Read(tempData, 0, tempData.Length);
                data.AddRange(tempData.Take(count));
            } while (count == buffsize);
            return Negotiate(data.ToArray());
        }
        /// <summary>
        /// 协商。最简单的协商：拒绝所有要求。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        string Negotiate(byte[] data)
        {
            List<byte> sendData = new List<byte>();
            for (int i = 0; i < data.Length; i += 3)
            {
                if (data[i] == 255)
                {
                    byte[] remoteData = data.Skip(i).Take(3).ToArray();
                    for (int j = 0; j < remoteData.Length; j++)
                    {
                        if (remoteData[j] == 253)
                            remoteData[j] = 252;
                    }
                    sendData.AddRange(remoteData);
                }
                else
                {
                    break;
                }
            }
            byte[] sendByte = sendData.ToArray();
            ns.Write(sendByte, 0, sendByte.Length);
            if (sendByte.Length == data.Length)
            {
                return recvData();
            }
            return Encoding.Default.GetString(data.Skip(sendByte.Length).ToArray());

        }
    }
}
