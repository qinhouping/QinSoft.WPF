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
    [Component]
    public class ChatSliderAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public ChatSliderAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.registerJsObject = new RegisterJsObject() { Name = "emchat", JsObject = new CefJsObjectBase() };
            this.address = "https://github.com/qinhouping";
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
                return this.address + "?v=" + Guid.NewGuid().ToString();
            }
            set
            {
                this.address = value;
                this.NotifyPropertyChange(() => this.Address);
            }
        }
        private RegisterJsObject registerJsObject;
        public RegisterJsObject RegisterJsObject
        {
            get
            {
                return this.registerJsObject;
            }
            set
            {
                this.registerJsObject = value;
                this.NotifyPropertyChange(() => this.RegisterJsObject);
            }
        }
        #endregion
    }
}
