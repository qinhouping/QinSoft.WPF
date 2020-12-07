using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace QinSoft.WPF.Core
{
    public delegate void RoutedEventHandler<T>(object sender, T args) where T : RoutedEventArgs;
}
