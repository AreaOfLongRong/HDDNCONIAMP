using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HDDNCONIAMP.Utils
{
    /// <summary>
    /// 文件操作工具类
    /// </summary>
    public class FileUtils
    {

        /// <summary>
        /// CHM帮助文档路径
        /// </summary>
        public static string HELP_CHM_PATH = AppDomain.CurrentDomain.BaseDirectory + "Help.chm";

        /// <summary>
        /// 私有构造函数，防止被实例化
        /// </summary>
        private FileUtils()
        {

        }

        /// <summary>
        /// 读取指定文件内容到字符串中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件内容字符串</returns>
        public static string ReadFileToString(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            using(StreamReader sr = new StreamReader(filePath, Encoding.Default))
            {
                string line = "";
                while((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line);
                }
            }
            return sb.ToString();
        }

    }
}
