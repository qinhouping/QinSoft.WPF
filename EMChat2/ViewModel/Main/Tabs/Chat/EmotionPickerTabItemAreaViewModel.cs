using EMChat2.Model.Entity;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.Log;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    public class EmotionPickerTabItemAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public EmotionPickerTabItemAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, EmotionPackageInfo emotionPackage)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.emotionPackage = emotionPackage;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private EmotionPackageInfo emotionPackage;
        public EmotionPackageInfo EmotionPackage
        {
            get
            {
                return this.emotionPackage;
            }
            set
            {
                this.emotionPackage = value;
                this.NotifyPropertyChange(() => this.EmotionPackage);
            }
        }
        #endregion
    }
}
