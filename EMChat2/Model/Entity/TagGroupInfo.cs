using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Entity
{
    public class TagGroupInfo : PropertyChangedBase
    {
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
