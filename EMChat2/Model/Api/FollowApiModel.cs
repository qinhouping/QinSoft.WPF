using EMChat2.Model.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 好友关闭模型
    /// </summary>
    public class FollowApiModel
    {
        /// <summary>
        /// 好友关系Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 业务ID 
        /// </summary>
        public string BusinessId { get; set; }

        /// <summary>
        /// 当前用户ID
        /// </summary>
        public string CurrentUserId { get; set; }

        /// <summary>
        /// 当前用户类型
        /// </summary>
        public UserTypeEnum CurrentUserType { get; set; }

        /// <summary>
        /// 对方用户ID
        /// </summary>
        public string OppositeUserId { get; set; }

        /// <summary>
        /// 对方用户类型
        /// </summary>
        public UserTypeEnum OppositeUserType { get; set; }

        /// <summary>
        /// 对方用户信息字符串
        /// </summary>
        public string OppositeUserInfo { get; set; }

        /// <summary>
        /// 添加好友时间
        /// </summary>
        public DateTime FollowTime { get; set; }

        /// <summary>
        /// 好友备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 好友描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 好友标签
        /// </summary>
        public IEnumerable<FollowTagApiModel> FollowTags { get; set; }
    }
}
