using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QinSoft.WPF.Test.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            mcob.ItemsSource = new ThreadSafeObservableCollection<string>(new string[] { "hello", "world", "test" });
            mcob.SelectedItems = new ThreadSafeObservableCollection<string>(new string[] { "hello", "world" });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mcob.SelectedItems = new ThreadSafeObservableCollection<string>(new string[] { "hello", "world" });
        }
    }
}
