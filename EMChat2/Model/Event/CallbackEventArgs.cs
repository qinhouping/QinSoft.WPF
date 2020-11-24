using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Event
{
    public class CallbackEventArgs : EventArgs
    {
        public bool IsSuccess { get; set; } = true;

        public string Message { get; set; } = "成功";
    }
}
