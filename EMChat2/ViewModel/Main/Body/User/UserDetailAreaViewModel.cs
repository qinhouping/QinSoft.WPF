using DotLiquid.Util;
using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
using EMChat2.View.Main.Body.User;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMChat2.Model.Enum;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    [Component]
    public class UserDetailAreaViewModel : PropertyChangedBase, IEventHandle<SelectUseDetailEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>
    {
        #region 构造函数
        public UserDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, CustomerListAreaViewModel customerListAreaViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.customerListAreaViewModel = customerListAreaViewModel;
            this.departmentDetailAreaViewModel = new DepartmentDetailAreaViewModel(this.windowManager, this.eventAggregator);
            this.staffDetailAreaViewModel = new StaffDetailAreaViewModel(this.windowManager, this.eventAggregator, this.applicationContextViewModel);
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
        private CustomerListAreaViewModel customerListAreaViewModel;
        public CustomerListAreaViewModel CustomerListAreaViewModel
        {
            get
            {
                return this.customerListAreaViewModel;
            }
            set
            {
                this.customerListAreaViewModel = value;
                this.NotifyPropertyChange(() => this.CustomerListAreaViewModel);
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

        public void Handle(SelectUseDetailEventArgs arg)
        {
            this.Type = arg.Type;
            switch (arg.Type)
            {
                case UserDetailType.CustomerList:
                    this.CustomerListAreaViewModel.Business = (BusinessEnum)arg.Data;
                    break;
                case UserDetailType.Department:
                    this.DepartmentDetailAreaViewModel.Department = arg.Data as DepartmentModel;
                    break;
                case UserDetailType.Staff:
                    this.StaffDetailAreaViewModel.Staff = arg.Data as StaffModel;
                    break;
            }
        }

        public void Handle(LogoutCallbackEventArgs arg)
        {
            this.DepartmentDetailAreaViewModel.Department = null;
            this.StaffDetailAreaViewModel.Staff = null;
            this.CustomerDetailAreaViewModel.Customer = null;
            this.Type = null;
        }

        public void Handle(ExitCallbackEventArgs arg)
        {
            this.DepartmentDetailAreaViewModel.Department = null;
            this.StaffDetailAreaViewModel.Staff = null;
            this.CustomerDetailAreaViewModel.Customer = null;
            this.Type = null;
        }
        #endregion
    }
}
