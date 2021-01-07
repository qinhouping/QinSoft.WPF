using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.IM
{
    /// <summary>
    /// IM文件上传响应结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IMUploadResponse<T>
    {
        /// <summary>
        /// 数量
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
}
