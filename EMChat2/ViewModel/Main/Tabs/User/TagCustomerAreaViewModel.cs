using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    [Component]
    public class TagCustomerAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public TagCustomerAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.customerDetailAreaViewModel = new CustomerDetailAreaViewModel(this.windowManager, this.eventAggregator);
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private CustomerDetailAreaViewModel customerDetailAreaViewModel;
        public CustomerDetailAreaViewModel CustomerDetailAreaViewModel
        {
            get
            {
                return this.customerDetailAreaViewModel;
            }
            set
            {
                this.customerDetailAreaViewModel = value;
                this.NotifyPropertyChange(() => this.CustomerDetailAreaViewModel);
            }
        }
        #endregion
    }
}
