using EMChat2.Model.Entity;
using QinSoft.Ioc.Attribute;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service
{
    [Component]
    public class EmotionPackageService
    {
        public async Task<ObservableCollection<EmotionPackageInfo>> LoadEmotionPackages()
        {
            return await Task.Factory.StartNew(() =>
            {
                EmotionPackageInfo emotionPackageInfo = new EmotionPackageInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Level = EmotionPackageLevel.System,
                    Name = "测试表情包",
                    ThumbUrl = "https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=2462792824,3234214141&fm=26&gp=0.jpg",
                    Emotions = new ObservableCollection<EmotionInfo>() {
                    new EmotionInfo(){
                        Id = Guid.NewGuid().ToString(),
                        Name="测试表情",
                        Url="https://ss1.bdstatic.com/70cFvXSh_Q1YnxGkpoWK1HF6hhy/it/u=2462792824,3234214141&fm=26&gp=0.jpg"
                    }
                }
                };
                return new ObservableCollection<EmotionPackageInfo>() { emotionPackageInfo };
            });
        }
    }
}
