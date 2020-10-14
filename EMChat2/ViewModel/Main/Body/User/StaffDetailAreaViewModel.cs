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
    public class StaffDetailAreaViewModel : PropertyChangedBase, IDisposable
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
                    this.Staff.Assign(this.TemporaryEditStaff);
                    if (isInform) this.eventAggregator.PublishAsync(new UserEditEventArgs() { User = this.TemporaryEditStaff });
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
                }, () =>
                {
                    return this.Staff != null && this.ApplicationContextViewModel.IsLogin && !this.Staff.Equals(this.ApplicationContextViewModel.CurrentStaff);
                });
            }
        }
        #endregion

        #region 方法
        public void Dispose()
        {
            this.Staff = null;
            this.eventAggregator.Unsubscribe(this);
        }
        #endregion
    }
}
