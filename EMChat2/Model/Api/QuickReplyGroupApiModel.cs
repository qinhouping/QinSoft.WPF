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
    /// 快速回复组模型
    /// </summary>
    public class QuickReplyGroupApiModel
    {
        /// <summary>
        /// 快速回复组ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户ID
        /// 系统级别忽略
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// 快速回复组级别
        /// </summary>
        public QuickReplyGroupLevelEnum Level { get; set; }

        /// <summary>
        /// 快速回复组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 快捷回复
        /// </summary>
        public IEnumerable<QuickReplyApiModel> QuickReplies { get; set; }
    }
}
