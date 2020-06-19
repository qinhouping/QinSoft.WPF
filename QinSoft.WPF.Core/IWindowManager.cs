using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace QinSoft.WPF.Core
{
    public interface IWindowManager
    {
        Window ShowWindow(object viewModel, IDictionary<string, object> setting = null);

        bool? ShowDialog(object viewModel, IDictionary<string, object> setting = null);
    }
}
