using HDDNCONIAMP.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPServer
{
    public class TcpConnention
    {
        public TcpClient client;
        public StreamWriter sWriter;
        public StreamReader sReader;
        public String ipAddr;
        public TcpConnention(TcpClient client)
        {
            this.client = client;
            IPEndPoint ip = (IPEndPoint)client.Client.RemoteEndPoint;
            IPAddress remote_ip = ip.Address;
            ipAddr = remote_ip.ToString();

            sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            sReader = new StreamReader(client.GetStream(), Encoding.ASCII);

        }
    }

    public class TcpServer
    {
        private TcpListener _server;
        private Boolean _isRunning;
        public Hashtable _hashTable = new Hashtable();//hash table of TcpConnention
        public delegate void WelcomeMessageHandler(TcpConnention conn);
        public event WelcomeMessageHandler OnWelcomeMessage;

        public TcpServer(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start();

            _isRunning = true;
            
            Thread t = new Thread(LoopClients);
            t.Start();
        }


        private void LoopClients()
        {
            while (_isRunning && !LifeTimeControl.closing)
            {
                // wait for client connection
                if (_server.Pending())
                {
                    TcpClient newClient = _server.AcceptTcpClient();

                    // client found.
                    // create a thread to handle communication
                    TcpConnention conn = new TcpConnention(newClient);
                    Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                    _hashTable[conn.ipAddr] = conn;
                    t.Start(conn);
                }
                
            }
        }

        public bool SendMessageTo(String ipAddr, String message)
        {
            try
            {
                if (_hashTable.ContainsKey(ipAddr))
                {
                    TcpConnention conn = (TcpConnention)_hashTable[ipAddr];
                    conn.sWriter.WriteLine(message);
                    conn.sWriter.Flush();
                    return true;
                }
            }
            catch {
                return false;
            }
            return false;
        }

        public void Close()
        {
            _isRunning = false;
            _server.Stop();
        }

        public void DisconnectClientWithIPAddr(String ipAddr)
        {
            try
            {
                if (_hashTable.ContainsKey(ipAddr))
                {
                    TcpConnention conn = (TcpConnention)_hashTable[ipAddr];
                    conn.client.Close();
                }
            }
            catch
            {
            }
        }

        private void HandleClient(object obj)
        {
            // retrieve client from parameter passed to thread
            TcpConnention conn = (TcpConnention)obj;
            TcpClient client = conn.client;
            // sets two streams
            // you could use the NetworkStream to read and write, 
            // but there is no forcing flush, even when requested
            
            String sData = null;

            OnWelcomeMessage?.Invoke(conn);
            while (client.Connected)
            {
                // reads from stream
                sData = conn.sReader.ReadLine();

                // shows content on the console.
                Console.WriteLine("Client &gt; " + sData);
            }
            _hashTable.Remove(conn.ipAddr);
        }
    }
}
