using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 客户模型
    /// </summary>
    public class CustomerApiModel
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 客户IMUserId
        /// </summary>
        public string ImUserId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 客户头像
        /// </summary>
        public string HeaderImageUrl { get; set; }

        /// <summary>
        /// 通行证UID
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public SexEnum Sex { get; set; }
    }
}
