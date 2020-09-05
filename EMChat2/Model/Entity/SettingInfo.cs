﻿using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Entity
{
    /// <summary>
    /// 设置信息实体
    /// </summary>
    public class SettingInfo : PropertyChangedBase
    {
        /// <summary>
        /// 是否置顶显示主窗口
        /// </summary>
        private bool isTopmostShellView;
        public bool IsTopmostShellView
        {
            get
            {
                return this.isTopmostShellView;
            }
            set
            {
                this.isTopmostShellView = value;
                this.NotifyPropertyChange(() => this.IsTopmostShellView);
            }
        }

        /// <summary>
        /// 是否显示会话侧边栏
        /// </summary>
        private bool isShowChatSlider = true;
        public bool IsShowChatSlider
        {
            get
            {
                return this.isShowChatSlider;
            }
            set
            {
                this.isShowChatSlider = value;
                this.NotifyPropertyChange(() => this.IsShowChatSlider);
            }
        }
    }
}
