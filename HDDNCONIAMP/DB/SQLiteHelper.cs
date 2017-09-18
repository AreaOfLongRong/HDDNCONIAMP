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
            return count > 0;
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
        /// 根据用户名检索用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户对象</returns>
        public User UserSearchByName(string userName)
        {
            IQuery<User> q = context.Query<User>();
            return q.Where(u => u.Name == userName).First();
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

        /// <summary>
        /// 检索所有用户
        /// </summary>
        /// <returns>所有用户列表</returns>
        public List<User> UserAllQuery()
        {
            return context.Query<User>().ToList();
        }

        #endregion

        #region Mesh设备分组相关



        #endregion

        #region Mesh设备信息相关



        #endregion

        #region Mesh预案管理相关

        /// <summary>
        /// 插入新的预案
        /// </summary>
        /// <param name="mpm">新的预案</param>
        public void MeshPlanInsert(MeshPlanManage mpm)
        {
            context.Insert<MeshPlanManage>(() =>
            new MeshPlanManage()
            {
                MeshIP = mpm.MeshIP,
                AudioVideoID = mpm.AudioVideoID,
                Model265IP = mpm.Model265IP,
                HKVideoIP = mpm.HKVideoIP
            });
        }

        /// <summary>
        /// 检索所有预案
        /// </summary>
        /// <returns>预案列表</returns>
        public List<MeshPlanManage> MeshPlanAllQuery()
        {
            return context.Query<MeshPlanManage>().ToList();
        }

        /// <summary>
        /// 检索所有预案
        /// </summary>
        /// <returns>由Mesh的IP为键，预案为值组成的字典</returns>
        public Dictionary<string, MeshPlanManage> MeshPlanAllQueryAsDictionary()
        {
            Dictionary<string, MeshPlanManage> result = new Dictionary<string, MeshPlanManage>();
            List<MeshPlanManage> mpmList = MeshPlanAllQuery();
            foreach (MeshPlanManage mpm in mpmList)
            {
                result.Add(mpm.MeshIP, mpm);
            }
            return result;
        }

        /// <summary>
        /// 检索指定Mesh IP地址对应的预案。
        /// </summary>
        /// <param name="meshIP"></param>
        /// <returns>预案</returns>
        public MeshPlanManage MeshPlanQueryByMeshIP(string meshIP)
        {
            return context.Query<MeshPlanManage>().Where(m => m.MeshIP == meshIP).First();
        }

        #endregion

        #region 应用程序配置相关

        /// <summary>
        /// 插入应用程序配置项
        /// </summary>
        /// <param name="setting">配置项</param>
        public void ApplicationSettingInsert(ApplicationSettings setting)
        {
            context.Insert<ApplicationSettings>(() =>
            new ApplicationSettings()
            {
                Key = setting.Key,
                Value = setting.Value,
                Description = setting.Description
            });
        }

        /// <summary>
        /// 更新应用程序配置项
        /// </summary>
        /// <param name="key">配置项关键字</param>
        /// <param name="value">配置项的值</param>
        /// <returns>影响的行数</returns>
        public int ApplicationSettingUpdate(string key, string value)
        {
            return context.Update<ApplicationSettings>(a => a.Key == key, u => new ApplicationSettings() { Value = value });
        }

        /// <summary>
        /// 删除配置项
        /// </summary>
        /// <param name="userName">待删除的配置项</param>
        /// <returns>删除影响到的行数</returns>
        public int ApplicationSettingDelete(string key)
        {
            return context.Delete<ApplicationSettings>(a => a.Key == key);
        }

        /// <summary>
        /// 检索所有配置项
        /// </summary>
        /// <returns>所有配置列表</returns>
        public List<ApplicationSettings> ApplicationSettingAllQuery()
        {
            return context.Query<ApplicationSettings>().ToList();
        }

        /// <summary>
        /// 检索所有应用程序配置项，填充到字典中，
        /// 所有的键可在ApplicationSettingKey类中找到。
        /// </summary>
        /// <returns>应用程序配置项字典</returns>
        public Dictionary<string, string> ApplicationSettingAsDictionary()
        {
            List<ApplicationSettings> allAS = ApplicationSettingAllQuery();
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (ApplicationSettings item in allAS)
            {
                result.Add(item.Key, item.Value);
            }
            return result;
        }

        /// <summary>
        /// 根据应用程序配置项的键检索其值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>配置项的值</returns>
        public ApplicationSettings ApplicationSettingValueBykey(string key)
        {
            IQuery<ApplicationSettings> q = context.Query<ApplicationSettings>();
            return q.Where(a => a.Key == key).First();
        }

        #endregion

    }
}
