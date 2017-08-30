using System.Collections.Generic;
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
            int count = q.Where(u => u.Name.Equals(userName) && u.Password.Equals(password)).Count();
            return  count> 0;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="authority">权限</param>
        /// <returns>注册用户的ID</returns>
        public int UserRegister(string userName, string password, string authority)
        {
            return (int)context.Insert<User>(() => new User() { Name = userName, Password = password, Authority = authority });
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user">待注册的用户</param>
        /// <returns>注册用户的ID</returns>
        public int UserRegister(User user)
        {
            return (int)context.Insert<User>(() => new User() { Name = user.Name, Password = user.Password, Authority = user.Authority });
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="newPassword">新的密码</param>
        /// <returns>修改影响到的行数</returns>
        public int UserModifyPassword(string userName, string newPassword)
        {
            return context.Update<User>(u => u.Name == userName, u => new User() { Password = newPassword });
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="newPassword">新的密码</param>
        /// <returns>修改影响到的行数</returns>
        public int UserModifyPassword(int id, string newPassword)
        {
            return context.Update<User>(u => u.ID == id, u => new User() { Password = newPassword });
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userName">待删除的用户名</param>
        /// <returns>删除影响到的行数</returns>
        public int UserDelete(string userName)
        {
            return context.Delete<User>(u => u.Name == userName);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userName">待删除的用户ID</param>
        /// <returns>删除影响到的行数</returns>
        public int UserDelete(int id)
        {
            return context.Delete<User>(u => u.ID == id);
        }

        public List<User> UserQuery()
        {
            return context.Query<User>().ToList();
        }

        #endregion

    }
}
