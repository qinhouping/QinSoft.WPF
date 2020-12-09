﻿using EMChat2.Common;
using EMChat2.Service;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Bottom
{
    [Component]
    public class BottomSettingAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public BottomSettingAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, SettingViewModel settingViewModel, UserService userService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.settingViewModel = settingViewModel;
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
        private SettingViewModel settingViewModel;
        public SettingViewModel SettingViewModel
        {
            get
            {
                return this.settingViewModel;
            }
            set
            {
                this.settingViewModel = value;
                this.NotifyPropertyChange(() => this.SettingViewModel);
            }
        }
        private UserService userService;
        #endregion

        #region 命令
        public ICommand OpenSettingCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    new Action(() => this.windowManager.ShowDialog(this.SettingViewModel)).ExecuteInUIThread();
                });
            }
        }
        #endregion
    }
}
