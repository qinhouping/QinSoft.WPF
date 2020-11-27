using EMChat2.Common;
using EMChat2.Common.Cef;
using EMChat2.ViewModel;
using QinSoft.Event;
using QinSoft.Ioc;
using QinSoft.Log;
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
using System.Windows.Threading;

namespace EMChat2
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private bool showGloalError = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowGloalError"]);
        protected override void OnStartup(StartupEventArgs e)
        {
            //UI线程未捕获异常处理事件（UI主线程）
            this.DispatcherUnhandledException += App_DispatcherUnhandledException; ;
            //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //加载窗口信息
            WindowInfoAttach.LoadWindowInfos();

            //初始化垃圾回收
            GcTimerTools.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //释放垃圾回收
            GcTimerTools.Dispose();

            //存储窗口信息
            WindowInfoAttach.StoreWindowInfos();

            base.OnExit(e);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(string.Format("App_DispatcherUnhandledException:{0}", e.Exception));
            e.Handled = true;
            ShowErrorInfo(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(string.Format("CurrentDomain_UnhandledException:{0}", e.ExceptionObject));
            ShowErrorInfo(new Exception(e.ExceptionObject.ToString()));
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Debug.WriteLine(string.Format("TaskScheduler_UnobservedTaskException:{0}", e.Exception));
            ShowErrorInfo(e.Exception);
        }

        private void ShowErrorInfo(Exception error, string title = "系统异常")
        {
            try
            {
                if (error == null) return;
                IWindowManager windowManager = ApplicationBooter.Current.IocApplicationContext.ObjectContainer.Get<WindowManagerImp>();
                EventAggregator eventAggregator = ApplicationBooter.Current.IocApplicationContext.ObjectContainer.Get<QinSoft.WPF.Core.EventAggregatorImp>();
                error.Fatal();
                if (!showGloalError) return;
                using (AlertViewModel alertViewModel = new AlertViewModel(windowManager, eventAggregator, error.Message, title, AlertType.Error))
                {
                    new Action(() => windowManager.ShowDialog(alertViewModel)).ExecuteInUIThread();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format("发生未处理异常:{0}，请通知管理员！", e.Message));
            }
        }
    }
}
