using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
using EMChat2.View.Main.Body.Chat;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using EMChat2.Model.Enum;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    [Component]
    public class EmotionPickerAreaViewModel : PropertyChangedBase, IEventHandle<LoginCallbackEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>
    {
        #region 构造函数
        public EmotionPickerAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.EmotionPackages = new ObservableCollection<EmotionPackageModel>();
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ObservableCollection<EmotionPackageModel> emotionPackages;
        public ObservableCollection<EmotionPackageModel> EmotionPackages
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
        private EmotionPackageModel selectedEmotionPackage;
        public EmotionPackageModel SelectedEmotionPackage
        {
            get
            {
                return this.selectedEmotionPackage;
            }
            set
            {
                this.selectedEmotionPackage = value;
                this.NotifyPropertyChange(() => this.SelectedEmotionPackage);
            }
        }
        #endregion

        #region 方法

        #endregion

        #region 命令
        public ICommand SelectEmotionCommand
        {
            get
            {
                return new RelayCommand<EmotionModel>((emotion) =>
                {
                    this.eventAggregator.PublishAsync(new TemporaryInputMessagContentChangedEventArgs() { MessageContent = MessageTools.CreateEmotionMessageContent(emotion) });
                }, (emotion) =>
                {
                    return emotion != null;
                });
            }
        }
        #endregion

        #region 事件处理


        public void Handle(LoginCallbackEventArgs arg)
        {
            if (!arg.IsSuccess) return;
            //TODO 测试数据
            new Action(() =>
            {
                this.EmotionPackages.Clear();
                this.EmotionPackages.Add(new EmotionPackageModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Level = EmotionPackageLevelEnum.System,
                    Name = "emoji",
                    ThumbUrl = "https://static.easyicon.net/preview/106/1069782.gif",
                    Emotions = new ObservableCollection<EmotionModel>()
                    {
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069782.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069783.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069784.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069785.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069786.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069787.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069788.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069789.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069790.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069791.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069792.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069793.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069794.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069795.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069795.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069797.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069798.gif"
                        },
                        new EmotionModel()
                        {
                            Id=Guid.NewGuid().ToString(),
                            Name="one",
                            Url="https://static.easyicon.net/preview/106/1069799.gif"
                        }
                    }
                });
            }).ExecuteInUIThread();
        }

        public void Handle(LogoutCallbackEventArgs arg)
        {
            new Action(() =>
            {
                this.EmotionPackages.Clear();
            }).ExecuteInUIThread();
        }

        public void Handle(ExitCallbackEventArgs arg)
        {
            new Action(() =>
            {
                this.EmotionPackages.Clear();
            }).ExecuteInUIThread();
        }
        #endregion
    }
}
