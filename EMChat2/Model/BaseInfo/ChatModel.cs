using EMChat2.Model.Enum;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 会话信息模型
    /// </summary>
    public class ChatModel : PropertyChangedBase
    {
        #region 属性
        /// <summary>
        /// 会话ID
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
        /// 业务ID
        /// </summary>
        private string businessId;
        public string BusinessId
        {
            get
            {
                return this.businessId;
            }
            set
            {
                this.businessId = value;
                this.NotifyPropertyChange(() => this.businessId);
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
        private ChatTypeEnum type;
        public ChatTypeEnum Type
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
        private string headerImage;
        public string HeaderImage
        {
            get
            {
                return this.headerImage;
            }
            set
            {
                this.headerImage = value;
                this.NotifyPropertyChange(() => this.HeaderImage);
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
        /// 当前参与会话用户（用于确定消息的发送范围）
        /// </summary>
        private ObservableCollection<UserModel> chatUsers;
        public ObservableCollection<UserModel> ChatUsers
        {
            get
            {
                return this.chatUsers ?? new ObservableCollection<UserModel>();
            }
            set
            {
                this.chatUsers = value;
                this.NotifyPropertyChange(() => this.ChatUsers);
            }
        }

        /// <summary>
        /// 所有会话用户
        /// 包括历史会话用户（用于渲染消息列表发送人信息）
        /// </summary>
        private ObservableCollection<UserModel> chatAllUsers;
        public ObservableCollection<UserModel> ChatAllUsers
        {
            get
            {
                return this.chatAllUsers;
            }
            set
            {
                this.chatAllUsers = value;
                this.NotifyPropertyChange(() => this.ChatAllUsers);
            }
        }

        /// <summary>
        /// 会话消息
        /// </summary>
        private ObservableCollection<MessageModel> messages;
        public ObservableCollection<MessageModel> Messages
        {
            get
            {
                return this.messages;
            }
            set
            {
                this.messages = value;
                this.NotifyPropertyChange(() => this.Messages);
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
            return this.id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.id.Equals((obj as ChatModel)?.id);
        }
        #endregion
    }
}
