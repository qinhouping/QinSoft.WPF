using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace EMChat2.Common
{
    public static class GcTimerTools
    {
        private static DispatcherTimer timer;

        public static void Initialize(int seconds = 60)
        {
            if (timer != null)
            {
                timer.Stop();
            }
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(seconds);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public static void Dispose()
        {
            if (timer == null) return;
            timer.Stop();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
