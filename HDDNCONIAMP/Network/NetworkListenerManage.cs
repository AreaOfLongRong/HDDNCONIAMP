using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HDDNCONIAMP.Network
{
    /// <summary>
    /// 网络监听管理
    /// </summary>
    public class NetworkListenerManage
    {

        /// <summary>
        /// GPS UDP监听器
        /// </summary>
        public GPSUDPListener PGPSUDPListener { get; set; }
        
        CancellationTokenSource mCTS;
        CancellationToken mCT;

        public NetworkListenerManage()
        {
            PGPSUDPListener = new GPSUDPListener();
            mCTS = new CancellationTokenSource();
            mCT = mCTS.Token;
        }

        /// <summary>
        /// 启动程序所用的所有网络监听
        /// </summary>
        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                if (PGPSUDPListener != null)
                {
                    //PGPSUDPListener.StartReceive();
                    PGPSUDPListener.StartReceive(mCT);
                    //PGPSUDPListener.TestReceiveMessage();
                }
            });
        }

        /// <summary>
        /// 关闭程序所用的所有网络监听
        /// </summary>
        public void Stop()
        {
            if (PGPSUDPListener != null)
            {
                //PGPSUDPListener.StopReceive();
                mCTS.Cancel();
                //PGPSUDPListener.StopReceive(mCT);
            }
        }

    }
}
