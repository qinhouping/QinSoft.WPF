using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 标签模型
    /// </summary>
    public class TagApiModel
    {
        /// <summary>
        /// 标签组ID
        /// </summary>
        public string TagGroupId { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string Name { get; set; }
    }
}
