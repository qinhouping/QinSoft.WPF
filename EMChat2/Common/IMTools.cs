using EMChat2.Model.Api;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.IM;
using IM.SDK;
using IM.SDK.Model;
using QinSoft.Event;
using QinSoft.Log;
using QinSoft.Log.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        #region 对外事件
        public static event EventHandler<MessageModel> OnReceiveMessage;
        public static event EventHandler OnSocketConnected;
        public static event EventHandler OnSocketDisconnected;
        public static event EventHandler<int> OnSocketReconnectFailed;
        public static event EventHandler<string> OnSocketError;
        #endregion

        /// <summary>
        /// 开始IM服务
        /// </summary>
        /// <param name="server">im服务</param>
        public static void Start(IMServerModel server)
        {
            if (socketClient != null) return;
            imServer = server;
            socketClient = new SimpleSocketClient(imServer.ApiUrl);
            socketClient.ReconnectFailed += SocketClient_ReconnectFailed;
            socketClient.OnSocketConnected += SocketClient_OnSocketConnected;
            socketClient.OnSocketDisconnected += SocketClient_OnSocketDisconnected;
            socketClient.OnSocketError += SocketClient_OnSocketError;
            socketClient.OnReceivePrivateMessage += SocketClient_OnReceivePrivateMessage;
            socketClient.Start(imServer.IP, imServer.Port);
        }

        /// <summary>
        /// 停止IM服务
        /// </summary>
        public static void Stop()
        {
            if (socketClient == null) return;
            socketClient.Stop();
            socketClient = null;
            imUser = null;
        }

        /// <summary>
        /// 登录IM服务
        /// </summary>
        /// <param name="user">im用户</param>
        /// <param name="callback">回调</param>
        public static void Login(IMUserModel user, Action<int, string> callback)
        {
            if (socketClient == null) return;
            imUser = user;
            socketClient.Login(new IM_LoginToken()
            {
                Appkey = user.AppKey,
                UserID = user.Id,
                Token = user.Token,
                Version = AppTools.AppVersion.Major,
                Device = AppTools.AppName
            }, callback);
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
        public static IMFileInfo UploadFile(FileInfo file)
        {
            string content = FileServerClient.Current.UploadFile(file.FullName, imServer.ApiUrl, imUser.Id, imUser.Token);
            IMUploadResponse<IMFileInfo> response = content.JsonToObject<IMUploadResponse<IMFileInfo>>();
            return response.Data;
        }

        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="file">图片文件</param>
        /// <returns>上传结果</returns>
        public static IMImageInfo UploadImage(FileInfo file)
        {
            string content = FileServerClient.Current.UploadImage(file.FullName, imServer.ApiUrl, imUser.Id, imUser.Token);
            IMUploadResponse<IMImageInfo> response = content.JsonToObject<IMUploadResponse<IMImageInfo>>();
            return response.Data;
        }

        #region 私有方法
        private static void SocketClient_OnSocketConnected(SimpleSocketClient client)
        {
            OnSocketConnected.Invoke(client, null);
        }

        private static void SocketClient_OnSocketDisconnected(SimpleSocketClient client)
        {
            OnSocketDisconnected?.Invoke(client, null);
        }

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
