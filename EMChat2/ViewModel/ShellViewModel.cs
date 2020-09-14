using EMChat2.Common;
using EMChat2.Common.Cef;
using EMChat2.Model.Event;
using EMChat2.Service;
using EMChat2.ViewModel.Main;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace EMChat2.ViewModel
{
    [Component]
    public class ShellViewModel : PropertyChangedBase, IEventHandle<LoginEventArgs>, IEventHandle<LogoutEventArgs>, IEventHandle<ExitEventArgs>, IEventHandle<CaptureScreenEventArgs>
    {
        #region 构造函数
        public ShellViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, BodyAreaViewModel bodyAreaViewModel, BottomAreaViewModel bottomAreaViewModel, TopAreaViewModel topAreaViewModel, UserService userService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.bodyAreaViewModel = bodyAreaViewModel;
            this.bottomAreaViewModel = bottomAreaViewModel;
            this.topAreaViewModel = topAreaViewModel;
            this.userService = userService;

            //TODO 测试数据
            new Action(() =>
            {
                this.IsFlash = true;
                this.BalloonTip = new BalloonTipInfo()
                {
                    Title = "Test",
                    Content = "测试提醒",
                    Icon = Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Warning
                };
            }).ExecuteInUIThread();
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
        private BodyAreaViewModel bodyAreaViewModel;
        public BodyAreaViewModel BodyAreaViewModel
        {
            get
            {
                return this.bodyAreaViewModel;
            }
            set
            {
                this.bodyAreaViewModel = value;
                this.NotifyPropertyChange(() => this.BodyAreaViewModel);
            }
        }
        private BottomAreaViewModel bottomAreaViewModel;
        public BottomAreaViewModel BottomAreaViewModel
        {
            get
            {
                return this.bottomAreaViewModel;
            }
            set
            {
                this.bottomAreaViewModel = value;
                this.NotifyPropertyChange(() => this.BottomAreaViewModel);
            }
        }
        private TopAreaViewModel topAreaViewModel;
        public TopAreaViewModel TopAreaViewModel
        {
            get
            {
                return this.topAreaViewModel;
            }
            set
            {
                this.topAreaViewModel = value;
                this.NotifyPropertyChange(() => this.TopAreaViewModel);
            }
        }
        private UserService userService;
        private bool isFlash;
        public bool IsFlash
        {
            get
            {
                return this.isFlash;
            }
            set
            {
                this.isFlash = value;
                this.NotifyPropertyChange(() => this.IsFlash);
            }
        }
        private BalloonTipInfo balloonTip;
        public BalloonTipInfo BalloonTip
        {
            get
            {
                return this.balloonTip;
            }
            set
            {
                this.balloonTip = value;
                this.NotifyPropertyChange(() => this.BalloonTip);
            }
        }
        #endregion

        #region 命令
        public ICommand OpenCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.windowManager.ShowWindow(this);
                });
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.windowManager.HideWindow(this);
                });
            }
        }

        public ICommand LogoutCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.userService.Logout();
                });
            }
        }

        public ICommand ExitCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.userService.Exit();
                });
            }
        }

        public ICommand ToggleTopmostCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.ApplicationContextViewModel.Setting.IsTopmostShellView = !this.ApplicationContextViewModel.Setting.IsTopmostShellView;
                });
            }
        }
        #endregion

        #region 事件处理
        public void Handle(LoginEventArgs arg)
        {
            if (!arg.IsSuccess) return;
            new Action(() => this.windowManager.ShowWindow(this)).ExecuteInUIThread();
        }

        public void Handle(LogoutEventArgs arg)
        {
            new Action(() => this.windowManager.HideWindow(this)).ExecuteInUIThread();
        }

        public void Handle(ExitEventArgs arg)
        {
            new Action(() => this.windowManager.CloseWindow(this)).ExecuteInUIThread();
        }

        public void Handle(CaptureScreenEventArgs arg)
        {
            if (arg.Action == CaptureScreenAction.Begin)
            {
                this.windowManager.HideWindow(this);
            }
            else if (arg.Action == CaptureScreenAction.End)
            {
                this.windowManager.ShowWindow(this);
            }
        }
        #endregion
    }
}
