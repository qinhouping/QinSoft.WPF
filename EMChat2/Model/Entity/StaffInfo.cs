using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
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
        #endregion
    }
}
