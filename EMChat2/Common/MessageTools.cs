using EMChat2.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common
{
    /// <summary>
    /// 消息工具类
    /// </summary>
    public static class MessageTools
    {
        public static object ParseMessageContent(string messageType, string messageContent)
        {
            if (string.IsNullOrEmpty(messageContent) || string.IsNullOrEmpty(messageType)) return null;
            switch (messageType)
            {
                case MessageTypeConst.Text: return messageContent.JsonToObject<TextMessageContent>();
                case MessageTypeConst.Emotion: return messageContent.JsonToObject<EmotionMessageContent>();
                case MessageTypeConst.Image: return messageContent.JsonToObject<ImageMessageContent>();
                case MessageTypeConst.Voice: return null;
                case MessageTypeConst.Video: return null;
                case MessageTypeConst.Link: return messageContent.JsonToObject<LinkMessageContent>();
                case MessageTypeConst.File: return messageContent.JsonToObject<FileMessageContent>();
                case MessageTypeConst.Mixed: return messageContent.JsonToObject<MixedMessageContent>();
                case MessageTypeConst.Tips: return messageContent.JsonToObject<TipsMessageContent>();
                case MessageTypeConst.Event:
                    {
                        EventMessageContentBase eventMessageContentBase = messageContent.JsonToObject<EventMessageContentBase>();
                        //TODO 针对不同事件反序列化不同事件对象
                        return eventMessageContentBase;
                    }
                default: return null;
            }
        }

        public static string GetMessageContentMark(string messageType, string messageContent)
        {
            if (string.IsNullOrEmpty(messageContent) || string.IsNullOrEmpty(messageType)) return null;
            switch (messageType)
            {
                case MessageTypeConst.Text: return messageContent.JsonToObject<TextMessageContent>().Content.Replace(Environment.NewLine, string.Empty);
                case MessageTypeConst.Emotion: return string.Format("[表情-{0}]", messageContent.JsonToObject<EmotionMessageContent>().Name);
                case MessageTypeConst.Image: return string.Format("[图片]");
                case MessageTypeConst.Voice: return null;
                case MessageTypeConst.Video: return null;
                case MessageTypeConst.Link: return string.Format("[链接-{0}]", messageContent.JsonToObject<LinkMessageContent>().Title);
                case MessageTypeConst.File: return string.Format("[文件-{0}]", messageContent.JsonToObject<FileMessageContent>().Name);
                case MessageTypeConst.Mixed: return string.Join("", messageContent.JsonToObject<MixedMessageContent>().Items.Select(u => GetMessageContentMark(u.Type, u.Content)));
                case MessageTypeConst.Tips: return string.Format("[提示-{0}]", messageContent.JsonToObject<TipsMessageContent>().Content);
                case MessageTypeConst.Event: return string.Format("[事件-{0}]", messageContent.JsonToObject<EventMessageContentBase>().Event);
                default: return null;
            }
        }
    }
}
