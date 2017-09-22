using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;


namespace NodeTopology
{
    public class myTelnet
    {
        TcpClient client;
        IPEndPoint remote;
        NetworkStream ns;
        int buffsize = 256;
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
        public myTelnet(string host, int port)
        {
            if (port < 1 || port > 65536)
            {
                Exception en = new Exception("端口号不在范围内");
                LogHelper.WriteLog(en.Message);
            }
            try
            {
                IPAddress ip = System.Net.Dns.GetHostAddresses(host)[0];
                remote = new IPEndPoint(ip, port);
                client = new TcpClient();
                var result = client.BeginConnect(ip, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                if (!success)
                {
                    LogHelper.WriteLog( "Telnet连接" + ip+ ":" +port+"超时！");
                }
                client.EndConnect(result);
                ns = client.GetStream();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
            }

        }
        public myTelnet(string host)
            : this(host, 40000)
        {
        }
        public void close()
        {
            if (client != null)
                client.Close();
        }
        ~myTelnet()
        {
            if (client != null)
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
                if (ns == null || !ns.DataAvailable)
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
            return System.Text.Encoding.Default.GetString(data.Skip(sendByte.Length).ToArray());

        }
    }

}
