using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
using EMChat2.Service;
using QinSoft.Event;
using QinSoft.Ioc;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public class ChatMessageContentControlViewModel : PropertyChangedBase
    {
        #region 构造函数
        public ChatMessageContentControlViewModel(MessageContentModel messageContent)
        {
            this.messageContent = messageContent;
            this.chatService = ApplicationBooter.Current.IocApplicationContext.ObjectContainer.Get<ChatService>();
        }
        #endregion

        #region 属性 
        private ChatService chatService;
        private MessageContentModel messageContent;
        public MessageContentModel MessageContent
        {
            get
            {
                return this.messageContent;
            }
            set
            {
                this.messageContent = value;
                this.NotifyPropertyChange(() => this.MessageContent);
            }
        }
        #endregion

        #region 命令
        public ICommand OpenImageMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ImageMessageContent imageMessageContent = MessageContent.Content as ImageMessageContent;
                    this.chatService.OpenImage(new string[] { imageMessageContent.Url }, 0);
                }, () =>
                {
                    return MessageContent.Type == MessageTypeConst.Image;
                });
            }
        }

        public ICommand OpenEmotionMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EmotionMessageContent emotionMessageContent = MessageContent.Content as EmotionMessageContent;
                    this.chatService.OpenImage(new string[] { emotionMessageContent.Url }, 0);
                }, () =>
                {
                    return MessageContent.Type == MessageTypeConst.Emotion;
                });
            }
        }

        public ICommand OpenLinkMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    LinkMessageContent linkMessageContent = MessageContent.Content as LinkMessageContent;
                    this.chatService.OpenLink(linkMessageContent.Url);
                }, () =>
                {
                    return MessageContent.Type == MessageTypeConst.Link;
                });
            }
        }

        public ICommand OpenFileMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FileMessageContent fileMessageContent = MessageContent.Content as FileMessageContent;
                    this.chatService.OpenFile(fileMessageContent.Url, fileMessageContent.Name);
                }, () =>
                {
                    return MessageContent.Type == MessageTypeConst.File;
                });
            }
        }

        #endregion
    }
}
