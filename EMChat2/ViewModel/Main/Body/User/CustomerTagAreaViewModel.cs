using DotLiquid.Util;
using EMChat2.Common;
using EMChat2.Event;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
using EMChat2.Service;
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

namespace EMChat2.ViewModel.Main.Body.User
{
    [Component]
    public class CustomerTagAreaViewModel : PropertyChangedBase, IEventHandle<LoginCallbackEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>
    {
        #region 构造函数
        public CustomerTagAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, UserService userService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.tagGroups = new ObservableCollection<TagGroupModel>();
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
        private ObservableCollection<TagGroupModel> tagGroups;
        public ObservableCollection<TagGroupModel> TagGroups
        {
            get
            {
                return this.tagGroups;
            }
            set
            {
                this.tagGroups = value;
                this.NotifyPropertyChange(() => this.TagGroups);
            }
        }
        private bool isAddingTagGroup;
        public bool IsAddingTagGroup
        {
            get
            {
                return this.isAddingTagGroup;
            }
            set
            {
                this.isAddingTagGroup = value;
                this.NotifyPropertyChange(() => this.IsAddingTagGroup);
                if (!isAddingTagGroup) this.TemporaryAddTagGroup = null;
            }
        }
        private TagGroupModel temporaryAddTagGroup;
        public TagGroupModel TemporaryAddTagGroup
        {
            get
            {
                return this.temporaryAddTagGroup;
            }
            set
            {
                this.temporaryAddTagGroup = value;
                this.NotifyPropertyChange(() => this.TemporaryAddTagGroup);
            }
        }
        private bool isEditingTagGroup;
        public bool IsEditingTagGroup
        {
            get
            {
                return this.isEditingTagGroup;
            }
            set
            {
                this.isEditingTagGroup = value;
                this.NotifyPropertyChange(() => this.IsEditingTagGroup);
                if (!this.isEditingTagGroup) this.TemporaryEditTagGroup = null;
            }
        }
        private TagGroupModel temporaryEditTagGroup;
        public TagGroupModel TemporaryEditTagGroup
        {
            get
            {
                return this.temporaryAddTagGroup;
            }
            set
            {
                this.temporaryAddTagGroup = value;
                this.NotifyPropertyChange(() => this.TemporaryEditTagGroup);
            }
        }
        private UserService userService;
        #endregion

        #region 方法
        private async void GetTagGroups(StaffModel staff)
        {
            IEnumerable<TagGroupModel> tagGroups = await userService.GetTagGroups(staff);
            if (tagGroups == null) return;
            new Action(() =>
            {
                lock (this.TagGroups)
                {
                    this.TagGroups.Clear();
                    foreach (TagGroupModel tagGroup in tagGroups)
                    {
                        this.TagGroups.Add(tagGroup);
                    }
                }
            }).ExecuteInUIThread();
        }

        private async void AddTagGroup(StaffModel staff, TagGroupModel tagGroup)
        {
            bool res = await userService.AddTagGroup(staff, tagGroup);
            if (!res) return;
            new Action(() =>
            {
                lock (this.TagGroups)
                {
                    this.TagGroups.Add(tagGroup);
                }
            }).ExecuteInUIThread();
        }

        private async void EditTagGroup(StaffModel staff, TagGroupModel tagGroup)
        {
            bool res = await userService.ModifyTagGroup(staff, tagGroup);
            if (!res) return;
            new Action(() =>
            {
                lock (this.TagGroups)
                {
                    this.TagGroups.FirstOrDefault(u => u.Equals(tagGroup)).Assign(tagGroup);
                }
            }).ExecuteInUIThread();
        }

        private async void RemoveTagGroup(StaffModel staff, TagGroupModel tagGroup)
        {
            bool res = await userService.RemoveTagGroup(staff, tagGroup);
            if (!res) return;
            new Action(() =>
            {
                lock (this.TagGroups)
                {
                    this.TagGroups.Remove(tagGroup);
                }
            }).ExecuteInUIThread();
        }
        #endregion

