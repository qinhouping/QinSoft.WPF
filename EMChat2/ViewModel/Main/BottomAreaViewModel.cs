using EMChat2.ViewModel.Main.Bottom;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.Log;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main
{
    [Component]
    public class BottomAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public BottomAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, BottomSettingAreaViewModel bottomSettingAreaViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.bottomSettingAreaViewModel = bottomSettingAreaViewModel;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private BottomSettingAreaViewModel bottomSettingAreaViewModel;
        public BottomSettingAreaViewModel BottomSettingAreaViewModel
        {
            get
            {
                return this.bottomSettingAreaViewModel;
            }
            set
            {
                this.bottomSettingAreaViewModel = value;
                this.NotifyPropertyChange(() => this.BottomSettingAreaViewModel);
            }
        }
        #endregion
    }
}
