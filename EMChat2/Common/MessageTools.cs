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
        public static object ParseMessageContent(MessageContentInfo messageContent)
        {
            if (messageContent == null) return null;
            switch (messageContent.Type)
            {
                case MessageTypeConst.Text: return messageContent.Content.JsonToObject<TextMessageContent>();
                case MessageTypeConst.Emotion: return messageContent.Content.JsonToObject<EmotionMessageContent>();
                case MessageTypeConst.Image: return messageContent.Content.JsonToObject<ImageMessageContent>();
                case MessageTypeConst.Voice: return null;
                case MessageTypeConst.Video: return null;
                case MessageTypeConst.Link: return messageContent.Content.JsonToObject<LinkMessageContent>();
                case MessageTypeConst.File: return messageContent.Content.JsonToObject<FileMessageContent>();
                case MessageTypeConst.Mixed: return messageContent.Content.JsonToObject<MixedMessageContent>();
                case MessageTypeConst.Tips: return messageContent.Content.JsonToObject<TipsMessageContent>();
                case MessageTypeConst.Event:
                    {
                        EventMessageContentBase eventMessageContentBase = messageContent.Content.JsonToObject<EventMessageContentBase>();
                        //TODO 针对不同事件反序列化不同事件对象
                        return eventMessageContentBase;
                    }
                default: return null;
            }
        }

        public static string GetMessageContentMark(MessageContentInfo messageContent)
        {
            if (messageContent == null) return null;
            switch (messageContent.Type)
            {
                case MessageTypeConst.Text: return messageContent.Content.JsonToObject<TextMessageContent>().Content.Replace(Environment.NewLine, string.Empty);
                case MessageTypeConst.Emotion: return string.Format("[表情-{0}]", messageContent.Content.JsonToObject<EmotionMessageContent>().Name);
                case MessageTypeConst.Image: return string.Format("[图片]");
                case MessageTypeConst.Voice: return null;
                case MessageTypeConst.Video: return null;
                case MessageTypeConst.Link: return string.Format("[链接-{0}]", messageContent.Content.JsonToObject<LinkMessageContent>().Title);
                case MessageTypeConst.File: return string.Format("[文件-{0}]", messageContent.Content.JsonToObject<FileMessageContent>().Name);
                case MessageTypeConst.Mixed: return string.Join("", messageContent.Content.JsonToObject<MixedMessageContent>().Items.Select(u => GetMessageContentMark(u)));
                case MessageTypeConst.Tips: return string.Format("[提示-{0}]", messageContent.Content.JsonToObject<TipsMessageContent>().Content);
                case MessageTypeConst.Event: return string.Format("[事件-{0}]", messageContent.Content.JsonToObject<EventMessageContentBase>().Event);
                default: return null;
            }
        }

        public static MessageContentInfo CreateEmotionMessageContent(EmotionInfo emotion)
        {
            if (emotion == null) return null;
            return new MessageContentInfo()
            {
                Type = MessageTypeConst.Emotion,
                Content = new EmotionMessageContent()
                {
                    Url = emotion.Url,
                    Name = emotion.Name,
                    IsGif = emotion.IsGif
                }.ObjectToJson()
            };
        }

        public static MessageContentInfo CreateImageMessageContent(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            return new MessageContentInfo()
            {
                Type = MessageTypeConst.Image,
                Content = new ImageMessageContent()
                {
                    Url = url
                }.ObjectToJson()
            };
        }
    }
}
