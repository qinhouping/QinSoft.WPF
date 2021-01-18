using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace QinSoft.WPF.Control
{
    public class AutoScrollListBox : ListBox
    {
        public static readonly DependencyProperty LoadProperty = DependencyProperty.Register("Load", typeof(ICommand), typeof(AutoScrollListBox));

        public ICommand Load
        {
            get
            {
                return this.GetValue(LoadProperty) as ICommand;
            }
            set
            {
                this.SetValue(LoadProperty, value);
            }
        }
        public AutoScrollListBox()
        {
            this.Loaded += AutoScrollListBox_Loaded;
        }

        #region 滚动加载
        private void AutoScrollListBox_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer scrollViewer = GetScrollViewerChild();
            if (scrollViewer == null) return;
            scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            if (scrollViewer.VerticalOffset == 0)
            {
                if (Load != null && Load.CanExecute(null)) Load.Execute(null);
                scrollViewer.ScrollToVerticalOffset(1);
            }
        }
        #endregion

        #region 自动滚动
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!this.IsMouseOver)
                ScrollToBottom();
        }

        protected virtual ScrollViewer GetScrollViewerChild()
        {
            if (VisualTreeHelper.GetChildrenCount(this) == 0) return null;
            Decorator decorator = VisualTreeHelper.GetChild(this, 0) as Decorator;
            return decorator?.Child as ScrollViewer;
        }

        protected virtual void ScrollToBottom()
        {
            Task.Factory.StartNew(() =>
            {
                lock (this)
                {
                    Thread.Sleep(50);//TODO 通过延迟滚动来解决未渲染滚动不了的bug
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        GetScrollViewerChild()?.ScrollToEnd();
                    }));
                }
            });
        }
        #endregion
    }
}
