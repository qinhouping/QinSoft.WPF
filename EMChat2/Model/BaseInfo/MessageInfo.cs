using EMChat2.Common;
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
    /// 消息内容实体
    /// </summary>
    public class MessageContentInfo : PropertyChangedBase
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

    /// <summary>
    /// 消息状态枚举
    /// </summary>
    public enum MessageStateEnum
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
        #region 可见消息
        public const string Text = "text";
        public const string Emotion = "emotion";
        public const string Image = "image";
        public const string Voice = "voice";
        public const string Video = "video";
        public const string Link = "link";
        public const string File = "file";
        public const string Mixed = "mixed";
        //系统提示消息
        public const string Tips = "tips";
        #endregion

        #region 不可见消息
        public const string Event = "event";
        #endregion
    }

    /// <summary>
    /// 消息信息实体
    /// </summary>
    public class MessageInfo : MessageContentInfo
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
            return this.id?.GetHashCode() ?? this.id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.id.Equals((obj as MessageInfo)?.id);
        }
        #endregion
    }

    /// <summary>
    /// 文本消息内容
    /// </summary>
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

    /// <summary>
    /// 图片消息内容
    /// </summary>
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

    /// <summary>
    /// 标签消息内容
    /// </summary>
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

    /// <summary>
    /// 文件消息内容
    /// </summary>
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

    /// <summary>
    /// 链接消息内容
    /// </summary>
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

    /// <summary>
    /// 提示消息内容
    /// </summary>
    public class TipsMessageContent : PropertyChangedBase
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

    /// <summary>
    /// 混合消息内容（用于复杂消息类型）
    /// </summary>
    public class MixedMessageContent : PropertyChangedBase
    {
        private MessageContentInfo[] items;
        public MessageContentInfo[] Items
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

    /// <summary>
    /// 事件消息内容
    /// </summary>
    public class EventMessageContentBase : PropertyChangedBase
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
    }
}