﻿using EMChat2.Common;
using EMChat2.Model.Entity;
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

        public async void OpenFile(string filePath, string name, string extension)
        {
            if (filePath.IsNetUrl())
            {
                FileDialog fileDialog = new SaveFileDialog();
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                fileDialog.FileName = name;
                fileDialog.Filter = "文件|*." + extension;
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    await HttpTools.DownloadAsync(filePath, null, null).ContinueWith(task =>
                    {
                        try
                        {
                            task.Result.StreamToFile(fileDialog.FileName);
                            systemService.StoreUrlMapping(new UrlMappingInfo() { Url = filePath, LocalFilePath = fileDialog.FileName });
                            Process.Start(fileDialog.FileName);
                        }
                        catch (Exception e)
                        {
                            new Action(() => this.windowManager.ShowDialog(new AlertViewModel(windowManager, "文件下载失败" + e.Message, "提示", AlertType.Error))).ExecuteInUIThread();
                        }
                    });
                }
            }
            else
            {
                Process.Start(filePath);
            }
        }
        #endregion
    }
}