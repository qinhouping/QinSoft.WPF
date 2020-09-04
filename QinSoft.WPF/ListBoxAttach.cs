using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace QinSoft.WPF
{
    public static class ListBoxAttach
    {

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList), typeof(ListBoxAttach), new PropertyMetadata(OnSelectedItemsChanged));
        public static IList GetSelectedItems(DependencyObject obj)
        {
            return obj.GetValue(SelectedItemsProperty) as IList;
        }

        public static void SetSelectedItems(DependencyObject obj, IList value)
        {
            obj.SetValue(SelectedItemsProperty, value);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ListBox listBox = d as ListBox;
            if (listBox == null) return;
            listBox.SelectionChanged -= ListBox_SelectionChanged;
            listBox.SelectedItems.Clear();
            if ((e.NewValue as IList) != null)
            {
                foreach (object item in e.NewValue as IList)
                {
                    listBox.SelectedItems.Add(item);
                }
            }
            listBox.SelectionChanged += ListBox_SelectionChanged;
        }

        private static void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IList source = GetSelectedItems(sender as DependencyObject);
            if (source == null) return;

            foreach (var item in e.AddedItems) source.Add(item);

            foreach (var item in e.RemovedItems) source.Remove(item);
        }
    }
}
