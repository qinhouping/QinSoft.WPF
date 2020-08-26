using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace QinSoft.WPF.Core
{
    public interface IWindowManager
    {
        Window ShowWindow(object viewModel, bool isSingleton = true, IDictionary<string, object> setting = null);

        bool? ShowDialog(object viewModel, bool isSingleton = true, IDictionary<string, object> setting = null);

        void HideWindow(object viewModel);

        void CloseWindow(object viewModel, bool? dialogResult);

        void CloseWindow(object viewModel);
    }
}
