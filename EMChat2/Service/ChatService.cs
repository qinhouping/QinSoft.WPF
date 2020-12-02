﻿using EMChat2.Common;
using EMChat2.Common.PipeFilter;
using EMChat2.Model.BaseInfo;
using EMChat2.Event;
using EMChat2.Model.IM;
using EMChat2.Service.PipeFilter;
using EMChat2.Service.PipeFilter.RecvMessage;
using EMChat2.Service.PipeFilter.SendMessage;
using EMChat2.ViewModel;
using EMChat2.ViewModel.Main.Tabs.Chat;
using Hardcodet.Wpf.TaskbarNotification;
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
    public class ChatService : IEventHandle<LoginCallbackEventArgs>, IEventHandle<LogoutCallbackEventArgs>, IEventHandle<ExitCallbackEventArgs>
    {
        #region 构造函数
        public ChatService(IWindowManager windowManager, EventAggregator eventAggregator, SystemService systemService)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            this.systemService = systemService;

            //添加IM事件处理
            IMTools.OnSocketConnected += IMTools_OnSocketConnected;
            IMTools.OnSocketDisconnected += IMTools_OnSocketDisconnected;
            IMTools.OnSocketError += IMTools_OnSocketError;
            IMTools.OnSocketReconnectFailed += IMTools_OnSocketReconnectFailed;
            IMTools.OnReceiveMessage += IMTools_OnReceiveMessage;

            this.sendMessageBeginPipeFilter = CreateSendMessageBeginPipeFilter();
            this.recvMessageBeginPipeFilter = CreateRecvMessageBeginPipeFilter();
        }
        #endregion

        #region 属性
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private SystemService systemService;
        private IMServerModel imServer;
        private IMUserModel imUser;
        private BeginPipeFilter sendMessageBeginPipeFilter;
        private BeginPipeFilter recvMessageBeginPipeFilter;
        #endregion

        #region 方法

        protected virtual BeginPipeFilter CreateSendMessageBeginPipeFilter()
        {
            BeginPipeFilter pipeFilter = new BeginPipeFilter();
            pipeFilter.SetNextPipeFilter(new CheckMessagePipeFilter()).SetNextPipeFilter(new ConvertMessageUrlPipeFilter()).SetNextPipeFilter(new PushMessagePipeFilter(this.eventAggregator)).SetNextPipeFilter(new StoreMessagePipeFilter());
            return pipeFilter;
        }

        protected virtual BeginPipeFilter CreateRecvMessageBeginPipeFilter()
        {
            BeginPipeFilter pipeFilter = new BeginPipeFilter();
            pipeFilter.SetNextPipeFilter(new ResolveEventMessagePipeFilter(this.eventAggregator)).SetNextPipeFilter(new RenderMessagePipeFilter(this.eventAggregator));
            return pipeFilter;
        }

        private async void HandleSendMessage(MessageModel message)
        {
            await new Action(() => sendMessageBeginPipeFilter.Begin(message.Clone())).ExecuteInTask();
        }

        private async void HandleRecvMessage(MessageModel message)
        {
            await new Action(() => recvMessageBeginPipeFilter.Begin(message.Clone())).ExecuteInTask();
        }

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

        public async void OpenFile(string filePath, string name)
        {
            filePath = await systemService.GetUrlMapping(filePath);
            if (filePath.IsNetUrl())
            {
                FileDialog fileDialog = new SaveFileDialog();
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                fileDialog.FileName = name;
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    DownloadFile(filePath, fileDialog.FileName);
                    Process.Start(fileDialog.FileName);
                }
            }
            else
            {
                Process.Start(filePath);
            }
        }

        private async void DownloadFile(string url, string filePath)
        {
            try
            {
                Stream stream = await HttpTools.DownloadAsync(url, null, null);
                await new Action(() => stream.StreamToFile(filePath)).ExecuteInTask();
                systemService.StoreUrlMapping(new UrlMappingModel() { Url = url, LocalFilePath = filePath });
                await this.eventAggregator.PublishAsync(new ShowBalloonTipEventArgs() { BalloonTip = new BalloonTipInfo() { Content = string.Format("文件[" + url + "]下载完成") } });
            }
            catch (Exception e)
            {
                using (AlertViewModel alertViewModel = new AlertViewModel(this.windowManager, this.eventAggregator, "保存失败" + e.Message, "提示", AlertType.Error))
                {
                    new Action(() => this.windowManager.ShowDialog(alertViewModel)).ExecuteInUIThread();
                }
            }
        }

        public async void SendMessage(MessageModel message)
        {
            await Task.Delay(100);
            HandleSendMessage(message);
        }
        #endregion

        #region 事件处理
        public void Handle(LoginCallbackEventArgs arg)
        {
            if (!arg.IsSuccess) return;
            this.imServer = arg.IMServer;
            this.imUser = arg.IMUser;
            IMTools.Start(this.imServer);
        }

        public void Handle(LogoutCallbackEventArgs arg)
        {
            IMTools.Stop();
            this.imServer = null;
            this.imUser = null;
        }

        public void Handle(ExitCallbackEventArgs arg)
        {
            IMTools.Stop();
            this.imServer = null;
            this.imUser = null;
        }
        #endregion

        #region IM事件处理

        private void IMTools_OnSocketConnected(object sender, EventArgs e)
        {
            this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
            {
                BalloonTip = new BalloonTipInfo()
                {
                    Icon = BalloonIcon.Info,
                    Content = "已经连接IM服务器"
                }
            });

            IMTools.Login(this.imUser, (arg1, arg2) =>
            {
                this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
                {
                    BalloonTip = new BalloonTipInfo()
                    {
                        Icon = arg1 == 0 ? BalloonIcon.Info : BalloonIcon.Error,
                        Title = "IM服务器登录",
                        Content = arg2
                    }
                });
            });
        }

        private void IMTools_OnSocketDisconnected(object sender, EventArgs e)
        {
            this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
            {
                BalloonTip = new BalloonTipInfo()
                {
                    Icon = BalloonIcon.Error,
                    Content = "已经断开IM服务器连接"
                }
            });
        }

        private void IMTools_OnSocketError(object sender, string e)
        {
            this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
            {
                BalloonTip = new BalloonTipInfo()
                {
                    Icon = BalloonIcon.Error,
                    Title = "IM服务器异常",
                    Content = e
                }
            });
        }

        private void IMTools_OnSocketReconnectFailed(object sender, int e)
        {
            this.eventAggregator.PublishAsync<ShowBalloonTipEventArgs>(new ShowBalloonTipEventArgs()
            {
                BalloonTip = new BalloonTipInfo()
                {
                    Icon = BalloonIcon.Error,
                    Title = "IM服务器重连失败",
                    Content = string.Format("重连次数:{0}", e)
                }
            });
        }

        private void IMTools_OnReceiveMessage(object sender, MessageModel e)
        {
            HandleRecvMessage(e);
        }
        #endregion
    }
}
