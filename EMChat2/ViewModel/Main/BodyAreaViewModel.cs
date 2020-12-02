using EMChat2.Event;
using EMChat2.ViewModel.Main.Tabs;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main
{
    [Component]
    public class BodyAreaViewModel : PropertyChangedBase, IEventHandle<OpenPrivateChatEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>
    {
        #region 构造函数
        public BodyAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ChatTabAreaViewModel chatTabAreaViewModel, UserTabAreaViewModel userTabAreaViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.chatTabAreaViewModel = chatTabAreaViewModel;
            this.userTabAreaViewModel = userTabAreaViewModel;
            this.SelectedIndex = 0;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private int selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }
            set
            {
                this.selectedIndex = value;
                this.NotifyPropertyChange(() => this.SelectedIndex);
            }
        }
        private ChatTabAreaViewModel chatTabAreaViewModel;
        public ChatTabAreaViewModel ChatTabAreaViewModel
        {
            get
            {
                return this.chatTabAreaViewModel;
            }
            set
            {
                this.chatTabAreaViewModel = value;
                this.NotifyPropertyChange(() => this.ChatTabAreaViewModel);
            }
        }
        private UserTabAreaViewModel userTabAreaViewModel;
        public UserTabAreaViewModel UserTabAreaViewModel
        {
            get
            {
                return this.userTabAreaViewModel;
            }
            set
            {
                this.userTabAreaViewModel = value;
                this.NotifyPropertyChange(() => this.UserTabAreaViewModel);
            }
        }
        #endregion

        #region 事件处理
        public void Handle(OpenPrivateChatEventArgs arg)
        {
            if (arg.IsActive) this.SelectedIndex = 0;
        }

        public void Handle(LogoutCallbackEventArgs arg)
        {
            this.SelectedIndex = 0;
        }

        public void Handle(ExitCallbackEventArgs arg)
        {
            this.SelectedIndex = 0;
        }
        #endregion
    }
}
