using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
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
using EMChat2.ViewModel.Main.Body.User;
using EMChat2.Model.Enum;

namespace EMChat2.ViewModel.Main.Body.Chat
{
    [Component]
    public class ChatListAreaViewModel : PropertyChangedBase, IEventHandle<LoginCallbackEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>, IEventHandle<NotReadMessageCountChangedEventArgs>, IEventHandle<TemporaryInputMessagContentChangedEventArgs>, IEventHandle<RefreshChatsEventArgs>, IEventHandle<UserInfoChangedEventArgs>, IEventHandle<OpenPrivateChatEventArgs>, IEventHandle<MessageStateChangedEventArgs>, IEventHandle<MessageIdChangedEventArgs>, IEventHandle<ReceiveMessageEventArgs>, IEventHandle<ActiveApplicationEventArgs>, IEventHandle<InputMessageChangedEventArgs>
    {
        #region 构造函数
        public ChatListAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, QuickReplyAreaViewModel quickReplyAreaViewModel, CustomerTagAreaViewModel customerTagAreaViewModel, ChatService chatService, SystemService systemService, UserService userService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.emotionPickerAreaViewModel = emotionPickerAreaViewModel;
            this.quickReplyAreaViewModel = quickReplyAreaViewModel;
            this.customerTagAreaViewModel = customerTagAreaViewModel;
            this.ChatItems = new ObservableCollection<ChatViewModel>();
            this.chatService = chatService;
            this.systemService = systemService;
            this.userService = userService;
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
        private CustomerTagAreaViewModel customerTagAreaViewModel;
        public CustomerTagAreaViewModel CustomerTagAreaViewModel
        {
            get
            {
                return this.customerTagAreaViewModel;
            }
            set
            {
                this.customerTagAreaViewModel = value;
                this.NotifyPropertyChange(() => this.CustomerTagAreaViewModel);
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
                this.NotifyPropertyChange(() => this.ChatItems);
                this.NoticeChatItemsChange();
                this.chatItems.CollectionChanged += (s, e) =>
                {
                    lock (this)
                    {
                        this.NoticeChatItemsChange();
                    }
                };

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
                if (this.selectedChatItem != null) this.selectedChatItem.IsSelected = false;
                this.selectedChatItem = value;
                this.NotifyPropertyChange(() => this.SelectedChatItem);
                if (this.selectedChatItem != null) this.selectedChatItem.IsSelected = true;
                this.eventAggregator.PublishAsync(new SelectChatDetailEventArgs() { ChatItem = this.selectedChatItem });
                this.selectedChatItem?.ReadMessage();
            }
        }
        private ChatService chatService;
        private SystemService systemService;
        private UserService userService;
        public int TotalNotReadMessageCount
        {
            get
            {
                lock (this.ChatItems)
                {
                    return this.ChatItems.Sum(u => u.NotReadMessagesCount);
                }
            }
        }
        private bool isRefreshingChats;
        #endregion

        #region 方法
        private ChatViewModel CreateChat(ChatModel chat)
        {
            switch (chat.Type)
            {
                case ChatTypeEnum.Private:
                    {

                        return new PrivateChatViewModel(this.windowManager, this.eventAggregator, this.ApplicationContextViewModel, this.EmotionPickerAreaViewModel, this.QuickReplyAreaViewModel, this.CustomerTagAreaViewModel, chat, this.chatService, this.systemService, this.userService);
                    }
                default: return null;
            }
        }

        private async void GetChats(StaffModel staff)
        {
            IEnumerable<ChatModel> chats = await userService.GetChats(staff);
            if (chats == null) return;
            new Action(() =>
            {
                lock (this.ChatItems)
                {
                    this.ChatItems.Clear();
                    foreach (ChatModel chat in chats)
                    {
                        ChatViewModel chatModel = CreateChat(chat);
                        if (!this.ChatItems.Contains(chatModel))
                            this.ChatItems.Add(chatModel);
                    }
                }
            }).ExecuteInUIThread();
        }

        private async Task OpenChat(StaffModel staff, string chatId)
        {
            ChatModel chat = await userService.OpenChat(staff, chatId);
            if (chat == null) return;
            new Action(() =>
            {
                lock (this.ChatItems)
                {
                    ChatViewModel chatModel = CreateChat(chat);
                    if (!this.ChatItems.Contains(chatModel))
                        this.ChatItems.Add(chatModel);
                    else
                        this.SelectedChatItem = this.ChatItems.FirstOrDefault(u => u.Equals(chatModel));
                }
            }).ExecuteInUIThread();
        }

