﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TcpClientTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        TcpClient client;
        StreamWriter sWriter;
        StreamReader sReader;

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new TcpClient("localhost", 9200);
           // client = new TcpClient("192.168.0.1", 9200);
            sWriter = new StreamWriter(client.GetStream(), Encoding.ASCII);
            sReader = new StreamReader(client.GetStream(), Encoding.ASCII);
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sWriter.WriteLine("test");
            sWriter.Flush();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            while (true)
            {
                int b = client.GetStream().ReadByte();
                if (b == -1)
                    break;
                Console.Write("{0:X} ", Convert.ToInt32(b));
            }
            
        }
    }
}
