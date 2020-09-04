using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common.Cef
{
    public interface ICefJsObject
    {
        ChromiumWebBrowser ChromiumWebBrowser { get; set; }

        Task<JsResponse> ExecuteJsAsync(string script, string scriptUrl = "about:blank", int startLine = 1, TimeSpan? timeout = null);

        void ExecuteJsAsync(string code, string scriptUrl = "about:blank", int startLine = 1);
    }



    public class JsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public object Result { get; set; }
    }
}
