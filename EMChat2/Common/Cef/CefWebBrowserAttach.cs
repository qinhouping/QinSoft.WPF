using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EMChat2.Common.Cef
{
    public static class CefWebBrowserAttach
    {
        public static readonly DependencyProperty RegisterJsObjectProperty =
                 DependencyProperty.RegisterAttached("RegisterJsObject", typeof(CefJsObject), typeof(CefWebBrowserAttach), new PropertyMetadata(OnRegisterJsObjectPropertyChangedCallback));

        public static void SetRegisterJsObject(DependencyObject dp, CefJsObject value)
        {
            dp.SetValue(RegisterJsObjectProperty, value);
        }

        public static CefJsObject GetRegisterJsObject(DependencyObject dp)
        {
            return dp.GetValue(RegisterJsObjectProperty) as CefJsObject;
        }

        private static void OnRegisterJsObjectPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CefWebBrowser webBrowser = d as CefWebBrowser;
            if (webBrowser == null) return;
            if (e.OldValue is CefJsObject)
            {
                ((e.OldValue) as CefJsObject).UnregisterJsConnector(webBrowser.CefJsConnector);
            }
            if (e.NewValue is CefJsObject)
            {
                ((e.NewValue) as CefJsObject).RegisterJsConnector(webBrowser.CefJsConnector);
            }
        }

    }
}
