using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
using EMChat2.Service;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Timers;
using EMChat2.ViewModel.Main.Body.User;
using EMChat2.Model.Enum;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public class PrivateChatViewModel : ChatViewModel, IDisposable
    {
        #region 构造函数
        public PrivateChatViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, QuickReplyAreaViewModel quickReplyAreaViewModel, CustomerTagAreaViewModel customerTagAreaViewModel, ChatModel chat, ChatService chatService, SystemService systemService, UserService userService) : base(windowManager, eventAggregator, applicationContextViewModel, emotionPickerAreaViewModel, chat, chatService, systemService, userService)
        {
            if (this.Chat.Type != ChatTypeEnum.Private) throw new ArgumentOutOfRangeException("is not private chat");
            this.PrivateChatSliderAreaViewModel = new PrivateChatSliderAreaViewModel(this.windowManager, this.eventAggregator, applicationContextViewModel, quickReplyAreaViewModel, customerTagAreaViewModel, chat, this.userService);
        }
        #endregion

        #region 属性
        private bool isShowChatSlider = true;
        public bool IsShowChatSlider
        {
            get
            {
                return this.isShowChatSlider;
            }
            set
            {
                this.isShowChatSlider = value;
                this.NotifyPropertyChange(() => IsShowChatSlider);
            }
        }

        private PrivateChatSliderAreaViewModel chatSliderAreaViewModel;
        public PrivateChatSliderAreaViewModel PrivateChatSliderAreaViewModel
        {
            get
            {
                return this.chatSliderAreaViewModel;
            }
            set
            {
                this.chatSliderAreaViewModel = value;
                this.NotifyPropertyChange(() => this.PrivateChatSliderAreaViewModel);
            }
        }
        private Timer isInputingTimer;
        private bool isInputing;
        public bool IsInputing
        {
            get
            {
                return this.isInputing;
            }
            set
            {
                this.isInputing = value;
                this.NotifyPropertyChange(() => this.IsInputing);
                isInputingTimer?.Stop();
                if (value)
                {
                    isInputingTimer = new Timer(5000);
                    isInputingTimer.Elapsed += (s, e) =>
                    {
                        this.IsInputing = false;
                    };
                    isInputingTimer.Start();
                }
            }
        }
        #endregion

        #region 命令 
        #endregion

        #region 方法
        public override async Task<bool> RecvMessage(MessageModel message)
        {
            bool res = false;
            new Action(() =>
            {
                lock (this.Chat.Messages)
                {
                    if (!this.Chat.Messages.Contains(message))
                    {
                        res = true;
                        this.Chat.Messages.Add(message);
                    }
                }
            }).ExecuteInUIThread();
            if (res && CanModifyMessageState(message, MessageStateEnum.Received))
            {
                MessageModel recvMessageEvent = MessageTools.CreateMessage(ApplicationContextViewModel.CurrentStaff, this.Chat, MessageTools.CreateRecvMessageEventMessageContent(message));
                await this.chatService.SendMessage(recvMessageEvent);
            }
            return true;
        }

        public override async Task<int> ReadMessage()
        {
            if (!IsSelected || !ApplicationContextViewModel.IsActived) return 0;
            MessageModel[] messages = NotReadMessages.Where(u => CanModifyMessageState(u, MessageStateEnum.Readed)).ToArray();
            if (messages.Count() == 0) return 0;
            int count = 10;
            for (int i = 0; i < Math.Ceiling(messages.Count() / (double)count); i++)
            {
                MessageModel readMessageEvent = MessageTools.CreateMessage(ApplicationContextViewModel.CurrentStaff, this.Chat, MessageTools.CreateReadMessageEventMessageContent(messages.Skip(i * count).Take(count).ToArray()));
                await this.chatService.SendMessage(readMessageEvent);
            }
            return messages.Count();
        }

        public virtual async void SendInputState(bool isInputing)
        {
            MessageModel inputMessageEvent = MessageTools.CreateMessage(ApplicationContextViewModel.CurrentStaff, this.Chat, MessageTools.CreateInputMessageEventMessageContent(this.Chat, isInputing));
            await this.chatService.SendMessage(inputMessageEvent);
        }

        public override void Dispose()
        {
            this.PrivateChatSliderAreaViewModel.Dispose();
            base.Dispose();
        }
        #endregion

        #region 事件处理
        #endregion
    }
}
