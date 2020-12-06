using EMChat2.Model.Api;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.IM;
using IEMWorks.Common;
using IM.SDK;
using IM.SDK.Model;
using QinSoft.Event;
using QinSoft.Log;
using QinSoft.Log.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace EMChat2.Common
{
    public static class IMTools
    {
        private static SimpleSocketClient socketClient;
        private static IMServerModel imServer;
        private static IMUserModel imUser;
        private static bool isReconnecting;

        #region 对外事件
        public static event EventHandler OnSocketConnected;
        public static event EventHandler OnSocketDisconnected;
        public static event EventHandler<string> OnSocketError;
        public static event EventHandler<int> OnSocketReconnectFailed;
        public static event EventHandler OnLoginSuccess;
        public static event EventHandler<string> OnLoginFailed;
        public static event EventHandler<MessageModel> OnReceiveMessage;
        public static event EventHandler<string> OnLog;
        #endregion

        /// <summary>
        /// 开始IM服务
        /// </summary>
        /// <param name="server">im服务</param>
        public static void Start(IMServerModel server, IMUserModel user)
        {
            if (socketClient != null) return;
            imServer = server;
            imUser = user;

            isReconnecting = false;

            socketClient = new SimpleSocketClient(imServer.ApiUrl, true);
            socketClient.OnSocketConnected += SocketClient_OnSocketConnected;
            socketClient.OnSocketDisconnected += SocketClient_OnSocketDisconnected;
            socketClient.OnSocketError += SocketClient_OnSocketError;
            socketClient.ReconnectFailed += SocketClient_ReconnectFailed;
            socketClient.OnReceivePrivateMessage += SocketClient_OnReceivePrivateMessage;
            socketClient.Log = (msg) =>
            {
                OnLog?.Invoke(socketClient, msg);
            };
            socketClient.Start(imServer.IP, imServer.Port);
        }

        /// <summary>
        /// 停止IM服务
        /// </summary>
        public static void Stop()
        {
            if (socketClient == null) return;
            isReconnecting = false;
            socketClient.Stop();
            socketClient.OnSocketConnected -= SocketClient_OnSocketConnected;
            socketClient.OnSocketDisconnected -= SocketClient_OnSocketDisconnected;
            socketClient.OnSocketError -= SocketClient_OnSocketError;
            socketClient.ReconnectFailed -= SocketClient_ReconnectFailed;
            socketClient.OnReceivePrivateMessage -= SocketClient_OnReceivePrivateMessage;
            socketClient = null;
            imUser = null;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>发送结果</returns>
        public static bool Send(MessageModel message)
        {
            int result = 0;
            MessageApiModel sendMessage = message.MessageToApiModel();
            string messageString = sendMessage.ObjectToJson();

            Semaphore semaphore = new Semaphore(0, message.ToUsers.Count());
            foreach (string toUser in message.ToUsers)
            {
                socketClient.SendToUser(message.FromUser, toUser, messageString, 0, (code, msg) =>
                {
                    result += code;
                    semaphore.Release();
                });
            }

            foreach (string toUser in message.ToUsers)
            {
                semaphore.WaitOne();
            }
            return result == 0;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>上传结果</returns>
        public static IMFileInfo UploadFile(FileInfo file, out string message)
        {
            string content = FileServerClient.Current.UploadFile(file.FullName, imServer.ApiUrl, imUser.Id, imUser.Token);
            IMUploadResponse<IMFileInfo> response = content.JsonToObject<IMUploadResponse<IMFileInfo>>();
            message = response.Message;
            return response.Data;
        }

        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="file">图片文件</param>
        /// <returns>上传结果</returns>
        public static IMImageInfo UploadImage(FileInfo file, out string message)
        {
            string content = FileServerClient.Current.UploadImage(file.FullName, imServer.ApiUrl, imUser.Id, imUser.Token);
            IMUploadResponse<IMImageInfo> response = content.JsonToObject<IMUploadResponse<IMImageInfo>>();
            message = response.Message;
            return response.Data;
        }

        /// <summary>
        /// 登录IM服务
        /// </summary>
        /// <param name="user">im用户</param>
        /// <param name="callback">回调</param>
        private static void Login(Action<int, string> callback)
        {
            if (socketClient == null) return;
            socketClient.Login(new IM_LoginToken()
            {
                Appkey = imUser.AppKey,
                UserID = imUser.Id,
                Token = imUser.Token,
                Version = AppTools.AppVersion.Major,
                Device = AppTools.AppFullName
            }, callback);
        }

        /// <summary>
        /// 重连后刷新token
        /// </summary>
        private static void RefreshToken()
        {
            new Action(() =>
            {
                HttpAPIClient apiClient = new HttpAPIClient(imServer.ApiUrl, imUser.Id, imUser.Token, null);
                JsonResult<IMToken> result = apiClient.RefreshToken(imUser.RefreshToken, imUser.Token);
                if (result.data != null)
                {
                    imUser.Token = result.data.Token;
                    imUser.RefreshToken = result.data.RefreshToken;
                }
                else
                {
                    throw new Exception("refresh token failed");
                }
            }).Retry();
        }

        #region 私有方法
        /// <summary>
        /// socket连接成功事件处理
        /// </summary>
        /// <param name="client"></param>
        private static void SocketClient_OnSocketConnected(SimpleSocketClient client)
        {
            OnSocketConnected.Invoke(client, null);
            if (isReconnecting)
            {
                RefreshToken();
                isReconnecting = false;
            }
            Login((arg1, arg2) =>
            {
                if (arg1 == 0) OnLoginSuccess.Invoke(client, null);
                else OnLoginFailed?.Invoke(client, arg2);
            });
        }

        /// <summary>
        /// socket异常断开连接事件处理
        /// </summary>
        /// <param name="client"></param>
        private static void SocketClient_OnSocketDisconnected(SimpleSocketClient client)
        {
            isReconnecting = true;
            OnSocketDisconnected?.Invoke(client, null);
        }

        /// <summary>
        /// 重连失败事件处理
        /// </summary>
        /// <param name="client"></param>
        /// <param name="count"></param>
        private static void SocketClient_ReconnectFailed(SimpleSocketClient client, int count)
        {
            OnSocketReconnectFailed?.Invoke(client, count);
        }

        private static void SocketClient_OnSocketError(SimpleSocketClient client, string error)
        {
            OnSocketError?.Invoke(client, error);
        }
        private static void SocketClient_OnReceivePrivateMessage(SimpleSocketClient client, IEnumerable<IM_ReceivePrivateMessage> messages)
        {
            foreach (IM_ReceivePrivateMessage privateMessage in messages)
            {
                MessageApiModel recvMessage = privateMessage.Content.JsonToObject<MessageApiModel>();
                MessageModel message = recvMessage.MessageToModel();
                OnReceiveMessage?.Invoke(client, message);
                socketClient.SendPrivateMessageReceipt(privateMessage.MsgID, privateMessage.SenderID, IM_ReceiptType.Receive, null);
            }
        }
        #endregion
    }
}
