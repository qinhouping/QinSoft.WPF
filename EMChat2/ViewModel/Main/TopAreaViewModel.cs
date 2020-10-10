using EMChat2.Model.BaseInfo;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main
{
    [Component]
    public class TopAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public TopAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel)
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
                this.NotifyPropertyChange(() => this.applicationContextViewModel);
            }
        }
        #endregion

        #region 命令
        public ICommand ChangeCurrentStaffStateCommand
        {
            get
            {
                return new RelayCommand<UserStateEnum>(state =>
                {
                    this.ApplicationContextViewModel.CurrentStaff.State = state;
                }, (state) =>
                {
                    return this.ApplicationContextViewModel.IsLogin && this.ApplicationContextViewModel.CurrentStaff.State != state;
                });
            }
        }
        #endregion
    }
}
