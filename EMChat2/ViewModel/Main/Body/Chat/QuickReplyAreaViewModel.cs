using EMChat2.Common;
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
using EMChat2.Service;

namespace EMChat2.ViewModel.Main.Tabs.Chat
{
    [Component]
    public class QuickReplyAreaViewModel : PropertyChangedBase, IEventHandle<LoginCallbackEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>
    {
        #region  构造函数   
        public QuickReplyAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, UserService userService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.QuickReplyGroups = new ObservableCollection<QuickReplyGroupModel>();
            this.userService = userService;
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
        private UserService userService;
        #endregion

        #region 方法
        private async void GetQuickReplyGroups(StaffModel staff)
        {
            IEnumerable<QuickReplyGroupModel> quickReplyGroups = await userService.GetQuickReplyGroups(staff);
            if (quickReplyGroups == null) return;
            new Action(() =>
            {
                lock (this.QuickReplyGroups)
                {
                    this.QuickReplyGroups.Clear();
                    foreach (QuickReplyGroupModel quickReplyGroup in quickReplyGroups)
                    {
                        this.QuickReplyGroups.Add(quickReplyGroup);
                    }
                }
            }).ExecuteInUIThread();
        }

        private async void AddQuickReplyGroup(StaffModel staff, QuickReplyGroupModel quickReplyGroup)
        {
            bool res = await userService.AddQuickReplyGroup(staff, quickReplyGroup);
            if (!res) return;
            new Action(() =>
            {
                lock (this.QuickReplyGroups)
                {
                    this.QuickReplyGroups.Add(quickReplyGroup);
                }
            }).ExecuteInUIThread();
        }

        private async void EditQuickReplyGroup(StaffModel staff, QuickReplyGroupModel quickReplyGroup)
        {
            bool res = await userService.ModifyQuickReplyGroup(staff, quickReplyGroup);
            if (!res) return;
            new Action(() =>
            {
                lock (this.QuickReplyGroups)
                {
                    this.QuickReplyGroups.FirstOrDefault(u => u.Equals(quickReplyGroup)).Assign(quickReplyGroup);
                }
            }).ExecuteInUIThread();
        }

        private async void RemoveQuickReplyGroup(StaffModel staff, QuickReplyGroupModel quickReplyGroup)
        {
            bool res = await userService.RemoveQuickReplyGroup(staff, quickReplyGroup);
            if (!res) return;
            new Action(() =>
            {
                lock (this.QuickReplyGroups)
                {
                    this.QuickReplyGroups.Remove(quickReplyGroup);
                }
            }).ExecuteInUIThread();
        }

        private async void AddQuickReply(QuickReplyGroupModel quickReplyGroup, QuickReplyModel quickReplyModel)
        {
            bool res = await userService.AddQuickReply(quickReplyGroup, quickReplyModel);
            if (!res) return;
            new Action(() =>
            {
                lock (quickReplyGroup.QuickReplies)
                {
                    quickReplyGroup.QuickReplies.Add(quickReplyModel);
                }
            }).ExecuteInUIThread();
        }

        private async void EditQuickReply(QuickReplyGroupModel quickReplyGroup, QuickReplyModel quickReplyModel)
        {
            bool res = await userService.ModifyQuickReply(quickReplyGroup, quickReplyModel);
            if (!res) return;
            new Action(() =>
            {
                lock (quickReplyGroup.QuickReplies)
                {
                    quickReplyGroup.QuickReplies.FirstOrDefault(u => u.Equals(quickReplyModel)).Assign(quickReplyModel);
                }
            }).ExecuteInUIThread();
        }
        private async void RemoveQuickReply(QuickReplyGroupModel quickReplyGroup, QuickReplyModel quickReplyModel)
        {
            bool res = await userService.RemoveQuickReply(quickReplyGroup, quickReplyModel);
            if (!res) return;
            new Action(() =>
            {
                lock (quickReplyGroup.QuickReplies)
                {
                    quickReplyGroup.QuickReplies.Remove(quickReplyModel);
                }
            }).ExecuteInUIThread();
        }
        #endregion

        #region 命令
        #region 快捷回复组
        public ICommand AddQuickReplyGroupCommand
        {
            get
            {
                return new RelayCommand<string>((businessId) =>
                {
                    this.IsAddingQuickReplyGroup = false;
                    this.TemporaryAddQuickReplyGroup = new QuickReplyGroupModel()
                    {
                        Id = null,
                        Name = null,
                        Level = QuickReplyGroupLevelEnum.User,
                        BusinessId = businessId,
                        QuickReplies = new ObservableCollection<QuickReplyModel>()
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
                    this.AddQuickReplyGroup(ApplicationContextViewModel.CurrentStaff, this.TemporaryAddQuickReplyGroup);
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
                    this.EditQuickReplyGroup(applicationContextViewModel.CurrentStaff, this.TemporaryEditQuickReplyGroup);
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
                    this.RemoveQuickReplyGroup(ApplicationContextViewModel.CurrentStaff, quickReplyGroup);
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
                        Id = null,
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
                    this.AddQuickReply(this.TemporaryQuickReplyGroup, this.TemporaryAddQuickReply);
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
                    this.EditQuickReply(this.TemporaryQuickReplyGroup2, this.TemporaryEditQuickReply);
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
                    this.RemoveQuickReply(this.SelectedQuickReplyGroup, quickReply);
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
                    if (SelectedQuickReplyGroup == null) return false;
                    BusinessSettingModel businessSetting = ApplicationContextViewModel.Setting?.BusinessSettings.FirstOrDefault(u => u.BusinessId == this.SelectedQuickReplyGroup.BusinessId);
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
            GetQuickReplyGroups(arg.Staff);
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
