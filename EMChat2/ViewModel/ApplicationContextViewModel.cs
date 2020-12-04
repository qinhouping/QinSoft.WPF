using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
using EMChat2.Service;
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
    public class ApplicationContextViewModel : PropertyChangedBase, IEventHandle<LoginCallbackEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>, IEventHandle<ActiveApplicationEventArgs>, IEventHandle<DeactiveApplicationEventArgs>
    {
        #region 构造函数
        public ApplicationContextViewModel(IWindowManager windowManager, EventAggregator eventAggregator, UserService userService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.userService = userService;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private UserService userService;
        private StaffModel currentStaff;
        public StaffModel CurrentStaff
        {
            get
            {
                return this.currentStaff;
            }
            set
            {
                this.currentStaff = value;
                this.NotifyPropertyChange(() => this.CurrentStaff);
                this.NotifyPropertyChange(() => this.IsLogin);
            }
        }
        public bool IsLogin
        {
            get
            {
                return this.CurrentStaff != null;
            }
        }
        private bool isActived;
        public bool IsActived
        {
            get
            {
                return this.isActived;
            }
            set
            {
                this.isActived = value;
                this.NotifyPropertyChange(() => this.IsActived);
            }
        }
        private SettingModel setting;
        public SettingModel Setting
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

        #region 私有方法
        public async void LoadSetting()
        {
            this.Setting = await this.userService.LoadSetting(CurrentStaff);
        }
        #endregion

        #region 事件处理
        public void Handle(LoginCallbackEventArgs arg)
        {
            if (!arg.IsSuccess) return;
            this.CurrentStaff = arg.Staff;
            this.LoadSetting();
        }

        public void Handle(LogoutCallbackEventArgs arg)
        {
            this.CurrentStaff = null;
            this.Setting = null;
        }

        public void Handle(ExitCallbackEventArgs arg)
        {
            this.CurrentStaff = null;
            this.Setting = null;
        }

        public void Handle(ActiveApplicationEventArgs Message)
        {
            this.IsActived = true;
        }

        public void Handle(DeactiveApplicationEventArgs Message)
        {
            this.IsActived = false;
        }
        #endregion
    }
}
