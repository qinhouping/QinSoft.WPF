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

            bool success = await new Func<bool>(() => ApiTools.LoginStaff(loginInfo.UserName, loginInfo.Password, out error, out staff, out imServer, out imUser)).ExecuteInTask();

            await this.eventAggregator.PublishAsync(new LoginCallbackEventArgs()
            {
                IsSuccess = success,
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
            string error = null;
            IEnumerable<DepartmentModel> departments = null;
            bool success = await new Func<bool>(() => ApiTools.GetDepartments(business.Id, staff.Id, out error, out departments)).ExecuteInTask();
            if (success)
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
            string error = null;
            IEnumerable<QuickReplyGroupModel> quickReplyGroups = null;
            bool success = await new Func<bool>(() => ApiTools.GetQuickReplyGroups(staff.Id, out error, out quickReplyGroups)).ExecuteInTask();
            if (success)
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
            string error = null;
            string quickReplyGroupId = null;
            bool success = await new Func<bool>(() => ApiTools.AddQuickReplyGroup(staff.Id, quickReplyGroup, out error, out quickReplyGroupId)).ExecuteInTask();
            if (success)
            {
                quickReplyGroup.Id = quickReplyGroupId;
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
            string error = null;
            bool success = await new Func<bool>(() => ApiTools.ModifyQuickReplyGroup(staff.Id, quickReplyGroup, out error)).ExecuteInTask();
            if (success)
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
            string error = null;
            bool success = await new Func<bool>(() => ApiTools.RemoveQuickReplyGroup(staff.Id, quickReplyGroup.Id, out error)).ExecuteInTask();
            if (success)
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
            string error = null;
            string quickReplyId = null;
            bool success = await new Func<bool>(() => ApiTools.AddQuickReply(quickReplyGroup.Id, quickReply, out error, out quickReplyId)).ExecuteInTask();
            if (success)
            {
                quickReply.Id = quickReplyId;
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
            string error = null;
            bool success = await new Func<bool>(() => ApiTools.ModifyQuickReply(quickReplyGroup.Id, quickReply, out error)).ExecuteInTask();
            if (success)
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
            string error = null;
            bool success = await new Func<bool>(() => ApiTools.RemoveQuickReply(quickReplyGroup.Id, quickReply.Id, out error)).ExecuteInTask();
            if (success)
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

        public virtual async Task<IEnumerable<TagGroupModel>> GetTagGroups(StaffModel staff)
        {
            if (staff == null) return null;
            string error = null;
            IEnumerable<TagGroupModel> tagGroups = null;
            bool success = await new Func<bool>(() => ApiTools.GetTagGroups(staff.Id, out error, out tagGroups)).ExecuteInTask();
            if (success)
            {
                return tagGroups;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "获取标签组失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return null;
            }
        }

        public virtual async Task<bool> AddTagGroup(StaffModel staff, TagGroupModel TagGroup)
        {
            if (staff == null || TagGroup == null) return false;
            string error = null;
            string tagGroupId = null;
            bool success = await new Func<bool>(() => ApiTools.AddTagGroup(staff.Id, TagGroup, out error, out tagGroupId)).ExecuteInTask();
            if (success)
            {
                TagGroup.Id = tagGroupId;
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "增加标签组失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
        }

        public virtual async Task<bool> ModifyTagGroup(StaffModel staff, TagGroupModel TagGroup)
        {
            if (staff == null || TagGroup == null) return false;
            string error = null;
            bool success = await new Func<bool>(() => ApiTools.ModifyTagGroup(staff.Id, TagGroup, out error)).ExecuteInTask();
            if (success)
            {
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "修改标签组失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
        }

        public virtual async Task<bool> RemoveTagGroup(StaffModel staff, TagGroupModel TagGroup)
        {
            if (staff == null || TagGroup == null) return false;
            string error = null;
            bool success = await new Func<bool>(() => ApiTools.RemoveTagGroup(staff.Id, TagGroup.Id, out error)).ExecuteInTask();
            if (success)
            {
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "移除标签组失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
        }

        public virtual async Task<IEnumerable<ChatModel>> GetChats(StaffModel staff)
        {
            if (staff == null) return null;
            string error = null;
            IEnumerable<ChatModel> chats = null;
            bool success = await new Func<bool>(() => ApiTools.GetChats(staff.Id, out error, out chats)).ExecuteInTask();
            if (success)
            {
                return chats;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "获取会话列表失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return null;
            }
        }

        public virtual async Task<ChatModel> OpenChat(StaffModel staff, string chatId)
        {
            if (staff == null || string.IsNullOrEmpty(chatId)) return null;
            string error = null;
            ChatModel chat = null;
            bool success = await new Func<bool>(() => ApiTools.OpenChat(staff.Id, chatId, out error, out chat)).ExecuteInTask();
            if (success)
            {
                return chat;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "打开会话失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return null;
            }
        }

        public virtual async Task<bool> CloseChat(StaffModel staff, ChatModel chat)
        {

            if (staff == null || chat == null) return false;
            string error = null;
            bool success = await new Func<bool>(() => ApiTools.CloseChat(staff.Id, chat.Id, out error)).ExecuteInTask();
            if (success)
            {
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "关闭会话失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
        }

        public virtual async Task<ChatModel> CreateChat(StaffModel staff, ChatModel chat)
        {
            if (staff == null || chat == null) return null;
            string error = null;
            bool success = await new Func<bool>(() => ApiTools.CreateChat(staff.Id, ref chat, out error)).ExecuteInTask();
            if (success)
            {
                return chat;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "打开会话失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return null;
            }
        }

        public virtual async Task<IEnumerable<MessageModel>> GetMessages(ChatModel chat, int count)
        {
            if (chat == null || count <= 0) return null;
            string error = null;
            IEnumerable<MessageModel> messages = null;
            DateTime? maxTime = null;
            lock (chat.Messages)
            {
                maxTime = chat.Messages.OrderBy(u => u.Time).FirstOrDefault()?.Time;
            }
            bool success = await new Func<bool>(() => ApiTools.GetMessages(chat.Id, maxTime, count, out error, out messages)).ExecuteInTask();
            if (success)
            {
                return messages;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "获取消息失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return null;
            }

        }

        public virtual async Task<bool> ModifyChat(StaffModel staff, ChatModel chat)
        {
            if (staff == null || chat == null) return false;
            string error = null;
            bool success = await new Func<bool>(() => ApiTools.ModifyChat(staff.Id, chat, out error)).ExecuteInTask();
            if (success)
            {
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "修改会话失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return false;
            }
        }


        public virtual async Task<string> AddFollow(StaffModel staff, UserModel user)
        {
            if (staff == null || user == null) return null;
            string error = null;
            string followId = null;
            bool success = await new Func<bool>(() => ApiTools.AddFollow(staff.Id, staff.Type, user, out error, out followId)).ExecuteInTask();
            if (success)
            {
                return followId;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "增加好友关系失败",
                        Content = error,
                        Icon = BalloonIcon.Error
                    }
                });
                return null;
            }
        }

        public virtual async Task<bool> ModifyFollow(StaffModel staff, UserModel user)
        {
            if (staff == null || user == null) return false;
            string error = null;
            bool success = await new Func<bool>(() => ApiTools.ModifyFollow(staff.Id, staff.Type, user, out error)).ExecuteInTask();
            if (success)
            {
                return true;
            }
            else
            {
                await this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo
                    {
                        Title = "修改好友关系失败",
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
