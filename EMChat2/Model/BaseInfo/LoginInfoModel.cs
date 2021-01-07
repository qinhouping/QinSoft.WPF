using EMChat2.Model.Enum;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 登录信息模型
    /// </summary>
    public class LoginInfoModel : PropertyChangedBase
    {
        #region 属性
        /// <summary>
        /// 用户名
        /// </summary>
        private string userName;
        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
                this.NotifyPropertyChange(() => this.UserName);
            }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        private string password;
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
                this.NotifyPropertyChange(() => this.Password);
            }
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        private string headerImageUrl;
        public string HeaderImageUrl
        {
            get
            {
                return this.headerImageUrl;
            }
            set
            {
                this.headerImageUrl = value;
                this.NotifyPropertyChange(() => this.HeaderImageUrl);
            }
        }

        /// <summary>
        /// 用户状态
        /// </summary>
        private UserStateEnum state;
        public UserStateEnum State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
                this.NotifyPropertyChange(() => this.State);
            }
        }

        /// <summary>
        /// 自动登录
        /// </summary>
        private bool isAutoLogin;
        public bool IsAutoLogin
        {
            get
            {
                return this.isAutoLogin;
            }
            set
            {
                this.isAutoLogin = value;
                this.NotifyPropertyChange(() => this.IsAutoLogin);
            }
        }

        /// <summary>
        /// 是否记住密码
        /// </summary>
        private bool isRememberPassword;
        public bool IsRememberPassword
        {
            get
            {
                return this.isRememberPassword;
            }
            set
            {
                this.isRememberPassword = value;
                this.NotifyPropertyChange(() => this.IsRememberPassword);
            }
        }
        #endregion

        #region 方法
        public override int GetHashCode()
        {
            return this.userName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.userName.Equals((obj as LoginInfoModel)?.userName);
        }
        #endregion
    }
}
