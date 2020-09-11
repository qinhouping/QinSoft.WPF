using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Entity
{
    /// <summary>
    /// 标签组信息实体
    /// </summary>
    public class TagGroupInfo : PropertyChangedBase
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

        /// <summary>
        /// 标签组名称
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
        /// 标签级别
        /// </summary>

        private TagLevel level;
        public TagLevel Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
                this.NotifyPropertyChange(() => this.Level);
            }
        }

        /// <summary>
        /// 标签列表
        /// </summary>
        private ThreadSafeObservableCollection<TagInfo> tags;
        public ThreadSafeObservableCollection<TagInfo> Tags
        {
            get
            {
                return this.tags ?? new ThreadSafeObservableCollection<TagInfo>();
            }
            set
            {
                this.tags = value;
                this.NotifyPropertyChange(() => this.Tags);
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
            return this.id.Equals((obj as TagGroupInfo)?.id);
        }
        #endregion
    }

    /// <summary>
    /// 标签级别
    /// </summary>
    public enum TagLevel
    {
        /// <summary>
        /// 系统级别
        /// </summary>
        [Description("系统")]
        System,
        /// <summary>
        /// 用户级别
        /// </summary>
        [Description("用户")]
        User
    }
}
