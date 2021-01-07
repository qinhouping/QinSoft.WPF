using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Enum
{
    /// <summary>
    /// 消息状态枚举
    /// </summary>
    public enum MessageStateEnum
    {
        /// <summary>
        /// 发送中
        /// </summary>
        [Description("发送中")]
        Sending,

        /// <summary>
        /// 审核失败
        /// </summary>
        [Description("审核失败")]
        CheckFailure,

        /// <summary>
        /// 发送成功
        /// </summary>
        [Description("发送成功")]
        SendSuccess,

        /// <summary>
        /// 发送失败
        /// </summary>
        [Description("发送失败")]
        SendFailure,

        /// <summary>
        /// 接收的
        /// </summary>
        [Description("已接收")]
        Received,

        /// <summary>
        /// 拒绝的
        /// </summary>
        [Description("已拒绝")]
        Refused,

        /// <summary>
        /// 已读的
        /// </summary>
        [Description("已读")]
        Readed,

        /// <summary>
        /// 撤回的
        /// </summary>
        [Description("已撤回")]
        Revoked
    }
}
