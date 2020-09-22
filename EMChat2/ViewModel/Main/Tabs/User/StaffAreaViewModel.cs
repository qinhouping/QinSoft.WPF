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

namespace EMChat2.ViewModel.Main.Tabs.User
{
    [Component]
    public class StaffAreaViewModel : PropertyChangedBase, IEventHandle<LoginEventArgs>
    {
        #region 构造函数
        public StaffAreaViewModel(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private ObservableCollection<DepartmentInfo> departments;
        public ObservableCollection<DepartmentInfo> Departments
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
        #endregion

        #region 事件处理
        public void Handle(LoginEventArgs arg)
        {
            if (!arg.IsSuccess) return;
            this.departments = new ObservableCollection<DepartmentInfo>() {
                new DepartmentInfo() {
                    Name = "东财咨询",
                    Staffs = new ObservableCollection<StaffInfo>() { new StaffInfo()
                    {
                        Id = Guid.NewGuid().ToString(),
                        WorkCode = "180366",
                        Name = "总经理",
                        HeaderImageUrl = "https://dss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=3095823371,2737858048&fm=26&gp=0.jpg",
                        ImUserId = "1112",
                        State = UserStateEnum.Busy,
                        Sex= SexEnum.Women
                    } },
                    Departments = new ObservableCollection<DepartmentInfo>()
                    {
                        new DepartmentInfo()
                        {
                            Name = "业务组",
                            Departments=new ObservableCollection<DepartmentInfo>()
                            {
                                new DepartmentInfo()
                                {
                                    Name = "业务一部",
                                    Staffs = new ObservableCollection<StaffInfo>() { new StaffInfo()
                                    {
                                        Id = Guid.NewGuid().ToString(),
                                        WorkCode = "180366",
                                        Name = "秦后平",
                                        HeaderImageUrl = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=1764219719,2359539133&fm=26&gp=0.jpg",
                                        ImUserId = "1111",
                                        State = UserStateEnum.Busy,
                                        Sex=SexEnum.Man,
                                        Remark="秦后平-测试备注",
                                        Description="秦后平-测试描述"
                                    } }
                                }
                            }
                        }
                    }
                }
            };
        }
        #endregion

        #region 命令
        public ICommand OpenDepartmentDetailCommand
        {
            get
            {
                return new RelayCommand<DepartmentInfo>((department) =>
                {
                    this.eventAggregator.PublishAsync(new DetailChangeEventArgs()
                    {
                        Type = DetailType.Department,
                        Data = department
                    });
                });
            }
        }

        public ICommand OpenStaffDetailCommand
        {
            get
            {
                return new RelayCommand<StaffInfo>((staff) =>
                {
                    this.eventAggregator.PublishAsync(new DetailChangeEventArgs()
                    {
                        Type = DetailType.Staff,
                        Data = staff
                    });
                });
            }
        }
        #endregion
    }
}
