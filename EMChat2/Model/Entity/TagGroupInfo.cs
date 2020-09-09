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

        private TagLevel tagLevel;
        public TagLevel TagLevel
        {
            get
            {
                return this.tagLevel;
            }
            set
            {
                this.tagLevel = value;
                this.NotifyPropertyChange(() => this.TagLevel);
            }
        }

        /// <summary>
        /// 标签列表
        /// </summary>
        private ObservableCollection<TagInfo> tags;
        public ObservableCollection<TagInfo> Tags
        {
            get
            {
                return this.tags;
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
        System,
        /// <summary>
        /// 用户级别
        /// </summary>
        User
    }
}
