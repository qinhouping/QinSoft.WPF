using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public abstract class UserInfo : PropertyChangedBase
    {
        #region 属性
        /// <summary>
        /// 用户id
        /// </summary>
        private string id;
        public virtual string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
                this.NotifyPropertyChange(() => this.Id);
            }
        }

        /// <summary>
        /// IM用户id（用于IM通信)
        /// </summary>
        private string imUserId;
        public virtual string ImUserId
        {
            get
            {
                return this.imUserId;
            }
            set
            {
                this.imUserId = value;
                this.NotifyPropertyChange(() => this.ImUserId);
            }
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        private string name;
        public virtual string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.NotifyPropertyChange(() => this.Name);
            }
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        private string headerImageUrl;
        public virtual string HeaderImageUrl
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
        public virtual UserStateEnum State
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
        /// 用户类型
        /// </summary>
        private UserType type;
        public virtual UserType Type
        {
            get
            {
                return this.type;
            }
            protected set
            {
                this.type = value;
                this.NotifyPropertyChange(() => this.Type);
            }
        }
        #endregion

        #region 方法
        public override int GetHashCode()
        {
            return this.imUserId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.imUserId.Equals((obj as UserInfo)?.imUserId);
        }
        #endregion
    }

    /// <summary>
    /// 用户状态枚举
    /// </summary>
    public enum UserStateEnum
    {
        /// <summary>
        /// 离线
        /// </summary>
        Offline,
        /// <summary>
        /// 在线
        /// </summary>
        Online,
        /// <summary>
        /// 离开（忙碌）
        /// </summary>
        Leave
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 外部客户
        /// </summary>
        Customer,
        /// <summary>
        /// 内部员工
        /// </summary>
        Staff,
        /// <summary>
        /// 系统用户
        /// </summary>
        SystemUser
    }
}
