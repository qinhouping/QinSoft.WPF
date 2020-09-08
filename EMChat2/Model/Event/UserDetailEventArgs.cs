﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Event
{
    public class UserDetailEventArgs : EventArgs
    {
        public UserDetailType Type { get; set; }

        public object Data { get; set; }
    }

    public enum UserDetailType
    {
        /// <summary>
        /// 新客户
        /// </summary>
        NewCustomer,
        /// <summary>
        /// 标签客户
        /// </summary>
        TagCustomer,
        /// <summary>
        /// 部门
        /// </summary>
        Department,
        /// <summary>
        /// 员工
        /// </summary>
        Staff
    }
}
