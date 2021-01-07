using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Event
{
    public class MessageIdChangedEventArgs : EventArgs
    {
        public string ChatId { get; set; }

        public string MessageId { get; set; }

        public string NewMessageId { get; set; }
    }
}
