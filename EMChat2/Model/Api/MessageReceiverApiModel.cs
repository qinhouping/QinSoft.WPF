using EMChat2.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 消息接受者
    /// </summary>
    public class MessageReceiverApiModel
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 消息接受者ID
        /// </summary>
        public string ToUserId { get; set; }

        /// <summary>
        /// 消息接受者类型
        /// </summary>
        public UserTypeEnum ToUserType { get; set; }
    }
}
