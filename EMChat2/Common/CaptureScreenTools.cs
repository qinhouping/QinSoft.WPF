using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common
{
    public static class CaptureScreenTools
    {
        public static void CallCaptureScreenProcess()
        {
            Process process = Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "Tools", "CaptureScreen.exe"));
            process.WaitForExit();
        }
    }
}
