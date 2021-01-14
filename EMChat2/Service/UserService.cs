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
using EMChat2.Model.Enum;
using Hardcodet.Wpf.TaskbarNotification;

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
            string error = null;
            StaffModel staff = null;
            IMServerModel imServer = null;
            IMUserModel imUser = null;

            bool isSuccess = await new Func<bool>(() => ApiTools.LoginStaff(loginInfo.UserName, loginInfo.Password, out error, out staff, out imServer, out imUser)).ExecuteInTask();

            await this.eventAggregator.PublishAsync(new LoginCallbackEventArgs()
            {
                IsSuccess = isSuccess,
                Message = error,
                Staff = staff,
                IMServer = imServer,
                IMUser = imUser
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

            foreach (BusinessModel business in staff.Businesses)
            {
                setting.BusinessSettings.Add(new BusinessSettingModel()
                {
                    BusinessId = business.Id,
                    AllowSendMessage = true,
                    AllowInputText = true,
                    AllowCaptureScreen = true,
                    AllowSelectFile = true,
                    AllowSelectImage = true,
                    AllowSelectQuickReply = true,
                    AllowRevokeMessage = true,
                    MaxRollbackMessageTotalMinutes = 2
                });
            }
            return setting;
        }

        public virtual async void StoreSetting(SettingModel setting)
        {
            await Task.Delay(1000);
        }

        public virtual async Task<IEnumerable<DepartmentModel>> GetDepartments(StaffModel staff)
        {
            if (staff == null) return null;
            BusinessModel business = staff.Businesses.FirstOrDefault(u => !u.Outside);
            if (business == null) return null;

            if (ApiTools.GetDepartments(business.Id, staff.Id, out string error, out IEnumerable<DepartmentModel> departments))
            {
                return departments;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "获取部门失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return null;
            }
        }

        public virtual async Task<IEnumerable<QuickReplyGroupModel>> GetQuickReplyGroups(StaffModel staff)
        {
            if (staff == null) return null;
            if (ApiTools.GetQuickReplyGroups(staff.Id, out string error, out IEnumerable<QuickReplyGroupModel> quickReplyGroups))
            {
                return quickReplyGroups;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "获取快捷回复失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return null;
            }
        }

        public virtual async Task<bool> AddQuickReplyGroup(StaffModel staff, QuickReplyGroupModel quickReplyGroup)
        {
            if (staff == null || quickReplyGroup == null) return false;
            if (ApiTools.AddQuickReplyGroup(staff.Id, quickReplyGroup, out string error, out string id))
            {
                quickReplyGroup.Id = id;
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "增加快捷回复组失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
        }

        public virtual async Task<bool> ModifyQuickReplyGroup(StaffModel staff, QuickReplyGroupModel quickReplyGroup)
        {
            if (staff == null || quickReplyGroup == null) return false;
            if (ApiTools.ModifyQuickReplyGroup(staff.Id, quickReplyGroup, out string error))
            {
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "修改快捷回复组失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
        }

        public virtual async Task<bool> RemoveQuickReplyGroup(StaffModel staff, QuickReplyGroupModel quickReplyGroup)
        {
            if (staff == null || quickReplyGroup == null) return false;
            if (ApiTools.RemoveQuickReplyGroup(staff.Id, quickReplyGroup.Id, out string error))
            {
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "移除快捷回复组失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
        }

        public virtual async Task<bool> AddQuickReply(QuickReplyGroupModel quickReplyGroup, QuickReplyModel quickReply)
        {
            if (quickReplyGroup == null || quickReply == null) return false;
            if (ApiTools.AddQuickReply(quickReplyGroup.Id, quickReply, out string error, out string id))
            {
                quickReply.Id = id;
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "增加快捷回复失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
        }

        public virtual async Task<bool> ModifyQuickReply(QuickReplyGroupModel quickReplyGroup, QuickReplyModel quickReply)
        {
            if (quickReplyGroup == null || quickReply == null) return false;
            if (ApiTools.ModifyQuickReply(quickReplyGroup.Id, quickReply, out string error))
            {
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "修改快捷回复失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
        }

        public virtual async Task<bool> RemoveQuickReply(QuickReplyGroupModel quickReplyGroup, QuickReplyModel quickReply)
        {
            if (quickReplyGroup == null || quickReplyGroup == null) return false;
            if (ApiTools.RemoveQuickReply(quickReplyGroup.Id, quickReply.Id, out string error))
            {
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "移除快捷回复失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
        }
        #endregion
    }
}
