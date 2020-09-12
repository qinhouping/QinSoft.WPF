using EMChat2.Common;
using EMChat2.Model.Entity;
using EMChat2.Model.Event;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMChat2.ViewModel
{
    [Component]
    public class ApplicationContextViewModel : PropertyChangedBase, IEventHandle<LoginEventArgs>, IEventHandle<LogoutEventArgs>
    {
        #region 构造函数
        public ApplicationContextViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);

            //TODO 测试数据
            new Action(() =>
            {
                this.Setting = new SettingInfo();
            }).ExecuteInUIThread();
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private StaffInfo currentStaff;
        public StaffInfo CurrentStaff
        {
            get
            {
                return this.currentStaff;
            }
            set
            {
                this.currentStaff = value;
                this.NotifyPropertyChange(() => this.CurrentStaff);
            }
        }
        private SettingInfo setting;
        public SettingInfo Setting
        {
            get
            {
                return this.setting;
            }
            set
            {
                this.setting = value;
                this.NotifyPropertyChange(() => this.Setting);
            }
        }
        #endregion

        #region 事件处理
        public void Handle(LoginEventArgs arg)
        {
            if (!arg.IsSuccess) return;
            this.CurrentStaff = arg.Staff;
        }

        public void Handle(LogoutEventArgs arg)
        {
            this.currentStaff = null;
        }
        #endregion
    }
}
