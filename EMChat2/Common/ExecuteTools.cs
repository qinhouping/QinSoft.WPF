using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EMChat2.Common
{
    public static class ExecuteTools
    {
        public static async Task ExecuteInTask(this Action action)
        {
            await Task.Factory.StartNew(action);
        }

        public static async Task<T> ExecuteInTask<T>(this Func<T> func)
        {
            return await Task.Factory.StartNew(func);
        }

        public static void ExecuteInThreadPool(this Action action)
        {
            ThreadPool.QueueUserWorkItem((x) =>
            {
                action.Invoke();
            });
        }

        public static Thread ExecuteInThread(this Action action, bool isBackground = true)
        {
            Thread thread = new Thread(new ThreadStart(action)) { IsBackground = isBackground };
            thread.Start();
            return thread;
        }

        public static void ExecuteInUIThread(this Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        public static void ActiveExecute(this ICommand command, object parameter = null)
        {
            if (command == null) return;
            if (command.CanExecute(parameter)) command.Execute(parameter);
        }
    }
}
