using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Entity
{
    /// <summary>
    /// 内部员工信息实体
    /// </summary>
    public class StaffInfo : UserInfo
    {
        #region 构造函数

        public StaffInfo()
        {
            this.Type = UserType.Staff;
        }
        #endregion

        #region 属性

        /// <summary>
        /// 工号
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
        /// 重新Name获取逻辑
        /// </summary>
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
        /// 业务列表
        /// </summary>
        private ObservableCollection<BusinessEnum> businessList;
        public ObservableCollection<BusinessEnum> BusinessList
        {
            get
            {
                return this.businessList;
            }
            set
            {
                this.businessList = value;
                this.NotifyPropertyChange(() => this.BusinessList);
            }
        }
        #endregion
    }
}
