using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Entity
{
    /// <summary>
    /// 在线涨停
    /// </summary>
    public enum UserStateEnum
    {
        /// <summary>
        /// 离线
        /// </summary>
        Offline,
        /// <summary>
        /// 在线
        /// </summary>
        Online,
        /// <summary>
        /// 离开（忙碌）
        /// </summary>
        Leave
    }
}
