using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.IM
{
    /// <summary>
    /// IM文件信息
    /// </summary>
    public class IMFileInfo
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// MD5
        /// </summary>
        public string MD5 { get; set; }
    }
}
