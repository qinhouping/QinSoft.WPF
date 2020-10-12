using EMChat2.Common;
using EMChat2.Model.BaseInfo;
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
        public ChatMessageContentControlViewModel(string type, object content)
        {
            this.type = type;
            this.content = content;
            this.chatService = ApplicationBooter.Current.IocApplicationContext.ObjectContainer.Get<ChatService>();
        }
        #endregion

        #region 属性 
        private ChatService chatService;
        private string type;
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
                this.NotifyPropertyChange(() => this.Type);
            }
        }
        private object content;
        public object Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
                this.NotifyPropertyChange(() => this.Content);
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
                    ImageMessageContent imageMessageContent = Content as ImageMessageContent;
                    this.chatService.OpenImage(new string[] { imageMessageContent.Url }, 0);
                }, () =>
                {
                    return Content is ImageMessageContent;
                });
            }
        }

        public ICommand OpenEmotionMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    EmotionMessageContent emotionMessageContent = Content as EmotionMessageContent;
                    this.chatService.OpenImage(new string[] { emotionMessageContent.Url }, 0);
                }, () =>
                {
                    return Content is EmotionMessageContent;
                });
            }
        }

        public ICommand OpenLinkMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    LinkMessageContent linkMessageContent = Content as LinkMessageContent;
                    this.chatService.OpenLink(linkMessageContent.Url);
                }, () =>
                {
                    return Content is LinkMessageContent;
                });
            }
        }

        public ICommand OpenFileMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    FileMessageContent fileMessageContent = Content as FileMessageContent;
                    this.chatService.OpenFile(fileMessageContent.Url, fileMessageContent.Name);
                }, () =>
                {
                    return Content is FileMessageContent;
                });
            }
        }

        #endregion
    }
}
