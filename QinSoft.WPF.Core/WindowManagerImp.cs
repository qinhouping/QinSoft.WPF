using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using QinSoft.Ioc.Attribute;

namespace QinSoft.WPF.Core
{
    [Component]
    public class WindowManagerImp : IWindowManager
    {
        private IList<Window> windows = null;

        public WindowManagerImp()
        {
            this.windows = new List<Window>();
        }

        public bool? ShowDialog(object viewModel, bool isSingleton = true, IDictionary<string, object> setting = null)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (isSingleton)
            {
                Window[] wins = this.GetWindows(viewModel);
                if (wins.Count() > 0)
                {
                    Window win = wins[0];
                    return win.ShowDialog();
                }
            }
            Type viewModelType = viewModel.GetType();
            Type winType = GetWindowType(viewModelType);
            if (winType == null) throw new Exception(string.Format("can not find view of viewModel({0})", viewModelType.FullName));
            Window window = CreateWindow(winType);
            InitWindow(window, viewModel, setting);
            return window.ShowDialog();
        }

        public Window ShowWindow(object viewModel, bool isSingleton = true, IDictionary<string, object> setting = null)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            if (isSingleton)
            {
                Window[] wins = this.GetWindows(viewModel);
                if (wins.Count() > 0)
                {
                    Window win = wins[0];
                    win.Show();
                    return win;
                }
            }
            Type viewModelType = viewModel.GetType();
            Type winType = GetWindowType(viewModelType);
            if (winType == null) throw new Exception(string.Format("can not find view of viewModel({0})", viewModelType.FullName));
            Window window = CreateWindow(winType);
            InitWindow(window, viewModel, setting);
            window.Show();
            return window;
        }

        protected virtual Type GetWindowType(Type viewModelType)
        {
            if (viewModelType == null) throw new ArgumentNullException("viewModelType");
            string winTypeFullName = Regex.Replace(viewModelType.FullName, "ViewModel", "View", RegexOptions.IgnoreCase);
            return AppDomain.CurrentDomain.GetAssemblies().Select(u => u.GetType(winTypeFullName, false, true)).Where(u => u != null).FirstOrDefault();
        }

        protected virtual Window CreateWindow(Type winType)
        {
            if (winType == null) throw new ArgumentNullException("winType");
            Window window = Activator.CreateInstance(winType) as Window;
            return window;
        }

        protected virtual Window GetOwner()
        {
            return this.windows.LastOrDefault(u => u.IsVisible == true && u.IsActive == true);
        }

        protected virtual void InitWindow(Window window, object dataContext, IDictionary<string, object> setting)
        {
            if (window == null) throw new ArgumentNullException("window");
            window.Owner = GetOwner();
            window.DataContext = dataContext;
            Type winType = window.GetType();
            if (setting != null)
            {
                foreach (KeyValuePair<string, object> set in setting)
                {
                    PropertyInfo propertyInfo = winType.GetProperty(set.Key);
                    if (propertyInfo != null && propertyInfo.CanWrite) propertyInfo.SetValue(window, set.Value, null);
                }
            }
            this.windows.Add(window);
            window.Closed += (s, e) =>
            {
                this.windows.Remove(s as Window);
            };
        }

        protected Window[] GetWindows(object viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            return this.windows.Where(u => viewModel.Equals(u.DataContext)).ToArray();
        }

        public void HideWindow(object viewModel)
        {
            foreach (Window win in GetWindows(viewModel))
            {
                win.Hide();
            }
        }

        public void CloseWindow(object viewModel, bool? dialogResult)
        {
            foreach (Window win in GetWindows(viewModel))
            {
                win.DialogResult = dialogResult;
                win.Close();
            }
        }

        public void CloseWindow(object viewModel)
        {
            foreach (Window win in GetWindows(viewModel))
            {
                win.Close();
            }
        }
    }
}
