using EMChat2.Common;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 设置信息模型
    /// 包括全局配置和业务相关配置
    /// </summary>
    public class SettingModel : PropertyChangedBase, ICloneable
    {
        public SettingModel()
        {
            this.businessSettings = new ObservableCollection<BusinessSettingModel>();
        }

        #region 通用设置
        /// <summary>
        /// 开机自动启动
        /// </summary>
        private bool autoStartup = true;
        public bool AutoStartup
        {
            get
            {
                return this.autoStartup;
            }
            set
            {
                this.autoStartup = value;
                this.NotifyPropertyChange(() => this.AutoStartup);
            }
        }

        /// <summary>
        /// 关闭面板时，结束程序
        /// </summary>
        private bool isCloseApplication = false;
        public bool IsCloseApplication
        {
            get
            {
                return this.isCloseApplication;
            }
            set
            {
                this.isCloseApplication = value;
                this.NotifyPropertyChange(() => this.IsCloseApplication);
            }
        }

        /// <summary>
        /// 新消息通知
        /// </summary>
        private bool isInform = true;
        public bool IsInform
        {
            get
            {
                return this.isInform;
            }
            set
            {
                this.isInform = value;
                this.NotifyPropertyChange(() => this.IsInform);
            }
        }
        #endregion

        #region 业务配置
        /// <summary>
        /// 业务相关配置信息
        /// </summary>
        private ObservableCollection<BusinessSettingModel> businessSettings;
        public ObservableCollection<BusinessSettingModel> BusinessSettings
        {
            get
            {
                return this.businessSettings;
            }
            set
            {
                this.businessSettings = value;
                this.NotifyPropertyChange(() => BusinessSettings);
            }
        }
        #endregion
        public object Clone()
        {
            SettingModel setting = this.CloneObject();
            setting.businessSettings = new ObservableCollection<BusinessSettingModel>(this.BusinessSettings?.Select(u => u.Clone() as BusinessSettingModel));
            return setting;
        }
    }
}
