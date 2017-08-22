using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HDDNCONIAMP.Model
{
    /// <summary>
    /// 用户信息类，对用户名、密码和权限进行描述
    /// </summary>
    public class UserInfo
    {

        /// <summary>
        /// 管理员账户
        /// </summary>
        public static UserInfo Administrator = new UserInfo(
            Properties.Settings.Default.AdministratorName, 
            Properties.Settings.Default.AdministratorPassword, 
            EUserAuthority.Administrator);

        /// <summary>
        /// 获取或设置用户名
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 获取或设置密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 获取或设置用户权限
        /// </summary>
        public EUserAuthority Authority { get; set; }

        /// <summary>
        /// 根据输入的用户名、密码和权限信息初始化一个用户信息实例
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="aughority">权限信息</param>
        public UserInfo(string name, string password, EUserAuthority aughority)
        {
            this.Name = name;
            this.Password = password;
            this.Authority = aughority;
        }

    }
}
