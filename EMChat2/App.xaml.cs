using EMChat2.Common;
using EMChat2.ViewModel;
using QinSoft.Ioc;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EMChat2
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //UI线程未捕获异常处理事件（UI主线程）
            this.DispatcherUnhandledException += App_DispatcherUnhandledException; ;
            //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            WindowInfoAttach.LoadwindowInfos();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            WindowInfoAttach.StorewindowInfos();

            base.OnExit(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(string.Format("App_DispatcherUnhandledException:{0}", e.Exception));
            e.Handled = true;
            ShowErrorInfo(e.Exception.Message);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(string.Format("CurrentDomain_UnhandledException:{0}", e.ExceptionObject));
            ShowErrorInfo(e.ExceptionObject.ToString());
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Debug.WriteLine(string.Format("TaskScheduler_UnobservedTaskException:{0}", e.Exception));
            ShowErrorInfo(e.Exception.Message);
        }

        private void ShowErrorInfo(string error, string title = "系统异常")
        {
            try
            {
                IWindowManager windowManager = ApplicationBooter.Current.IocApplicationContext.ObjectContainer.Get<WindowManagerImp>();
                new Action(() => windowManager.ShowDialog(new AlertViewModel(windowManager, error, title, AlertType.Error))).ExecuteInUIThread();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
