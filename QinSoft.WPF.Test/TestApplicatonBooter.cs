using QinSoft.WPF.Core;
using QinSoft.WPF.Test.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinSoft.WPF.Test
{
    public class TestApplicatonBooter : ApplicatonBooter
    {
        public TestApplicatonBooter() : base()
        {
            this.OnStartUp<MainViewModel>();
        }
    }
}
