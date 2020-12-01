using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common.Pipe
{
    /// <summary>
    /// 处理管理事件参数
    /// </summary>
    public class HandlePipeEventArgs
    {
        /// <summary>
        /// 入参
        /// </summary>
        public object InArg { get; set; }

        /// <summary>
        /// 出差
        /// </summary>
        public object OutArg { get; set; }

        /// <summary>
        /// 是否结束
        /// </summary>
        public bool Cancel { get; set; }
    }
}
