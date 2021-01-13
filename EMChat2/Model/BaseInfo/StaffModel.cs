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
    /// 内部员工信息模型
    /// </summary>
    public class StaffModel : UserModel
    {
        #region 构造函数

        public StaffModel()
        {
            this.Type = UserTypeEnum.Staff;
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
        /// 员工工号
        /// </summary>
        private string workCode;
        public string WorkCode
        {
            get
            {
                return this.workCode;
            }
            set
            {
                this.workCode = value;
                this.NotifyPropertyChange(() => this.WorkCode);
            }
        }

        /// <summary>
        /// 员工名称
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
        /// 员工头像
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
        /// 员工性别
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
        /// 好友关系ID
        /// </summary>
        private string followId;
        public string FollowId
        {
            get
            {
                return this.followId;
            }
            set
            {
                this.followId = value;
                this.NotifyPropertyChange(() => this.FollowId);
            }
        }

        /// <summary>
        /// 员工备注
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
        /// 员工描述
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
        /// 员工业务列表
        /// </summary>
        private ObservableCollection<BusinessModel> businesses;
        public ObservableCollection<BusinessModel> Businesses
        {
            get
            {
                return this.businesses;
            }
            set
            {
                this.businesses = value;
                this.NotifyPropertyChange(() => this.Businesses);
            }
        }

        /// <summary>
        /// 员工业务ID
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
    }
}
