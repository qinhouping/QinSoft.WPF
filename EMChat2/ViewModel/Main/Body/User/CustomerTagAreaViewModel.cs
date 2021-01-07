using DotLiquid.Util;
using EMChat2.Common;
using EMChat2.Event;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
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
        public CustomerTagAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.tagGroups = new ObservableCollection<TagGroupModel>();
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
        #endregion

        #region 命令
        #region 标签组相关
        public ICommand AddTagGroupCommand
        {
            get
            {
                return new RelayCommand<BusinessEnum>((business) =>
                {
                    this.IsAddingTagGroup = false;
                    this.TemporaryAddTagGroup = new TagGroupModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = null,
                        Level = TagGroupLevelEnum.User,
                        Business = business,
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
                    lock (this.TagGroups)
                    {
                        this.TagGroups.Add(this.TemporaryAddTagGroup);
                    }
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
                        this.TagGroups.FirstOrDefault(u => u.Equals(this.TemporaryEditTagGroup)).Assign(this.TemporaryEditTagGroup);
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
                    lock (this.TagGroups)
                    {
                        this.TagGroups.Remove(tagGroup);
                    }
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
        public void Handle(LoginCallbackEventArgs Message)
        {
            //TODO 测试数据
            new Action(() =>
            {
                this.TagGroups.Clear();
                this.TagGroups.Add(new TagGroupModel()
                {
                    Id = "1",
                    Name = "客户类型",
                    Level = TagGroupLevelEnum.System,
                    Business = BusinessEnum.Advisor,
                    Tags = new ObservableCollection<TagModel>(){
                            new TagModel(){ Id="0", Name="决策版" },
                            new TagModel(){ Id="1", Name="领航版" },
                            new TagModel(){ Id="2", Name="大师版" },
                            new TagModel(){ Id="3", Name="先锋版" },
                            new TagModel(){ Id="4", Name="经典版" }}
                });
                this.TagGroups.Add(new TagGroupModel()
                {
                    Id = "2",
                    Name = "成交类型",
                    Level = TagGroupLevelEnum.System,
                    Business = BusinessEnum.Advisor,
                    Tags = new ObservableCollection<TagModel>(){
                            new TagModel(){ Id="11", Name="首次" },
                            new TagModel(){ Id="12", Name="升级" },
                            new TagModel(){ Id="13", Name="续费" }}
                });
                this.TagGroups.Add(new TagGroupModel()
                {
                    Id = "3",
                    Name = "是否到期",
                    Level = TagGroupLevelEnum.System,
                    Business = BusinessEnum.PreSale,
                    Tags = new ObservableCollection<TagModel>(){
                            new TagModel(){ Id="21", Name="已到期" },
                            new TagModel(){ Id="22", Name="未到期" }}
                });
                this.TagGroups.Add(new TagGroupModel()
                {
                    Id = "4",
                    Name = "是否有意向",
                    Level = TagGroupLevelEnum.User,
                    Business = BusinessEnum.PreSale,
                    Tags = new ObservableCollection<TagModel>(){
                            new TagModel(){ Id="31", Name="有意向" },
                            new TagModel(){ Id="32", Name="无意向" }}
                });

            }).ExecuteInUIThread();
        }

        public void Handle(LogoutCallbackEventArgs Message)
        {
            new Action(() =>
            {
                this.TagGroups.Clear();
            }).ExecuteInUIThread();
        }

        public void Handle(ExitCallbackEventArgs Message)
        {
            new Action(() =>
            {
                this.TagGroups.Clear();
            }).ExecuteInUIThread();
        }
        #endregion

        #region 方法
        public IEnumerable<TagModel> GetSelectedTags(BusinessEnum business)
        {
            lock (this.tagGroups)
            {
                return this.tagGroups.Where(u => u.Business == business).SelectMany(u => u.Tags).Where(u => u.IsSelected);
            }
        }
        #endregion
    }
}
