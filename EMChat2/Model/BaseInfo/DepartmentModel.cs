using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 部门信息模型
    /// </summary>
    public class DepartmentModel : PropertyChangedBase
    {
        #region 属性
        /// <summary>
        /// 部门ID
        /// </summary>
        private string id;
        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
                this.NotifyPropertyChange(() => this.Id);
            }
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        private string name;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                this.NotifyPropertyChange(() => this.Name);
            }
        }

        /// <summary>
        /// 子部门信息
        /// </summary>
        private ObservableCollection<DepartmentModel> departments;
        public ObservableCollection<DepartmentModel> Departments
        {
            get
            {
                return this.departments ?? new ObservableCollection<DepartmentModel>();
            }
            set
            {
                this.departments = value;
                this.NotifyPropertyChange(() => this.Departments);
                this.NotifyPropertyChange(() => this.AllStaffs);
            }
        }

        /// <summary>
        /// 员工信息
        /// </summary>
        private ObservableCollection<StaffModel> staffs;
        public ObservableCollection<StaffModel> Staffs
        {
            get
            {
                return this.staffs ?? new ObservableCollection<StaffModel>();
            }
            set
            {
                this.staffs = value;
                this.NotifyPropertyChange(() => this.Staffs);
                this.NotifyPropertyChange(() => this.AllStaffs);
            }
        }

        /// <summary>
        /// 所有员工
        /// </summary>
        public StaffModel[] AllStaffs
        {
            get
            {
                List<StaffModel> staffLIst = new List<StaffModel>();
                if (this.staffs != null)
                    staffLIst.AddRange(this.staffs);
                if (this.departments != null)
                    foreach (DepartmentModel department in this.departments)
                        staffLIst.AddRange(department.AllStaffs);
                return staffLIst.Distinct().ToArray();
            }
        }
        #endregion

        #region 方法
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.id.Equals((obj as DepartmentModel)?.id);
        }
        #endregion
    }
}
