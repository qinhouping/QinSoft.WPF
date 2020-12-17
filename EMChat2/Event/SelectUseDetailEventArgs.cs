using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Event
{
    public class SelectUseDetailEventArgs : EventArgs
    {
        public UserDetailType Type { get; set; }

        public object Data { get; set; }
    }

    public enum UserDetailType
    {
        /// <summary>
        /// 客户列表
        /// </summary>
        CustomerList,

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
