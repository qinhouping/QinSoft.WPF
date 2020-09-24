using Newtonsoft.Json;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EMChat2.Model.Entity
{
    public class QuickReplyGroupInfo : PropertyChangedBase
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

        private QuickReplyGroupLevel level;
        public QuickReplyGroupLevel Level
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
                this.NotifyPropertyChange(() => this.Business);
            }
        }

        private ObservableCollection<QuickReplyInfo> quickReplys;
        public ObservableCollection<QuickReplyInfo> QuickReplys
        {
            get
            {
                return this.quickReplys;
            }
            set
            {
                this.quickReplys = value;
                this.NotifyPropertyChange(() => this.QuickReplys);

                ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.quickReplys);
                this.QuickReplysCollectionView = collectionView;
            }
        }

        private ICollectionView quickReplysCollectionView;
        [JsonIgnore]
        public ICollectionView QuickReplysCollectionView
        {
            get
            {
                return this.quickReplysCollectionView;
            }
            set
            {
                this.quickReplysCollectionView = value;
                this.NotifyPropertyChange(() => this.QuickReplysCollectionView);
            }
        }
    }

    public enum QuickReplyGroupLevel
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
