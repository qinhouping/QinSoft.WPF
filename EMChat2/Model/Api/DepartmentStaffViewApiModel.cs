﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 用户部门员工模型
    /// </summary>
    public class DepartmentStaffViewApiModel
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
        public StaffApiModel Staff { get; set; }

        /// <summary>
        /// 当前用户ID
        /// </summary>
        public string CurrentUserId { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// 好友关系
        /// </summary>
        public FollowApiModel Follow { get; set; }
    }
}
