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
    /// 会话用户模型
    /// </summary>
    public class ChatUserApiModel
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string ChatId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserTypeEnum UserType { get; set; }

        /// <summary>
        /// 用户信息字符串
        /// </summary>
        [JsonIgnore]
        public string UserInfo { get; set; }

        /// <summary>
        /// 会话中用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否已退出会话
        /// </summary>
        public virtual bool Exited { get; set; }
    }
}
