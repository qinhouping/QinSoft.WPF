using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EMChat2.Service;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    public class StaffDetailAreaViewModel : PropertyChangedBase, IDisposable
    {
        #region 构造函数
        public StaffDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, UserService userService, bool isInform = true)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.userService = userService;
            this.isInform = isInform;
        }
        #endregion

        #region 属性
        private bool isInform;
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ApplicationContextViewModel applicationContextViewModel;
        private UserService userService;
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
        private StaffModel staff;
        public StaffModel Staff
        {
            get
            {
                return this.staff;
            }
            set
            {
                this.staff = value;
                this.NotifyPropertyChange(() => this.Staff);
                this.IsEditingStaff = false;
            }
        }
        private bool isEditingStaff;
        public bool IsEditingStaff
        {
            get
            {
                return this.isEditingStaff;
            }
            set
            {
                this.isEditingStaff = value;
                this.NotifyPropertyChange(() => this.IsEditingStaff);
                if (!this.IsEditingStaff)
                {
                    this.TemporaryEditStaff = null;
                }
            }
        }
        private StaffModel temporaryEditStaff;
        public StaffModel TemporaryEditStaff
        {
            get
            {
                return this.temporaryEditStaff;
            }
            set
            {
                this.temporaryEditStaff = value;
                this.NotifyPropertyChange(() => this.TemporaryEditStaff);
            }
        }
        #endregion

        #region 命令
        public ICommand EditStaffCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.IsEditingStaff = false;
                    this.TemporaryEditStaff = this.Staff.CloneObject();
                    this.IsEditingStaff = true;
                }, () =>
                {
                    return this.Staff != null && this.ApplicationContextViewModel.IsLogin && !this.Staff.Equals(this.ApplicationContextViewModel.CurrentStaff);
                });
            }
        }

        public ICommand ConfirmEditStaffCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.SaveStaff(this.ApplicationContextViewModel.CurrentStaff, this.TemporaryEditStaff);
                });
            }
        }

        public ICommand CancelEditStaffCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.IsEditingStaff = false;
                });
            }
        }

        public ICommand OpenPrivateChatCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.eventAggregator.PublishAsync(new OpenPrivateChatEventArgs() { ChatUser = this.Staff, IsActive = true });
                }, () =>
                {
                    return this.Staff != null && this.ApplicationContextViewModel.IsLogin && !this.Staff.Equals(this.ApplicationContextViewModel.CurrentStaff);
                });
            }
        }
        #endregion

        #region 方法
        private async void SaveStaff(StaffModel staff, StaffModel user)
        {
            bool res = false;
            if (string.IsNullOrEmpty(user.FollowId))
            {
                string followId = await this.userService.AddFollow(staff, user);
                if (string.IsNullOrEmpty(followId))
                {
                    res = false;
                }
                else
                {
                    user.FollowId = followId;
                    user.FollowTime = DateTime.Now;
                    res = true;
                }
            }
            else
            {
                res = await this.userService.ModifyFollow(staff, user);
            }
            if (!res) return;
            this.Staff.Assign(user);
            if (isInform) await this.eventAggregator.PublishAsync(new UserInfoChangedEventArgs() { User = user });
            this.IsEditingStaff = false;
        }

        public void Dispose()
        {
            this.Staff = null;
            this.eventAggregator.Unsubscribe(this);
        }
        #endregion
    }
}
