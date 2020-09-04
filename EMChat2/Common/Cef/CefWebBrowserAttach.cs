using CefSharp;
using CefSharp.ModelBinding;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EMChat2.Common.Cef
{
    public static class ChromiumWebBrowserAttach
    {
        public static readonly DependencyProperty RegisterJsObjectProperty =
            DependencyProperty.RegisterAttached("RegisterJsObject", typeof(RegisterJsObject), typeof(ChromiumWebBrowserAttach), new PropertyMetadata(OnRegisterJsObjectPropertyChangedCallback));
        public static void SetRegisterJsObject(DependencyObject dp, RegisterJsObject value)
        {
            dp.SetValue(RegisterJsObjectProperty, value);
        }
        public static RegisterJsObject GetRegisterJsObject(DependencyObject dp)
        {
            return dp.GetValue(RegisterJsObjectProperty) as RegisterJsObject;
        }

        private static void OnRegisterJsObjectPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChromiumWebBrowser chromiumWebBrowser = d as ChromiumWebBrowser;
            if (chromiumWebBrowser == null || chromiumWebBrowser.IsInitialized) return;
            RegisterJsObject registerJsObject = GetRegisterJsObject(chromiumWebBrowser);
            chromiumWebBrowser.RegisterAsyncJsObject(registerJsObject.Name, registerJsObject.JsObject, new BindingOptions() { CamelCaseJavascriptNames = true });
            registerJsObject.JsObject.ChromiumWebBrowser = chromiumWebBrowser;
        }
    }

    public class RegisterJsObject
    {
        public string Name { get; set; }

        public ICefJsObject JsObject { get; set; }
    }
}
