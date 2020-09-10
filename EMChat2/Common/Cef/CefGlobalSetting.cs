using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.Wpf;
using EMChat2.Common;

namespace EMChat2.Common.Cef
{
    public static class CefGlobalSetting
    {
        public static void Initialize()
        {
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.SubprocessExitIfParentProcessClosed = true;

            CefSettings cefSettings = new CefSettings();
            cefSettings.Locale = "zh_CN";
            cefSettings.UserAgent = string.Format("QinSoft.WPF-EMChat2.0({0})-ChromiumWebBrowser({1})", VersionTools.AppVersion, VersionTools.GetVersion(typeof(ChromiumWebBrowser)));
            CefSharp.Cef.Initialize(cefSettings);
        }
    }
}
