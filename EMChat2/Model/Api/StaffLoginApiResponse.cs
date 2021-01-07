using EMChat2.Common;
using EMChat2.Model.Api;
using EMChat2.Model.IM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMChat2.Api.Models.Response
{
    /// <summary>
    /// 员工登录响应模型
    /// </summary>
    public class StaffLoginResponse
    {
        /// <summary>
        /// 员工
        /// </summary>
        public StaffApiModel Staff { get; set; }

        /// <summary>
        /// IM服务器信息
        /// </summary>
        public IMServerModel IMServer { get; set; }

        /// <summary>
        /// IM用户信息
        /// </summary>
        public IMUserModel IMUser { get; set; }

        /// <summary>
        /// 接口Token
        /// </summary>
        public string Token { get; set; }
    }
}