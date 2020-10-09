using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Event;
using EMChat2.ViewModel.Main.Body.User;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    public class CustomerDetailAreaViewModel : PropertyChangedBase, IEventHandle<UserEditEventArgs>, IDisposable
    {
        #region 构造函数
        public CustomerDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, bool isInform = true)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.isInform = isInform;
        }
        #endregion

        #region 属性
        private bool isInform;
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
                this.IsEditingCustomer = false;
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
                if (!this.IsEditingCustomer)
                {
                    this.TemporaryCustomerTagAreaViewModel?.Dispose();
                    this.TemporaryCustomerTagAreaViewModel = null;
                    this.TemporaryEditCustomer = null;
                }
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
        private CustomerTagAreaViewModel temporaryCustomerTagAreaViewModel;
        public CustomerTagAreaViewModel TemporaryCustomerTagAreaViewModel
        {
            get
            {
                return this.temporaryCustomerTagAreaViewModel;
            }
            set
            {
                this.temporaryCustomerTagAreaViewModel = value;
                this.NotifyPropertyChange(() => this.TemporaryCustomerTagAreaViewModel);
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
                    this.IsEditingCustomer = false;
                    this.TemporaryEditCustomer = this.Customer.Clone();
                    this.TemporaryCustomerTagAreaViewModel?.Dispose();
                    this.TemporaryCustomerTagAreaViewModel = new CustomerTagAreaViewModel(this.windowManager, this.eventAggregator, this.applicationContextViewModel, this.TemporaryEditCustomer.Business, this.TemporaryEditCustomer.Tags.CloneArray());
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
                    this.TemporaryEditCustomer.Tags = new ObservableCollection<TagInfo>(this.TemporaryCustomerTagAreaViewModel.SelectedTags);
                    if (isInform) this.eventAggregator.PublishAsync(new UserEditEventArgs() { User = this.TemporaryEditCustomer });
                    else this.Customer.Assign(this.TemporaryEditCustomer);
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

        #region 事件处理
        public void Handle(UserEditEventArgs arg)
        {
            if (!(arg.User is CustomerInfo)) return;
            CustomerInfo newCustomer = arg.User as CustomerInfo;
            if (!newCustomer.Equals(this.Customer)) return;
            this.Customer.Assign(newCustomer);
        }
        #endregion
    }
}
