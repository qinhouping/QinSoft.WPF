using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Api
{
    /// <summary>
    /// 快捷回复
    /// </summary>
    public class QuickReplyApiModel
    {
        /// <summary>
        /// 快捷回复组ID
        /// </summary>
        public string QuickReplyGroupId { get; set; }

        /// <summary>
        /// 快捷回复ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 快捷回复名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 快捷回复内容类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 快捷回复内容
        /// </summary>
        public string Content { get; set; }
    }
}
