using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMChat2.Common.Cef
{
    public class CefJsObjectBase : ICefJsObject
    {
        public ChromiumWebBrowser ChromiumWebBrowser { get; set; }

        public async Task<JsResponse> ExecuteJsAsync(string script, string scriptUrl = "about:blank", int startLine = 1, TimeSpan? timeout = null)
        {
            if (ChromiumWebBrowser == null) return null;
            while (true)
            {
                if (ChromiumWebBrowser.IsInitialized)
                {
                    break;
                }
                Thread.Sleep(50);
            }
            JavascriptResponse response = await ChromiumWebBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(script, scriptUrl, startLine, timeout);
            return new JsResponse() { Success = response.Success, Message = response.Message, Result = response.Result };
        }

        public void ExecuteJsAsync(string code, string scriptUrl = "about:blank", int startLine = 1)
        {
            if (ChromiumWebBrowser == null) return;
            ChromiumWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(code, scriptUrl, startLine);
        }
    }
}
