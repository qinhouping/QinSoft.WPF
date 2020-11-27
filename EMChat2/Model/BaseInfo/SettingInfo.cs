﻿using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 设置信息实体
    /// 包括全局配置和业务相关配置
    /// </summary>
    public class SettingInfo : PropertyChangedBase
    {
        public SettingInfo()
        {
            this.businessSettings = new Dictionary<BusinessEnum, BusinessSettingInfo>();
        }

        /// <summary>
        /// 截图时是否隐藏主窗口
        /// </summary>
        private bool isHideWhenCaptureScreen = true;
        public bool IsHideWhenCaptureScreen
        {
            get
            {
                return this.isHideWhenCaptureScreen;
            }
            set
            {
                this.isHideWhenCaptureScreen = value;
                this.NotifyPropertyChange(() => IsHideWhenCaptureScreen);
            }
        }

        /// <summary>
        /// 业务相关配置信息
        /// </summary>
        private IDictionary<BusinessEnum, BusinessSettingInfo> businessSettings;
        public IDictionary<BusinessEnum, BusinessSettingInfo> BusinessSettings
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
    }
}
