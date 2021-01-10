using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
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
    public class CustomerDetailAreaViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数
        public CustomerDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, CustomerTagAreaViewModel customerTagAreaViewModel, bool isInform = true)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.customerTagAreaViewModel = customerTagAreaViewModel;
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
        private ObservableCollection<TagGroupModel> temporaryTagGroups;
        public ObservableCollection<TagGroupModel> TemporaryTagGroups
        {
            get
            {
                return this.temporaryTagGroups;
            }
            set
            {
                this.temporaryTagGroups = value;
                this.NotifyPropertyChange(() => this.TemporaryTagGroups);
            }
        }
        private CustomerModel customer;
        public CustomerModel Customer
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
                    this.TemporaryTagGroups = null;
                    this.TemporaryEditCustomer = null;
                }
            }
        }
        private CustomerModel temporaryEditCustomer;
        public CustomerModel TemporaryEditCustomer
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
                    this.IsEditingCustomer = false;
                    this.TemporaryEditCustomer = this.Customer.CloneObject();
                    IEnumerable<TagGroupModel> tagGroups = this.CustomerTagAreaViewModel.TagGroups.Where(u => u.BusinessId == this.Customer.BusinessId).CloneArray();
                    foreach (TagGroupModel tagGroup in tagGroups)
                    {
                        tagGroup.Tags = new ObservableCollection<TagModel>(tagGroup.Tags.CloneArray());
                        tagGroup.Tags.ToList().ForEach(u => u.IsSelected = this.TemporaryEditCustomer.TagIds.Contains(u.Id));
                    }
                    this.TemporaryTagGroups = new ObservableCollection<TagGroupModel>(tagGroups);
                    this.IsEditingCustomer = true;
                }, () =>
                {
                    return this.Customer != null;
                });
            }
        }

        public ICommand ConfirmEditCustomerCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.TemporaryEditCustomer.TagIds = new ObservableCollection<string>(this.TemporaryTagGroups.SelectMany(u => u.Tags).Where(u => u.IsSelected).Select(u => u.Id));
                    this.Customer.Assign(this.TemporaryEditCustomer);
                    if (isInform) this.eventAggregator.PublishAsync(new UserInfoChangedEventArgs() { User = this.TemporaryEditCustomer });
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

        public ICommand OpenPrivateChatCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.eventAggregator.PublishAsync(new OpenPrivateChatEventArgs() { ChatUser = this.Customer, IsActive = true });
                }, () =>
                {
                    return this.Customer != null;
                });
            }
        }
        #endregion

        #region 方法
        public void Dispose()
        {
            this.Customer = null;
            this.eventAggregator.Unsubscribe(this);
        }
        #endregion
    }
}
