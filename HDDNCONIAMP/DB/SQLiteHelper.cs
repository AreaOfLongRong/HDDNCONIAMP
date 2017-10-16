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
            if (q == null)
                return false;

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
        /// 用户更新操作
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>影响到的行数</returns>
        public int UserUpdate(User user)
        {
            return context.Update<User>(u => u.ID == user.ID,
                u => new User()
                {
                    Name = user.Name,
                    Password= user.Password,
                    Authority = user.Authority
                });
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

        /// <summary>
        /// Mesh预案最大索引
        /// </summary>
        private static int MaxMeshDeviceGroupIndex = -1;

        /// <summary>
        /// 获取下一个Mesh设备分组编码
        /// </summary>
        /// <returns>编码值</returns>
        public int GetNextMeshDeviceGroupID()
        {
            return MaxMeshDeviceGroupIndex += 1;
        }

        /// <summary>
        /// 插入新的设备分组
        /// </summary>
        /// <param name="groupName">分组名称</param>
        /// <returns>分组编号</returns>
        public int MeshDeviceGroupInsert(string groupName)
        {
            return (int)context.Insert<MeshDeviceGroup>(() => new MeshDeviceGroup() { GroupName = groupName });
        }

        /// <summary>
        /// 删除指定分组名称的设备分组记录
        /// </summary>
        /// <param name="groupName">待删除的设备分组名称</param>
        /// <returns>影响的行数</returns>
        public int MeshDeviceGroupDelete(string groupName)
        {
            return context.Delete<MeshDeviceGroup>(m => m.GroupName == groupName);
        }

        /// <summary>
        /// 更新指定ID的分组名称
        /// </summary>
        /// <param name="id">待更新的分组编号</param>
        /// <param name="groupName">新的分组名称</param>
        /// <returns>影响的行数</returns>
        public int MeshDeviceGroupUpdate(int id, string groupName)
        {
            return context.Update<MeshDeviceGroup>(m => m.ID == id, mdg => new MeshDeviceGroup() { GroupName = groupName });
        }

        /// <summary>
        /// 检索所有Mesh设备分组
        /// </summary>
        /// <returns>Mesh设备分组列表</returns>
        public List<MeshDeviceGroup> MeshDeviceGroupAllQuery()
        {
            List<MeshDeviceGroup> result = context.Query<MeshDeviceGroup>().ToList();
            if (result != null && result.Count > 0)
                MaxMeshDeviceGroupIndex = result[result.Count - 1].ID;
            return result;
        }

        /// <summary>
        /// 检索所有Mesh设备分组名称列表
        /// </summary>
        /// <returns>所有Mesh设备分组名称列表</returns>
        public List<string> MeshDeviceGroupNameAllQuery()
        {
            List<string> mdgNameList = new List<string>();
            foreach (var item in MeshDeviceGroupAllQuery())
            {
                mdgNameList.Add(item.GroupName);
            }
            return mdgNameList;
        }

        #endregion

        #region Mesh设备信息相关

        /// <summary>
        /// 添加Mesh设备信息
        /// </summary>
        /// <param name="mdi">Mesh设备信息</param>
        public void MeshDeviceInfoInsert(MeshDeviceInfo mdi)
        {
            context.Insert<MeshDeviceInfo>(() =>
            new MeshDeviceInfo()
            {
                GroupName = mdi.GroupName,
                Alias = mdi.Alias,
                IPV4 = mdi.IPV4,
                Power = mdi.Power,
                Frequency = mdi.Frequency,
                BandWidth = mdi.BandWidth,
                Battery = mdi.Battery
            });
        }

        /// <summary>
        /// 删除Mesh设备信息
        /// </summary>
        /// <param name="meshDeviceIPV4"></param>
        public int MeshDeviceInfoDelete(string meshDeviceIPV4)
        {
            return context.Delete<MeshDeviceInfo>(m => m.IPV4 == meshDeviceIPV4);
        }

        /// <summary>
        /// 更新Mesh设备信息
        /// </summary>
        /// <param name="info">新的Mesh设备信息</param>
        /// <returns></returns>
        public int MeshDeviceInfoUpdate(MeshDeviceInfo info)
        {
            return context.Update<MeshDeviceInfo>(m => m.ID == info.ID,
                u => new MeshDeviceInfo()
                {
                    GroupName = info.GroupName,
                    Alias = info.Alias,
                    IPV4 = info.IPV4,
                    Power = info.Power,
                    Frequency = info.Frequency,
                    BandWidth = info.BandWidth,
                    Battery = info.Battery
                });
        }

        /// <summary>
        /// 检索所有的Mesh设备信息列表
        /// </summary>
        /// <returns>所有的Mesh设备信息列表</returns>
        public List<MeshDeviceInfo> MeshDeviceInfoAllQuery()
        {
            return context.Query<MeshDeviceInfo>().ToList();
        }

        /// <summary>
        /// 检索指定IP对应的Mesh设备信息对象
        /// </summary>
        /// <param name="meshIP">IP地址</param>
        /// <returns>Mesh设备信息对象</returns>
        public MeshDeviceInfo MeshDeviceInfoQueryByIP(string meshIP)
        {
            int count = context.Query<MeshDeviceInfo>().Where(m => m.IPV4 == meshIP).Count();
            if (count>0)
                return context.Query<MeshDeviceInfo>().Where(m => m.IPV4 == meshIP).First();
            return null;
        }

        #endregion

        #region Mesh预案管理相关

        /// <summary>
        /// Mesh预案最大索引
        /// </summary>
        private static int MaxMeshPlanIndex = -1;

        /// <summary>
        /// 插入新的预案
        /// </summary>
        /// <param name="mpm">新的预案</param>
        public void MeshPlanInsert(MeshPlanManage mpm)
        {
            context.Insert<MeshPlanManage>(() =>
            new MeshPlanManage()
            {
                GroupName = mpm.GroupName,
                Alias = mpm.Alias,
                MeshIP = mpm.MeshIP,
                Model265IP = mpm.Model265IP,
                Model265ID = mpm.Model265ID,
                TCPToCOMIP = mpm.TCPToCOMIP,
                HKVideoIP = mpm.HKVideoIP
            });
        }

        /// <summary>
        /// 删除指定Mesh设备IP所对应的预案
        /// </summary>
        /// <param name="meshIP">待删除的预案中的Mesh设备IP</param>
        /// <returns>返回影响的记录数</returns>
        public int MeshPlanDelete(string meshIP)
        {
            return context.Delete<MeshPlanManage>(m => m.MeshIP == meshIP);
        }

        /// <summary>
        /// 更新Mesh预案
        /// </summary>
        /// <param name="mpm"></param>
        /// <returns></returns>
        public int MeshPlanUpdate(MeshPlanManage mpm)
        {
            return context.Update<MeshPlanManage>(m => m.ID == mpm.ID,
                u => new MeshPlanManage()
                {
                    GroupName = mpm.GroupName,
                    Alias = mpm.Alias,
                    MeshIP = mpm.MeshIP,
                    Model265ID = mpm.Model265ID,
                    Model265IP = mpm.Model265IP,
                    TCPToCOMIP = mpm.TCPToCOMIP,
                    HKVideoIP = mpm.HKVideoIP
                });
        }

        /// <summary>
        /// 检索所有预案
        /// </summary>
        /// <returns>预案列表</returns>
        public List<MeshPlanManage> MeshPlanAllQuery()
        {
            List<MeshPlanManage> list = context.Query<MeshPlanManage>().ToList();
            if (list != null && list.Count > 0)
                MaxMeshPlanIndex = list[list.Count - 1].ID;
            return list;
        }

        /// <summary>
        /// 获取下一个Mesh预案的ID
        /// </summary>
        /// <returns></returns>
        public int GetNextMeshPlanID()
        {
            return MaxMeshPlanIndex += 1;
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

        /// <summary>
        /// 检索指定的TCP转串口IP对应的预案
        /// </summary>
        /// <param name="tcpToCOMIP">TCP转串口IP</param>
        /// <returns></returns>
        public MeshPlanManage MeshPlanQuerByTCPToCOMIP(string tcpToCOMIP)
        {
            IQuery<MeshPlanManage> result = context.Query<MeshPlanManage>().Where(m => m.TCPToCOMIP == tcpToCOMIP);
            int count = result.Count();
            if (count > 0)
                return result.First();
            else
                return null;
        }

        /// <summary>
        /// 获取所有Mesh设备IP列表
        /// </summary>
        /// <returns>所有Mesh设备IP列表</returns>
        public List<string> MeshIPListQuery()
        {
            IQuery<MeshPlanManage> result = context.Query<MeshPlanManage>();
            return result.Select(m => m.MeshIP).ToList();
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
