using QinSoft.WPF.Core;
using QinSoft.WPF.Test.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinSoft.WPF.Test
{
    public class TestApplicationBooter : ApplicationBooter
    {
        public TestApplicationBooter() : base()
        {
            this.OnStartUp<MainViewModel>();
        }
    }
}
