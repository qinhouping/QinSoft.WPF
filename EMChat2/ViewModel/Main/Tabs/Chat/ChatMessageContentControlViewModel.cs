using EMChat2.Common;
using EMChat2.Model.Entity;
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
        public ChatMessageContentControlViewModel(string msgType, object msgContent)
        {
            this.msgType = msgType;
            this.msgContent = msgContent;
            this.chatService = ApplicationBooter.Current.IocApplicationContext.ObjectContainer.Get<ChatService>();
        }
        #endregion

        #region 属性 
        private ChatService chatService;
        private string msgType;
        public string MsgType
        {
            get
            {
                return this.msgType;
            }
            set
            {
                this.msgType = value;
                this.NotifyPropertyChange(() => this.MsgType);
            }
        }
        private object msgContent;
        public object MsgContent
        {
            get
            {
                return this.msgContent;
            }
            set
            {
                this.msgContent = value;
                this.NotifyPropertyChange(() => this.MsgContent);
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
                    ImageMessageContent imageMessageContent = MsgContent as ImageMessageContent;
                    this.chatService.OpenImage(new string[] { imageMessageContent.Url }, 0);
                });
            }
        }

        public ICommand OpenEmotionMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EmotionMessageContent emotionMessageContent = MsgContent as EmotionMessageContent;
                    this.chatService.OpenImage(new string[] { emotionMessageContent.Url }, 0);
                });
            }
        }

        public ICommand OpenLinkMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    LinkMessageContent linkMessageContent = MsgContent as LinkMessageContent;
                    this.chatService.OpenLink(linkMessageContent.Url);
                });
            }
        }

        public ICommand OpenFileMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FileMessageContent fileMessageContent = MsgContent as FileMessageContent;
                    this.chatService.OpenFile(fileMessageContent.Url, fileMessageContent.Name, fileMessageContent.Extension);
                });
            }
        }

        #endregion
    }
}
