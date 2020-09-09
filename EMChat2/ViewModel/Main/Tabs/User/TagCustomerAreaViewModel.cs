using EMChat2.Model.Entity;
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
    public class TagCustomerAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public TagCustomerAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.customerDetailAreaViewModel = new CustomerDetailAreaViewModel(this.windowManager, this.eventAggregator);
            this.tagGroups = new ObservableCollection<TagGroupInfo>();
            this.selectedTags = new ObservableCollection<TagInfo>();
            this.customers = new ObservableCollection<CustomerInfo>();
            this.selectedCustomer = null;

            //TODO 测试数据
            this.tagGroups = new ObservableCollection<TagGroupInfo>()
            {
                new TagGroupInfo(){
                    Id="1", Name="客户类型",
                    TagLevel= TagLevel.System,
                    Tags=new ObservableCollection<TagInfo>(){
                        new TagInfo(){ Id="0", Name="决策版" },
                        new TagInfo(){ Id="1", Name="领航版" },
                        new TagInfo(){ Id="2", Name="大师版" },
                        new TagInfo(){ Id="3", Name="先锋版" },
                        new TagInfo(){ Id="4", Name="经典版" }
                    }
                },
                new TagGroupInfo(){
                    Id="2", Name="成交类型",
                    TagLevel= TagLevel.System,
                    Tags=new ObservableCollection<TagInfo>(){
                        new TagInfo(){ Id="11", Name="首次" },
                        new TagInfo(){ Id="12", Name="升级" },
                        new TagInfo(){ Id="13", Name="续费" }
                    }
                },
                new TagGroupInfo(){
                    Id="3", Name="是否到期",
                    TagLevel= TagLevel.System,
                    Tags=new ObservableCollection<TagInfo>(){
                        new TagInfo(){ Id="21", Name="是" },
                        new TagInfo(){ Id="22", Name="否" }
                    }
                },
                 new TagGroupInfo(){
                    Id="3", Name="个人标签",
                    TagLevel= TagLevel.User,
                    Tags=new ObservableCollection<TagInfo>(){
                        new TagInfo(){ Id="有意向", Name="是" },
                        new TagInfo(){ Id="无意向", Name="否" }
                    }
                }
            };

            this.customers = new ObservableCollection<CustomerInfo>()
            {
                new CustomerInfo()
                {
                     Id="customer1",
                     Business=BusinessEnum.Advisor,
                     Description="测试客户1-测试描述",
                     FollowTime=DateTime.Now,
                     HeaderImageUrl="https://dss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=3401732207,3726302783&fm=26&gp=0.jpg",
                     ImUserId="customer1",
                     Name="测试客户1",
                     Remark="测试客户1-测试备注",
                     Sex= SexEnum.Man,
                     State= UserStateEnum.Online,
                     Tags=new ObservableCollection<TagInfo>()
                     {
                         new TagInfo(){ Id="2", Name="大师版" },
                         new TagInfo(){ Id="11", Name="首次" },
                         new TagInfo(){ Id="21", Name="是" }
                     },
                     Uid= "customer1"
                },
                 new CustomerInfo()
                {
                     Id="customer2",
                     Business=BusinessEnum.Advisor,
                     Description="测试客户2-测试描述",
                     FollowTime=DateTime.Now,
                     HeaderImageUrl="https://dss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=2156833431,1671740038&fm=26&gp=0.jpg",
                     ImUserId="customer2",
                     Name="测试客户2",
                     Remark="测试客户2-测试备注",
                     Sex= SexEnum.Man,
                     State= UserStateEnum.Online,
                     Tags=new ObservableCollection<TagInfo>()
                     {
                         new TagInfo(){ Id="2", Name="大师版" },
                         new TagInfo(){ Id="11", Name="首次" },
                         new TagInfo(){ Id="22", Name="否" }
                     },
                     Uid= "customer2"
                }
            };
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ObservableCollection<TagGroupInfo> tagGroups;
        public ObservableCollection<TagGroupInfo> TagGroups
        {
            get
            {
                return this.tagGroups;
            }
            set
            {
                this.tagGroups = value;
                this.NotifyPropertyChange(() => this.TagGroups);
            }
        }
        private ObservableCollection<TagInfo> selectedTags;
        public ObservableCollection<TagInfo> SelectedTags
        {
            get
            {
                return selectedTags;
            }
            set
            {
                this.selectedTags = value;
                this.NotifyPropertyChange(() => this.SelectedTags);
            }
        }
        private ObservableCollection<CustomerInfo> customers;
        public ObservableCollection<CustomerInfo> Customers
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
        private CustomerInfo selectedCustomer;
        public CustomerInfo SelectedCustomer
        {
            get
            {
                return this.selectedCustomer;
            }
            set
            {
                this.selectedCustomer = value;
                this.NotifyPropertyChange(() => this.SelectedCustomer);
                this.customerDetailAreaViewModel.Customer = value;
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
                this.business = value;
                this.NotifyPropertyChange(() => this.Business);
            }
        }
        #endregion
    }
}
