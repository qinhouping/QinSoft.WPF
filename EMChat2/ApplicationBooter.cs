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

            bool isNewInstance = false;
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Mutex mutex = new Mutex(true, appName, out isNewInstance);
            if (!allowMulitInstance && !isNewInstance)
            {
                MessageBox.Show("The app is running now");
                Application.Current.Shutdown();
            }
            else
            {
                this.OnStartUp<LoginViewModel>();
            }
        }
    }
}
