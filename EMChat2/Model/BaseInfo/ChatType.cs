using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    public enum ChatTypeEnum
    {
        /// <summary>
        /// 私聊
        /// </summary>
        Private,

        /// <summary>
        /// 群聊
        /// </summary>
        Group,

        /// <summary>
        /// 群发，只是提交一个群发任务给后台
        /// </summary>
        GroupSend
    }
}
