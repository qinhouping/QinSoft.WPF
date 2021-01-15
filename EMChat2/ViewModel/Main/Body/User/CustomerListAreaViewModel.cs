using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
using EMChat2.ViewModel.Main.Body.User;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMChat2.Model.Enum;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    [Component]
    public class CustomerListAreaViewModel : PropertyChangedBase, IEventHandle<LoginCallbackEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>, IEventHandle<UserInfoChangedEventArgs>
    {
        #region 构造函数
        public CustomerListAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, CustomerTagAreaViewModel customerTagAreaViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.customerTagAreaViewModel = customerTagAreaViewModel;
            this.customerDetailAreaViewModel = new CustomerDetailAreaViewModel(this.windowManager, this.eventAggregator, this.applicationContextViewModel, this.customerTagAreaViewModel);
            this.customers = new ObservableCollection<CustomerModel>();
            this.selectedCustomer = null;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ApplicationContextViewModel applicationContextViewModel;
        public ApplicationContextViewModel ApplicationContextViewModel
        {
            get
            {
                return this.applicationContextViewModel;
            }
            set
            {
                this.applicationContextViewModel = value;
                this.NotifyPropertyChange(() => this.ApplicationContextViewModel);
            }
        }
        private CustomerTagAreaViewModel customerTagAreaViewModel;
        public CustomerTagAreaViewModel CustomerTagAreaViewModel
        {
            get
            {
                return this.customerTagAreaViewModel;
            }
            set
            {
                this.customerTagAreaViewModel = value;
                this.NotifyPropertyChange(() => this.CustomerTagAreaViewModel);
            }
        }
        private ObservableCollection<CustomerModel> customers;
        public ObservableCollection<CustomerModel> Customers
        {
            get
            {
                return this.customers;
            }
            set
            {
                this.customers = value;
                this.NotifyPropertyChange(() => this.Customers);
            }
        }
        private CustomerModel selectedCustomer;
        public CustomerModel SelectedCustomer
        {
            get
            {
                return this.selectedCustomer;
            }
            set
            {
                this.selectedCustomer = value;
                this.NotifyPropertyChange(() => this.SelectedCustomer);
                this.CustomerDetailAreaViewModel.Customer = value;
            }
        }
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

        private string businessId;
        public string BusinessId
        {
            get
            {
                return this.businessId;
            }
            set
            {
                if (this.businessId == value) return;
                this.businessId = value;
                this.NotifyPropertyChange(() => this.BusinessId);
            }
        }
        #endregion

        #region 事件处理

        public void Handle(LoginCallbackEventArgs arg)
        {
            if (!arg.IsSuccess) return;
        }

        public void Handle(LogoutCallbackEventArgs arg)
        {
            new Action(() =>
            {
                this.Customers.Clear();
            }).ExecuteInUIThread();
        }

        public void Handle(ExitCallbackEventArgs arg)
        {
            new Action(() =>
            {
                this.Customers.Clear();
            }).ExecuteInUIThread();
        }

        public void Handle(UserInfoChangedEventArgs arg)
        {
            lock (this.Customers)
                this.Customers.FirstOrDefault(u => u.Equals(arg.User)).Assign(arg.User);
        }
        #endregion
    }
}
