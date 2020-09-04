using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common
{
    public static class VersionTools
    {
        public static Version GetAppVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        public static Version GetVersion(Type type)
        {
            Assembly assembly = type.Assembly;
            return assembly.GetName().Version;
        }
    }
}
