using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EMChat2.Common.Cef
{
    /// <summary>
    /// CEF流浪器附加
    /// 目前最多同时附加5个js对象
    /// </summary>
    public static class CefWebBrowserAttach
    {
        public static readonly DependencyProperty RegisterJsObject1Property = DependencyProperty.RegisterAttached("RegisterJsObject1", typeof(CefJsObject), typeof(CefWebBrowserAttach), new PropertyMetadata(OnRegisterJsObjectPropertyChangedCallback));

        public static readonly DependencyProperty RegisterJsObject2Property = DependencyProperty.RegisterAttached("RegisterJsObject2", typeof(CefJsObject), typeof(CefWebBrowserAttach), new PropertyMetadata(OnRegisterJsObjectPropertyChangedCallback));

        public static readonly DependencyProperty RegisterJsObject3Property = DependencyProperty.RegisterAttached("RegisterJsObject3", typeof(CefJsObject), typeof(CefWebBrowserAttach), new PropertyMetadata(OnRegisterJsObjectPropertyChangedCallback));

        public static readonly DependencyProperty RegisterJsObject4Property = DependencyProperty.RegisterAttached("RegisterJsObject4", typeof(CefJsObject), typeof(CefWebBrowserAttach), new PropertyMetadata(OnRegisterJsObjectPropertyChangedCallback));

        public static readonly DependencyProperty RegisterJsObject5Property = DependencyProperty.RegisterAttached("RegisterJsObject5", typeof(CefJsObject), typeof(CefWebBrowserAttach), new PropertyMetadata(OnRegisterJsObjectPropertyChangedCallback));

        public static void SetRegisterJsObject1(DependencyObject dp, CefJsObject value)
        {
            dp.SetValue(RegisterJsObject1Property, value);
        }

        public static CefJsObject GetRegisterJsObject1(DependencyObject dp)
        {
            return dp.GetValue(RegisterJsObject1Property) as CefJsObject;
        }

        public static void SetRegisterJsObject2(DependencyObject dp, CefJsObject value)
        {
            dp.SetValue(RegisterJsObject2Property, value);
        }

        public static CefJsObject GetRegisterJsObject2(DependencyObject dp)
        {
            return dp.GetValue(RegisterJsObject2Property) as CefJsObject;
        }

        public static void SetRegisterJsObject3(DependencyObject dp, CefJsObject value)
        {
            dp.SetValue(RegisterJsObject3Property, value);
        }

        public static CefJsObject GetRegisterJsObject3(DependencyObject dp)
        {
            return dp.GetValue(RegisterJsObject3Property) as CefJsObject;
        }

        public static void SetRegisterJsObject4(DependencyObject dp, CefJsObject value)
        {
            dp.SetValue(RegisterJsObject4Property, value);
        }

        public static CefJsObject GetRegisterJsObject4(DependencyObject dp)
        {
            return dp.GetValue(RegisterJsObject4Property) as CefJsObject;
        }

        public static void SetRegisterJsObject5(DependencyObject dp, CefJsObject value)
        {
            dp.SetValue(RegisterJsObject5Property, value);
        }

        public static CefJsObject GetRegisterJsObject5(DependencyObject dp)
        {
            return dp.GetValue(RegisterJsObject5Property) as CefJsObject;
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
