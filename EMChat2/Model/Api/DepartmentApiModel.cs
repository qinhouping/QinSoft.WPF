using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 部门模型
    /// </summary>
    public class DepartmentApiModel
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
        /// 下级部门
        /// </summary>
        public virtual IEnumerable<DepartmentApiModel> Departments { get; set; }

        /// <summary>
        /// 部门员工
        /// </summary>
        public virtual IEnumerable<DepartmentStaffApiModel> DepartmentStaffs { get; }
    }
}
