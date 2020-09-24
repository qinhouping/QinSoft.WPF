using EMChat2.ViewModel.Main.Tabs.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Event
{
    public class ChatDetailEventArgs : EventArgs
    {
        public ChatViewModel ChatItem { get; set; }
    }
}
