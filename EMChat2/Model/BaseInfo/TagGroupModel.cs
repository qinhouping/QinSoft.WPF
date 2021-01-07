using EMChat2.Model.Enum;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 标签组信息模型
    /// </summary>
    public class TagGroupModel : PropertyChangedBase
    {
        #region 属性
        /// <summary>
        /// 标签组ID
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

        private TagGroupLevelEnum level;
        public TagGroupLevelEnum Level
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
        /// 业务类型
        /// </summary>
        private BusinessEnum business;
        public BusinessEnum Business
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

        /// <summary>
        /// 标签列表
        /// </summary>
        private ObservableCollection<TagModel> tags;
        public ObservableCollection<TagModel> Tags
        {
            get
            {
                return this.tags ?? new ObservableCollection<TagModel>();
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
            return this.id.Equals((obj as TagGroupModel)?.id);
        }
        #endregion
    }
}
