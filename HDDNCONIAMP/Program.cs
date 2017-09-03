using System;
using System.IO;
using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace HDDNCONIAMP
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //加载log4net配置
            FileInfo configFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.ConfigureAndWatch(configFile);

            //创建logger
            var logger = LogManager.GetLogger(typeof(Program));
            try
            {
                logger.Info("启动程序...");
                Application.Run(new FormMain());
            }
            catch (Exception ex)
            {
                logger.Error("程序出现异常：" + ex.ToString());
            }
        }
    }
}
