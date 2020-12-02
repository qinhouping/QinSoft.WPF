using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    public class QuickReplyModel : PropertyChangedBase, IExpandable
    {
        #region 属性
        /// <summary>
        /// 快捷回复ID
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
        /// 快捷回复名称
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
                this.NotifyPropertyChange(() => this.Name);
            }
        }

        /// <summary>
        /// 快捷回复是否展开
        /// </summary>
        private bool isExpanded;
        public bool IsExpanded
        {
            get
            {
                return this.isExpanded;
            }
            set
            {
                this.isExpanded = value;
                this.NotifyPropertyChange(() => this.IsExpanded);
            }
        }

        /// <summary>
        /// 快捷回复内容
        /// </summary>
        private MessageContentModel content;
        public MessageContentModel Content
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

        #region 方法
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.id.Equals((obj as QuickReplyModel)?.id);
        }
        #endregion
    }
}
