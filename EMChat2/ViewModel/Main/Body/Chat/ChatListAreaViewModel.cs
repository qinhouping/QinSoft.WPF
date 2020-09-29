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
    public class ChatListAreaViewModel : PropertyChangedBase, IEventHandle<LoginEventArgs>, IEventHandle<NotReadMessageCountChangedEventArgs>, IEventHandle<InputMessageContentEventArgs>, IEventHandle<RefreshChatsEventArgs>, IEventHandle<CustomerEditEventArgs>
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
                this.eventAggregator.PublishAsync(new ChatDetailEventArgs() { ChatItem = this.selectedChatItem });
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
        private ChatInfo CreatePrivateChat(UserInfo userInfo, BusinessEnum? business = null)
        {
            List<string> ids = new List<string>() { applicationContextViewModel.CurrentStaff.ImUserId, userInfo.ImUserId, business?.ToString() };
            ids.Sort();
            ChatInfo chat = new ChatInfo();
            chat.Id = Guid.NewGuid().ToString();
            chat.ChatId = string.Join("_", ids).MD5();
            chat.Business = business;
            chat.Type = ChatTypeEnum.Private;
            chat.Name = userInfo.Name;
            chat.HeaderImageUrl = userInfo.HeaderImageUrl;
            chat.IsTop = false;
            chat.IsInform = false;
            chat.ChatUsers = new ObservableCollection<UserInfo>(new UserInfo[] { applicationContextViewModel.CurrentStaff, userInfo });
            return chat;
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
                    }
                    chat.Dispose();
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
                lock (this.ChatItems)
                {
                    this.ChatItems.Clear();

                    this.ChatItems.Add(
                        new PrivateChatViewModel(
                            this.windowManager,
                            this.eventAggregator,
                            this.ApplicationContextViewModel,
                            this.EmotionPickerAreaViewModel,
                            this.QuickReplyAreaViewModel,
                            this.CreatePrivateChat(new CustomerInfo()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ImUserId = "1",
                                Name = "私聊-投顾",
                                HeaderImageUrl = "https://tse3-mm.cn.bing.net/th/id/OIP.BiS73OXRCWwEyT1aajtTpAAAAA?w=175&h=180&c=7&o=5&pid=1.7",
                                State = UserStateEnum.Online,
                                Business = BusinessEnum.Advisor,
                                Uid = "1"
                            },
                            BusinessEnum.Advisor),
                            this.chatService,
                            this.systemService));

                    this.ChatItems.Add(
                        new PrivateChatViewModel(
                            this.windowManager,
                            this.eventAggregator,
                            this.ApplicationContextViewModel,
                            this.EmotionPickerAreaViewModel,
                            this.QuickReplyAreaViewModel,
                            this.CreatePrivateChat(new CustomerInfo()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ImUserId = "2",
                                Name = "私聊-售前",
                                HeaderImageUrl = "https://dss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=3497889296,4029642021&fm=111&gp=0.jpg",
                                State = UserStateEnum.Online,
                                Business = BusinessEnum.PreSale,
                                Uid = "1"
                            },
                            BusinessEnum.PreSale),
                            this.chatService,
                            this.systemService));
                }
            }).ExecuteInUIThread();
        }

        public void Handle(NotReadMessageCountChangedEventArgs Message)
        {
            this.NotifyPropertyChange(() => this.TotalNotReadMessageCount);
        }

        public void Handle(InputMessageContentEventArgs arg)
        {
            if (this.SelectedChatItem == null) return;
            this.SelectedChatItem.TemporaryInputMessagContent = arg.MessageContent.Clone();
        }

        public void Handle(RefreshChatsEventArgs Message)
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

        public void Handle(CustomerEditEventArgs arg)
        {
            lock (this.ChatItems)
            {
                foreach (ChatViewModel chatItem in this.ChatItems)
                {
                    switch (chatItem.Chat.Type)
                    {
                        case ChatTypeEnum.Private:
                            {
                                if (!chatItem.Chat.ChatUsers.Contains(arg.Customer)) continue;
                                chatItem.Chat.Name = arg.Customer.Name;
                                chatItem.Chat.HeaderImageUrl = arg.Customer.HeaderImageUrl;
                                new Action(() => chatItem.MessagesCollectionView.Refresh()).ExecuteInUIThread();
                            }
                            break;
                        case ChatTypeEnum.Group:
                        case ChatTypeEnum.GroupSend:
                            break;
                    }
                }
            }
        }
        #endregion
    }
}
