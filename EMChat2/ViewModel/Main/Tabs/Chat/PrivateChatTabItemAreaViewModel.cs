using EMChat2.Common;
using EMChat2.Model.Entity;
using EMChat2.Model.Event;
using EMChat2.Service;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public class PrivateChatTabItemAreaViewModel : ChatTabItemAreaViewModel
    {
        #region 构造函数
        public PrivateChatTabItemAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, EmotionPickerAreaViewModel emotionPickerAreaViewModel, ChatSliderAreaViewModel chatSliderAreaViewModel, ChatInfo chat, ChatService chatService, SystemService systemService) : base(windowManager, eventAggregator, applicationContextViewModel, emotionPickerAreaViewModel, chat, chatService, systemService)
        {
            if (this.Chat.Type != ChatType.Private) throw new ArgumentOutOfRangeException("is not private chat");
            this.chatSliderAreaViewModel = chatSliderAreaViewModel;

            //TODO 测试数据
            this.Messages.Add(new MessageInfo()
            {
                Id = Guid.NewGuid().ToString(),
                MsgId = "1",
                FromUser = "1111",
                ToUsers = new string[] { "1" },
                MsgTime = DateTime.Now.AddYears(-1),
                ChatId = "123123123",
                Type = MessageTypeConst.Text,
                State = MessageState.SendFailure,
                Content = new TextMessageContent
                {
                    Content = @"搜索了一下Platform SDK的头文件，在ShellApi.h中找到了uFlags的定义，我将这些值声明为GetFileInfoFlags枚举类型(详细代码在后面)，我 目前只用到了SHGFI_ICON和SHGFI_SMALLICON，SHGFI_ICON表示要获取文件大图标，SHGFI_SMALLICON表示要 获取小图标，我第一次用SHGFI_SMALLICON时程序出现了异常，后来仔细看MSDN才知道SHGFI_SMALLICON和 SHGFI_ICON必须要连用。

    最后我把这些API函数声明、结构体声明、枚举类型声明等东西总的放在一个FileInfo类中，代码我会放在文章的最后部分以防止大家阅 读文章不便。

    最后我在我的文件浏览菜单项中定义了一个静态方法来获取文件夹或者文件的小图标，代码如下："
                }.ObjectToJson()
            });
            this.Messages.Add(new MessageInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Type = MessageTypeConst.Image,
                MsgId = "1",
                FromUser = "1111",
                ToUsers = new string[] { "1" },
                MsgTime = DateTime.Now.AddYears(-1),
                ChatId = "123123123",
                State = MessageState.Received,
                Content = new ImageMessageContent
                {
                    Url = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1598607415218&di=18fe58bd0a17a50d39f7e321e5d7cd85&imgtype=0&src=http%3A%2F%2Fh.hiphotos.baidu.com%2Fzhidao%2Fpic%2Fitem%2F9c16fdfaaf51f3de9ba8ee1194eef01f3a2979a8.jpg"
                }.ObjectToJson()
            });
            this.Messages.Add(new MessageInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Type = MessageTypeConst.Emotion,
                MsgId = "2",
                FromUser = "1",
                ToUsers = new string[] { "1" },
                MsgTime = DateTime.Now.AddYears(-1),
                ChatId = "123123123",
                State = MessageState.Received,
                Content = new EmotionMessageContent
                {
                    Url = "https://static.easyicon.net/preview/106/1069782.gif",
                    Name = "图片",
                    IsGif = true
                }.ObjectToJson()
            });
            this.Messages.Add(new MessageInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Type = MessageTypeConst.File,
                MsgId = "3",
                FromUser = "1111",
                ToUsers = new string[] { "1" },
                MsgTime = DateTime.Now.AddYears(-1),
                ChatId = "123123123",
                State = MessageState.Received,
                Content = new FileMessageContent
                {
                    Url = "http://swdlcdn.eastmoney.com/swchighexper/highexper.exe",
                    Name = "操盘密码",
                    Extension = "exe"
                }.ObjectToJson()
            });
            this.Messages.Add(new MessageInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Type = MessageTypeConst.Link,
                MsgId = "4",
                FromUser = "1",
                ToUsers = new string[] { "1" },
                MsgTime = DateTime.Now.AddYears(-1),
                ChatId = "123123123",
                State = MessageState.Received,
                Content = new LinkMessageContent
                {
                    Url = "https://www.baidu.com/",
                    ThumbUrl = "https://dss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=3738723861,292586857&fm=26&gp=0.jpg",
                    Title = "测试链接",
                    Description = "百度（纳斯达克：BIDU）是全球最大的中文搜索引擎，中国最大的以信息和知识为核心的互联网综合服务公司，全球领先的人工智能平台型公司。百度愿景是：成为最懂用户，并能帮助人们成长的全球顶级高科技公司。"
                }.ObjectToJson()
            });
            this.Messages.Add(new MessageInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Type = MessageTypeConst.Mixed,
                MsgId = "5",
                FromUser = "1111",
                ToUsers = new string[] { "1" },
                MsgTime = DateTime.Now.AddYears(-1),
                ChatId = "123123123",
                State = MessageState.Sending,
                Content = new MixedMessageContent()
                {
                    Items = new MessageContentInfo[] {
                        new MessageContentInfo()
                        {
                           Type = MessageTypeConst.Emotion,
                             Content=new EmotionMessageContent {
                                Url = "https://static.easyicon.net/preview/106/1069782.gif",
                                Name = "图片",
                                IsGif = true
                            }.ObjectToJson()
                        },
                        new MessageContentInfo()
                        {
                            Type=MessageTypeConst.Text,
                            Content= new TextMessageContent(){
                                Content= "百度（纳斯达克：BIDU）是全球最大的中文搜索引擎，中国最大的以信息和知识为核心的互联网综合服务公司，全球领先的人工智能平台型公司。百度愿景是：成为最懂用户，并能帮助人们成长的全球顶级高科技公司。"
                            }.ObjectToJson()
                        },
                        new MessageContentInfo()
                        {
                            Type=MessageTypeConst.Link,
                            Content= new LinkMessageContent {
                                Url = "https://www.baidu.com/",
                                ThumbUrl = "https://dss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=3738723861,292586857&fm=26&gp=0.jpg",
                                Title = "测试链接",
                                Description = "百度（纳斯达克：BIDU）是全球最大的中文搜索引擎，中国最大的以信息和知识为核心的互联网综合服务公司，全球领先的人工智能平台型公司。百度愿景是：成为最懂用户，并能帮助人们成长的全球顶级高科技公司。"
                            }.ObjectToJson()
                        }
                    }
                }.ObjectToJson()
            });
            this.Messages.Add(new MessageInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Type = MessageTypeConst.Tips,
                MsgId = "5",
                FromUser = "System",
                ToUsers = new string[] { "1" },
                MsgTime = DateTime.Now.AddYears(-1),
                ChatId = "123123123",
                State = MessageState.Readed,
                Content = new TipsMessageContent()
                {
                    Content = "测试提示"
                }.ObjectToJson()
            });

            this.InputMessageContent = new MessageContentInfo()
            {
                Type = MessageTypeConst.Text,
                Content = new TextMessageContent()
                {
                    Content = "hello\r\nworld"
                }.ObjectToJson()
            };
        }
        #endregion

        #region 属性
        private ChatSliderAreaViewModel chatSliderAreaViewModel;
        public ChatSliderAreaViewModel ChatSliderAreaViewModel
        {
            get
            {
                return this.chatSliderAreaViewModel;
            }
            set
            {
                this.chatSliderAreaViewModel = value;
                this.NotifyPropertyChange(() => this.ChatSliderAreaViewModel);
            }
        }
        #endregion

        #region 命令 
        #endregion

        public override void Dispose()
        {
            this.eventAggregator.Subscribe(this.ChatSliderAreaViewModel);
            this.eventAggregator.Unsubscribe(this);
        }
    }
}
