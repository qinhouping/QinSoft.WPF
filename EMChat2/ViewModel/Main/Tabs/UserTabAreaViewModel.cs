using EMChat2.ViewModel.Main.Tabs.User;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs
{
    [Component]
    public class UserTabAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public UserTabAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, CustomerAreaViewModel customerAreaViewModel, StaffAreaViewModel staffAreaViewModel, UserDetailAreaViewModel userDetailAreaViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.customerAreaViewModel = customerAreaViewModel;
            this.staffAreaViewModel = staffAreaViewModel;
            this.userDetailAreaViewModel = userDetailAreaViewModel;
        }

        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private CustomerAreaViewModel customerAreaViewModel;
        public CustomerAreaViewModel CustomerAreaViewModel
        {
            get
            {
                return this.customerAreaViewModel;
            }
            set
            {
                this.customerAreaViewModel = value;
                this.NotifyPropertyChange(() => this.CustomerAreaViewModel);
            }
        }
        private StaffAreaViewModel staffAreaViewModel;
        public StaffAreaViewModel StaffAreaViewModel
        {
            get
            {
                return this.staffAreaViewModel;
            }
            set
            {
                this.staffAreaViewModel = value;
                this.NotifyPropertyChange(() => this.StaffAreaViewModel);
            }
        }
        private UserDetailAreaViewModel userDetailAreaViewModel;
        public UserDetailAreaViewModel UserDetailAreaViewModel
        {
            get
            {
                return this.userDetailAreaViewModel;
            }
            set
            {
                this.userDetailAreaViewModel = value;
                this.NotifyPropertyChange(() => this.UserDetailAreaViewModel);
            }
        }
        #endregion
    }
}
