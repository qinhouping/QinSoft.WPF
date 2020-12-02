using EMChat2.Model.BaseInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Event
{
    public class TemporaryInputMessagContentChangedEventArgs : EventArgs
    {
        public MessageContentModel MessageContent { get; set; }
    }
}
