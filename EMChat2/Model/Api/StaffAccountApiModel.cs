using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 员工账号模型
    /// </summary>
    public class StaffAccountApiModel
    {
        /// <summary>
        /// 员工工号
        /// </summary>
        public string WorkCode { get; set; }

        /// <summary>
        /// 员工密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public string StaffId { get; set; }

        /// <summary>
        /// 员工是否有效
        /// </summary>
        public virtual bool IsValid { get; set; }
    }
}
