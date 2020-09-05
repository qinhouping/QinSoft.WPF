using CefSharp;
using QinSoft.Ioc.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EMChat2.Common.Cef
{
    [Component]
    public class ComputerInfoCefJsObject : CefJsObject
    {
        public ComputerInfoCefJsObject() : base("computerInfo") { }

        public void GetEnviroment(IJavascriptCallback callback)
        {
            callback.ExecuteAsync(new EnvironmentInfo()
            {
                OSVersion = Environment.OSVersion,
                CurrentDirectory = Environment.CurrentDirectory,
                Drives = Environment.GetLogicalDrives(),
                Version = Environment.Version,
                MachineName = Environment.MachineName,
                SystemDirectory = Environment.SystemDirectory,
                UserName = Environment.UserName,
                UserDomainName = Environment.UserDomainName,
                TickCount = Environment.TickCount,
                WorkingSet = Environment.WorkingSet
            }.ObjectToJson());
        }
    }

    public class EnvironmentInfo
    {
        public OperatingSystem OSVersion { get; set; }
        public string CurrentDirectory { get; set; }
        public string[] Drives { get; set; }
        public Version Version { get; set; }
        public string MachineName { get; set; }
        public string SystemDirectory { get; set; }
        public string UserName { get; set; }
        public string UserDomainName { get; set; }
        public long TickCount { get; set; }
        public long WorkingSet { get; set; }
    }
}
