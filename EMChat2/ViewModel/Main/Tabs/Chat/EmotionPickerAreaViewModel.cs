using EMChat2.Model.Entity;
using EMChat2.Service;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    [Component]
    public class EmotionPickerAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public EmotionPickerAreaViewModel(EmotionPackageService emotionPackageService)
        {
            this.emotionPackageService = emotionPackageService;

            ReloadEmotionPackage();
        }
        #endregion

        #region 属性
        private EmotionPackageService emotionPackageService;
        private ObservableCollection<EmotionPackageInfo> emotionPackages;
        public ObservableCollection<EmotionPackageInfo> EmotionPackages
        {
            get
            {
                return this.emotionPackages;
            }
            set
            {
                this.emotionPackages = value;
                this.NotifyPropertyChange(() => this.EmotionPackages);
            }
        }
        #endregion

        #region 内部方法
        private async void ReloadEmotionPackage()
        {
            this.emotionPackages = await this.emotionPackageService.LoadEmotionPackages();
        }
        #endregion
    }
}
