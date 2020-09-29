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
    /// 外部客户信息实体
    /// </summary>
    public class CustomerInfo : UserInfo
    {
        #region 构造函数
        public CustomerInfo()
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

        private ObservableCollection<TagInfo> tags;
        public ObservableCollection<TagInfo> Tags
        {
            get
            {
                return this.tags ?? new ObservableCollection<TagInfo>();
            }
            set
            {
                this.tags = value;
                this.NotifyPropertyChange(() => this.Tags);
            }
        }
        #endregion
    }
}
