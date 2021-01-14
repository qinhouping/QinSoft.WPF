using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
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
using EMChat2.Model.Enum;
using EMChat2.Service;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    [Component]
    public class StaffAreaViewModel : PropertyChangedBase, IEventHandle<LoginCallbackEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>
    {
        #region 构造函数
        public StaffAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator, UserService userService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.departments = new ObservableCollection<DepartmentModel>();
            this.uesrService = userService;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ObservableCollection<DepartmentModel> departments;
        public ObservableCollection<DepartmentModel> Departments
        {
            get
            {
                return this.departments;
            }
            set
            {
                this.departments = value;
                this.NotifyPropertyChange(() => this.Departments);
            }
        }
        private UserService uesrService;
        #endregion

        #region 命令
        public ICommand OpenDepartmentDetailCommand
        {
            get
            {
                return new RelayCommand<DepartmentModel>((department) =>
                {
                    this.eventAggregator.PublishAsync(new SelectUseDetailEventArgs()
                    {
                        Type = UserDetailType.Department,
                        Data = department
                    });
                }, (department) =>
                {
                    return department != null;
                });
            }
        }

        public ICommand OpenStaffDetailCommand
        {
            get
            {
                return new RelayCommand<StaffModel>((staff) =>
                {
                    this.eventAggregator.PublishAsync(new SelectUseDetailEventArgs()
                    {
                        Type = UserDetailType.Staff,
                        Data = staff
                    });
                }, (staff) =>
                {
                    return staff != null;
                });
            }
        }
        #endregion

        #region 方法
        private async void GetDepartments(StaffModel staff)
        {
            IEnumerable<DepartmentModel> departments = await this.uesrService.GetDepartments(staff);
            if (departments == null) return;
            new Action(() =>
            {
                lock (this.Departments)
                {
                    this.Departments.Clear();
                    foreach (DepartmentModel department in departments)
                    {
                        this.Departments.Add(department);
                    }
                }
            }).ExecuteInUIThread();
        }
        #endregion

        #region 事件处理
        public void Handle(LoginCallbackEventArgs arg)
        {
            if (!arg.IsSuccess) return;
            GetDepartments(arg.Staff);
        }

        public void Handle(LogoutCallbackEventArgs arg)
        {
            new Action(() =>
            {
                lock (this.Departments)
                {
                    this.Departments.Clear();
                }
            }).ExecuteInUIThread();
        }

        public void Handle(ExitCallbackEventArgs arg)
        {
            new Action(() =>
            {
                lock (this.Departments)
                {
                    this.Departments.Clear();
                }
            }).ExecuteInUIThread();
        }
        #endregion
    }
}
