using EMChat2.Common;
using EMChat2.Model.Entity;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public class PrivateChatTabItemAreaViewModel : ChatTabItemAreaViewModel
    {
        #region 构造函数
        public PrivateChatTabItemAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, UserInfo user) : base(windowManager, eventAggregator, applicationContextViewModel, CreateChat(applicationContextViewModel.CurrentStaff, user))
        {
            this.chatUser = user;
        }
        #endregion

        #region 属性
        private UserInfo chatUser;
        public UserInfo ChatUser
        {
            get
            {
                return this.chatUser;
            }
            set
            {
                this.chatUser = value;
                this.NotifyPropertyChange(() => this.ChatUser);
            }
        }
        #endregion

        #region 方法
        private static ChatInfo CreateChat(StaffInfo currentStaff, UserInfo userInfo)
        {
            ChatInfo chat = new ChatInfo();
            List<string> ids = new List<string>() { currentStaff.ImUserId, userInfo.ImUserId };
            ids.Sort();
            chat.Id = Guid.NewGuid().ToString();
            chat.RoomId = string.Join(",", ids).MD5();
            chat.Name = userInfo.Name;
            chat.Type = ChatType.Private;
            chat.HeaderImageUrl = userInfo.HeaderImageUrl;
            chat.IsTop = true;
            chat.ChatUsers = new ObservableCollection<UserInfo>(new UserInfo[] { currentStaff, userInfo });
            return chat;
        }
        #endregion
    }
}
