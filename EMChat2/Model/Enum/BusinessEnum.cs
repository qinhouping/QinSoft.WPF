using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Enum
{
    /// <summary>
    /// 业务类型枚举
    /// </summary>
    public enum BusinessEnum
    {
        /// <summary>
        /// 内部
        /// </summary>
        [Description("内部")]
        Inside,

        #region 外部
        /// <summary>
        /// 售前
        /// </summary>
        [Description("售前")]
        [OutSideBussinessAttribute]
        PreSale,

        /// <summary>
        /// 售后
        /// </summary>
        [Description("售后")]
        [OutSideBussinessAttribute]
        PostSale,

        /// <summary>
        /// 投顾
        /// </summary>
        [Description("投顾")]
        [OutSideBussinessAttribute]
        Advisor,

        /// <summary>
        /// 专家
        /// </summary>
        [Description("专家")]
        [OutSideBussinessAttribute]
        Expert
        #endregion
    }

    /// <summary>
    /// 外部业务
    /// </summary>
    public class OutSideBussinessAttribute : Attribute
    {

    }
}
