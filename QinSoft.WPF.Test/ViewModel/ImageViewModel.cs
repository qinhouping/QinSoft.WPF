using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QinSoft.WPF.Test.ViewModel
{
    public class ImageViewModel : PropertyChangedBase
    {
        private string url;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                this.url = value;
                this.NotifyPropertyChange(() => this.Url);
            }
        }

        public ImageViewModel(string url)
        {
            this.url = url;
        }
    }
}
