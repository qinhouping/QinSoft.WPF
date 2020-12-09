using EMChat2.Common;
using EMChat2.Event;
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

namespace EMChat2.ViewModel
{
    [Component]
    public class SettingViewModel : PropertyChangedBase, IEventHandle<SettingLoadEventArgs>
    {
        #region 构造函数
        [Constructor]
        public SettingViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel)
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
        public SettingModel Setting
        {
            get
            {
                return this.applicationContextViewModel.Setting;
            }
        }
        #endregion

        #region 命令
        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    new Action(() => this.windowManager.CloseWindow(this)).ExecuteInUIThread();
                });
            }
        }
        #endregion

        #region 事件处理
        public void Handle(SettingLoadEventArgs arg)
        {
            this.NotifyPropertyChange(() => this.Setting);
        }
        #endregion
    }
}
