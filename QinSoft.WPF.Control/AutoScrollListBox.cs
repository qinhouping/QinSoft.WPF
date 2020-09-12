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
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace QinSoft.WPF.Control
{
    public class AutoScrollListBox : ListBox
    {
        public AutoScrollListBox()
        {
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
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
                Thread.Sleep(50);//TODO 通过延迟滚动来解决未渲染滚动不了的bug
                this.Dispatcher.Invoke(new Action(() =>
                {
                    GetScrollViewerChild()?.ScrollToEnd();
                }));
            });
        }
    }
}
