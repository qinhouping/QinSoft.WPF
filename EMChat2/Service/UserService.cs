﻿using EMChat2.Model.Entity;
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
        public async void Login(LoginInfo loginInfo)
        {
            await eventAggregator.PublishAsync(new LoginEventArgs()
            {
                IsSuccess = true,
                StaffInfo = new StaffInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    WorkCode = "180366",
                    Name = "秦后平",
                    HeaderImageUrl = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=1764219719,2359539133&fm=26&gp=0.jpg",
                    ImUserId = "1111",
                    State = loginInfo.State
                }
            });
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
