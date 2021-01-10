using EMChat2.Common;
using EMChat2.Model.Enum;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 外部客户信息模型
    /// </summary>
    public class CustomerModel : UserModel
    {
        #region 构造函数
        public CustomerModel()
        {
            this.Type = UserTypeEnum.Customer;
        }
        #endregion

        #region 属性
        public override string NickName
        {
            get
            {
                if (string.IsNullOrEmpty(remark)) return this.name;
                else return this.remark;
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
        /// 通信证id
        /// </summary>
        private string uid;

        public string Uid
        {
            get
            {
                return this.uid;
            }
            set
            {
                this.uid = value;
                this.NotifyPropertyChange(() => this.Uid);
            }
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.NotifyPropertyChange(() => this.Name);
                this.NotifyPropertyChange(() => this.NickName);
            }
        }

        /// <summary>
        /// 客户头像
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
                this.NotifyPropertyChange(() => this.HeaderImage);
            }
        }

        /// <summary>
        /// 客户性别
        /// </summary>
        private SexEnum sex;
        public SexEnum Sex
        {
            get
            {
                return this.sex;
            }
            set
            {
                this.sex = value;
                this.NotifyPropertyChange(() => this.Sex);
            }
        }

        /// <summary>
        /// 客户备注
        /// </summary>
        private string remark;

        public string Remark
        {
            get
            {
                return this.remark;
            }
            set
            {
                this.remark = value;
                this.NotifyPropertyChange(() => this.Remark);
                this.NotifyPropertyChange(() => this.NickName);
            }
        }

        /// <summary>
        /// 客户描述
        /// </summary>
        private string description;
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
                this.NotifyPropertyChange(() => this.Description);
            }
        }

        /// <summary>
        /// 添加客户时间
        /// </summary>
        private DateTime followTime;
        public DateTime FollowTime
        {
            get
            {
                return this.followTime;
            }
            set
            {
                this.followTime = value;
                this.NotifyPropertyChange(() => this.FollowTime);
            }
        }

        /// <summary>
        /// 客户标签ID列表
        /// </summary>
        private ObservableCollection<string> tagIds;
        public ObservableCollection<string> TagIds
        {
            get
            {
                return this.tagIds;
            }
            set
            {
                this.tagIds = value;
                this.NotifyPropertyChange(() => this.TagIds);
            }
        }

        /// <summary>
        /// 客户业务ID
        /// </summary>
        private string businessId;
        public string BusinessId
        {
            get
            {
                return this.businessId;
            }
            set
            {
                this.businessId = value;
                this.NotifyPropertyChange(() => this.BusinessId);
            }
        }

        /// <summary>
        /// 员工状态
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
        #endregion

        #region 方法
        public override int GetHashCode()
        {
            return (this.Id + this.BusinessId).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Id.Equals((obj as CustomerModel)?.Id) && this.BusinessId.Equals((obj as CustomerModel)?.BusinessId);
        }
        #endregion
    }
}
