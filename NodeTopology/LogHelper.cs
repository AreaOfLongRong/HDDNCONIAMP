using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace NodeTopology
{
    public class LogHelper
    {
        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public static void WriteLog(Type t, Exception ex)
        {
            //log4net.ILog log = log4net.LogManager.GetLogger(t);
            ILog log = log4net.LogManager.GetLogger("RollingLogFileAppender");
            log.Error("Error", ex);
        }

        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void WriteLog(Type t, string msg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(t);
            log.Error(msg);
        }
        
        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog(string msg)
        {
            //ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            ILog log = log4net.LogManager.GetLogger("RollingLogFileAppender");
            log.Info(msg);
        }
        
        public static void WriteLogOffline(string msg)
        {
            ILog log = log4net.LogManager.GetLogger("OffLineLogFileAppender");
            log.Info(msg);
        }
        
        
        public static void WriteLogOffline(Exception ex)
        {
            ILog log = log4net.LogManager.GetLogger("OffLineLogFileAppender");
            log.Error("Error", ex);
        }
    }
}
