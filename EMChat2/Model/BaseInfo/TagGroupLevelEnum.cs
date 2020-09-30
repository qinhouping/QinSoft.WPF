﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 标签级别
    /// </summary>
    public enum TagGroupLevelEnum
    {
        /// <summary>
        /// 系统级别
        /// </summary>
        [Description("系统")]
        System,

        /// <summary>
        /// 用户级别
        /// </summary>
        [Description("用户")]
        User
    }
}
