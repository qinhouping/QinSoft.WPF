﻿using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Entity
{
    /// <summary>
    /// 部门信息实体
    /// </summary>
    public class DepartmentInfo : PropertyChangedBase
    {
        /// <summary>
        /// 子部门信息
        /// </summary>
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
                this.NotifyPropertyChange(() => this.AllStaffs);
            }
        }

        /// <summary>
        /// 员工信息
        /// </summary>
        private ObservableCollection<StaffInfo> staffs;
        public ObservableCollection<StaffInfo> Staffs
        {
            get
            {
                return this.staffs;
            }
            set
            {
                this.staffs = value;
                this.NotifyPropertyChange(() => this.Staffs);
                this.NotifyPropertyChange(() => this.AllStaffs);
            }
        }

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

        private string description;

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
                this.NotifyPropertyChange(() => this.Description);
            }
        }

        /// <summary>
        /// 所有客服
        /// </summary>
        public StaffInfo[] AllStaffs
        {
            get
            {
                List<StaffInfo> staffInfos = new List<StaffInfo>();
                if (this.staffs != null)
                    staffInfos.AddRange(this.staffs);
                if (this.departments != null)
                    foreach (DepartmentInfo department in this.departments)
                        staffInfos.AddRange(department.AllStaffs);
                return staffInfos.Distinct().ToArray();
            }
        }
    }
}