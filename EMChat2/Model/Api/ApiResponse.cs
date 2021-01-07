using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Api.Models.Response
{
    /// <summary>
    /// 响应模型
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 响应码
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 响应模型
    /// </summary>
    public class ApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// 分页响应模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageApiResponse<T> : ApiResponse<IEnumerable<T>>
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }
    }
}
