using QinSoft.WPF.Core;
using QinSoft.WPF.Test.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinSoft.WPF.Test
{
    public class ApplicationBooter : ApplicationBase
    {
        public ApplicationBooter() : base()
        {
            this.OnStartUp<MainViewModel>();
        }
    }
}
