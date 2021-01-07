using EMChat2.Model.BaseInfo;
using EMChat2.Event;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EMChat2.Model.Enum;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    [Component]
    public class CustomerAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public CustomerAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
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
        #endregion

        #region 命令
        public ICommand OpenCustomerListCommand
        {
            get
            {
                return new RelayCommand<BusinessEnum>((business) =>
                {
                    this.eventAggregator.PublishAsync(new SelectUseDetailEventArgs()
                    {
                        Type = UserDetailType.CustomerList,
                        Data = business
                    });
                });
            }
        }
        #endregion
    }
}
