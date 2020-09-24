using EMChat2.Common;
using EMChat2.Model.Entity;
using EMChat2.Model.Event;
using EMChat2.Service;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.Log;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public LoginViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, MainViewModel MainViewModel, UserService userService, SystemService systemService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.MainViewModel = MainViewModel;
            this.userService = userService;
            this.systemService = systemService;

            LoadLoginInfo();
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
        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel
        {
            get
            {
                return this.mainViewModel;
            }
            set
            {
                this.mainViewModel = value;
                this.NotifyPropertyChange(() => this.MainViewModel);
            }
        }
        private UserService userService;
        private SystemService systemService;
        private LoginInfo oldLoginInfo;
        private LoginInfo loginInfo;
        public LoginInfo LoginInfo
        {
            get
            {
                return this.loginInfo;
            }
            set
            {
                this.loginInfo = value;
                this.NotifyPropertyChange(() => this.LoginInfo);
            }
        }
        private bool isLogging;
        public bool IsLogging
        {
            get
            {
                return this.isLogging;
            }
            set
            {
                this.isLogging = value;
                this.NotifyPropertyChange(() => this.IsLogging);
            }
        }
        #endregion

        #region 方法
        private async void LoadLoginInfo()
        {
            this.LoginInfo = await this.systemService.LoadLoginInfo();
            this.oldLoginInfo = this.LoginInfo.Clone();
            this.LoginInfo.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != "UserName") return;
                if (this.LoginInfo.UserName?.Equals(this.oldLoginInfo.UserName) != true)
                {
                    this.LoginInfo.HeaderImageUrl = null;

                }
                else
                {
                    this.LoginInfo.HeaderImageUrl = this.oldLoginInfo.HeaderImageUrl;
                }
            };

            if (this.loginInfo.IsAutoLogin)
            {
                this.LoginCommand.Execute(null);
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
                    this.IsLogging = true;
                    this.userService.Login(this.loginInfo);
                });
            }
        }

        public ICommand ChangeStateCommand
        {
            get
            {
                return new RelayCommand<UserStateEnum>((state) =>
                {
                    this.LoginInfo.State = state;
                });
            }
        }
        #endregion

        #region 事件处理
        public void Handle(LoginEventArgs arg)
        {
            IsLogging = false;
            if (arg.IsSuccess)
            {
                this.loginInfo.HeaderImageUrl = arg.Staff.HeaderImageUrl;
                this.systemService.StoreLoginInfo(this.LoginInfo);
                new Action(() => this.windowManager.HideWindow(this)).ExecuteInUIThread();
            }
            else
            {
                using (AlertViewModel alertViewModel = new AlertViewModel(this.windowManager, this.eventAggregator, arg.Message, "登录提示", AlertType.Error))
                {
                    new Action(() => this.windowManager.ShowDialog(alertViewModel)).ExecuteInUIThread();
                }
            }
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
