using EMChat2.Common;
using EMChat2.Model.BaseInfo;
using EMChat2.ViewModel;
using EMChat2.ViewModel.Main.Tabs.Chat;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EMChat2.Service
{
    /// <summary>
    /// 会话服务，负责会话相关逻辑
    /// </summary>
    [Component]
    public class ChatService
    {
        #region 构造函数
        public ChatService(IWindowManager windowManager, EventAggregator eventAggregator, SystemService systemService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.systemService = systemService;
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private SystemService systemService;
        #endregion

        #region 方法
        public async void OpenLink(string url)
        {
            await new Action(() =>
            {
                Process.Start(url);
            }).ExecuteInTask();
        }

        public async void OpenImage(string[] sources, int index)
        {
            for (int i = 0; i < sources.Length; i++)
            {
                sources[i] = await systemService.GetUrlMapping(sources[i]);
            }
            new Action(() =>
            {
                using (PictureExplorerViewModel pictureExplorerViewModel = new PictureExplorerViewModel(this.windowManager, this.eventAggregator, systemService, sources, index))
                {
                    this.windowManager.ShowDialog(pictureExplorerViewModel);
                }
            }).ExecuteInUIThread();
        }

        public async void OpenFile(string filePath, string name, string extension)
        {
            filePath = await systemService.GetUrlMapping(filePath);
            if (filePath.IsNetUrl())
            {
                FileDialog fileDialog = new SaveFileDialog();
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                fileDialog.FileName = name;
                fileDialog.Filter = "文件|*." + extension;
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    await DownloadFile(filePath, fileDialog.FileName);
                    Process.Start(filePath);
                }
            }
            else
            {
                Process.Start(filePath);
            }
        }

        private async Task DownloadFile(string url, string filePath)
        {
            try
            {
                Stream stream = await HttpTools.DownloadAsync(url, null, null);
                stream.StreamToFile(filePath);
                systemService.StoreUrlMapping(new UrlMappingInfo() { Url = url, LocalFilePath = filePath });
            }
            catch (Exception e)
            {
                using (AlertViewModel alertViewModel = new AlertViewModel(this.windowManager, this.eventAggregator, "保存失败" + e.Message, "提示", AlertType.Error))
                {
                    new Action(() => this.windowManager.ShowDialog(alertViewModel)).ExecuteInUIThread();
                }
            }
        }
        #endregion
    }
}
