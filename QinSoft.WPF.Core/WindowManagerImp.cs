﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using QinSoft.Ioc.Attribute;

namespace QinSoft.WPF.Core
{
    [Component]
    public class WindowManagerImp : IWindowManager
    {
        private IList<Window> windows;

        public WindowManagerImp()
        {
            this.windows = new List<Window>();
        }

        public bool? ShowDialog(object viewModel, IDictionary<string, object> setting)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
            Type viewModelType = viewModel.GetType();
            Type winType = GetWindowType(viewModelType);
            if (winType == null) throw new Exception(string.Format("can not find view of viewModel({0})", viewModelType.FullName));
            Window window = CreateWindow(winType);
            InitWindow(window, viewModel, setting);
            return window.ShowDialog();
        }

        public Window ShowWindow(object viewModel, IDictionary<string, object> setting)
        {
            if (viewModel == null) throw new ArgumentNullException("viewModel");
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
            string winTypeFullName = viewModelType.FullName.Replace("ViewModel", "View");
            return AppDomain.CurrentDomain.GetAssemblies().Select(u => u.GetType(winTypeFullName)).Where(u => u != null).FirstOrDefault();
        }

        protected virtual Window CreateWindow(Type winType)
        {
            Window window = Activator.CreateInstance(winType) as Window;
            return window;
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
                    if (propertyInfo != null) propertyInfo.SetValue(window, set.Value, null);
                }
            }
            this.windows.Add(window);
            window.Closed += (s, e) =>
            {
                this.windows.Remove(s as Window);
            };
        }

        protected virtual Window GetOwner()
        {
            return this.windows.LastOrDefault(u => u.Owner == null);
        }
    }
}
