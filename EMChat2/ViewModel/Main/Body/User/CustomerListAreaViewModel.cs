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

        private BusinessEnum business;
        public BusinessEnum Business
        {
            get
            {
                return this.business;
            }
            set
            {
                if (this.business == value) return;
                this.business = value;
                this.NotifyPropertyChange(() => this.Business);

                //TODO 测试数据
                new Action(() =>
                {
                    this.Customers.Clear();
                    this.Customers.Add(new CustomerModel()
                    {
                        Id = "customer1",
                        Business = BusinessEnum.Advisor,
                        Description = "测试客户1-测试描述",
                        FollowTime = DateTime.Now,
                        HeaderImageUrl = "https://dss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=3401732207,3726302783&fm=26&gp=0.jpg",
                        ImUserId = "customer1",
                        Name = "测试客户1",
                        Remark = "测试客户1-测试备注",
                        Sex = SexEnum.Man,
                        State = UserStateEnum.Online,
                        Tags = new ObservableCollection<TagModel>()
                     {
                         new TagModel(){ Id="2", Name="大师版", IsSelected=true },
                         new TagModel(){ Id="11", Name="首次", IsSelected=true },
                         new TagModel(){ Id="21", Name="是", IsSelected=true }
                     },
                        Uid = "customer1"
                    });
                    this.Customers.Add(new CustomerModel()
                    {
                        Id = "customer2",
                        Business = BusinessEnum.Advisor,
                        Description = "测试客户2-测试描述",
                        FollowTime = DateTime.Now,
                        HeaderImageUrl = "https://dss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=2156833431,1671740038&fm=26&gp=0.jpg",
                        ImUserId = "customer2",
                        Name = "测试客户2",
                        Remark = "测试客户2-测试备注",
                        Sex = SexEnum.Man,
                        State = UserStateEnum.Busy,
                        Uid = "customer2"
                    });
                    this.Customers.Add(new CustomerModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ImUserId = "2",
                        Name = "私聊-售前",
                        HeaderImageUrl = "https://dss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=3497889296,4029642021&fm=111&gp=0.jpg",
                        State = UserStateEnum.Online,
                        Business = BusinessEnum.PreSale,
                        Uid = "1"
                    });
                }).ExecuteInUIThread();
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
            this.Customers.FirstOrDefault(u => u.Equals(arg.User)).Assign(arg.User);
        }
        #endregion
    }
}
