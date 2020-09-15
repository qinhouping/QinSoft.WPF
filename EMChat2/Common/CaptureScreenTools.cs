using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EMChat2.Common
{
    public static class CaptureScreenTools
    {
        public static Image CallCaptureScreenProcess()
        {
            Process process = Process.Start(Path.Combine(Directory.GetCurrentDirectory(), "Tools", "CaptureScreen.exe"));
            process.WaitForExit();
            if (process.ExitCode == 1) return Clipboard.GetImage();
            else return null;
        }
    }
}
