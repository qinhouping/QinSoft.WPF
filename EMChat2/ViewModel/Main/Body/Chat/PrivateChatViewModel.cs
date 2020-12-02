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

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public class PrivateChatViewModel : ChatViewModel, IDisposable
    {
        #region 构造函数
        public PrivateChatViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, QuickReplyAreaViewModel quickReplyAreaViewModel, ChatModel chat, ChatService chatService, SystemService systemService) : base(windowManager, eventAggregator, applicationContextViewModel, emotionPickerAreaViewModel, chat, chatService, systemService)
        {
            if (this.Chat.Type != ChatTypeEnum.Private) throw new ArgumentOutOfRangeException("is not private chat");
            this.PrivateChatSliderAreaViewModel = new PrivateChatSliderAreaViewModel(this.windowManager, this.eventAggregator, applicationContextViewModel, quickReplyAreaViewModel, chat);
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
        #endregion

        #region 命令 
        #endregion

        #region 方法
        public override void Dispose()
        {
            this.PrivateChatSliderAreaViewModel.Dispose();
            base.Dispose();
        }
        #endregion
    }
}
