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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs
{
    [Component]
    public class ChatTabAreaViewModel : PropertyChangedBase, IEventHandle<LoginEventArgs>, IEventHandle<NotReadMessageCountChangedEventArgs>, IEventHandle<InputMessageContentEventArgs>, IEventHandle<RefreshChatsEventArgs>
    {
        #region 构造函数
        public ChatTabAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, QuickReplyAreaViewModel quickReplyAreaViewModel, ChatService chatService, SystemService systemService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.emotionPickerAreaViewModel = emotionPickerAreaViewModel;
            this.quickReplyAreaViewModel = quickReplyAreaViewModel;
            this.ChatTabItems = new ObservableCollection<ChatTabItemAreaViewModel>();
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
        private ObservableCollection<ChatTabItemAreaViewModel> chatTabItems;
        public ObservableCollection<ChatTabItemAreaViewModel> ChatTabItems
        {
            get
            {
                return this.chatTabItems;
            }
            set
            {
                this.chatTabItems = value;
                this.chatTabItems.CollectionChanged += (s, e) =>
                {
                    this.NotifyPropertyChange(() => this.TotalNotReadMessageCount);
                    this.eventAggregator.PublishAsync(new RefreshChatsEventArgs());
                };
                this.NotifyPropertyChange(() => this.ChatTabItems);
                this.NotifyPropertyChange(() => this.TotalNotReadMessageCount);

                ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.chatTabItems);
                collectionView.SortDescriptions.Add(new SortDescription("IsTopSort", ListSortDirection.Descending));
                collectionView.SortDescriptions.Add(new SortDescription("LastMessageTimeSort", ListSortDirection.Descending));
                collectionView.SortDescriptions.Add(new SortDescription("CreateTimeSort", ListSortDirection.Descending));
                this.ChatTabItemsCollectionView = collectionView;
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
                this.selectedChatTabItem = value;
                this.NotifyPropertyChange(() => this.SelectedChatTabItem);
            }
        }
        private ICollectionView chatTabItemsCollectionView;
        public ICollectionView ChatTabItemsCollectionView
        {
            get
            {
                return this.chatTabItemsCollectionView;
            }
            set
            {
                this.chatTabItemsCollectionView = value;
                this.NotifyPropertyChange(() => this.ChatTabItemsCollectionView);
            }
        }
        private ChatService chatService;
        private SystemService systemService;
        public int TotalNotReadMessageCount
        {
            get
            {
                lock (this.ChatTabItems)
                {
                    return this.ChatTabItems.Sum(u => u.NotReadMessageCount);
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
            chat.Name = userInfo.Name;
            chat.Type = ChatType.Private;
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
                                this.ApplicationContextViewModel,
                                this.EmotionPickerAreaViewModel,
                                this.QuickReplyAreaViewModel,
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
            this.NotifyPropertyChange(() => this.TotalNotReadMessageCount);
        }

        public void Handle(InputMessageContentEventArgs arg)
        {
            if (this.SelectedChatTabItem == null) return;
            this.SelectedChatTabItem.TemporaryInputMessagContent = arg.MessageContent.Clone();
        }

        public void Handle(RefreshChatsEventArgs Message)
        {
            if (!isRefreshingChats)
            {
                isRefreshingChats = true;
                lock (this.ChatTabItems)
                {
                    new Action(() => this.ChatTabItemsCollectionView.Refresh()).ExecuteInUIThread();
                }
                isRefreshingChats = false;
            }
        }
        #endregion
    }
}