        private async void CreatePrivateChat(StaffModel staff, string businessId, UserModel user)
        {
            ChatModel chat = new ChatModel();
            chat.Id = null;
            chat.BusinessId = businessId;
            chat.Type = ChatTypeEnum.Private;
            chat.Name = null;
            chat.HeaderImage = null;
            chat.IsTop = false;
            chat.IsInform = true;
            chat.ChatUsers = new ObservableCollection<UserModel>(new UserModel[] { applicationContextViewModel.CurrentStaff, user });
            chat = await userService.CreateChat(staff, chat);
            if (chat == null) return;
            new Action(() =>
            {
                lock (this.ChatItems)
                {
                    ChatViewModel chatModel = CreateChat(chat);
                    if (!this.ChatItems.Contains(chatModel))
                        this.ChatItems.Add(chatModel);
                    else
                        this.SelectedChatItem = this.ChatItems.FirstOrDefault(u => u.Equals(chatModel));
                }
            }).ExecuteInUIThread();
        }

        private async void CloseChat(StaffModel staff, ChatViewModel chat)
        {
            bool res = await userService.CloseChat(staff, chat.Chat);
            if (!res) return;
            new Action(() =>
            {
                lock (this.ChatItems)
                {
                    this.ChatItems.Remove(chat);
                    chat.Dispose();
                }
            }).ExecuteInUIThread();
        }

        protected async void NoticeChatItemsChange()
        {
            this.NotifyPropertyChange(() => this.TotalNotReadMessageCount);

            await Task.Delay(50); //TODO 通过延迟滚动来解决不能自动选择的bug
            if (this.SelectedChatItem == null)
            {
                lock (this.ChatItems)
                {
                    if (this.ChatItemsCollectionView?.IsEmpty == false)
                    {
                        this.ChatItemsCollectionView.MoveCurrentToFirst();
                        this.SelectedChatItem = this.ChatItemsCollectionView.CurrentItem as ChatViewModel;
                    }
                }
            }
        }

        private async void OnRecvMessage(ReceiveMessageEventArgs arg)
        {
            ChatViewModel chat = null;
            lock (this.ChatItems) chat = this.ChatItems.FirstOrDefault(u => u.Chat.Id.Equals(arg.Message.ChatId));
            if (chat == null) await this.OpenChat(ApplicationContextViewModel.CurrentStaff, arg.Message.ChatId);
            lock (this.ChatItems) chat = this.ChatItems.FirstOrDefault(u => u.Chat.Id.Equals(arg.Message.ChatId));
            if (chat == null) return;
            if (chat.RecvMessage(arg.Message) && chat.Chat.IsInform)
            {
                arg.IsInform = true;
            }
        }
        #endregion

        #region 命令

        public ICommand CloseChatCommand
        {
            get
            {
                return new RelayCommand<ChatViewModel>((chat) =>
                {
                    this.CloseChat(ApplicationContextViewModel.CurrentStaff, chat);
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
            this.GetChats(arg.Staff);
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
            this.SelectedChatItem.TemporaryInputMessagContent = arg.MessageContent.Clone() as MessageContentModel;
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
                                    chatItem.Chat.Name = arg.User.NickName;
                                    chatItem.Chat.HeaderImage = arg.User.HeaderImage;
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
                if (arg.ChatUser is CustomerModel)
                {
                    CustomerModel customer = arg.ChatUser as CustomerModel;
                    this.CreatePrivateChat(ApplicationContextViewModel.CurrentStaff, customer.BusinessId, customer);
                }
                else if (arg.ChatUser is StaffModel)
                {
                    StaffModel staff = arg.ChatUser as StaffModel;
                    this.CreatePrivateChat(ApplicationContextViewModel.CurrentStaff, staff.BusinessId, staff);
                }
                else if (arg.ChatUser is SystemUserModel)
                {
                    return;
                }
            }).ExecuteInUIThread();
        }

        public void Handle(MessageStateChangedEventArgs arg)
        {
            ChatViewModel chat = null;
            lock (this.ChatItems) chat = this.ChatItems.FirstOrDefault(u => u.Chat.Id.Equals(arg.ChatId));
            if (chat == null) return;
            chat.UpdateMessage(arg.MessageId, arg.MessageState);
        }

        public void Handle(MessageIdChangedEventArgs arg)
        {
            ChatViewModel chat = null;
            lock (this.ChatItems) chat = this.ChatItems.FirstOrDefault(u => u.Chat.Id.Equals(arg.ChatId));
            if (chat == null) return;
            chat.UpdateMessage(arg.MessageId, arg.NewMessageId);
        }

        public void Handle(ReceiveMessageEventArgs arg)
        {
            OnRecvMessage(arg);
        }

        public void Handle(ActiveApplicationEventArgs arg)
        {
            this.SelectedChatItem?.ReadMessage();
        }

        public void Handle(InputMessageChangedEventArgs arg)
        {
            PrivateChatViewModel chat = null;
            lock (this.ChatItems) chat = this.ChatItems.FirstOrDefault(u => u.Chat.Id.Equals(arg.ChatId)) as PrivateChatViewModel;
            if (chat == null) return;
            chat.IsInputing = arg.IsInputing;
        }
        #endregion
    }
}
