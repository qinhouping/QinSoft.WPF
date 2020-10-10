using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 设置信息实体
    /// </summary>
    public class SettingInfo : PropertyChangedBase
    {
        /// <summary>
        /// 是否置顶显示主窗口
        /// </summary>
        private bool isTopmostMainView;
        public bool IsTopmostMainView
        {
            get
            {
                return this.isTopmostMainView;
            }
            set
            {
                this.isTopmostMainView = value;
                this.NotifyPropertyChange(() => this.IsTopmostMainView);
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
        /// 允许最大消息撤回的时间间隔（单位分钟，默认2分钟）
        /// </summary>
        private int maxRollbackMessageTotalMinutes = 2;
        public int MaxRollbackMessageTotalMinutes
        {
            get
            {
                return this.maxRollbackMessageTotalMinutes;
            }
            set
            {
                this.maxRollbackMessageTotalMinutes = value;
                this.NotifyPropertyChange(() => MaxRollbackMessageTotalMinutes);
            }
        }
    }
}
