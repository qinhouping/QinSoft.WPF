using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 员工模型
    /// </summary>
    public class StaffApiModel
    {
        /// <summary>
        /// 员工ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// IM用户ID
        /// </summary>
        public string ImUserId { get; set; }

        /// <summary>
        /// 员工名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 员工头像
        /// </summary>
        public string HeaderImageUrl { get; set; }

        /// <summary>
        /// 员工工号
        /// </summary>
        public string WorkCode { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public SexEnum Sex { get; set; }

        /// <summary>
        /// 业务列表
        /// </summary>
        [JsonIgnore]
        public IEnumerable<UserBusinessApiModel> UserBusinesses { get; set; }
    }
}
