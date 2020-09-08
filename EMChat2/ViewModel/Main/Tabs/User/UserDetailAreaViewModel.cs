using EMChat2.Model.Event;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    [Component]
    public class UserDetailAreaViewModel : PropertyChangedBase, IEventHandle<UserDetailEventArgs>
    {
        #region 构造函数
        public UserDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private UserDetailType userDetailType;
        public UserDetailType UserDetailType
        {
            get
            {
                return this.userDetailType;
            }
            set
            {
                this.userDetailType = value;
                this.NotifyPropertyChange(() => this.UserDetailType);
            }
        }
        private object detailData;
        public object DetailData
        {
            get
            {
                return this.detailData;
            }
            set
            {
                this.detailData = value;
                this.NotifyPropertyChange(() => this.DetailData);
            }
        }
        #endregion

        #region 事件处理

        public void Handle(UserDetailEventArgs arg)
        {
            this.UserDetailType = arg.Type;
            this.DetailData = arg.Data;
        }
        #endregion
    }
}
