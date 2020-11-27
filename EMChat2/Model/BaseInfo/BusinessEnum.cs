using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 业务类型枚举
    /// </summary>
    public enum BusinessEnum
    {
        /// <summary>
        /// 空业务
        /// </summary>
        [Description("空业务")]
        None,

        /// <summary>
        /// 售前
        /// </summary>
        [Description("售前")]
        PreSale,

        /// <summary>
        /// 售后
        /// </summary>
        [Description("售后")]
        PostSale,

        /// <summary>
        /// 投顾
        /// </summary>
        [Description("投顾")]
        Advisor,

        /// <summary>
        /// 专家
        /// </summary>
        [Description("专家")]
        Expert
    }
}
