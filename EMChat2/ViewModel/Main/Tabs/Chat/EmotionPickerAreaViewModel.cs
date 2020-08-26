using EMChat2.Model.Entity;
using EMChat2.View.Main.Tabs.Chat;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    [Component]
    public class EmotionPickerAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public EmotionPickerAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.emotionPickerTabItems = new ObservableCollection<TabItem>();

            //TODO 测试数据
            this.emotionPickerTabItems.Add(
                new EmotionPickerTabItemAreaView()
                {
                    DataContext = new EmotionPickerTabItemAreaViewModel(this.windowManager, this.eventAggregator, new EmotionPackageInfo()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Level = EmotionPackageLevel.System,
                        Name = "emoji",
                        ThumbUrl = "https://static.easyicon.net/preview/106/1069782.gif",
                        Emotions = new ObservableCollection<EmotionInfo>()
                    {
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069782.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069783.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069784.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069785.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069786.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069787.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069788.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069789.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069790.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069791.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069792.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069793.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069794.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069795.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069795.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069797.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069798.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069799.gif"
                        }
                    }
                    })
                });
            this.emotionPickerTabItems.Add(
                new EmotionPickerTabItemAreaView()
                {
                    DataContext = new EmotionPickerTabItemAreaViewModel(this.windowManager, this.eventAggregator, new EmotionPackageInfo()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Level = EmotionPackageLevel.System,
                        Name = "emoji2",
                        ThumbUrl = "https://static.easyicon.net/preview/106/1064760.gif",
                        Emotions = new ObservableCollection<EmotionInfo>()
                    {
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1064760.gif"
                        },
                        new EmotionInfo()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1064761.gif"
                        }
                    }
                    })
                });
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ObservableCollection<TabItem> emotionPickerTabItems;
        public ObservableCollection<TabItem> EmotionPickerTabItems
        {
            get
            {
                return this.emotionPickerTabItems;
            }
            set
            {
                this.emotionPickerTabItems = value;
                this.NotifyPropertyChange(() => this.EmotionPickerTabItems);
            }
        }
        #endregion
    }
}
