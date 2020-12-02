using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.BaseInfo
{
    /// <summary>
    /// 链接地址影视关系模型
    /// </summary>
    public class UrlMappingModel
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
            return this.Url.Equals((obj as UrlMappingModel).Url);
        }
        #endregion
    }
}
