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
    public abstract class ChatTabItemAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public ChatTabItemAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, ChatInfo chat)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.emotionPickerAreaViewModel = emotionPickerAreaViewModel;
            this.chat = chat;
            this.messages = new ObservableCollection<MessageInfo>();
            this.messages.CollectionChanged += (s, e) =>
            {
                this.NotifyPropertyChange(() => this.NotReadMessageCount);
                this.NotifyPropertyChange(() => this.LastMessage);
            };

            //TODO 测试数据
            this.messages.Add(new TextMessageInfo()
            {
                Id = Guid.NewGuid().ToString(),
                MsgId = "1",
                FromUser = "1111",
                ToUsers = new string[] { "1" },
                Business = null,
                MsgTime = DateTime.Now.AddYears(-1),
                RoomId = null,
                ChatId = "123123123",
                State = MsgState.Received,
                Text = new TextMessageContext
                {
                    Content = "测试信息"
                }
            });
        }
        #endregion

        #region 属性
        protected IWindowManager windowManager;
        protected EventAggregator eventAggregator;
        private ChatInfo chat;
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
    }
}
