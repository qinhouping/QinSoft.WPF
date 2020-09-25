using EMChat2.Model.BaseInfo;
using EMChat2.Model.Event;
using EMChat2.View.Main.Body.User;
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
    public class UserDetailAreaViewModel : PropertyChangedBase, IEventHandle<UseDetailEventArgs>
    {
        #region 构造函数
        public UserDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, TagCustomerAreaViewModel tagCustomerAreaViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.tagCustomerAreaViewModel = tagCustomerAreaViewModel;
            this.departmentDetailAreaViewModel = new DepartmentDetailAreaViewModel(this.windowManager, this.eventAggregator);
            this.staffDetailAreaViewModel = new StaffDetailAreaViewModel(this.windowManager, this.eventAggregator);
            this.customerDetailAreaViewModel = new CustomerDetailAreaViewModel(this.windowManager, this.eventAggregator);
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private UserDetailType? type;
        public UserDetailType? Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
                this.NotifyPropertyChange(() => this.Type);
            }
        }
        private TagCustomerAreaViewModel tagCustomerAreaViewModel;
        public TagCustomerAreaViewModel TagCustomerAreaViewModel
        {
            get
            {
                return this.tagCustomerAreaViewModel;
            }
            set
            {
                this.tagCustomerAreaViewModel = value;
                this.NotifyPropertyChange(() => this.TagCustomerAreaViewModel);
            }
        }
        private DepartmentDetailAreaViewModel departmentDetailAreaViewModel;
        public DepartmentDetailAreaViewModel DepartmentDetailAreaViewModel
        {
            get
            {
                return this.departmentDetailAreaViewModel;
            }
            set
            {
                this.departmentDetailAreaViewModel = value;
                this.NotifyPropertyChange(() => this.DepartmentDetailAreaViewModel);
            }
        }
        private StaffDetailAreaViewModel staffDetailAreaViewModel;
        public StaffDetailAreaViewModel StaffDetailAreaViewModel
        {
            get
            {
                return this.staffDetailAreaViewModel;
            }
            set
            {
                this.staffDetailAreaViewModel = value;
                this.NotifyPropertyChange(() => this.StaffDetailAreaViewModel);
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
        #endregion

        #region 事件处理

        public void Handle(UseDetailEventArgs arg)
        {
            this.Type = arg.Type;
            switch (arg.Type)
            {
                case UserDetailType.TagCustomer:
                    this.TagCustomerAreaViewModel.Business = (BusinessEnum)arg.Data;
                    break;
                case UserDetailType.Department:
                    this.DepartmentDetailAreaViewModel.Department = arg.Data as DepartmentInfo;
                    break;
                case UserDetailType.Staff:
                    this.StaffDetailAreaViewModel.Staff = arg.Data as StaffInfo;
                    break;
                case UserDetailType.Customer:
                    this.CustomerDetailAreaViewModel.Customer = arg.Data as CustomerInfo;
                    break;
            }
        }
        #endregion
    }
}
