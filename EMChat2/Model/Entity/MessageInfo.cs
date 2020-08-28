using EMChat2.Common;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using static EMChat2.Common.FileInfoTools;

namespace EMChat2.Model.Entity
{
    /// <summary>
    /// 消息信息实体
    /// </summary>
    public class MessageInfo : PropertyChangedBase
    {
        #region 属性
        /// <summary>
        /// ID
        /// </summary>
        private string id;
        public virtual string Id
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
        public virtual string MsgId
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
        public virtual DateTime MsgTime
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
        public virtual string ChatId
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
        public virtual string RoomId
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
        public virtual string FromUser
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
        public virtual string[] ToUsers
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
        public virtual BusinessEnum? Business
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

        public virtual string Content { get; set; }

        public virtual string Mark { get; }

        private MsgState state;
        public MsgState State
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

    public enum MsgState
    {
        /// <summary>
        /// 发送中
        /// </summary>
        Sending,
        /// <summary>
        /// 发送的
        /// </summary>
        SendSuccess,
        /// <summary>
        /// 发送失败的
        /// </summary>
        SendFailure,
        /// <summary>
        /// 接收的
        /// </summary>
        Received,
        /// <summary>
        /// 拒绝的
        /// </summary>
        Refused,
        /// <summary>
        /// 已读的
        /// </summary>
        Readed,
        /// <summary>
        /// 撤回的
        /// </summary>
        Revoked,
        /// <summary>
        /// 移除的
        /// </summary>
        Deleted
    }

    /// <summary>
    /// 消息类型常量
    /// </summary>
    public static class MsgTypeConst
    {
        public static readonly string Text = "text";
        public static readonly string Emotion = "emotion";
        public static readonly string Image = "image";
        public static readonly string Voice = "voice";
        public static readonly string Video = "video";
        public static readonly string Revoke = "revoke";
        public static readonly string Link = "link";
        public static readonly string File = "file";
        public static readonly string Mixed = "mixed";

        public static readonly string Tips = "tips";
    }

    public class TextMessageInfo : MessageInfo
    {
        #region 构造函数
        public TextMessageInfo()
        {
            this.Type = MsgTypeConst.Text;
        }
        #endregion

        #region 属性  
        private TextMessageContent text;
        public TextMessageContent Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                this.NotifyPropertyChange(() => this.Text);
                this.NotifyPropertyChange(() => this.Mark);
                this.NotifyPropertyChange(() => this.Content);
            }
        }

        public override string Mark
        {
            get
            {
                return this.text.Content;
            }
        }

        public override string Content
        {
            get
            {
                return this.text.ObjectToJson();
            }
            set
            {
                this.text = value.JsonToObject<TextMessageContent>();
            }
        }
        #endregion
    }

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

    public class ImageMessageInfo : MessageInfo
    {
        #region 构造函数
        public ImageMessageInfo()
        {
            this.Type = MsgTypeConst.Image;
        }
        #endregion

        #region 属性  
        private ImageMessageContent image;
        public ImageMessageContent Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                this.NotifyPropertyChange(() => this.Image);
                this.NotifyPropertyChange(() => this.Mark);
                this.NotifyPropertyChange(() => this.Content);
            }
        }

        public override string Mark
        {
            get
            {
                return "[图片]";
            }
        }

        public override string Content
        {
            get
            {
                return this.image.ObjectToJson();
            }
            set
            {
                this.image = value.JsonToObject<ImageMessageContent>();
                this.NotifyPropertyChange(() => this.Image);
            }
        }
        #endregion
    }

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

    public class EmotionMessageInfo : MessageInfo
    {
        #region 构造函数
        public EmotionMessageInfo()
        {
            this.Type = MsgTypeConst.Emotion;
        }
        #endregion

        #region 属性  
        private EmotionMessageContent emotion;
        public EmotionMessageContent Emotion
        {
            get
            {
                return this.emotion;
            }
            set
            {
                this.emotion = value;
                this.NotifyPropertyChange(() => this.Emotion);
                this.NotifyPropertyChange(() => this.Mark);
                this.NotifyPropertyChange(() => this.Content);
            }
        }

        public override string Mark
        {
            get
            {
                return string.Format("[表情-{0}]", this.emotion.Name);
            }
        }

        public override string Content
        {
            get
            {
                return this.emotion.ObjectToJson();
            }
            set
            {
                this.emotion = value.JsonToObject<EmotionMessageContent>();
                this.NotifyPropertyChange(() => this.Emotion);
            }
        }
        #endregion
    }

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

    public class FileMessageInfo : MessageInfo
    {
        #region 构造函数 
        public FileMessageInfo()
        {
            this.Type = MsgTypeConst.File;
        }
        #endregion

        #region 属性  
        private FileMessageContent file;
        public FileMessageContent File
        {
            get
            {
                return this.file;
            }
            set
            {
                this.file = value;
                this.NotifyPropertyChange(() => this.File);
                this.NotifyPropertyChange(() => this.Mark);
                this.NotifyPropertyChange(() => this.Content);
            }
        }
        public override string Mark
        {
            get
            {
                return string.Format("[文件-{0}]", this.file.Name);
            }
        }

        public override string Content
        {
            get
            {
                return this.file.ObjectToJson();
            }
            set
            {
                this.file = value.JsonToObject<FileMessageContent>();
                this.NotifyPropertyChange(() => this.File);
            }
        }
        #endregion
    }

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

    public class LinkMessageInfo : MessageInfo
    {
        #region 构造函数 
        public LinkMessageInfo()
        {
            this.Type = MsgTypeConst.Link;
        }
        #endregion

        #region 属性  
        private LinkMessageContent link;
        public LinkMessageContent Link
        {
            get
            {
                return this.link;
            }
            set
            {
                this.link = value;
                this.NotifyPropertyChange(() => this.Link);
                this.NotifyPropertyChange(() => this.Mark);
                this.NotifyPropertyChange(() => this.Content);
            }
        }
        public override string Mark
        {
            get
            {
                return string.Format("[链接-{0}]", link.Title);
            }
        }

        public override string Content
        {
            get
            {
                return this.link.ObjectToJson();
            }
            set
            {
                this.link = value.JsonToObject<LinkMessageContent>();
                this.NotifyPropertyChange(() => this.Link);
            }
        }
        #endregion
    }

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

    public class MixedMessageInfo : MessageInfo
    {
        #region 构造函数 
        public MixedMessageInfo()
        {
            this.Type = MsgTypeConst.Mixed;
        }
        #endregion

        #region 属性  
        private MixedMessageContent mixed;
        public MixedMessageContent Mixed
        {
            get
            {
                return this.mixed;
            }
            set
            {
                this.mixed = value;
                this.NotifyPropertyChange(() => this.Mixed);
                this.NotifyPropertyChange(() => this.Mark);
                this.NotifyPropertyChange(() => this.Content);
            }
        }
        public override string Mark
        {
            get
            {
                List<string> marks = new List<string>();
                foreach (MessageInfo message in this.mixed.Items)
                {
                    marks.Add(message.Mark);
                }
                return string.Join("", marks);
            }
        }

        public override string Content
        {
            get
            {
                return this.mixed.ObjectToJson();
            }
            set
            {
                this.mixed = value.JsonToObject<MixedMessageContent>();
                this.NotifyPropertyChange(() => this.Mixed);
            }
        }
        #endregion
    }

    public class MixedMessageContent : PropertyChangedBase
    {
        private MessageInfo[] items;
        public MessageInfo[] Items
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
}