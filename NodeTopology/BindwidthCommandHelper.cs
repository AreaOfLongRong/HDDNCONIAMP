using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NodeTopology
{
    public class BindwidthCommandHelper
    {
        public static void ChangeBindwidth(string ip,int bindwidth)
        {
            MyTelnet tn = new MyTelnet(ip);
            try
            {
                string recvStr = tn.recvDataWaitWord("help", 1);
                tn.sendData(string.Format("bandwidth {0}",bindwidth));
                recvStr = tn.recvDataWaitWord("OK\n#user@/>", 1);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("在获取NODE信息时异常: " + ex.Message);
            }
            finally
            {
                tn.close();
            }
    }
    }
}
