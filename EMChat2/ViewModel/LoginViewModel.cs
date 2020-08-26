using EMChat2.Common;
using EMChat2.Model.Event;
using EMChat2.Service;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.Log;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Input;

namespace EMChat2.ViewModel
{
    [Component]
    public class LoginViewModel : PropertyChangedBase, IEventHandle<LoginEventArgs>, IEventHandle<LogoutEventArgs>, IEventHandle<ExitEventArgs>
    {
        #region 构造函数
        public LoginViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, ShellViewModel shellViewModel, UserService userService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.shellViewModel = shellViewModel;
            this.userService = userService;
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
        private ShellViewModel shellViewModel;
        public ShellViewModel ShellViewModel
        {
            get
            {
                return this.shellViewModel;
            }
            set
            {
                this.shellViewModel = value;
                this.NotifyPropertyChange(() => this.ShellViewModel);
            }
        }
        private UserService userService;
        #endregion

        #region 命令
        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.windowManager.CloseWindow(this);
                });
            }
        }

        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.userService.Login();
                });
            }
        }
        #endregion

        #region 事件处理
        public void Handle(LoginEventArgs arg)
        {
            new Action(() => this.windowManager.HideWindow(this)).ExecuteInUIThread();
        }

        public void Handle(LogoutEventArgs arg)
        {
            new Action(() => this.windowManager.ShowWindow(this)).ExecuteInUIThread();
        }

        public void Handle(ExitEventArgs arg)
        {
            new Action(() => this.windowManager.CloseWindow(this)).ExecuteInUIThread();
        }
        #endregion
    }
}
