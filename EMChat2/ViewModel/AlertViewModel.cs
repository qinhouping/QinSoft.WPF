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
    public class AlertViewModel : PropertyChangedBase
    {
        #region 构造函数

        [Constructor]
        public AlertViewModel(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
        }

        public AlertViewModel(IWindowManager windowManager, string content, string title = "系统提示", AlertType alertType = AlertType.Info)
        {
            this.windowManager = windowManager;
            this.title = title;
            this.content = content;
            this.alertType = alertType;
        }

        #endregion

        #region 属性
        private IWindowManager windowManager;
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
                    this.windowManager.CloseWindow(this, true);
                });
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.windowManager.CloseWindow(this, null);
                });
            }
        }
        #endregion
    }

    public enum AlertType
    {
        Success,
        Info,
        Warning,
        Error
    }
}
