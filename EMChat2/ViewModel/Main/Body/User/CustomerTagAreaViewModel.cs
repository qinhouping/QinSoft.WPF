using DotLiquid.Util;
using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Body.User
{
    public class CustomerTagAreaViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数
        public CustomerTagAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, BusinessEnum business, IEnumerable<TagInfo> defaultTags = null)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.business = business;
            this.defaultTags = defaultTags;
            this.tagGroups = new ObservableCollection<TagGroupInfo>();

            this.LoadTagGroups();
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private IEnumerable<TagInfo> defaultTags;
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
        private BusinessEnum business;
        public BusinessEnum Business
        {
            get
            {
                return this.business;
            }
            set
            {
                this.business = value;
                this.NotifyPropertyChange(() => this.Business);
            }
        }
        private ObservableCollection<TagGroupInfo> tagGroups;
        public ObservableCollection<TagGroupInfo> TagGroups
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
        public IEnumerable<TagInfo> SelectedTags
        {
            get
            {
                lock (this.tagGroups)
                {
                    return this.tagGroups.SelectMany(u => u.Tags).Where(u => u.IsSelected);
                }
            }
        }
        #endregion

        #region 方法
        private async void LoadTagGroups()
        {
            await new Action(() =>
            {
                List<TagGroupInfo> data = new List<TagGroupInfo>();
                data.Add(new TagGroupInfo()
                {
                    Id = "1",
                    Name = "客户类型",
                    Level = TagGroupLevelEnum.System,
                    Tags = new ObservableCollection<TagInfo>(){
                            new TagInfo(){ Id="0", Name="决策版客户XXXXXX" },
                            new TagInfo(){ Id="1", Name="领航版" },
                            new TagInfo(){ Id="2", Name="大师版" },
                            new TagInfo(){ Id="3", Name="先锋版" },
                            new TagInfo(){ Id="4", Name="经典版" }
                        }
                });
                data.Add(new TagGroupInfo()
                {
                    Id = "2",
                    Name = "成交类型",
                    Level = TagGroupLevelEnum.System,
                    Tags = new ObservableCollection<TagInfo>(){
                            new TagInfo(){ Id="11", Name="首次" },
                            new TagInfo(){ Id="12", Name="升级" },
                            new TagInfo(){ Id="13", Name="续费" }
                        }
                });
                data.Add(new TagGroupInfo()
                {
                    Id = "3",
                    Name = "是否到期",
                    Level = TagGroupLevelEnum.System,
                    Tags = new ObservableCollection<TagInfo>(){
                            new TagInfo(){ Id="21", Name="已到期" },
                            new TagInfo(){ Id="22", Name="未到期" }
                        }
                });
                data.Add(new TagGroupInfo()
                {
                    Id = "3",
                    Name = "是否有意向",
                    Level = TagGroupLevelEnum.User,
                    Tags = new ObservableCollection<TagInfo>(){
                            new TagInfo(){ Id="31", Name="有意向" },
                            new TagInfo(){ Id="32", Name="无意向" }
                        }
                });

                new Action(() =>
                {
                    lock (this.TagGroups)
                    {
                        this.TagGroups.Clear();
                        data.ForEach(u => this.TagGroups.Add(u));
                        this.TagGroups.SelectMany(u => u.Tags).ToList().ForEach(u => u.Assign(this.defaultTags?.FirstOrDefault(i => i.Equals(u))));
                    }
                }).ExecuteInUIThread();

            }).ExecuteInTask();
        }
        public void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
        }
        #endregion
    }
}
