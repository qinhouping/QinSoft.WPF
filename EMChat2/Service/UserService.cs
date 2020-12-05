using EMChat2.Model.BaseInfo;
using EMChat2.Event;
using EMChat2.Model.IM;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMChat2.Common;

namespace EMChat2.Service
{
    /// <summary>
    /// 用户服务，负责用户信息相关逻辑
    /// </summary>
    [Component]
    public class UserService
    {
        #region 构造函数
        public UserService(IWindowManager windowManager, EventAggregator eventAggregator)
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

        #region 方法
        public virtual async void Login(LoginInfoModel loginInfo)
        {
            await this.eventAggregator.PublishAsync(new LoginCallbackEventArgs()
            {
                IsSuccess = true,
                IMServer = TestData.TestIMServer,
                Staff = TestData.Current.TestStaff,
                IMUser = TestData.Current.TestIMUser
            });
        }

        public virtual async void Logout()
        {
            await eventAggregator.PublishAsync(new LogoutCallbackEventArgs());
        }

        public virtual async void Exit()
        {
            await eventAggregator.PublishAsync(new ExitCallbackEventArgs());
        }

        public virtual async Task<SettingModel> LoadSetting(StaffModel staff)
        {
            if (staff == null) throw new ArgumentNullException("staff");

            await Task.Delay(1000);

            //TODO 测试数据
            SettingModel setting = new SettingModel();

            foreach (BusinessEnum business in staff.BusinessList)
            {
                setting.BusinessSettings[business] = new BusinessSettingModel()
                {
                    AllowSendMessage = true,
                    AllowInputText = true,
                    AllowCaptureScreen = true,
                    AllowSelectFile = true,
                    AllowSelectImage = true,
                    AllowSelectQuickReply = true,
                    AllowRevokeMessage = true,
                    MaxRollbackMessageTotalMinutes = 2
                };
            }
            return setting;
        }
        #endregion
    }
}
