using EMChat2.ViewModel.Main;
using EMChat2.ViewModel.Main.Tabs.Chat;
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
    public class ShellViewModel : PropertyChangedBase
    {
        #region
        public ShellViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, BodyAreaViewModel bodyAreaViewModel, BottomAreaViewModel bottomAreaViewModel, TopAreaViewModel topAreaViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.applicationContextViewModel = applicationContextViewModel;
            this.bodyAreaViewModel = bodyAreaViewModel;
            this.bottomAreaViewModel = bottomAreaViewModel;
            this.topAreaViewModel = topAreaViewModel;
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
        #endregion
    }
}
