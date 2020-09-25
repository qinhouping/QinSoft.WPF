using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 系统账号信息实体，用于广播，事件推送等
    /// </summary>
    public class SystemUserInfo : UserInfo
    {
        #region 构造函数
        public SystemUserInfo()
        {
            this.Type = UserTypeEnum.SystemUser;
        }
        #endregion
    }
}
