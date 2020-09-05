using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EMChat2.Common.Cef
{
    public class CefWebBrowser : ChromiumWebBrowser
    {
        static CefWebBrowser()
        {
            //cef全局设置
            CefGlobalSetting.Initialize();
        }

        public CefWebBrowser()
        {
            RegisterJsConnector();

            this.IsBrowserInitializedChanged += CefWebBrowser_IsBrowserInitializedChanged;
        }

        private void CefWebBrowser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsInitialized) this.ShowDevTools();
        }

        public CefJsConnector CefJsConnector { get; protected set; }

        protected virtual void RegisterJsConnector()
        {
            if (this.IsInitialized) return;
            CefJsConnector cefJsConnector = new CefJsConnector(this);
            this.RegisterAsyncJsObject("emchatJsConnector", cefJsConnector);
            this.CefJsConnector = cefJsConnector;
        }
    }
}
