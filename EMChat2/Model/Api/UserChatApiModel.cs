using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 可见会话模型
    /// </summary>
    public class UserChatApiModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string ChatId { get; set; }

        /// <summary>
        /// 会话信息
        /// </summary>
        public ChatApiModel Chat { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>
        public virtual bool IsTop { get; set; }

        /// <summary>
        /// 是否通知
        /// </summary>
        public virtual bool IsInform { get; set; } 

        /// <summary>
        /// 打开时间
        /// </summary>
        public DateTime OpenTime { get; set; }
    }
}
