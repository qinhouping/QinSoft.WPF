using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 系统账号信息模型，用于广播，事件推送等
    /// </summary>
    public class SystemUserModel : UserModel
    {
        #region 构造函数
        public SystemUserModel()
        {
            this.Type = UserTypeEnum.SystemUser;
        }
        #endregion
    }
}
