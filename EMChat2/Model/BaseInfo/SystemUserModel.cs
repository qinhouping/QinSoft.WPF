using EMChat2.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 系统账号信息模型，用于广播，事件推送等
    /// </summary>
    public class SystemUserModel : UserModel
    {
        #region 构造函数
        public SystemUserModel()
        {
            this.Type = UserTypeEnum.SystemUser;
        }
        #endregion

        #region 属性
        public override string NickName
        {
            get
            {
                return this.name;
            }
        }

        public override string HeaderImage
        {
            get
            {
                return this.headerImageUrl;
            }
        }

        /// <summary>
        /// 系统账号名称
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
        /// 系统账号头像
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
        #endregion
    }
}
