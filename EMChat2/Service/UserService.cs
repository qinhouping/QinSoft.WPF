using EMChat2.Model.BaseInfo;
using EMChat2.Model.Event;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async void Login(LoginInfo loginInfo)
        {
            await Task.Delay(1000);
            await eventAggregator.PublishAsync(new LoginCallbackEventArgs()
            {
                IsSuccess = true,
                Staff = new StaffInfo()
                {
                    Id = "180366",
                    WorkCode = "180366",
                    Name = "秦后平",
                    HeaderImageUrl = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=1764219719,2359539133&fm=26&gp=0.jpg",
                    ImUserId = "4735344555340783734",
                    State = loginInfo.State,
                    BusinessList = new ObservableCollection<BusinessEnum>() {
                        BusinessEnum.Inside,
                        BusinessEnum.Advisor,
                        BusinessEnum.Expert
                    }
                },
                IMServerInfo = new IMServerInfo
                {
                    ApiUrl = "https://61.152.230.122:16060",
                    IP = "61.152.230.122",
                    Port = 17070
                },
                IMUserInfo = new IMUserInfo()
                {
                    Id = "4735344555340783734",
                    AppKey = "EMgUNSkPGf",
                    Token = "LelmsBHVpZXoRmrLUn/L0a3MktjsJf/6OOpkir1+9RPoMHfW7CvE9P4la25TgGztLFBe62cY/MXOm1DGc5oV1M05/+8Q+oByQERh63qB0NtaTm+gcyUBKK0HpsS3sYYnB1YmT2205eOzj0/u06MzqvQ5CdkRnk6SaXHC1DJYLtCH8kb7tCvKH9k4X28UYOxNKFHwdvClsYNeIG7eTkBbobqtilI2jf1mCAGx/HhzgEFkvMUG7ecPmC1G+oVUabYH/Mup29mgNqWac9AVxBa+0A==",
                    RefreshToken = "s3cGDtPj3KXDQb2IAEI0OffA974zlOFoA+st54wd7o9IVPpMlYvTFVKCiVB5L/bkaU/v01oRBJbdyIqnNNk3jBJa7uCMq3sAGKvrZHrFKUo="
                }
            });
        }

        public async void Logout()
        {
            await eventAggregator.PublishAsync(new LogoutCallbackEventArgs());
        }

        public async void Exit()
        {
            await eventAggregator.PublishAsync(new ExitCallbackEventArgs());
        }

        public SettingInfo LoadSetting(StaffInfo staff)
        {
            if (staff == null) throw new ArgumentNullException("staff");
            //测试数据
            SettingInfo setting = new SettingInfo();

            foreach (BusinessEnum business in staff.BusinessList)
            {
                setting.BusinessSettings[business] = new BusinessSettingInfo()
                {
                    AllowSendMessage = true,
                    AllowInputText = true,
                    AllowCaptureScreen = true,
                    AllowSelectFile = true,
                    AllowSelectImage = true,
                    AllowSelectQuickReply = true,
                    AllowRollBackMessage = true,
                    MaxRollbackMessageTotalMinutes = 2
                };
            }
            return setting;
        }
        #endregion
    }
}
