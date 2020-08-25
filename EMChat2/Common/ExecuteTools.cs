using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EMChat2.Common
{
    public static class ExecuteTools
    {
        public static async void ExecuteInTask(this Action action)
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

        public static Thread ExecuteInThread(this Action action, string name = null, ThreadPriority priority = ThreadPriority.Normal, bool isBackground = true)
        {
            Thread thread = new Thread(new ThreadStart(action));
            thread.Name = name;
            thread.Priority = priority;
            thread.IsBackground = isBackground;
            thread.Start();
            return thread;
        }

        public static Thread ExecuteInThread(this Action<object> action, object arg, string name = null, ThreadPriority priority = ThreadPriority.Normal, bool isBackground = true)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(action));
            thread.Name = name;
            thread.Priority = priority;
            thread.IsBackground = isBackground;
            thread.Start(arg);
            return thread;
        }

        public static void ExecuteInUIThread(this Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }
    }
}
