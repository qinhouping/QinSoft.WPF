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
    /// 会话模型
    /// </summary>
    public class ChatApiModel
    {
        /// <summary>
        /// 会话ID
        /// </summary>

        public string Id { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// 会话类型
        /// </summary>
        public ChatTypeEnum Type { get; set; }

        /// <summary>
        /// 会话创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 会话名称-私聊无效
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 会话头像-私聊无效
        /// </summary>
        public string HeaderImageUrl { get; set; }

        /// <summary>
        /// 会话用户
        /// </summary>
        public virtual IEnumerable<ChatUserApiModel> ChatUsres { get; set; }
    }
}
