using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Event
{
    public class CallbackEventArgs
    {
        public bool IsSuccess { get; set; }

        public int Code { get; set; }

        public string Message { get; set; }
    }
}
