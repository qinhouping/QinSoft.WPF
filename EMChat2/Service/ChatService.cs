using EMChat2.Common;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service
{
    [Component]
    public class ChatService
    {
        #region 构造函数
        public ChatService(IWindowManager windowManager, EventAggregator eventAggregator, SystemService systemService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.systemService = systemService;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private SystemService systemService;
        #endregion

        public async void OpenLink(string url)
        {
            await new Func<object>(() =>
            {
                Process.Start(url);
                return null;
            }).ExecuteInTask();
        }

        public async void OpenImage(string[] sources, int index)
        {
            await new Func<object>(() =>
            {
                new Action(() => this.windowManager.ShowDialog(new PictureExplorerViewModel(this.windowManager, systemService, sources, index))).ExecuteInUIThread();
                return null;
            }).ExecuteInTask();
        }
    }
}
