﻿using EMChat2.Common;
using EMChat2.Model.Entity;
using EMChat2.Model.Event;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    Level = QuickReplyGroupLevel.System,
                    Name = "系统默认分组",
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
                    Level = QuickReplyGroupLevel.User,
                    Name = "个人默认分组",
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
                ChangeSelctedQuickReplyGroup();
                this.quickReplyGroups.CollectionChanged += (s, e) =>
                {
                    ChangeSelctedQuickReplyGroup();
                };
                this.NotifyPropertyChange(() => this.QuickReplyGroups);
            }
        }
        #endregion

        #region 方法
        private void ChangeSelctedQuickReplyGroup()
        {
            lock (this.QuickReplyGroups)
            {
                if (this.SelectedQuickReplyGroup == null && this.QuickReplyGroups.Count > 0)
                {
                    this.SelectedQuickReplyGroup = this.QuickReplyGroups.FirstOrDefault();
                }
            }
        }
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
                return new RelayCommand(() =>
                {
                    lock (this.QuickReplyGroups)
                    {
                        this.QuickReplyGroups.Add(new QuickReplyGroupInfo()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Level = QuickReplyGroupLevel.User,
                            Name = "新建分组",
                            QuickReplys = new ObservableCollection<QuickReplyInfo>()
                        });
                    }
                });
            }
        }

        public ICommand EditQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand<QuickReplyGroupInfo>((quickReplyGroup) =>
                {
                    lock (this.QuickReplyGroups)
                    {
                        quickReplyGroup.Name = "编辑分组";
                    }
                });
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
                return new RelayCommand<QuickReplyGroupInfo>((quickReplyGroup) =>
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
                    if (this.SelectedQuickReplyGroup != null) this.SelectedQuickReplyGroup.QuickReplys.Remove(quickReply);
                });
            }
        }
        #endregion
    }
}
