using EMChat2.Common;
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
        /// 备注
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
                this.NotifyPropertyChange(() => this.Name);
            }
        }

        /// <summary>
        /// 重写获取Name逻辑
        /// </summary>
        [AssignIgnore]
        public override string Name
        {
            get
            {
                if (string.IsNullOrEmpty(remark)) return base.Name;
                else return this.remark;
            }
            set
            {
                base.Name = value;
                this.NotifyPropertyChange(() => this.Name);
                this.NotifyPropertyChange(() => this.OriName);
            }
        }

        /// <summary>
        /// 原始名称
        /// </summary>
        public string OriName
        {
            get
            {
                return base.Name;
            }
        }

        /// <summary>
        /// 描述
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
        /// 性别
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
        /// 业务
        /// </summary>
        private BusinessEnum business;
        public BusinessEnum Business
        {
            get
            {
                return this.business;
            }
            set
            {
                this.business = value;
                this.NotifyPropertyChange(() => this.business);
            }
        }

        /// <summary>
        /// 添加好友时间
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
        /// 客户标签列表
        /// </summary>
        private ObservableCollection<TagModel> tags;
        public ObservableCollection<TagModel> Tags
        {
            get
            {
                return this.tags ?? new ObservableCollection<TagModel>();
            }
            set
            {
                this.tags = value;
                this.NotifyPropertyChange(() => this.Tags);
            }
        }
        #endregion

        #region 方法
        public override int GetHashCode()
        {
            return (this.Id + this.Business).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Id.Equals((obj as CustomerModel)?.Id) && this.Business.Equals((obj as CustomerModel)?.Business);
        }
        #endregion
    }
}
