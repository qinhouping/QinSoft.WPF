using EMChat2.Common;
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
    public class ShellViewModel : PropertyChangedBase, IEventHandle<LoginEventArgs>, IEventHandle<LogoutEventArgs>, IEventHandle<ExitEventArgs>
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
        #endregion

        #region 命令
        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    //this.windowManager.HideWindow(this);
                    this.userService.Exit();
                });
            }
        }
        #endregion

        #region 事件处理
        public void Handle(LoginEventArgs arg)
        {
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
        #endregion
    }
}
