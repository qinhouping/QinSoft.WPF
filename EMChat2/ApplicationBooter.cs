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
            if (Current != null) throw new InvalidProgramException("An instantiation of the applicationbooter object already exists");
            Current = this;
            this.OnStartUp<LoginViewModel>();
        }
    }
}