        #region 命令
        #region 标签组相关
        public ICommand AddTagGroupCommand
        {
            get
            {
                return new RelayCommand<string>((businessId) =>
                {
                    this.IsAddingTagGroup = false;
                    this.TemporaryAddTagGroup = new TagGroupModel()
                    {
                        Id = null,
                        Name = null,
                        Level = TagGroupLevelEnum.User,
                        BusinessId = businessId,
                        Tags = new ObservableCollection<TagModel>()
                    };
                    this.IsAddingTagGroup = true;
                });
            }
        }

        public ICommand ConfirmAddTagGroupCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.AddTagGroup(ApplicationContextViewModel.CurrentStaff, this.TemporaryAddTagGroup);
                    this.IsAddingTagGroup = false;
                }, () =>
                {
                    return this.TemporaryAddTagGroup != null && !string.IsNullOrEmpty(this.TemporaryAddTagGroup.Name);
                });
            }
        }

        public ICommand CancelAddTagGroupCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.IsAddingTagGroup = false;
                });
            }
        }

        public ICommand EditTagGroupCommand
        {
            get
            {
                return new RelayCommand<TagGroupModel>((tagGroup) =>
                {
                    this.IsEditingTagGroup = false;
                    this.temporaryEditTagGroup = tagGroup.CloneObject();
                    this.IsEditingTagGroup = true;
                }, (tagGroup) =>
                {
                    return tagGroup != null && tagGroup.Level == TagGroupLevelEnum.User;
                });
            }
        }

        public ICommand ConfirmEditTagGroupCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    lock (this.TagGroups)
                    {
                        this.EditTagGroup(ApplicationContextViewModel.CurrentStaff, this.TemporaryEditTagGroup);
                    }
                    this.IsAddingTagGroup = false;
                });
            }
        }

        public ICommand CancelEditTagGroupCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.IsEditingTagGroup = false;
                });
            }
        }

        public ICommand RemoveTagGroupCommand
        {
            get
            {
                return new RelayCommand<TagGroupModel>((tagGroup) =>
                {
                    this.RemoveTagGroup(ApplicationContextViewModel.CurrentStaff, tagGroup);
                }, (tagGroup) =>
                {
                    return tagGroup != null && tagGroup.Level == TagGroupLevelEnum.User && tagGroup.Tags.Count == 0;
                });
            }
        }
        #endregion

        #region 标签相关
        public ICommand RemoveTagCommand
        {
            get
            {
                return new RelayCommand<TagModel>((tag) =>
                {
                    lock (this.TagGroups)
                    {
                        foreach (TagGroupModel tagGroup in this.TagGroups)
                        {
                            lock (tagGroup.Tags)
                            {
                                tagGroup.Tags.Remove(tag);
                            }
                        }
                    }
                }, (tag) =>
                {
                    lock (this.TagGroups)
                    {
                        return tag != null && !this.TagGroups.Any(u => u.Tags.Contains(tag) && u.Level != TagGroupLevelEnum.User);
                    }
                });
            }
        }
        #endregion
        #endregion

        #region 事件处理
        public void Handle(LoginCallbackEventArgs arg)
        {
            if (!arg.IsSuccess) return;
            GetTagGroups(arg.Staff);
        }

        public void Handle(LogoutCallbackEventArgs arg)
        {
            new Action(() =>
            {
                this.TagGroups.Clear();
            }).ExecuteInUIThread();
        }

        public void Handle(ExitCallbackEventArgs arg)
        {
            new Action(() =>
            {
                this.TagGroups.Clear();
            }).ExecuteInUIThread();
        }
        #endregion

        #region 方法
        public IEnumerable<TagModel> GetSelectedTags(string businessId)
        {
            lock (this.tagGroups)
            {
                return this.tagGroups.Where(u => u.BusinessId == businessId).SelectMany(u => u.Tags).Where(u => u.IsSelected);
            }
        }
        #endregion
    }
}
