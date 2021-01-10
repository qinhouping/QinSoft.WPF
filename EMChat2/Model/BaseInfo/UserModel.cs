using EMChat2.Common;
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
    /// 用户信息
    /// </summary>
    public abstract class UserModel : PropertyChangedBase
    {
        #region 属性
        /// <summary>
        /// 用户类型
        /// </summary>
        private UserTypeEnum type;
        public virtual UserTypeEnum Type
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
        /// IM用户id（只用于IM通信)
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
        /// 用户昵称
        /// </summary>
        public abstract string NickName
        {
            get;
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        public abstract string HeaderImage
        {
            get;
        }
        #endregion

        #region 方法
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.id.Equals((obj as UserModel)?.id);
        }
        #endregion
    }
}
