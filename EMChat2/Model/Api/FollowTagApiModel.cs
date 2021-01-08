using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 好友关系标签模型
    /// </summary>
    public class FollowTagApiModel
    {
        /// <summary>
        /// 好友关系ID
        /// </summary>
        public string FollowId { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        public string TagId { get; set; }

        /// <summary>
        /// 标签组ID
        /// </summary>
        public string TagGroupId { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public TagApiModel Tag { get; set; }
    }
}
