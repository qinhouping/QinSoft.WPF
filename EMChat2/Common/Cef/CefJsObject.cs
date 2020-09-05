using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common.Cef
{
    public class CefJsObject
    {
        public string JsObjectName { get; protected set; }

        public CefJsConnector CefJsConnector { get; protected set; }

        public CefJsObject(string jsObjectName)
        {
            if (string.IsNullOrEmpty(jsObjectName)) throw new ArgumentException("jsObjectName");
            this.JsObjectName = jsObjectName;
        }

        public virtual void RegisterJsConnector(CefJsConnector cefJsConnector)
        {
            if (cefJsConnector == null) throw new ArgumentNullException("cefJsConnector");
            if (this.CefJsConnector != null) throw new InvalidOperationException("cefJsObject has been registered");
            if (cefJsConnector.Connect(this)) this.CefJsConnector = cefJsConnector;
        }
        public virtual void UnregisterJsConnector(CefJsConnector cefJsConnector)
        {
            if (cefJsConnector == null) throw new ArgumentNullException("cefJsConnector");
            if (this.CefJsConnector == null) throw new InvalidOperationException("cefJsObject has been unregistered");
            if (cefJsConnector.Unconnect(this)) this.CefJsConnector = null;
        }
    }
}
