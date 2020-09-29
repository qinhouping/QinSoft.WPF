using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Event;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        private bool isEditingCustomer;
        public bool IsEditingCustomer
        {
            get
            {
                return this.isEditingCustomer;
            }
            set
            {
                this.isEditingCustomer = value;
                this.NotifyPropertyChange(() => this.IsEditingCustomer);
                if (!this.IsEditingCustomer) this.TemporaryEditCustomer = null;
            }
        }
        private CustomerInfo temporaryEditCustomer;
        public CustomerInfo TemporaryEditCustomer
        {
            get
            {
                return this.temporaryEditCustomer;
            }
            set
            {
                this.temporaryEditCustomer = value;
                this.NotifyPropertyChange(() => this.TemporaryEditCustomer);
            }
        }
        #endregion

        #region 命令
        public ICommand EditCustomerCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.TemporaryEditCustomer = this.Customer.Clone();
                    this.IsEditingCustomer = true;
                });
            }
        }

        public ICommand ConfirmEditCustomerCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.Customer.Assign(this.TemporaryEditCustomer);
                    this.eventAggregator.PublishAsync(new CustomerEditEventArgs() { Customer = this.Customer });
                    this.IsEditingCustomer = false;
                });
            }
        }

        public ICommand CancelEditCustomerCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.IsEditingCustomer = false;
                });
            }
        }
        #endregion

        #region 方法
        public void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
        }
        #endregion
    }
}
