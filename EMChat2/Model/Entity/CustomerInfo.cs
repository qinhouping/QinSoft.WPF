using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Entity
{
    /// <summary>
    /// 外部客户信息实体
    /// </summary>
    public class CustomerInfo : UserInfo
    {
        #region 构造函数
        public CustomerInfo()
        {
            this.Type = UserType.Customer;
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
                this.NotifyPropertyChange(() => this.Remark);
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
        #endregion
    }
}
