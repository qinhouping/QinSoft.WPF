using EMChat2.ViewModel.Main.Tabs.Chat;
using EMChat2.ViewModel;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Windows;
using System.Diagnostics;

namespace EMChat2
{
    public class ApplicationBooter : ApplicationBase
    {
        private bool allowMulitInstance = Convert.ToBoolean(ConfigurationManager.AppSettings["AllowMulitInstance"]);
        public static ApplicationBooter Current { get; protected set; }

        public ApplicationBooter() : base()
        {
            if (Current != null) throw new InvalidProgramException("An instantiation of the applicationbooter object already exists");
            Current = this;

            if (!allowMulitInstance && IsExistsInstance())
            {
                MessageBox.Show("程序已经在运行！");
                Application.Current.Shutdown();
            }
            else
            {
                this.OnStartUp<LoginViewModel>();
            }
        }

        private bool IsExistsInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            return processes.Any(u => u.Id != current.Id);
        }
    }
}
