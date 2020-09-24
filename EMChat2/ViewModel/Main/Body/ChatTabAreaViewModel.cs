using EMChat2.Common;
using EMChat2.Common.Cef;
using EMChat2.Model.Entity;
using EMChat2.Model.Event;
using EMChat2.Service;
using EMChat2.View.Main.Body.Chat;
using EMChat2.ViewModel.Main.Body.Chat;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs
{
    [Component]
    public class ChatTabAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public ChatTabAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ChatListAreaViewModel chatListAreaViewModel, ChatDetailAreaViewModel chatDetailAreaViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.chatListAreaViewModel = chatListAreaViewModel;
            this.chatDetailAreaViewModel = chatDetailAreaViewModel;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ChatListAreaViewModel chatListAreaViewModel;
        public ChatListAreaViewModel ChatListAreaViewModel
        {
            get
            {
                return this.chatListAreaViewModel;
            }
            set
            {
                this.chatListAreaViewModel = value;
                this.NotifyPropertyChange(() => this.ChatListAreaViewModel);
            }
        }
        private ChatDetailAreaViewModel chatDetailAreaViewModel;
        public ChatDetailAreaViewModel ChatDetailAreaViewModel
        {
            get
            {
                return this.chatDetailAreaViewModel;
            }
            set
            {
                this.chatDetailAreaViewModel = value;
                this.NotifyPropertyChange(() => this.ChatDetailAreaViewModel);
            }
        }
        #endregion
    }
}
