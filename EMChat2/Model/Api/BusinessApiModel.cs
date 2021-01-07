using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 业务模型
    /// </summary>
    public class BusinessApiModel
    {
        /// <summary>
        /// 业务ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 业务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 业务描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否是外部业务
        /// </summary>
        public virtual bool Outside { get; set; }
    }
}
