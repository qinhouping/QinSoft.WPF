using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// IM用户信息（用于IM通信）
    /// </summary>
    public class IMUserInfo
    {
        /// <summary>
        /// IM用户ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 应用Key
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
