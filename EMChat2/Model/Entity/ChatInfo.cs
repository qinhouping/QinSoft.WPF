using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Entity
{
    /// <summary>
    /// 会话信息实体
    /// </summary>
    public class ChatInfo : PropertyChangedBase
    {
        #region 构造函数
        public ChatInfo()
        {
            this.createTime = DateTime.Now;
        }
        #endregion

        #region 属性

        /// <summary>
        /// id
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
        /// 房间id（群聊才会设置该值）
        /// </summary>
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

        /// <summary>
        /// 会话id
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
        /// 业务
        /// </summary>
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
                this.NotifyPropertyChange(() => this.business);
            }
        }

        /// <summary>
        /// 会话名称
        /// </summary>
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
                this.NotifyPropertyChange(() => this.name);
            }
        }

        /// <summary>
        /// 会话类型
        /// </summary>
        private ChatType type;
        public ChatType Type
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

        /// <summary>
        /// 会话头像
        /// </summary>
        private string headerImageUrl;
        public string HeaderImageUrl
        {
            get
            {
                return this.headerImageUrl;
            }
            set
            {
                this.headerImageUrl = value;
                this.NotifyPropertyChange(() => this.HeaderImageUrl);
            }
        }

        /// <summary>
        /// 是否置顶
        /// </summary>
        private bool isTop;
        public bool IsTop
        {
            get
            {
                return this.isTop;
            }
            set
            {
                this.isTop = value;
                this.NotifyPropertyChange(() => this.IsTop);
            }
        }

        /// <summary>
        /// 是否提醒
        /// </summary>
        private bool isInform;
        public bool IsInform
        {
            get
            {
                return this.isInform;
            }
            set
            {
                this.isInform = value;
                this.NotifyPropertyChange(() => this.IsInform);
            }
        }

        /// <summary>
        /// 参与会话用户
        /// </summary>
        private ObservableCollection<UserInfo> chatUsers;
        public ObservableCollection<UserInfo> ChatUsers
        {
            get
            {
                return this.chatUsers;
            }
            set
            {
                this.chatUsers = value;
                this.NotifyPropertyChange(() => this.ChatUsers);
            }
        }

        /// <summary>
        /// 会话创建时间
        /// </summary>
        private DateTime createTime;
        public DateTime CreateTime
        {
            get
            {
                return this.createTime;
            }
            set
            {
                this.createTime = value;
                this.NotifyPropertyChange(() => this.CreateTime);
            }
        }
        #endregion

        #region 方法
        public override int GetHashCode()
        {
            return this.chatId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.chatId.Equals((obj as ChatInfo)?.chatId);
        }
        #endregion
    }

    public enum ChatType
    {
        /// <summary>
        /// 私聊
        /// </summary>
        Private,
        /// <summary>
        /// 群聊
        /// </summary>
        Group,

    }
}
