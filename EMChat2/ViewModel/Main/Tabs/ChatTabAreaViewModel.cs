using EMChat2.Common;
using EMChat2.Common.Cef;
using EMChat2.Model.Entity;
using EMChat2.Model.Event;
using EMChat2.Service;
using EMChat2.View.Main.Tabs.Chat;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs
{
    [Component]
    public class ChatTabAreaViewModel : PropertyChangedBase, IEventHandle<LoginEventArgs>, IEventHandle<NotReadMessageCountChangedEventArgs>, IEventHandle<SelectEmotionEventArgs>, IEventHandle<SelectImageEventArgs>, IEventHandle<SelectFileEventArgs>
    {
        #region 构造函数
        public ChatTabAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, ChatService chatService, SystemService systemService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.emotionPickerAreaViewModel = emotionPickerAreaViewModel;
            this.ChatTabItems = new ThreadSafeObservableCollection<ChatTabItemAreaViewModel>();
            this.chatService = chatService;
            this.systemService = systemService;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ApplicationContextViewModel applicationContextViewModel;
        public ApplicationContextViewModel ApplicationContextViewModel
        {
            get
            {
                return this.applicationContextViewModel;
            }
            set
            {
                this.applicationContextViewModel = value;
                this.NotifyPropertyChange(() => this.ApplicationContextViewModel);
            }
        }
        private EmotionPickerAreaViewModel emotionPickerAreaViewModel;
        public EmotionPickerAreaViewModel EmotionPickerAreaViewModel
        {
            get
            {
                return this.emotionPickerAreaViewModel;
            }
            set
            {
                this.emotionPickerAreaViewModel = value;
                this.NotifyPropertyChange(() => this.EmotionPickerAreaViewModel);
            }
        }
        private ThreadSafeObservableCollection<ChatTabItemAreaViewModel> chatTabItems;
        public ThreadSafeObservableCollection<ChatTabItemAreaViewModel> ChatTabItems
        {
            get
            {
                return this.chatTabItems;
            }
            set
            {
                this.chatTabItems = value;
                this.ChangeSelectedChatTabItem();
                this.chatTabItems.CollectionChanged += (s, e) =>
                {
                    this.ChangeSelectedChatTabItem();
                };
                this.NotifyPropertyChange(() => this.ChatTabItems);
            }
        }
        private ChatTabItemAreaViewModel selectedChatTabItem;
        public ChatTabItemAreaViewModel SelectedChatTabItem
        {
            get
            {
                return this.selectedChatTabItem;
            }
            set
            {
                if (this.selectedChatTabItem?.Equals(value) == true) return;
                this.selectedChatTabItem = value;
                this.NotifyPropertyChange(() => this.SelectedChatTabItem);
            }
        }
        private ChatService chatService;
        private SystemService systemService;
        private int totalNotReadMessageCount;
        public int TotalNotReadMessageCount
        {
            get
            {
                return this.totalNotReadMessageCount;
            }
            set
            {
                this.totalNotReadMessageCount = value;
                this.NotifyPropertyChange(() => this.TotalNotReadMessageCount);
            }
        }
        #endregion

        #region 方法
        private ChatInfo CreatePrivateChat(UserInfo userInfo, BusinessEnum? business = null)
        {
            List<string> ids = new List<string>() { applicationContextViewModel.CurrentStaff.ImUserId, userInfo.ImUserId, business?.ToString() };
            ids.Sort();
            ChatInfo chat = new ChatInfo();
            chat.Id = Guid.NewGuid().ToString();
            chat.ChatId = string.Join("_", ids).MD5();
            chat.Business = business;
            chat.Name = userInfo.Name;
            chat.Type = ChatType.Private;
            chat.HeaderImageUrl = userInfo.HeaderImageUrl;
            chat.IsTop = true;
            chat.IsInform = false;
            chat.ChatUsers = new ThreadSafeObservableCollection<UserInfo>(new UserInfo[] { applicationContextViewModel.CurrentStaff, userInfo });
            return chat;
        }

        private void ChangeSelectedChatTabItem()
        {
            lock (this.ChatTabItems)
            {
                if (this.SelectedChatTabItem == null && this.ChatTabItems.Count > 0)
                {
                    this.SelectedChatTabItem = this.ChatTabItems.First();
                }
            }
        }
        #endregion

        #region 命令

        public ICommand CloseChatCommand
        {
            get
            {
                return new RelayCommand<ChatTabItemAreaViewModel>((chat) =>
                {
                    lock (this.ChatTabItems)
                    {
                        this.ChatTabItems.Remove(chat);
                        chat.Dispose();
                    }
                });
            }
        }
        #endregion

        #region 事件处理
        public void Handle(LoginEventArgs arg)
        {
            if (!arg.IsSuccess) return;

            //TODO 测试数据
            new Action(() =>
            {
                lock (this.ChatTabItems)
                {
                    this.ChatTabItems.Clear();
                    for (int i = 0; i < 10; i++)
                    {
                        this.ChatTabItems.Add(
                            new PrivateChatTabItemAreaViewModel(
                                this.windowManager,
                                this.eventAggregator,
                                this.applicationContextViewModel,
                                this.emotionPickerAreaViewModel,
                                this.CreatePrivateChat(new CustomerInfo()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    ImUserId = "1",
                                    Name = "私聊-投顾" + i,
                                    HeaderImageUrl = "https://tse3-mm.cn.bing.net/th/id/OIP.BiS73OXRCWwEyT1aajtTpAAAAA?w=175&h=180&c=7&o=5&pid=1.7",
                                    State = UserStateEnum.Online,
                                    Business = BusinessEnum.Advisor,
                                    Uid = "1"
                                },
                                BusinessEnum.Advisor),
                                this.chatService,
                                this.systemService));
                    }
                }
            }).ExecuteInUIThread();
        }

        public void Handle(NotReadMessageCountChangedEventArgs Message)
        {
            lock (this.ChatTabItems)
            {
                this.TotalNotReadMessageCount = ChatTabItems.Sum(u => u.NotReadMessageCount);
            }
        }

        public void Handle(SelectEmotionEventArgs arg)
        {
            if (this.SelectedChatTabItem == null) return;
            this.SelectedChatTabItem.TemporaryInputMessagContent = MessageTools.CreateEmotionMessageContent(arg.Emotion);
        }

        public void Handle(SelectImageEventArgs arg)
        {
            if (this.SelectedChatTabItem == null) return;
            this.SelectedChatTabItem.TemporaryInputMessagContent = MessageTools.CreateImageMessageContent(arg.File);
        }

        public void Handle(SelectFileEventArgs arg)
        {
            if (this.SelectedChatTabItem == null) return;
            this.SelectedChatTabItem.TemporaryInputMessagContent = MessageTools.CreateFileMessageContent(arg.File);
        }
        #endregion
    }
}
