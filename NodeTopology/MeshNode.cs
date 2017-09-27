using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeTopology
{
    [Serializable]
    public class MeshNode
    {
        //所有收发功率均为父节点传递给当前节点的！
        //string[] portnumber; //端口号

        //public string[] Portnumber
        //{
        //    get { return portnumber; }
        //    set { portnumber = value; }
        //}
        //node fathernode;


        //int txspeed;    //TX Speed (Mbps) 发送速度
        //int txsnr;//TX SNR (dB) 发送功率
        //int rxspeed;//RX Speed (Mbps) 接收速度
        //int rxsnr;        //RX SNR (dB) 接收功率
        string macaddress; // MAC Address
        string ipaddress;  //IP Address
        bool isRoot;   //该节点是否为父节点

        public bool IsRoot
        {
            get { return isRoot; }
            set { isRoot = value; }
        }
        bool haschild; //该节点是否为子节点(是否有子节点根据报文情况进行判断)

        public bool Haschild
        {
            get { return haschild; }
            set { haschild = value; }
        }

        /// <summary>
        /// 以下为基础信息
        /// </summary>
        double battery; //输入电压V
        double frequency; //频率分布MHz
        double txpower; //主发送功能率dBm （不涉及点对点之间的通讯，点对点之间见 TX Speed 和 TX SNR）
        double bandwidth; //输出带宽


        //构造子节点,子节点必须有父节点
        public MeshNode(string inimacaddress)
        {
            this.macaddress = inimacaddress;
            //this.fathernode = Pfathernode;
            this.isRoot = false;
        }

        //构造根节点(根节点无父节点)
        public MeshNode()
        {
            isRoot = true;
            //fathernode = null;
        }

        //设置父节点
        //public node Fathernode
        //{
        //    get { return fathernode; }
        //    set { fathernode = value; }
        //}

        public double BandWidth
        {
            get { return bandwidth; }
            set { bandwidth = value; }
        }


        public double TxPower
        {
            get { return txpower; }
            set { txpower = value; }
        }

        public double Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        public double Battery
        {
            get { return battery; }
            set { battery = value; }
        }

        public string IpAddress
        {
            get { return ipaddress; }
            set { ipaddress = value; }
        }

        public string MacAddress
        {
            get { return macaddress; }
            set { macaddress = value; }
        }

        //public int RxSnr
        //{
        //  get { return rxsnr; }
        //  set { rxsnr = value; }
        //}

        
        //public int RxSpeed
        //{
        //  get { return rxspeed; }
        //  set { rxspeed = value; }
        //}


        //public int TxSnr
        //{
        //    get { return txsnr; }
        //    set { txsnr = value; }
        //}

        //public int TxSpeed
        //{
        //    get { return txspeed; }
        //    set { txspeed = value; }
        //}

        //public string[] PortNumber
        //{
        //    get { return portnumber; }
        //    set { portnumber = value; }
        //}

    }

    [Serializable]
    public class MeshRelation 
    {
        MeshNode localnode;

        public MeshNode Localnode
        {
            get { return localnode; }
            set { localnode = value; }
        }
        MeshNode remotenode;

        public MeshNode Remotenode
        {
            get { return remotenode; }
            set { remotenode = value; }
        }
       

        public MeshRelation(MeshNode fromnode, MeshNode tonode)
        {
            this.localnode = fromnode;
            this.remotenode = tonode;
        }

        int localport;
        public int Localport
        {
            get { return localport; }
            set { localport = value; }
        }

        int remoteport;

        public int Remoteport
        {
            get { return remoteport; }
            set { remoteport = value; }
        }

        int txspeed;

        public int Txspeed
        {
            get { return txspeed; }
            set { txspeed = value; }
        }
        int txsnr;

        public int Txsnr
        {
            get { return txsnr; }
            set { txsnr = value; }
        }
        int rxspeed;

        public int Rxspeed
        {
            get { return rxspeed; }
            set { rxspeed = value; }
        }
        int rxsnr;

        public int Rxsnr
        {
            get { return rxsnr; }
            set { rxsnr = value; }
        }
        int findtimes;

        public int Findtimes
        {
            get { return findtimes; }
            set { findtimes = value; }
        }

    
    }




}
