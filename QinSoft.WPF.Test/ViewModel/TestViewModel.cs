using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace QinSoft.WPF.Test.ViewModel
{
    [Component]
    public class TestViewModel : PropertyChangedBase, IEventHandle<string>
    {
        private EventAggregator eventAggregator;
        private string content;
        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
                this.NotifyPropertyChange(() => this.Content);
            }
        }
        public TestViewModel(EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }

        public void Handle(string Arg)
        {
            Thread.Sleep(1000);
            this.Content = Arg;
        }
    }
}
