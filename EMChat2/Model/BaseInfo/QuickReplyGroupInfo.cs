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

namespace EMChat2.Model.BaseInfo
{
    public class QuickReplyGroupInfo : PropertyChangedBase
    {
        #region 属性
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

        private QuickReplyGroupLevelEnum level;
        public QuickReplyGroupLevelEnum Level
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
            return this.id.Equals((obj as QuickReplyGroupInfo)?.id);
        }
        #endregion
    }
}
