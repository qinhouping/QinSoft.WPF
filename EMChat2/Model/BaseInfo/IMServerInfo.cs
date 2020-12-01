using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// IM服务配置信息（用于IM通信）
    /// </summary>
    public class IMServerInfo
    {
        /// <summary>
        /// Api地址
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// socket IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// socket端口
        /// </summary>
        public int Port { get; set; }
    }
}
