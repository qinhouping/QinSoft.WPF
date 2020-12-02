using Newtonsoft.Json;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 标签模型
    /// </summary>
    public class TagModel : PropertyChangedBase, ISelectable
    {
        #region 属性
        /// <summary>
        /// 标签ID
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
        /// 标签名字
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
        #endregion

        #region 实现接口 ISelectable
        private bool isSelected;
        [JsonIgnore]
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
                this.NotifyPropertyChange(() => this.IsSelected); ;
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
            return this.id.Equals((obj as TagModel)?.id);
        }
        #endregion
    }
}
