using EMChat2.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 消息模型
    /// </summary>
    public class MessageApiModel
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 消息时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string ChatId { get; set; }

        /// <summary>
        /// 消息发送者ID
        /// </summary>
        public string FromUserId { get; set; }

        /// <summary>
        /// 消息发送者类型
        /// </summary>
        public UserTypeEnum FromUserType { get; set; }

        /// <summary>
        /// 消息接收者
        /// </summary>
        public IEnumerable<MessageReceiverApiModel> ToUsers { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public MessageStateEnum State { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
    }
}
