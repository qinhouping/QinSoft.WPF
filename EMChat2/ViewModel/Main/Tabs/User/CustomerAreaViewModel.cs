using EMChat2.Model.Event;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    [Component]
    public class CustomerAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public CustomerAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        #endregion

        #region 命令
        public ICommand OpenTagCustomerDetailCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.eventAggregator.PublishAsync(new DetailChangeEventArgs()
                    {
                        Type = DetailType.TagCustomer,
                        Data = null
                    });
                });
            }
        }
        #endregion
    }
}
