using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 表情包级别
    /// </summary>
    public enum EmotionPackageLevelEnum
    {
        /// <summary>
        /// 系统表情包
        /// </summary>
        System,
        /// <summary>
        /// 收藏表情包
        /// </summary>
        Favorite,
        /// <summary>
        /// 自定义表情包
        /// </summary>
        User
    }
}
