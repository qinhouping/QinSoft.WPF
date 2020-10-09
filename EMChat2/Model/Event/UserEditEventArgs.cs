using EMChat2.Model.BaseInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Event
{
    public class UserEditEventArgs : EventArgs
    {
        public UserInfo User { get; set; }
    }
}
