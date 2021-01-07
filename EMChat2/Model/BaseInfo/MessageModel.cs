﻿using EMChat2.Common;
using EMChat2.Model.Enum;
using Newtonsoft.Json;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{

    /// <summary>
    /// 消息内容
    /// </summary>
    public class MessageContent : PropertyChangedBase, ICloneable
    {
        public virtual object Clone()
        {
            return new MessageContent();
        }
    }

    /// <summary>
    /// 消息内容模型
    /// 包括消息类型和消息内容
    /// </summary>
    public class MessageContentModel : PropertyChangedBase, ICloneable
    {
        private string type;
        public virtual string Type
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

        private MessageContent content;
        [JsonIgnore]
        public virtual MessageContent Content
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

        [JsonProperty("Content")]
        public string ContentString
        {
            get
            {
                return MessageTools.SerializeMessageContent(this.content);
            }
            set
            {
                this.content = MessageTools.DeserializeMessageContent(this.type, value);
                this.NotifyPropertyChange(() => this.ContentString);
                this.NotifyPropertyChange(() => this.Content);
            }
        }

        public virtual object Clone()
        {
            return new MessageContentModel()
            {
                Type = this.Type,
                Content = this.Content?.Clone() as MessageContent
            };
        }
    }


    /// <summary>
    /// 消息模型
    /// </summary>
    public class MessageModel : MessageContentModel
    {
        #region 属性
        /// <summary>
        /// 消息ID
        /// </summary>
        private string id;
        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
                this.NotifyPropertyChange(() => this.Id);
            }
        }

        /// <summary>
        /// 消息发送时间
        /// </summary>
        private DateTime time;
        public DateTime Time
        {
            get
            {
                return this.time;
            }
            set
            {
                this.time = value;
                this.NotifyPropertyChange(() => this.Time);
            }
        }

        /// <summary>
        /// 会话ID
        /// </summary>
        private string chatId;
        public string ChatId
        {
            get
            {
                return this.chatId;
            }
            set
            {
                this.chatId = value;
                this.NotifyPropertyChange(() => this.ChatId);
            }
        }

        /// <summary>
        /// 发送者
        /// </summary>
        private UserModel fromUser;
        public UserModel FromUser
        {
            get
            {
                return this.fromUser;
            }
            set
            {
                this.fromUser = value;
                this.NotifyPropertyChange(() => this.FromUser);
            }
        }

        /// <summary>
        /// 接收者列表
        /// </summary>
        private UserModel[] toUsers;
        public UserModel[] ToUsers
        {
            get
            {
                return this.toUsers;
            }
            set
            {
                this.toUsers = value;
                this.NotifyPropertyChange(() => this.ToUsers);
            }
        }

        /// <summary>
        /// 消息状态
        /// </summary>
        private MessageStateEnum state;
        public MessageStateEnum State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
                this.NotifyPropertyChange(() => this.State);
            }
        }
        #endregion

        #region 方法
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.id.Equals((obj as MessageModel)?.id);
        }

        public override object Clone()
        {
            return new MessageModel()
            {
                Id = this.Id,
                Time = this.Time,
                ChatId = this.ChatId,
                FromUser = this.FromUser,
                ToUsers = this.ToUsers,
                State = this.State,
                Type = this.Type,
                Content = this.Content?.Clone() as MessageContent
            };
        }
        #endregion
    }

    #region 具体消息内容

    /// <summary>
    /// 文本消息内容
    /// </summary>
    public class TextMessageContent : MessageContent
    {
        private string content;
        public string Content
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

        public override object Clone()
        {
            return new TextMessageContent()
            {
                Content = this.Content
            };
        }
    }

    /// <summary>
    /// 图片消息内容
    /// </summary>
    public class ImageMessageContent : MessageContent
    {
        private string url;
        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
                this.NotifyPropertyChange(() => this.Url);
            }
        }

        public override object Clone()
        {
            return new ImageMessageContent()
            {
                Url = this.Url
            };
        }
    }

    /// <summary>
    /// 标签消息内容
    /// </summary>
    public class EmotionMessageContent : MessageContent
    {
        private string url;
        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
                this.NotifyPropertyChange(() => this.Url);
            }
        }
        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.NotifyPropertyChange(() => this.Name);
            }
        }

        private bool isGif;
        public bool IsGif
        {
            get
            {
                return this.isGif;
            }
            set
            {
                this.isGif = value;
                this.NotifyPropertyChange(() => this.IsGif);
            }
        }

        public override object Clone()
        {
            return new EmotionMessageContent()
            {
                Url = this.Url,
                Name = this.Name,
                IsGif = this.IsGif
            };
        }
    }

    /// <summary>
    /// 文件消息内容
    /// </summary>
    public class FileMessageContent : MessageContent
    {
        private string url;
        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
                this.NotifyPropertyChange(() => this.Url);
            }
        }
        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.NotifyPropertyChange(() => this.Name);
            }
        }
        private string extension;
        public string Extension
        {
            get
            {
                return this.extension;
            }
            set
            {
                this.extension = value;
                this.NotifyPropertyChange(() => this.Extension);
            }
        }

        public override object Clone()
        {
            return new FileMessageContent()
            {
                Url = this.Url,
                Name = this.Name,
                Extension = this.Extension
            };
        }
    }

    /// <summary>
    /// 链接消息内容
    /// </summary>
    public class LinkMessageContent : MessageContent
    {
        private string url;
        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
                this.NotifyPropertyChange(() => this.Url);
            }
        }

        private string thumbUrl;
        public string ThumbUrl
        {
            get
            {
                return this.thumbUrl;
            }
            set
            {
                this.thumbUrl = value;
                this.NotifyPropertyChange(() => this.ThumbUrl);
            }
        }

        private string title;
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                this.NotifyPropertyChange(() => this.Title);
            }
        }

        private string description;
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
                this.NotifyPropertyChange(() => this.Description);
            }
        }

        public override object Clone()
        {
            return new LinkMessageContent()
            {
                Url = this.Url,
                ThumbUrl = this.ThumbUrl,
                Title = this.Title,
                Description = this.Description
            };
        }
    }

    /// <summary>
    /// 混合消息内容（用于复杂消息类型）
    /// </summary>
    public class MixedMessageContent : MessageContent
    {
        private MessageContentModel[] items;
        public MessageContentModel[] Items
        {
            get
            {
                return this.items;
            }
            set
            {
                this.items = value;
                this.NotifyPropertyChange(() => this.Items);
            }
        }

        public override object Clone()
        {
            return new MixedMessageContent()
            {
                Items = this.Items?.Select(u => u.Clone() as MessageContentModel).ToArray()
            };
        }
    }

    /// <summary>
    /// 分享消息内容
    /// </summary>
    public class ShareMessageContent : MessageContent
    {
        private string chatId;
        public string ChatId
        {
            get
            {
                return this.chatId;
            }
            set
            {
                this.chatId = value;
                this.NotifyPropertyChange(() => this.ChatId);
            }
        }

        private string[] messageIds;
        public string[] MessageIds
        {
            get
            {
                return this.messageIds;
            }
            set
            {
                this.messageIds = value;
                this.NotifyPropertyChange(() => this.MessageIds);
            }
        }
        public override object Clone()
        {
            return new ShareMessageContent()
            {
                ChatId = this.ChatId,
                MessageIds = this.MessageIds?.Select(u => u).ToArray()
            };
        }
    }

    /// <summary>
    /// 提示消息内容
    /// </summary>
    public class TipsMessageContent : MessageContent
    {
        private string content;
        public string Content
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

        public override object Clone()
        {
            return new TipsMessageContent()
            {
                Content = this.Content
            };
        }
    }



    #region 事件消息内容
    /// <summary>
    /// 事件消息内容
    /// </summary>
    public class EventMessageContent : MessageContent
    {
        private string _event;
        public string Event
        {
            get
            {
                return this._event;
            }
            set
            {
                this._event = value;
                this.NotifyPropertyChange(() => this.Event);
            }
        }

        public override object Clone()
        {
            return new EventMessageContent()
            {
                Event = this.Event
            };
        }
    }

    public class RecvMessageEventMessageContent : EventMessageContent
    {
        public RecvMessageEventMessageContent()
        {
            this.Event = EventMessageTypeConst.RecvMessage;
        }

        private string messageId;
        public string MessageId
        {
            get
            {
                return this.messageId;
            }
            set
            {
                this.messageId = value;
                this.NotifyPropertyChange(() => this.MessageId);
            }
        }

        public override object Clone()
        {
            return new RecvMessageEventMessageContent()
            {
                Event = this.Event,
                MessageId = this.MessageId
            };
        }
    }

    public class ReadMessageEventMessageContent : EventMessageContent
    {
        public ReadMessageEventMessageContent()
        {
            this.Event = EventMessageTypeConst.ReadMessage;
        }

        private string[] messageIds;

        public string[] MessageIds
        {
            get
            {
                return this.messageIds;
            }
            set
            {
                this.messageIds = value;
                this.NotifyPropertyChange(() => this.MessageIds);
            }
        }

        public override object Clone()
        {
            return new ReadMessageEventMessageContent()
            {
                Event = this.Event,
                messageIds = this.messageIds?.Select(u => u).ToArray()
            };
        }
    }

    public class RefuseMessageEventMessageContent : EventMessageContent
    {
        public RefuseMessageEventMessageContent()
        {
            this.Event = EventMessageTypeConst.RefuseMessage;
        }

        private string messageId;
        public string MessageId
        {
            get
            {
                return this.messageId;
            }
            set
            {
                this.messageId = value;
                this.NotifyPropertyChange(() => this.MessageId);
            }
        }

        public override object Clone()
        {
            return new RefuseMessageEventMessageContent()
            {
                Event = this.Event,
                MessageId = this.MessageId
            };
        }
    }

    public class RevokeMessageEventMessageContent : EventMessageContent
    {
        public RevokeMessageEventMessageContent()
        {
            this.Event = EventMessageTypeConst.RevokeMessage;
        }

        private string messageId;
        public string MessageId
        {
            get
            {
                return this.messageId;
            }
            set
            {
                this.messageId = value;
                this.NotifyPropertyChange(() => this.MessageId);
            }
        }

        public override object Clone()
        {
            return new RevokeMessageEventMessageContent()
            {
                Event = this.Event,
                MessageId = this.MessageId
            };
        }
    }

    public class InputMessageEventMessageContent : EventMessageContent
    {
        public InputMessageEventMessageContent()
        {
            this.Event = EventMessageTypeConst.InputMessage;
        }

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
            }
        }

        private string chatId;
        public string ChatId
        {
            get
            {
                return this.chatId;
            }
            set
            {
                this.chatId = value;
                this.NotifyPropertyChange(() => this.ChatId);
            }
        }
        public override object Clone()
        {
            return new InputMessageEventMessageContent()
            {
                Event = this.Event,
                ChatId = this.ChatId,
                IsInputing = this.IsInputing
            };
        }
    }
    #endregion
    #endregion
}