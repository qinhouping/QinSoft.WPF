﻿using EMChat2.Model.Enum;
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
    public class QuickReplyGroupModel : PropertyChangedBase
    {
        #region 属性
        /// <summary>
        /// 快捷回复组ID
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
        /// 快捷回复组名称
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
        /// 快捷回复组级别
        /// </summary>
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

        /// <summary>
        /// 快捷回复组所属业务ID
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
                this.NotifyPropertyChange(() => this.BusinessId);
            }
        }

        /// <summary>
        /// 快捷回复组明细
        /// </summary>
        private ObservableCollection<QuickReplyModel> quickReplies;
        public ObservableCollection<QuickReplyModel> QuickReplies
        {
            get
            {
                return this.quickReplies;
            }
            set
            {
                this.quickReplies = value;
                this.NotifyPropertyChange(() => this.QuickReplies);
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
            return this.id.Equals((obj as QuickReplyGroupModel)?.id);
        }
        #endregion
    }
}
