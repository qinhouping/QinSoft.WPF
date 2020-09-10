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
    public static class VersionTools
    {
        /// <summary>
        /// 应用主程序版本
        /// </summary>
        public static readonly Version AppVersion = Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// 获取类型所在程序集版本
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>程序集版本</returns>
        public static Version GetVersion(Type type)
        {
            Assembly assembly = type.Assembly;
            return assembly.GetName().Version;
        }
    }
}
