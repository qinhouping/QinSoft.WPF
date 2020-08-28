using EMChat2.Model.Entity;
using EMChat2.Service;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public abstract class ChatTabItemAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public ChatTabItemAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, ChatInfo chat, ChatService chatService, SystemService systemService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.emotionPickerAreaViewModel = emotionPickerAreaViewModel;
            this.chat = chat;
            this.chatService = chatService;
            this.systemService = systemService;
            this.messages = new ObservableCollection<MessageInfo>();
            this.messages.CollectionChanged += (s, e) =>
            {
                this.NotifyPropertyChange(() => this.NotReadMessageCount);
                this.NotifyPropertyChange(() => this.LastMessage);
            };
        }
        #endregion

        #region 属性
        protected IWindowManager windowManager;
        protected EventAggregator eventAggregator;
        private ChatInfo chat;
        private ChatService chatService;
        private SystemService systemService;
        public ChatInfo Chat
        {
            get
            {
                return this.chat;
            }
            set
            {
                this.chat = value;
                this.NotifyPropertyChange(() => this.Chat);
            }
        }
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
        private ObservableCollection<MessageInfo> messages;
        public ObservableCollection<MessageInfo> Messages
        {
            get
            {
                return this.messages;
            }
            set
            {
                this.messages = value;
                this.NotifyPropertyChange(() => this.Messages);
                this.NotifyPropertyChange(() => this.NotReadMessageCount);
                this.NotifyPropertyChange(() => this.LastMessage);
            }
        }

        public int NotReadMessageCount
        {
            get
            {
                return this.messages.Where(u => u.State.Equals(MsgState.Received)).Count();
            }
        }

        public MessageInfo LastMessage
        {
            get
            {
                return this.messages.LastOrDefault();
            }
        }
        #endregion

        #region 命令
        public ICommand OpenLinkMessageCommand
        {
            get
            {
                return new RelayCommand<LinkMessageInfo>((message) =>
                {
                    this.chatService.OpenLink(message.Link.Url);
                });
            }
        }

        public ICommand OpenImageMessageCommand
        {
            get
            {
                return new RelayCommand<ImageMessageInfo>((message) =>
                {
                    ImageMessageInfo[] imageMessageInfos = this.Messages.OfType<ImageMessageInfo>().ToArray();
                    int index = Array.IndexOf(imageMessageInfos, message);

                    string[] sources = imageMessageInfos.Select(u =>
                    {
                        return systemService.GetUrlMapping(u.Image.Url);
                    }).Where(u => !string.IsNullOrEmpty(u)).ToArray();
                    this.chatService.OpenImage(sources, index);
                });
            }
        }
        #endregion
    }
}
