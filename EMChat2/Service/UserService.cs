using EMChat2.Model.Event;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service
{
    [Component]
    public class UserService
    {
        #region 构造函数
        public UserService(EventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }
        #endregion

        #region 属性
        private EventAggregator eventAggregator;
        #endregion

        #region 方法
        public async void Login()
        {
            await eventAggregator.PublishAsync(new LoginEventArgs());
        }

        public async void Logout()
        {
            await eventAggregator.PublishAsync(new LogoutEventArgs());
        }

        public async void Exit()
        {
            await eventAggregator.PublishAsync(new ExitEventArgs());
        }
        #endregion
    }
}
