﻿using EMChat2.ViewModel.Main.Tabs.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Event
{
    public class SelectChatDetailEventArgs : EventArgs
    {
        public ChatViewModel ChatItem { get; set; }
    }
}
