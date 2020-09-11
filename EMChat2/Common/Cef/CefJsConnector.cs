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

        /// <summary>
        /// 连接Js对象
        /// </summary>
        /// <param name="cefJsObject"></param>
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

        /// <summary>
        /// 取消连接Js对象
        /// </summary>
        /// <param name="cefJsObject"></param>
        public virtual bool Unconnect(CefJsObject cefJsObject)
        {
            if (cefJsObject == null) throw new ArgumentException("cefJsObject");
            return cefJsObjects.Remove(cefJsObject.JsObjectName);
        }


        /// <summary>
        /// 执行JavaScript代码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="scriptUrl"></param>
        /// <param name="startLine"></param>
        public virtual void ExecuteJsAsync(string code, string scriptUrl = "about:blank", int startLine = 1)
        {
            if (this.CefWebBrowser.IsInitialized)
                this.CefWebBrowser.GetBrowser().MainFrame.ExecuteJavaScriptAsync(code, scriptUrl, startLine);
        }

        /// <summary>
        /// 执行JavaScript代码，并返回执行结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="scriptUrl"></param>
        /// <param name="startLine"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public virtual async Task<JavascriptResponse> EvaluateJsAsync(string code, string scriptUrl = "about:blank", int startLine = 1, TimeSpan? timeSpan = null)
        {
            if (this.CefWebBrowser.IsInitialized)
                return await this.CefWebBrowser.GetBrowser().MainFrame.EvaluateScriptAsync(code, scriptUrl, startLine, timeSpan);
            else
                return null;
        }

        /// <summary>
        /// 执行C#代码
        /// </summary>
        /// <param name="jsObjectName"></param>
        /// <param name="method"></param>
        /// <param name="paramList"></param>
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
