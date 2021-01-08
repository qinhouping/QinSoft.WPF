using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 用户业务模型
    /// </summary>
    public class UserBusinessApiModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// 业务信息
        /// </summary>
        public BusinessApiModel Business { get; set; }
    }
}
