using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace EMChat2.Common
{
    public static class WindowInfoAttach
    {
        #region 属性
        private static string windoInfoFilePath = ConfigurationManager.AppSettings["WindowInfoFilePath"];
        private static IList<WindowInfo> windowInfos = new List<WindowInfo>();
        public static readonly DependencyProperty WindowInfoProperty =
              DependencyProperty.RegisterAttached("WindowInfo", typeof(string), typeof(WindowInfoAttach), new PropertyMetadata(OnWindowInfoPropertyChangedCallback));
        public static void SetWindowInfo(DependencyObject dp, string value)
        {
            dp.SetValue(WindowInfoProperty, value);
        }
        public static string GetWindowInfo(DependencyObject dp)
        {
            return dp.GetValue(WindowInfoProperty) as string;
        }
        #endregion

        #region 方法
        private static void OnWindowInfoPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window window = d as Window;
            if (window == null) return;
            string windowKey = GetWindowInfo(window);
            if (string.IsNullOrEmpty(windowKey)) windowKey = window.GetType().FullName;
            WindowInfo windowInfo = windowInfos.FirstOrDefault(u => u.Key.Equals(windowKey));
            if (windowInfo == null)
            {
                windowInfo = new WindowInfo() { Key = windowKey };
                windowInfos.Add(windowInfo);
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.Manual;
                window.Left = windowInfo.Left;
                window.Top = windowInfo.Top;
                window.Width = windowInfo.Width;
                window.Height = windowInfo.Height;
                window.WindowState = windowInfo.State;
            }
            window.StateChanged += Window_StateChanged;
            window.SizeChanged += Window_SizeChanged;
            window.LocationChanged += Window_LocationChanged;

            window.Closed += Window_Closed;
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            Window window = sender as Window;
            window.StateChanged -= Window_StateChanged;
            window.SizeChanged -= Window_SizeChanged;
            window.LocationChanged -= Window_LocationChanged;
        }

        private static void Window_LocationChanged(object sender, EventArgs e)
        {
            Window window = sender as Window;
            string windowKey = GetWindowInfo(window);
            if (string.IsNullOrEmpty(windowKey)) windowKey = window.GetType().FullName;
            WindowInfo windowInfo = windowInfos.FirstOrDefault(u => u.Key.Equals(windowKey));
            windowInfo.Top = window.Top;
            windowInfo.Left = window.Left;
        }

        private static void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Window window = sender as Window;
            string windowKey = GetWindowInfo(window);
            if (string.IsNullOrEmpty(windowKey)) windowKey = window.GetType().FullName;
            WindowInfo windowInfo = windowInfos.FirstOrDefault(u => u.Key.Equals(windowKey));
            windowInfo.Width = window.Width;
            windowInfo.Height = window.Height;
        }

        private static void Window_StateChanged(object sender, EventArgs e)
        {
            Window window = sender as Window;
            string windowKey = GetWindowInfo(window);
            if (string.IsNullOrEmpty(windowKey)) windowKey = window.GetType().FullName;
            WindowInfo windowInfo = windowInfos.FirstOrDefault(u => u.Key.Equals(windowKey));
            windowInfo.State = window.WindowState;
        }

        public static void LoadwindowInfos()
        {
            windowInfos = windoInfoFilePath.FileToStream().StreamToString().JsonToObject<List<WindowInfo>>() ?? new List<WindowInfo>().ToList();
        }

        public static void StorewindowInfos()
        {
            windowInfos.ObjectToJson().StringToStream().StreamToFile(windoInfoFilePath);
        }
        #endregion
    }

    public class WindowInfo
    {
        public string Key { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public WindowState State { get; set; }
    }
}
