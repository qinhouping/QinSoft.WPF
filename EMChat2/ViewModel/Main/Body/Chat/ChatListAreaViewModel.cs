using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Event;
using EMChat2.Service;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Body.Chat
{
    [Component]
    public class ChatListAreaViewModel : PropertyChangedBase, IEventHandle<LoginCallbackEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>, IEventHandle<NotReadMessageCountChangedEventArgs>, IEventHandle<TemporaryInputMessagContentChangedEventArgs>, IEventHandle<RefreshChatsEventArgs>, IEventHandle<UserInfoChangedEventArgs>, IEventHandle<OpenPrivateChatEventArgs>, IEventHandle<MessageStateChangedEventArgs>, IEventHandle<ReceiveMessageEventArgs>
    {
        #region 构造函数
        public ChatListAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, QuickReplyAreaViewModel quickReplyAreaViewModel, ChatService chatService, SystemService systemService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.emotionPickerAreaViewModel = emotionPickerAreaViewModel;
            this.quickReplyAreaViewModel = quickReplyAreaViewModel;
            this.ChatItems = new ObservableCollection<ChatViewModel>();
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
        private QuickReplyAreaViewModel quickReplyAreaViewModel;
        public QuickReplyAreaViewModel QuickReplyAreaViewModel
        {
            get
            {
                return this.quickReplyAreaViewModel;
            }
            set
            {
                this.quickReplyAreaViewModel = value;
                this.NotifyPropertyChange(() => this.QuickReplyAreaViewModel);
            }
        }
        private ObservableCollection<ChatViewModel> chatItems;
        public ObservableCollection<ChatViewModel> ChatItems
        {
            get
            {
                return this.chatItems;
            }
            set
            {
                this.chatItems = value;
                this.chatItems.CollectionChanged += (s, e) =>
                {
                    this.NotifyPropertyChange(() => this.TotalNotReadMessageCount);

                    //自动选择第一项
                    if (this.SelectedChatItem == null)
                    {
                        lock (this.ChatItems)
                        {
                            if (!this.ChatItemsCollectionView.IsEmpty)
                            {
                                this.ChatItemsCollectionView.MoveCurrentToFirst();
                                this.SelectedChatItem = this.ChatItemsCollectionView.CurrentItem as ChatViewModel;
                            }
                        }
                    }
                };
                this.NotifyPropertyChange(() => this.ChatItems);
                this.NotifyPropertyChange(() => this.TotalNotReadMessageCount);

                ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.chatItems);
                collectionView.SortDescriptions.Add(new SortDescription("IsTopSort", ListSortDirection.Descending));
                collectionView.SortDescriptions.Add(new SortDescription("LastMessageTimeSort", ListSortDirection.Descending));
                this.ChatItemsCollectionView = collectionView;
            }
        }
        private ICollectionView chatItemsCollectionView;
        public ICollectionView ChatItemsCollectionView
        {
            get
            {
                return this.chatItemsCollectionView;
            }
            set
            {
                this.chatItemsCollectionView = value;
                this.NotifyPropertyChange(() => this.ChatItemsCollectionView);
            }
        }
        private ChatViewModel selectedChatItem;
        public ChatViewModel SelectedChatItem
        {
            get
            {
                return this.selectedChatItem;
            }
            set
            {
                this.selectedChatItem = value;
                this.NotifyPropertyChange(() => this.SelectedChatItem);
                this.eventAggregator.PublishAsync(new SelectChatDetailEventArgs() { ChatItem = this.selectedChatItem });
            }
        }
        private ChatService chatService;
        private SystemService systemService;
        public int TotalNotReadMessageCount
        {
            get
            {
                lock (this.ChatItems)
                {
                    return this.ChatItems.Sum(u => u.NotReadMessageCount);
                }
            }
        }
        private bool isRefreshingChats;
        #endregion

        #region 方法
        private ChatInfo CreatePrivateChat(UserInfo user, BusinessEnum business = BusinessEnum.Inside)
        {
            List<string> ids = new List<string>() { applicationContextViewModel.CurrentStaff.ImUserId, user.ImUserId, business.ToString() };
            ids.Sort();
            ChatInfo chat = new ChatInfo();
            chat.Id = Guid.NewGuid().ToString();
            chat.Id = string.Join("_", ids).MD5();
            chat.Business = business;
            chat.Type = ChatTypeEnum.Private;
            chat.Name = user.Name;
            chat.HeaderImageUrl = user.HeaderImageUrl;
            chat.IsTop = false;
            chat.IsInform = false;
            chat.ChatUsers = new ObservableCollection<UserInfo>(new UserInfo[] { applicationContextViewModel.CurrentStaff, user });
            chat.ChatAllUsers = new ObservableCollection<UserInfo>(new UserInfo[] { applicationContextViewModel.CurrentStaff, user });
            chat.CreateTime = DateTime.Now;
            return chat;
        }
        private PrivateChatViewModel CreatePrivateChat(ChatInfo chat)
        {
            return new PrivateChatViewModel(this.windowManager, this.eventAggregator, this.ApplicationContextViewModel, this.EmotionPickerAreaViewModel, this.QuickReplyAreaViewModel, chat, this.chatService, this.systemService);
        }
        #endregion

        #region 命令

        public ICommand CloseChatCommand
        {
            get
            {
                return new RelayCommand<ChatViewModel>((chat) =>
                {
                    lock (this.ChatItems)
                    {
                        this.ChatItems.Remove(chat);
                        chat.Dispose();
                    }
                }, (chat) =>
                {
                    return chat != null;
                });
            }
        }
        #endregion

        #region 事件处理
        public void Handle(LoginCallbackEventArgs arg)
        {
            if (!arg.IsSuccess) return;

            //TODO 测试数据
            new Action(() =>
            {
                lock (this.ChatItems)
                {
                    this.ChatItems.Clear();

                    this.ChatItems.Add(this.CreatePrivateChat(this.CreatePrivateChat(new CustomerInfo()
                    {
                        Id = "测试客户UID",
                        ImUserId = "4735344555340783734",
                        Name = "私聊-投顾",
                        HeaderImageUrl = "https://tse3-mm.cn.bing.net/th/id/OIP.BiS73OXRCWwEyT1aajtTpAAAAA?w=175&h=180&c=7&o=5&pid=1.7",
                        State = UserStateEnum.Online,
                        Business = BusinessEnum.Advisor,
                        Uid = "测试客户UID"
                    }, BusinessEnum.Advisor)));
                }
            }).ExecuteInUIThread();
        }

        public void Handle(LogoutCallbackEventArgs arg)
        {
            new Action(() =>
            {
                lock (this.ChatItems)
                {
                    foreach (ChatViewModel chat in this.ChatItems.ToArray())
                    {
                        this.ChatItems.Remove(chat);
                        chat.Dispose();
                    }
                }
            }).ExecuteInUIThread();
        }

        public void Handle(ExitCallbackEventArgs arg)
        {
            new Action(() =>
            {
                lock (this.ChatItems)
                {
                    foreach (ChatViewModel chat in this.ChatItems.ToArray())
                    {
                        this.ChatItems.Remove(chat);
                        chat.Dispose();
                    }
                }
            }).ExecuteInUIThread();
        }

        public void Handle(NotReadMessageCountChangedEventArgs arg)
        {
            this.NotifyPropertyChange(() => this.TotalNotReadMessageCount);
        }

        public void Handle(TemporaryInputMessagContentChangedEventArgs arg)
        {
            if (this.SelectedChatItem == null) return;
            this.SelectedChatItem.TemporaryInputMessagContent = arg.MessageContent.Clone();
        }

        public void Handle(RefreshChatsEventArgs arg)
        {
            if (!isRefreshingChats)
            {
                isRefreshingChats = true;

                new Action(() =>
                {
                    lock (this.ChatItems)
                    {
                        this.ChatItemsCollectionView.Refresh();
                    }
                }).ExecuteInUIThread();

                isRefreshingChats = false;
            }
        }

        public void Handle(UserInfoChangedEventArgs arg)
        {
            lock (this.ChatItems)
            {
                foreach (ChatViewModel chatItem in this.ChatItems)
                {
                    switch (chatItem.Chat.Type)
                    {
                        case ChatTypeEnum.Private:
                            {
                                if (chatItem.Chat.ChatUsers.Contains(arg.User))
                                {
                                    chatItem.Chat.ChatUsers.FirstOrDefault(u => u.Equals(arg.User)).Assign(arg.User);
                                    chatItem.Chat.Name = arg.User.Name;
                                    chatItem.Chat.HeaderImageUrl = arg.User.HeaderImageUrl;
                                    new Action(() => chatItem.MessagesCollectionView.Refresh()).ExecuteInUIThread();
                                }
                            }
                            break;
                        case ChatTypeEnum.Group:
                        case ChatTypeEnum.GroupSend:
                            break;
                    }
                }
            }
        }

        public void Handle(OpenPrivateChatEventArgs arg)
        {
            new Action(() =>
            {
                PrivateChatViewModel privateChatViewModel = null;
                if (arg.ChatUser is CustomerInfo)
                {
                    CustomerInfo customer = arg.ChatUser as CustomerInfo;
                    privateChatViewModel = CreatePrivateChat(this.CreatePrivateChat(customer, customer.Business));
                }
                else if (arg.ChatUser is StaffInfo)
                {
                    StaffInfo staff = arg.ChatUser as StaffInfo;
                    privateChatViewModel = CreatePrivateChat(this.CreatePrivateChat(staff));
                }
                else if (arg.ChatUser is SystemUserInfo)
                {
                    return;
                }
                lock (this.ChatItems)
                {
                    if (!this.ChatItems.Contains(privateChatViewModel)) this.ChatItems.Add(privateChatViewModel);
                    if (arg.IsActive) this.SelectedChatItem = this.ChatItems.FirstOrDefault(u => u.Equals(privateChatViewModel));
                }
            }).ExecuteInUIThread();
        }

        public void Handle(MessageStateChangedEventArgs arg)
        {
            ChatViewModel chat = this.ChatItems.FirstOrDefault(u => u.Chat.Id.Equals(arg.Message.ChatId));
            if (chat == null) return;
            MessageInfo message = chat.Messages.FirstOrDefault(u => u.Equals(arg.Message));
            if (message == null) return;
            message.State = arg.Message.State;
        }

        public void Handle(ReceiveMessageEventArgs arg)
        {
            ChatViewModel chat = this.ChatItems.FirstOrDefault(u => u.Chat.Id.Equals(arg.Message.ChatId));
            if (chat == null) return;
            arg.Message.State = MessageStateEnum.Readed;
            new Action(() => chat.Messages.Add(arg.Message)).ExecuteInUIThread();
        }
        #endregion
    }
}
