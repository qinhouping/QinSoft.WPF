using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 部门员工模型
    /// </summary>
    public class DepartmentStaffApiModel
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public string StaffId { get; set; }

        /// <summary>
        /// 员工信息
        /// </summary>
        [JsonIgnore]
        public StaffApiModel Staff { get; }
    }
}
