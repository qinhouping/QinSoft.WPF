using EMChat2.Common;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Entity
{
    /// <summary>
    /// 消息信息实体
    /// </summary>
    [Serializable]
    public class MessageInfo : PropertyChangedBase
    {
        #region 属性
        /// <summary>
        /// ID
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

        private string msgId;
        public string MsgId
        {
            get
            {
                return this.msgId;
            }
            set
            {
                this.msgId = value;
                this.NotifyPropertyChange(() => this.MsgId);
            }
        }

        private DateTime msgTime;
        public DateTime MsgTime
        {
            get
            {
                return this.msgTime;
            }
            set
            {
                this.msgTime = value;
                this.NotifyPropertyChange(() => this.MsgTime);
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

        private string roomId;
        public string RoomId
        {
            get
            {
                return this.roomId;
            }
            set
            {
                this.roomId = value;
                this.NotifyPropertyChange(() => this.RoomId);
            }
        }

        private string fromUser;
        public string FromUser
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

        private string[] toUsers;
        public string[] ToUsers
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

        private BusinessEnum? business;
        public BusinessEnum? Business
        {
            get
            {
                return this.business;
            }
            set
            {
                this.business = value;
                this.NotifyPropertyChange(() => this.Business);
            }
        }

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

        public string Content { get; set; }

        private MessageState state;
        public MessageState State
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
            return this.msgId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.msgId.Equals((obj as MessageInfo)?.msgId);
        }
        #endregion
    }

    public enum MessageState
    {
        /// <summary>
        /// 发送中
        /// </summary>
        [Description("发送中")]
        Sending,
        /// <summary>
        /// 发送成功
        /// </summary>
        [Description("发送成功")]
        SendSuccess,
        /// <summary>
        /// 发送失败
        /// </summary>
        [Description("发送失败")]
        SendFailure,
        /// <summary>
        /// 接收的
        /// </summary>
        [Description("已接收")]
        Received,
        /// <summary>
        /// 拒绝的
        /// </summary>
        [Description("已拒绝")]
        Refused,
        /// <summary>
        /// 已读的
        /// </summary>
        [Description("已读")]
        Readed,
        /// <summary>
        /// 撤回的
        /// </summary>
        [Description("已撤回")]
        Revoked,
        /// <summary>
        /// 移除的
        /// </summary>
        [Description("已移除")]
        Deleted
    }

    /// <summary>
    /// 消息类型常量
    /// </summary>
    public static class MessageTypeConst
    {
        public const string Text = "text";
        public const string Emotion = "emotion";
        public const string Image = "image";
        public const string Voice = "voice";
        public const string Video = "video";
        public const string Revoke = "revoke";
        public const string Link = "link";
        public const string File = "file";
        public const string Mixed = "mixed";

        public static readonly string Event = "event";

        public static readonly string Tips = "tips";
    }


    [Serializable]

    public class TextMessageContent : PropertyChangedBase
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
    }

    [Serializable]
    public class ImageMessageContent : PropertyChangedBase
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
    }


    [Serializable]
    public class EmotionMessageContent : PropertyChangedBase
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
    }

    [Serializable]
    public class FileMessageContent : PropertyChangedBase
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
    }

    [Serializable]
    public class LinkMessageContent : PropertyChangedBase
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
    }


    [Serializable]
    public class MixedMessageContent : PropertyChangedBase
    {
        private MixedMessageContentItem[] items;
        public MixedMessageContentItem[] Items
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
    }

    [Serializable]
    public class MixedMessageContentItem : PropertyChangedBase
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

        private string content;
        public virtual string Content
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
    }
}