using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Event
{
    public class InputMessageChangedEventArgs : EventArgs
    {
        public bool IsInputing { get; set; }

        public string ChatId { get; set; }
    }
}
