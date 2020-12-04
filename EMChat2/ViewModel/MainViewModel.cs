using EMChat2.Common;
using EMChat2.Common.Cef;
using EMChat2.Event;
using EMChat2.Service;
using EMChat2.ViewModel.Main;
using EMChat2.ViewModel.Main.Tabs.Chat;
using Hardcodet.Wpf.TaskbarNotification;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace EMChat2.ViewModel
{
    [Component]
    public class MainViewModel : PropertyChangedBase, IEventHandle<LoginCallbackEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>, IEventHandle<CaptureScreenEventArgs>, IEventHandle<ShowBalloonTipEventArgs>, IEventHandle<ReceiveMessageEventArgs>, IEventHandle<ActiveApplicationEventArgs>
    {
        #region 构造函数
        public MainViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, TopAreaViewModel topAreaViewModel, BodyAreaViewModel bodyAreaViewModel, BottomAreaViewModel bottomAreaViewModel, UserService userService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.topAreaViewModel = topAreaViewModel;
            this.bodyAreaViewModel = bodyAreaViewModel;
            this.bottomAreaViewModel = bottomAreaViewModel;
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
                    new Action(() => this.windowManager.ShowWindow(this)).ExecuteInUIThread();
                });
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    new Action(() => this.windowManager.HideWindow(this)).ExecuteInUIThread();
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

        public ICommand ActiveCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.eventAggregator.PublishAsync<ActiveApplicationEventArgs>(new ActiveApplicationEventArgs());
                });
            }
        }

        public ICommand DeactiveCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.eventAggregator.PublishAsync<DeactiveApplicationEventArgs>(new DeactiveApplicationEventArgs());
                });
            }
        }
        #endregion

        #region 事件处理
        public void Handle(LoginCallbackEventArgs arg)
        {
            if (!arg.IsSuccess) return;
            new Action(() => this.windowManager.ShowWindow(this)).ExecuteInUIThread();
        }

        public void Handle(LogoutCallbackEventArgs arg)
        {
            new Action(() => this.windowManager.HideWindow(this)).ExecuteInUIThread();
        }

        public void Handle(ExitCallbackEventArgs arg)
        {
            new Action(() => this.windowManager.CloseWindow(this)).ExecuteInUIThread();
        }

        public void Handle(CaptureScreenEventArgs arg)
        {
            if (arg.Action == CaptureScreenAction.Begin)
            {
                new Action(() => this.windowManager.HideWindow(this)).ExecuteInUIThread();
            }
            else if (arg.Action == CaptureScreenAction.End)
            {
                new Action(() => this.windowManager.ShowWindow(this)).ExecuteInUIThread();
            }
        }

        public void Handle(ShowBalloonTipEventArgs arg)
        {
            this.BalloonTip = arg.BalloonTip;
        }

        public void Handle(ReceiveMessageEventArgs arg)
        {
            if (this.BodyAreaViewModel.ChatTabAreaViewModel.ChatListAreaViewModel.TotalNotReadMessageCount > 0 && !ApplicationContextViewModel.IsActived)
            {
                this.IsFlash = true;
                this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo()
                    {
                        Icon = BalloonIcon.Info,
                        Title = "新消息",
                        Content = MessageTools.GetMessageContentMark(arg.Message)
                    }
                });
            }
        }

        public void Handle(ActiveApplicationEventArgs Message)
        {
            this.IsFlash = false;
        }
        #endregion
    }
}
