using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HDDNCONIAMP.Model;

namespace HDDNCONIAMP.Events
{
    /// <summary>
    /// 用户登陆/登出事件参数
    /// </summary>
    public class UserLoginOrOutEventArgs : EventArgs
    {
        /// <summary>
        /// 获取或设置关联用户
        /// </summary>
        public UserInfo User { get; set; }

        /// <summary>
        /// 获取或设置是否是登陆操作，登陆为true，登出为false
        /// </summary>
        public bool IsLogin { get; set; }

        public UserLoginOrOutEventArgs(UserInfo user, bool isLogin)
        {
            this.User = user;
            this.IsLogin = isLogin;
        }

    }
}
