﻿using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
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
using EMChat2.Model.Enum;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    [Component]
    public class QuickReplyAreaViewModel : PropertyChangedBase, IEventHandle<LoginCallbackEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>
    {
        #region  构造函数   
        public QuickReplyAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.QuickReplyGroups = new ObservableCollection<QuickReplyGroupModel>();
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
        private ObservableCollection<QuickReplyGroupModel> quickReplyGroups;
        public ObservableCollection<QuickReplyGroupModel> QuickReplyGroups
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
        private QuickReplyGroupModel selectedQuickReplyGroup;
        public QuickReplyGroupModel SelectedQuickReplyGroup
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
        private QuickReplyGroupModel temporaryAddQuickReplyGroup;
        public QuickReplyGroupModel TemporaryAddQuickReplyGroup
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
        private QuickReplyGroupModel temporaryEditQuickReplyGroup;
        public QuickReplyGroupModel TemporaryEditQuickReplyGroup
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
        private QuickReplyGroupModel temporaryQuickReplyGroup;
        public QuickReplyGroupModel TemporaryQuickReplyGroup
        {
            get
            {
                return this.temporaryQuickReplyGroup;
            }
            set
            {
                this.temporaryQuickReplyGroup = value;
                this.NotifyPropertyChange(() => this.TemporaryQuickReplyGroup);
            }
        }
        private bool isAddingQuickReply;
        public bool IsAddingQuickReply
        {
            get
            {
                return this.isAddingQuickReply;
            }
            set
            {
                this.isAddingQuickReply = value;
                this.NotifyPropertyChange(() => this.IsAddingQuickReply);
                if (!this.isAddingQuickReply)
                {
                    this.TemporaryAddQuickReply = null;
                    this.TemporaryQuickReplyGroup = null;
                }
            }
        }
        private QuickReplyModel temporaryAddQuickReply;
        public QuickReplyModel TemporaryAddQuickReply
        {
            get
            {
                return this.temporaryAddQuickReply;
            }
            set
            {
                this.temporaryAddQuickReply = value;
                this.NotifyPropertyChange(() => this.TemporaryAddQuickReply);
            }
        }
        private QuickReplyGroupModel temporaryQuickReplyGroup2;
        public QuickReplyGroupModel TemporaryQuickReplyGroup2
        {
            get
            {
                return this.temporaryQuickReplyGroup2;
            }
            set
            {
                this.temporaryQuickReplyGroup2 = value;
                this.NotifyPropertyChange(() => this.TemporaryQuickReplyGroup2);
            }
        }
        private bool isEditingQuickReply;
        public bool IsEditingQuickReply
        {
            get
            {
                return this.isEditingQuickReply;
            }
            set
            {
                this.isEditingQuickReply = value;
                this.NotifyPropertyChange(() => this.IsEditingQuickReply);
                if (!this.isEditingQuickReply)
                {
                    this.TemporaryEditQuickReply = null;
                    this.TemporaryQuickReplyGroup2 = null;
                }
            }
        }
        private QuickReplyModel temporaryEditQuickReply;
        public QuickReplyModel TemporaryEditQuickReply
        {
            get
            {
                return this.temporaryEditQuickReply;
            }
            set
            {
                this.temporaryEditQuickReply = value;
                this.NotifyPropertyChange(() => this.TemporaryEditQuickReply);
            }
        }
        #endregion

        #region 方法

        #endregion

        #region 命令
        #region 快捷回复组
        public ICommand AddQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand<BusinessEnum>((business) =>
                {
                    this.IsAddingQuickReplyGroup = false;
                    this.TemporaryAddQuickReplyGroup = new QuickReplyGroupModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = null,
                        Level = QuickReplyGroupLevelEnum.User,
                        Business = business,
                        QuickReplys = new ObservableCollection<QuickReplyModel>()
                    };
                    this.IsAddingQuickReplyGroup = true;
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
                        this.QuickReplyGroups.Add(this.TemporaryAddQuickReplyGroup);
                    }
                    this.IsAddingQuickReplyGroup = false;
                }, () =>
                {
                    return this.TemporaryAddQuickReplyGroup != null && !string.IsNullOrEmpty(this.TemporaryAddQuickReplyGroup.Name);
                });
            }
        }

        public ICommand CancelAddQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.IsAddingQuickReplyGroup = false;
                });
            }
        }

        public ICommand EditQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand<QuickReplyGroupModel>((quickReplyGroup) =>
                {
                    this.IsEditingQuickReplyGroup = false;
                    this.TemporaryEditQuickReplyGroup = quickReplyGroup.CloneObject();
                    this.IsEditingQuickReplyGroup = true;
                }, (quickReplyGroup) =>
                {
                    return quickReplyGroup != null && quickReplyGroup.Level == QuickReplyGroupLevelEnum.User;
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
                }, () =>
                {
                    return this.TemporaryEditQuickReplyGroup != null && !string.IsNullOrEmpty(this.TemporaryEditQuickReplyGroup.Name);
                });
            }
        }

        public ICommand CancelEditQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.IsEditingQuickReplyGroup = false;
                });
            }
        }

        public ICommand RemoveQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand<QuickReplyGroupModel>((quickReplyGroup) =>
                {
                    lock (this.QuickReplyGroups)
                    {
                        this.QuickReplyGroups.Remove(quickReplyGroup);
                    }
                }, (quickReplyGroup) =>
                {
                    return quickReplyGroup != null && quickReplyGroup.Level == QuickReplyGroupLevelEnum.User;
                });
            }
        }
        #endregion

        #region 快捷回复
        public ICommand AddQuickReplyCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.IsAddingQuickReply = false;
                    this.TemporaryQuickReplyGroup = this.SelectedQuickReplyGroup;
                    this.TemporaryAddQuickReply = new QuickReplyModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = null,
                        Content = null
                    };
                    this.IsAddingQuickReply = true;
                }, () =>
                {
                    return this.SelectedQuickReplyGroup != null && this.SelectedQuickReplyGroup.Level == QuickReplyGroupLevelEnum.User;
                });
            }
        }

        public ICommand ConfirmAddQuickReplyCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    lock (this.TemporaryQuickReplyGroup.QuickReplys)
                    {
                        this.TemporaryQuickReplyGroup.QuickReplys.Add(this.TemporaryAddQuickReply);
                    }
                    this.IsAddingQuickReply = false;
                }, () =>
                {
                    return this.TemporaryQuickReplyGroup != null && this.TemporaryAddQuickReply != null && !string.IsNullOrEmpty(this.TemporaryAddQuickReply.Name) && this.TemporaryAddQuickReply.Content != null;
                });
            }
        }

        public ICommand CancelAddQuickReplyCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.IsAddingQuickReply = false;
                });
            }
        }

        public ICommand EditQuickReplyCommand
        {
            get
            {
                return new RelayCommand<QuickReplyModel>((quickReply) =>
                {
                    this.IsEditingQuickReply = false;
                    this.TemporaryQuickReplyGroup2 = this.SelectedQuickReplyGroup;
                    this.TemporaryEditQuickReply = quickReply.CloneObject();
                    this.IsEditingQuickReply = true;
                }, (quickReply) =>
                {
                    return this.SelectedQuickReplyGroup != null && this.SelectedQuickReplyGroup.Level == QuickReplyGroupLevelEnum.User && quickReply != null;
                });
            }
        }

        public ICommand ConfirmEditQuickReplyCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    lock (this.TemporaryQuickReplyGroup2.QuickReplys)
                    {
                        this.TemporaryQuickReplyGroup2.QuickReplys.FirstOrDefault(u => u.Equals(this.TemporaryEditQuickReply)).Assign(this.TemporaryEditQuickReply);
                    }
                    this.IsEditingQuickReply = false;
                }, () =>
                {
                    return this.TemporaryQuickReplyGroup2 != null && this.TemporaryEditQuickReply != null && !string.IsNullOrEmpty(this.TemporaryEditQuickReply.Name) && this.TemporaryEditQuickReply.Content != null;
                });
            }
        }

        public ICommand CancelEditQuickReplyCommand
        {
            get
            {
                return new RelayCommand<QuickReplyModel>((quickReply) =>
                {
                    this.IsEditingQuickReply = false;
                });
            }
        }

        public ICommand RemoveQuickReplyCommand
        {
            get
            {
                return new RelayCommand<QuickReplyModel>((quickReply) =>
                {
                    lock (this.SelectedQuickReplyGroup.QuickReplys)
                    {
                        this.SelectedQuickReplyGroup.QuickReplys.Remove(quickReply);
                    }
                }, (quickReply) =>
                {
                    return this.SelectedQuickReplyGroup != null && this.SelectedQuickReplyGroup.Level == QuickReplyGroupLevelEnum.User && quickReply != null;
                });
            }
        }

        public ICommand SelectQuickReplyCommand
        {
            get
            {
                return new RelayCommand<QuickReplyModel>((quickReply) =>
                {
                    this.eventAggregator.PublishAsync(new TemporaryInputMessagContentChangedEventArgs() { MessageContent = quickReply.Content });
                }, (quickReply) =>
                {
                    BusinessSettingModel businessSetting = ApplicationContextViewModel.Setting?.BusinessSettings.FirstOrDefault(u => u.Business == this.SelectedQuickReplyGroup.Business);
                    if (businessSetting != null)
                    {
                        return businessSetting.AllowSelectQuickReply;
                    }
                    return false;
                });
            }
        }
        #endregion
        #endregion

        #region 事件处理
        public void Handle(LoginCallbackEventArgs arg)
        {
            if (!arg.IsSuccess) return;
            // TODO 测试数据
            new Action(() =>
            {
                this.QuickReplyGroups.Clear();
                this.QuickReplyGroups.Add(new QuickReplyGroupModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Level = QuickReplyGroupLevelEnum.System,
                    Name = "系统默认分组",
                    Business = BusinessEnum.Advisor,
                    QuickReplys = new ObservableCollection<QuickReplyModel>()
                    {
                        new QuickReplyModel()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name="问候",
                            Content=MessageTools.CreateTextMessageContent("测试快捷回复")
                        }
                    }
                });
                this.QuickReplyGroups.Add(new QuickReplyGroupModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Level = QuickReplyGroupLevelEnum.User,
                    Name = "个人默认分组",
                    Business = BusinessEnum.Advisor,
                    QuickReplys = new ObservableCollection<QuickReplyModel>()
                    {
                        new QuickReplyModel()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name="问候",
                            Content=MessageTools.CreateTextMessageContent("百度（纳斯达克：BIDU）是全球最大的中文搜索引擎，中国最大的以信息和知识为核心的互联网综合服务公司，全球领先的人工智能平台型公司。百度愿景是：成为最懂用户，并能帮助人们成长的全球顶级高科技公司。")
                        }
                    }
                });
            }).ExecuteInUIThread();
        }

        public void Handle(LogoutCallbackEventArgs arg)
        {
            new Action(() =>
            {
                this.QuickReplyGroups.Clear();
            }).ExecuteInUIThread();
        }

        public void Handle(ExitCallbackEventArgs arg)
        {
            new Action(() =>
            {
                this.QuickReplyGroups.Clear();
            }).ExecuteInUIThread();
        }
        #endregion
    }
}
