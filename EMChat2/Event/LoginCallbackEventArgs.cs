using EMChat2.Model.BaseInfo;
using EMChat2.Model.IM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Event
{
    public class LoginCallbackEventArgs : CallbackEventArgs
    {
        public StaffModel Staff { get; set; }

        public IMServerModel IMServer { get; set; }

        public IMUserModel IMUser { get; set; }
    }
}
