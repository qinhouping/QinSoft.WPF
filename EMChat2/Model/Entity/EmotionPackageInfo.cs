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
    /// 表情包信息实体
    /// </summary>
    public class EmotionPackageInfo : PropertyChangedBase
    {
        #region 属性
        /// <summary>
        /// 表情包ID
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
        /// 表情包名称
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
        /// 表情包缩略图
        /// </summary>
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
        /// <summary>
        /// 表情包级别
        /// </summary>
        private EmotionPackageLevel level;
        public EmotionPackageLevel Level
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
        /// 表情列表
        /// </summary>
        private ObservableCollection<EmotionInfo> emotions;
        public ObservableCollection<EmotionInfo> Emotions
        {
            get
            {
                return this.emotions;
            }
            set
            {
                this.emotions = value;
                this.NotifyPropertyChange(() => this.Emotions);
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
            return this.id.Equals((obj as EmotionPackageInfo)?.id);
        }
        #endregion
    }

    /// <summary>
    /// 表情包级别
    /// </summary>
    public enum EmotionPackageLevel
    {
        /// <summary>
        /// 系统表情包
        /// </summary>
        System,
        /// <summary>
        /// 收藏表情包
        /// </summary>
        Favorite,
        /// <summary>
        /// 自定义表情包
        /// </summary>
        User
    }
}
