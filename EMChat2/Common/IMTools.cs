using EMChat2.Model.BaseInfo;
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
        private static IMServerInfo imServerInfo;
        private static IMUserInfo imUserInfo;

        #region 对外事件
        public static event EventHandler<MessageInfo> OnReceiveMessage;
        public static event EventHandler OnSocketConnected;
        public static event EventHandler OnSocketDisconnected;
        public static event EventHandler<int> OnSocketReconnectFailed;
        public static event EventHandler<string> OnSocketError;
        #endregion

        public static void Start(IMServerInfo serverInfo)
        {
            if (socketClient != null) return;
            imServerInfo = serverInfo;
            socketClient = new SimpleSocketClient(imServerInfo.ApiUrl);
            socketClient.ReconnectFailed += SocketClient_ReconnectFailed;
            socketClient.OnSocketConnected += SocketClient_OnSocketConnected;
            socketClient.OnSocketDisconnected += SocketClient_OnSocketDisconnected;
            socketClient.OnSocketError += SocketClient_OnSocketError;
            socketClient.OnReceivePrivateMessage += SocketClient_OnReceivePrivateMessage;
            socketClient.Start(imServerInfo.IP, imServerInfo.Port);
        }

        public static void Stop()
        {
            if (socketClient == null) return;
            socketClient.Stop();
            socketClient = null;
            imUserInfo = null;
        }

        public static void Login(IMUserInfo userInfo, Action<int, string> callback)
        {
            if (socketClient == null) return;
            imUserInfo = userInfo;
            socketClient.Login(new IM_LoginToken()
            {
                Appkey = userInfo.AppKey,
                UserID = userInfo.Id,
                Token = userInfo.Token,
                Version = 2,
                Device = AppTools.AppName
            }, callback);
        }

        public static bool Send(MessageInfo message)
        {
            int result = 0;
            int count = 0;
            foreach (string toUser in message.ToUsers)
            {
                socketClient.SendToUser(message.FromUser, toUser, message.ObjectToJson(), 0, (arg1, arg2) =>
                {
                    count++;
                    result += arg1;
                });
            }
            while (count < message.ToUsers.Count())
            {
                Thread.Sleep(50);
            }
            return result == 0;
        }

        public static UrlEntity Upload(FileInfo file)
        {
            return FileServerClient.Current.UploadFile(file.FullName, imServerInfo.ApiUrl, imUserInfo.Id, imUserInfo.Token).JsonToObject<UrlEntity>();
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
            foreach (IM_ReceivePrivateMessage message in messages)
            {
                OnReceiveMessage?.Invoke(client, message.Content.JsonToObject<MessageInfo>());
                socketClient.SendPrivateMessageReceipt(message.MsgID, message.SenderID, IM_ReceiptType.Receive, null);
            }
        }
        #endregion
    }

    public class UrlEntity
    {
        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string ThumbUrl { get; set; }

        /// <summary>
        /// MD5
        /// </summary>
        public string MD5 { get; set; }
    }
}
