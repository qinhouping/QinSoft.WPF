using EMChat2.Common;
using EMChat2.ViewModel;
using QinSoft.WPF.Core;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EMChat2.View
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            (this.DataContext as MainViewModel).CloseCommand.ActiveExecute();
        }
    }
}
