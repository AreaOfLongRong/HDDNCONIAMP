using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chloe;
using Chloe.SQLite;
using HDDNCONIAMP.DB.Model;

namespace HDDNCONIAMP.DB
{
    /// <summary>
    /// SQLite帮助工具类
    /// </summary>
    public class SQLiteHelper
    {

        /// <summary>
        /// 静态唯一实例
        /// </summary>
        private static SQLiteHelper _instance = new SQLiteHelper();

        /// <summary>
        /// SQLite数据库上下文
        /// </summary>
        private static SQLiteContext context = new SQLiteContext(new SQLiteConnectionFactory("Data Source=.\\DB\\HDDNCONIAMP.db3;Version=3;Pooling=True;Max Pool Size=100;"));

        /// <summary>
        /// 私有构造函数，防止被实例化
        /// </summary>
        private SQLiteHelper()
        {
            
        }

        /// <summary>
        /// 获取SQLite帮助工具类的实例对象
        /// </summary>
        /// <returns></returns>
        public static SQLiteHelper GetInstance()
        {
            return _instance;
        }

        #region 用户相关

        /// <summary>
        /// 用户登录判断
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录成功返回true，否则返回false</returns>
        public bool UserLogin(string userName, string password)
        {
            IQuery<User> q = context.Query<User>();
            //User user = q.Where(u => u.Name.Equals(userName) && u.Password.Equals(password)).First();
            ;
            
            return q.Where(u => u.Name.Equals(userName) && u.Password.Equals(password)).Count() > 0;
        }

        #endregion

    }
}
