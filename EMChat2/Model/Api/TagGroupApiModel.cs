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
    /// 标签组模型
    /// </summary>
    public class TagGroupApiModel
    {
        /// <summary>
        /// 标签组ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户ID-系统级别忽略
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// 标签组级别
        /// </summary>
        public TagGroupLevelEnum Level { get; set; }

        /// <summary>
        /// 标签组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public IEnumerable<TagApiModel> Tags { get; set; }
    }
}
