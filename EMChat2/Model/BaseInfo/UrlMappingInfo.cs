using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 链接地址影视关系实体
    /// </summary>
    public class UrlMappingInfo
    {
        #region 属性
        /// <summary>
        /// 网络路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 本地文件路径
        /// </summary>
        public string LocalFilePath { get; set; }
        #endregion

        #region 方法
        public override int GetHashCode()
        {
            return this.Url.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Url.Equals((obj as UrlMappingInfo).Url);
        }
        #endregion
    }
}
