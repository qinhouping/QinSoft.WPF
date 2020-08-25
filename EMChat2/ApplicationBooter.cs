using EMChat2.ViewModel.Main.Tabs.Chat;
using EMChat2.ViewModel;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMChat2
{
    public class ApplicationBooter : ApplicationBase
    {
        public static ApplicationBooter Current { get; protected set; }

        public ApplicationBooter() : base()
        {
            Current = this;
            this.OnStartUp<ShellViewModel>();
        }
    }
}
