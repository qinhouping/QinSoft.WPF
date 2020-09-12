using EMChat2.Model.Entity;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    public class StaffDetailAreaViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数
        public StaffDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
        }

        public StaffDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, StaffInfo staff)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.staff = staff;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private StaffInfo staff;
        public StaffInfo Staff
        {
            get
            {
                return this.staff;
            }
            set
            {
                this.staff = value;
                this.NotifyPropertyChange(() => this.Staff);
            }
        }
        #endregion

        #region 方法
        public void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
        }
        #endregion
    }
}
