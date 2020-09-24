using EMChat2.Model.Event;
using EMChat2.Service;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Body.Chat
{
    [Component]
    public class ChatDetailAreaViewModel : PropertyChangedBase, IEventHandle<ChatDetailEventArgs>
    {
        #region 构造函数
        public ChatDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ChatViewModel currentChatItem;
        public ChatViewModel CurrentChatItem
        {
            get
            {
                return this.currentChatItem;
            }
            set
            {
                this.currentChatItem = value;
                this.NotifyPropertyChange(() => this.CurrentChatItem);
            }
        }
        #endregion

        #region 事件处理
        public void Handle(ChatDetailEventArgs arg)
        {
            this.CurrentChatItem = arg.ChatItem;
        }
        #endregion
    }
}
