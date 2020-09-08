using EMChat2.Model.Entity;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    public class CustomerDetailAreaViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数
        public CustomerDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }
        public CustomerDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, CustomerInfo customer)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.customer = customer;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private CustomerInfo customer;
        public CustomerInfo Customer
        {
            get
            {
                return this.customer;
            }
            set
            {
                this.customer = value;
                this.NotifyPropertyChange(() => this.Customer);
            }
        }
        #endregion

        public void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
        }
    }
}
