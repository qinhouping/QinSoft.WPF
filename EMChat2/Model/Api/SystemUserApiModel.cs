using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class SystemUserApiModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// IM用户ID
        /// </summary>
        public string ImUserId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeaderImageUrl { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Number { get; set; }
    }
}
