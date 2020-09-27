using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Event;
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
using System.Windows.Data;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    [Component]
    public class QuickReplyAreaViewModel : PropertyChangedBase
    {
        #region  构造函数   
        public QuickReplyAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.QuickReplyGroups = new ObservableCollection<QuickReplyGroupInfo>();

            // TODO 测试数据
            new Action(() =>
            {
                this.QuickReplyGroups.Clear();
                this.QuickReplyGroups.Add(new QuickReplyGroupInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Level = QuickReplyGroupLevelEnum.System,
                    Name = "系统默认分组",
                    Business = BusinessEnum.Advisor,
                    QuickReplys = new ObservableCollection<QuickReplyInfo>()
                    {
                        new QuickReplyInfo()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name="问候",
                            Content= new TextMessageContent(){ Content="你好！"}.ObjectToJson(),
                            Type= MessageTypeConst.Text
                        }
                    }
                });
                this.QuickReplyGroups.Add(new QuickReplyGroupInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Level = QuickReplyGroupLevelEnum.User,
                    Name = "个人默认分组",
                    Business = BusinessEnum.PreSale,
                    QuickReplys = new ObservableCollection<QuickReplyInfo>()
                    {
                        new QuickReplyInfo()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name="问候",
                            Content= new MixedMessageContent() {
                                Items = new MessageContentInfo[] {
                                    new MessageContentInfo()
                                    {
                                        Type=MessageTypeConst.Text,
                                        Content= new TextMessageContent(){
                                            Content= "百度（纳斯达克：BIDU）是全球最大的中文搜索引擎，中国最大的以信息和知识为核心的互联网综合服务公司，全球领先的人工智能平台型公司。百度愿景是：成为最懂用户，并能帮助人们成长的全球顶级高科技公司。"
                                        }.ObjectToJson()
                                    },
                                    new MessageContentInfo()
                                    {
                                        Type=MessageTypeConst.Link,
                                        Content= new LinkMessageContent {
                                            Url = "https://www.baidu.com/",
                                            ThumbUrl = "https://dss0.bdstatic.com/70cFuHSh_Q1YnxGkpoWK1HF6hhy/it/u=3738723861,292586857&fm=26&gp=0.jpg",
                                            Title = "测试链接",
                                            Description = "百度（纳斯达克：BIDU）是全球最大的中文搜索引擎，中国最大的以信息和知识为核心的互联网综合服务公司，全球领先的人工智能平台型公司。百度愿景是：成为最懂用户，并能帮助人们成长的全球顶级高科技公司。"
                                        }.ObjectToJson()
                                    }
                                }
                            }.ObjectToJson(),
                            Type= MessageTypeConst.Mixed
                        }
                    }
                });
            }).ExecuteInUIThread();
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ApplicationContextViewModel applicationContextViewModel;
        public ApplicationContextViewModel ApplicationContextViewModel
        {
            get
            {
                return this.applicationContextViewModel;
            }
            set
            {
                this.applicationContextViewModel = value;
                this.NotifyPropertyChange(() => this.ApplicationContextViewModel);
            }
        }
        private ObservableCollection<QuickReplyGroupInfo> quickReplyGroups;
        public ObservableCollection<QuickReplyGroupInfo> QuickReplyGroups
        {
            get
            {
                return this.quickReplyGroups;
            }
            set
            {
                this.quickReplyGroups = value;
                this.NotifyPropertyChange(() => this.QuickReplyGroups);
            }
        }
        private QuickReplyGroupInfo selectedQuickReplyGroup;
        public QuickReplyGroupInfo SelectedQuickReplyGroup
        {
            get
            {
                return this.selectedQuickReplyGroup;
            }
            set
            {
                this.selectedQuickReplyGroup = value;
                this.NotifyPropertyChange(() => this.SelectedQuickReplyGroup);
            }
        }
        private bool isAddingQuickReplyGroup;
        public bool IsAddingQuickReplyGroup
        {
            get
            {
                return this.isAddingQuickReplyGroup;
            }
            set
            {
                this.isAddingQuickReplyGroup = value;
                this.NotifyPropertyChange(() => this.IsAddingQuickReplyGroup);
                if (!this.isAddingQuickReplyGroup) this.TemporaryAddQuickReplyGroup = null;
            }
        }
        private QuickReplyGroupInfo temporaryAddQuickReplyGroup;
        public QuickReplyGroupInfo TemporaryAddQuickReplyGroup
        {
            get
            {
                return this.temporaryAddQuickReplyGroup;
            }
            set
            {
                this.temporaryAddQuickReplyGroup = value;
                this.NotifyPropertyChange(() => this.TemporaryAddQuickReplyGroup);
            }
        }
        private bool isEditingQuickReplyGroup;
        public bool IsEditingQuickReplyGroup
        {
            get
            {
                return this.isEditingQuickReplyGroup;
            }
            set
            {
                this.isEditingQuickReplyGroup = value;
                this.NotifyPropertyChange(() => this.IsEditingQuickReplyGroup);
                if (!this.isEditingQuickReplyGroup) this.TemporaryEditQuickReplyGroup = null;
            }
        }
        private QuickReplyGroupInfo temporaryEditQuickReplyGroup;
        public QuickReplyGroupInfo TemporaryEditQuickReplyGroup
        {
            get
            {
                return this.temporaryEditQuickReplyGroup;
            }
            set
            {
                this.temporaryEditQuickReplyGroup = value;
                this.NotifyPropertyChange(() => this.TemporaryEditQuickReplyGroup);
            }
        }
        #endregion

        #region 方法

        #endregion

        #region 命令
        public ICommand SelectQuickReplyCommand
        {
            get
            {
                return new RelayCommand<QuickReplyInfo>((quickReply) =>
                {
                    this.eventAggregator.PublishAsync(new InputMessageContentEventArgs() { MessageContent = quickReply });
                });
            }
        }

        public ICommand AddQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand<BusinessEnum?>((business) =>
                {
                    this.IsAddingQuickReplyGroup = true;
                    this.TemporaryAddQuickReplyGroup = new QuickReplyGroupInfo()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = string.Empty,
                        Level = QuickReplyGroupLevelEnum.User,
                        Business = business,
                        QuickReplys = new ObservableCollection<QuickReplyInfo>()
                    };
                });
            }
        }

        public ICommand ConfirmAddQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    lock (this.QuickReplyGroups)
                    {
                        if (this.SelectedQuickReplyGroup != null)
                        {
                            this.QuickReplyGroups.Insert(this.QuickReplyGroups.IndexOf(this.SelectedQuickReplyGroup), this.TemporaryAddQuickReplyGroup);
                        }
                        else
                        {
                            this.QuickReplyGroups.Add(this.TemporaryAddQuickReplyGroup);
                        }
                    }
                    this.IsAddingQuickReplyGroup = false;
                }, () => !string.IsNullOrEmpty(this.TemporaryAddQuickReplyGroup?.Name));
            }
        }

        public ICommand EditQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand<QuickReplyGroupInfo>((quickReplyGroup) =>
                {
                    this.IsEditingQuickReplyGroup = true;
                    this.TemporaryEditQuickReplyGroup = quickReplyGroup.Clone();
                });
            }
        }

        public ICommand ConfirmEditQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    lock (this.QuickReplyGroups)
                    {
                        this.QuickReplyGroups.FirstOrDefault(u => u.Equals(this.TemporaryEditQuickReplyGroup)).Assign(this.TemporaryEditQuickReplyGroup);
                    }
                    this.IsEditingQuickReplyGroup = false;
                }, () => !string.IsNullOrEmpty(this.TemporaryEditQuickReplyGroup?.Name));
            }
        }

        public ICommand RemoveQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand<QuickReplyGroupInfo>((quickReplyGroup) =>
                {
                    lock (this.QuickReplyGroups)
                    {
                        this.QuickReplyGroups.Remove(quickReplyGroup);
                    }
                });
            }
        }


        public ICommand AddQuickReplyCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }
        }

        public ICommand ConfirmAddQuickReplyCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                });
            }
        }

        public ICommand EditQuickReplyCommand
        {
            get
            {
                return new RelayCommand<QuickReplyInfo>((quickReply) =>
                {

                });
            }
        }

        public ICommand RemoveQuickReplyCommand
        {
            get
            {
                return new RelayCommand<QuickReplyInfo>((quickReply) =>
                {

                });
            }
        }
        #endregion
    }
}
