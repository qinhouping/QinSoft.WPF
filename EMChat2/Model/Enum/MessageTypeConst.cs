using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Model.Enum
{

    /// <summary>
    /// 消息类型常量
    /// </summary>
    public static class MessageTypeConst
    {
        #region 可见消息同时入库
        public const string Text = "text";
        public const string Emotion = "emotion";
        public const string Image = "image";
        public const string Voice = "voice";
        public const string Video = "video";
        public const string Link = "link";
        public const string File = "file";
        public const string Mixed = "mixed";
        public const string Shared = "shared";
        #endregion

        #region 可见消息但是不入库
        public const string Tips = "tips";
        #endregion 

        #region 不可见消息同时不入库
        public const string Event = "event";
        #endregion

        public static string[] AllowSavedMessageTypes = new string[] { Text, Emotion, Image, Voice, Video, Link, File, Mixed, Shared };
    }

    /// <summary>
    /// 事件消息类型常量
    /// </summary>
    public static class EventMessageTypeConst
    {
        public const string RecvMessage = "recv_message";
        public const string ReadMessage = "read_message";
        public const string RefuseMessage = "refuse_message";
        public const string RevokeMessage = "revoke_message";
        public const string InputMessage = "input_message";
    }
}
