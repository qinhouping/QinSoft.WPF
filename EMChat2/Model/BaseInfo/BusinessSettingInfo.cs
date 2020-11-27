using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 业务相关配置信息
    /// </summary>
    public class BusinessSettingInfo : PropertyChangedBase
    {
        /// <summary>
        /// 是否允许发送消息（总开关）
        /// </summary>
        private bool allowSendMessage;
        public bool AllowSendMessage
        {
            get
            {
                return this.allowSendMessage;
            }
            set
            {
                this.allowSendMessage = value;
                this.NotifyPropertyChange(() => AllowSendMessage);
            }
        }

        /// <summary>
        /// 是否允许文字输入
        /// </summary>
        private bool allowInputText = false;
        public bool AllowInputText
        {
            get
            {
                return this.allowInputText;
            }
            set
            {
                this.allowInputText = value;
                this.NotifyPropertyChange(() => AllowInputText);
            }
        }

        /// <summary>
        /// 是否允许截屏
        /// </summary>
        private bool allowCaptureScreen;
        public bool AllowCaptureScreen
        {
            get
            {
                return this.allowCaptureScreen;
            }
            set
            {
                this.allowCaptureScreen = value;
                this.NotifyPropertyChange(() => AllowCaptureScreen);
            }
        }


        /// <summary>
        /// 是否允许选择图片
        /// </summary>
        private bool allowSelectImage;
        public bool AllowSelectImage
        {
            get
            {
                return this.allowSelectImage;
            }
            set
            {
                this.allowSelectImage = value;
                this.NotifyPropertyChange(() => AllowSelectImage);
            }
        }

        /// <summary>
        /// 是否允许选择文件
        /// </summary>
        private bool allowSelectFile = false;
        public bool AllowSelectFile
        {
            get
            {
                return this.allowSelectFile;
            }
            set
            {
                this.allowSelectFile = value;
                this.NotifyPropertyChange(() => AllowSelectFile);
            }
        }


        /// <summary>
        /// 是否允许撤回消息
        /// </summary>
        private bool allowRollbackMessage;
        public bool AllowRollBackMessage
        {
            get
            {
                return this.allowRollbackMessage;
            }
            set
            {
                this.allowRollbackMessage = value;
                this.NotifyPropertyChange(() => AllowRollBackMessage);
            }
        }

        /// <summary>
        /// 允许最大消息撤回的时间间隔
        /// </summary>
        private int maxRollbackMessageTotalMinutes;
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

        /// <summary>
        /// 是否允许选择快速回复
        /// </summary>
        private bool allowSelectQuickReply;
        public bool AllowSelectQuickReply
        {
            get
            {
                return this.allowSelectQuickReply;
            }
            set
            {
                this.allowSelectQuickReply = value;
                this.NotifyPropertyChange(() => AllowSelectQuickReply);
            }
        }
    }
}
