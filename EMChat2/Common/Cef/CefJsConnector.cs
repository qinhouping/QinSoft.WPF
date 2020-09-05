using CefSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMChat2.Common.Cef
{
    /// <summary>
    /// CEF浏览器连接器
    /// </summary>
    public class CefJsConnector
    {
        public CefWebBrowser CefWebBrowser { get; private set; }

        protected IDictionary<string, CefJsObject> cefJsObjects;

        public CefJsConnector(CefWebBrowser cefWebBrowser)
        {
            if (cefWebBrowser == null) throw new ArgumentNullException("cefWebBrowser");
            this.CefWebBrowser = cefWebBrowser;
            this.cefJsObjects = new Dictionary<string, CefJsObject>();
        }

        public virtual bool Connect(CefJsObject cefJsObject)
        {
            if (cefJsObject == null) throw new ArgumentException("cefJsObject");
            if (cefJsObjects.ContainsKey(cefJsObject.JsObjectName)) return false;
            else
            {
                cefJsObjects[cefJsObject.JsObjectName] = cefJsObject;
                return true;
            }
        }

        public virtual bool Unconnect(CefJsObject cefJsObject)
        {
            if (cefJsObject == null) throw new ArgumentException("cefJsObject");
            return cefJsObjects.Remove(cefJsObject.JsObjectName);
        }


        public virtual void ExecuteJsAsync(string code, string scriptUrl = "about:blank", int startLine = 1)
        {
            if (this.CefWebBrowser.IsInitialized)
                this.CefWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(code, scriptUrl, startLine);
        }

        public virtual async Task<JavascriptResponse> EvaluateJsAsync(string code, string scriptUrl = "about:blank", int startLine = 1, TimeSpan? timeSpan = null)
        {
            if (this.CefWebBrowser.IsInitialized)
                return await this.CefWebBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(code, scriptUrl, startLine, timeSpan);
            else
                return null;
        }

        public virtual async void ExecuteCSharpAsync(string jsObjectName, string method, params object[] paramList)
        {
            try
            {
                CefJsObject jsObject = null;
                if (!cefJsObjects.TryGetValue(jsObjectName, out jsObject)) return;
                MethodInfo methodInfo = jsObject.GetType().GetMethod(method);
                await new Func<object>(() => methodInfo.Invoke(jsObject, paramList)).ExecuteInTask<object>();
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("ExecuteCSharpAsync Error:{0}", e));
            }
        }
    }
}
