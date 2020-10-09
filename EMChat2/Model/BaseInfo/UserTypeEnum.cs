using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserTypeEnum
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
