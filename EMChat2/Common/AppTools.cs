using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common
{
    /// <summary>
    /// 版本工具
    /// </summary>
    public static class AppTools
    {
        /// <summary>
        /// 应用主程序版本
        /// </summary>
        public static readonly Version AppVersion = Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// 应用程序全名
        /// </summary>
        public static readonly string AppFullName = Assembly.GetExecutingAssembly().GetName().FullName;

        /// <summary>
        /// 应用程序名称
        /// </summary>
        public static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        /// 获取应用程序中程序集版本
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Version GetVersion(Type type)
        {
            if (type == null) return null;
            else return type.Assembly.GetName().Version;
        }
    }
}
