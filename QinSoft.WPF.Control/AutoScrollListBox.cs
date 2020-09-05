using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace QinSoft.WPF.Control
{
    public class AutoScrollListBox : ListBox
    {
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            AutoScroll();
            base.OnItemsChanged(e);
        }

        protected void AutoScroll()
        {
            Decorator decorator = (Decorator)VisualTreeHelper.GetChild(this, 0);
            ScrollViewer scrollViewer = (ScrollViewer)decorator.Child;
            if (!this.IsFocused)
                scrollViewer.ScrollToVerticalOffset(100);
        }
    }
}
