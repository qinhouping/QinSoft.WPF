using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Event
{
    public class DetailChangeEventArgs : EventArgs
    {
        public DetailType Type { get; set; }

        public object Data { get; set; }
    }

    public enum DetailType
    {
        /// <summary>
        /// 空
        /// </summary>
        None,
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
