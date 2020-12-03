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
                Staff = new StaffModel()
                {
                    Id = "000003",
                    WorkCode = "000003",
                    Name = "测试账号",
                    HeaderImageUrl = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=1764219719,2359539133&fm=26&gp=0.jpg",
                    ImUserId = "4735344555340783734",
                    State = loginInfo.State,
                    BusinessList = new ObservableCollection<BusinessEnum>() {
                        BusinessEnum.Inside,
                        BusinessEnum.Advisor,
                        BusinessEnum.Expert
                    }
                },
                IMServer = new IMServerModel
                {
                    ApiUrl = "https://61.152.230.122:16060",
                    IP = "61.152.230.122",
                    Port = 17070
                },
                IMUser = new IMUserModel()
                {
                    Id = "4735344555340783734",
                    AppKey = "EMgUNSkPGf",
                    Token = "5eC3pgapWygNkVEOiyZFAxq77VpFNh3Z8ghzhKnWkNVqdGDHr1nOQaAITNUsgZGG3vDeVEcKQSIJNJhjvF5tTis6zy+Gs2ttkFtMIAv9RkJVs9FscJVIjzlXH8TnSlFTji8nBl3Et/92CeFoBSakAi8YzeduDxWiskeDYp1CEmCHv/o+K1DHOXbvQXqk7BN8ZjWflLByLhNdAC1gELu6Hvv8HzXlzXfIMVYJkDxyuSoJ5s/UfvSumsB/IBZcIWb3brZlFBQJ9ZFRtvqMeBBggA==",
                    RefreshToken = "bNqeO4yufVGZkcqZcEyvHRWcXyfcuf9dqsyKDYflQkmWSGtWT05m0tt/jlVFR8P56iPV5eKpbwbDwcPhPFy019MDmRGKWrelBq+KCc3QwIY="
                }
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
                    AllowRollBackMessage = true,
                    MaxRollbackMessageTotalMinutes = 2
                };
            }
            return setting;
        }
        #endregion
    }
}
