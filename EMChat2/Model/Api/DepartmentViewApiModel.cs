using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 用户部门模型
    /// </summary>
    public class DepartmentViewApiModel
    {
        /// <summary>
        /// 父部门ID
        /// </summary>
        public string ParentDepartmentId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 当前用户ID
        /// </summary>
        public string CurrentUserId { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// 下级部门
        /// </summary>
        [JsonIgnore]
        public IEnumerable<DepartmentViewApiModel> Departments { get; set; }

        /// <summary>
        /// 部门员工
        /// </summary>
        [JsonIgnore]
        public IEnumerable<DepartmentStaffViewModel> DepartmentStaffs { get; set; }
    }
}
