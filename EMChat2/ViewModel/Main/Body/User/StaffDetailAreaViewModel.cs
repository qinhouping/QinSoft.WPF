using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Event;
using QinSoft.Event;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    public class StaffDetailAreaViewModel : PropertyChangedBase, IEventHandle<UserEditEventArgs>, IDisposable
    {
        #region 构造函数
        public StaffDetailAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, ApplicationContextViewModel applicationContextViewModel, bool isInform = true)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.applicationContextViewModel = applicationContextViewModel;
            this.isInform = isInform;
        }
        #endregion

        #region 属性
        private bool isInform;
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
        private StaffInfo staff;
        public StaffInfo Staff
        {
            get
            {
                return this.staff;
            }
            set
            {
                this.staff = value;
                this.NotifyPropertyChange(() => this.Staff);
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
        private StaffInfo temporaryEditStaff;
        public StaffInfo TemporaryEditStaff
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
                    this.TemporaryEditStaff = this.Staff.Clone();
                    this.IsEditingStaff = true;
                });
            }
        }

        public ICommand ConfirmEditStaffCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (isInform) this.eventAggregator.PublishAsync(new UserEditEventArgs() { User = this.TemporaryEditStaff });
                    else this.Staff.Assign(this.TemporaryEditStaff);
                    this.IsEditingStaff = false;
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
                });
            }
        }
        #endregion

        #region 方法
        public void Dispose()
        {
            this.eventAggregator.Unsubscribe(this);
        }
        #endregion

        #region 事件处理
        public void Handle(UserEditEventArgs arg)
        {
            if (!(arg.User is StaffInfo)) return;
            StaffInfo newStaff = arg.User as StaffInfo;
            if (!newStaff.Equals(this.Staff)) return;
            this.Staff.Assign(newStaff);
        }
        #endregion
    }
}
