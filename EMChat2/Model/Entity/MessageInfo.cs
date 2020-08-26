using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace EMChat2.Model.Entity
{
    /// <summary>
    /// 消息信息实体
    /// </summary>
    public abstract class MessageInfo : PropertyChangedBase
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

        public abstract string Mark { get; }

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
        public static readonly string Mixed = "mixed";
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
        private TextMessageContext text;
        public TextMessageContext Text
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
            }
        }

        public override string Mark
        {
            get
            {
                return this.text.Content;
            }
        }
        #endregion
    }

    public class TextMessageContext : PropertyChangedBase
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
}
