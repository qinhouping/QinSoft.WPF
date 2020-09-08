using EMChat2.Model.Entity;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.ViewModel.Main.Tabs.User
{
    public class StaffDetailAreaViewModel : PropertyChangedBase
    {
        #region 构造函数
        public StaffDetailAreaViewModel()
        {

        }
        #endregion

        #region 属性
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
        #endregion
    }
}
