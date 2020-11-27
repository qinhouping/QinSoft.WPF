using CefSharp;
using EMChat2.Model.BaseInfo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common.Cef
{
    public class ChatInfoCefJsObject : CefJsObject
    {
        #region 构造函数
        public ChatInfoCefJsObject(StaffInfo staff, ChatInfo chat) : base("chatInfo")
        {
            this.staff = staff;
            this.chat = chat;
            token = Guid.NewGuid().ToString();
        }

        public ChatInfoCefJsObject(StaffInfo staff, ChatInfo chat, string token) : base("chatInfo")
        {
            this.staff = staff;
            this.chat = chat;
            this.token = token;
        }
        #endregion

        #region 属性
        private ChatInfo chat;
        private StaffInfo staff;
        private string token;
        #endregion

        #region 方法
        public void GetStaff(string token, IJavascriptCallback callback)
        {
            if (token != this.token) callback.ExecuteAsync(new ActionResponse<StaffInfo>() { Code = 500, ErrorMessage = "Token认证失败", Data = null }.ObjectToJson());
            else callback.ExecuteAsync(new ActionResponse<StaffInfo>() { Code = 200, ErrorMessage = null, Data = this.staff }.ObjectToJson());
        }

        public void GetChat(string token, IJavascriptCallback callback)
        {
            if (token != this.token) callback.ExecuteAsync(new ActionResponse<ChatInfo>() { Code = 500, ErrorMessage = "Token认证失败", Data = null }.ObjectToJson());
            else callback.ExecuteAsync(new ActionResponse<ChatInfo>() { Code = 200, ErrorMessage = null, Data = this.chat }.ObjectToJson());
        }

        public string AppendToken(string url, string tokenName = "token")
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentException("url");
            if (string.IsNullOrEmpty(tokenName)) throw new ArgumentException("tokenName");
            string flag = "&";
            if (!url.Contains("?"))
            {
                flag = "?";
            }
            int index = url.IndexOf("#");
            if (index == -1) index = url.Length;
            url = string.Format("{0}{1}{2}", url.Substring(0, index), string.Format("{0}{1}={2}", flag, tokenName, this.token), url.Substring(index, url.Length - index));
            return url;
        }
        #endregion
    }

    class ActionResponse<T>
    {
        public int Code;

        public string ErrorMessage;

        public T Data;
    }
}
