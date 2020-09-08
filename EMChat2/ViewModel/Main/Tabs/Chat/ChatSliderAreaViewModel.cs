using EMChat2.Common.Cef;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public class ChatSliderAreaViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数
        public ChatSliderAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, string address)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.address = address;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private string address;
        public string Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;
                this.NotifyPropertyChange(() => this.Address);
            }
        }
        #endregion

        public void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
        }
    }
}
