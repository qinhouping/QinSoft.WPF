using EMChat2.Model.Entity;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    public class DepartmentDetailAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public DepartmentDetailAreaViewModel()
        {

        }
        #endregion

        #region 属性
        private DepartmentInfo department;
        public DepartmentInfo Department
        {
            get
            {
                return this.department;
            }
            set
            {
                this.department = value;
                this.NotifyPropertyChange(() => this.Department);
            }
        }
        #endregion
    }
}
