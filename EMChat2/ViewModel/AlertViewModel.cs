using EMChat2.Common;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EMChat2.ViewModel
{
    [Component]
    public class AlertViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数

        [Constructor]
        public AlertViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }

        public AlertViewModel(IWindowManager windowManager, EventAggregator eventAggregator, string content, string title = "系统提示", AlertType alertType = AlertType.Info)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.title = title;
            this.content = content;
            this.alertType = alertType;
        }

        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private string title = "系统提示";
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                this.NotifyPropertyChange(() => this.Title);
            }
        }

        private string content;
        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
                this.NotifyPropertyChange(() => this.Content);
            }
        }

        private AlertType alertType = AlertType.Info;
        public AlertType AlertType
        {
            get
            {
                return this.alertType;
            }
            set
            {
                this.alertType = value;
                this.NotifyPropertyChange(() => this.AlertType);
            }
        }


        #endregion

        #region 命令
        public ICommand ConfirmCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    new Action(() => this.windowManager.CloseWindow(this, true)).ExecuteInUIThread();
                });
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    new Action(() => this.windowManager.CloseWindow(this, null)).ExecuteInUIThread();
                });
            }
        }
        #endregion

        public void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
        }
    }

    public enum AlertType
    {
        Success,
        Info,
        Warning,
        Error
    }
}
