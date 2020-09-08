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
    public class DepartmentDetailAreaViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数
        public DepartmentDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }
        public DepartmentDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, DepartmentInfo department)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.department = department;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private DepartmentInfo department;
        public DepartmentInfo Department
        {
            get
            {
                return this.department;
            }
            set
            {
                this.department = value;
                this.NotifyPropertyChange(() => this.Department);
            }
        }
        #endregion

        public void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
        }
    }
}
