using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HDDNCONIAMP.Utils
{
    /// <summary>
    /// 路径工具类
    /// </summary>
    public class PathUtils
    {

        /// <summary>
        /// 临时日志文件目录
        /// </summary>
        public static string LOG_TEMP_DIRECTORY;
        
        /// <summary>
        /// 私有构造函数，防止被实例化
        /// </summary>
        private PathUtils()
        {

        }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static PathUtils()
        {
            LOG_TEMP_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + "Temp\\Logs";
            if (!Directory.Exists(LOG_TEMP_DIRECTORY))
            {
                Directory.CreateDirectory(LOG_TEMP_DIRECTORY);
            }
        }



    }
}
