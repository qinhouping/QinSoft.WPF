﻿using EMChat2.Model.Entity;
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
        public static object ParseMessageContent(string messageContent, string messageType)
        {
            if (string.IsNullOrEmpty(messageContent) || string.IsNullOrEmpty(messageType)) return null;
            switch (messageType)
            {
                case MessageTypeConst.Text: return messageContent.JsonToObject<TextMessageContent>();
                case MessageTypeConst.Emotion: return messageContent.JsonToObject<EmotionMessageContent>();
                case MessageTypeConst.Image: return messageContent.JsonToObject<ImageMessageContent>();
                case MessageTypeConst.Voice: return null;
                case MessageTypeConst.Video: return null;
                case MessageTypeConst.Revoke: return null;
                case MessageTypeConst.Link: return messageContent.JsonToObject<LinkMessageContent>();
                case MessageTypeConst.File: return messageContent.JsonToObject<FileMessageContent>();
                case MessageTypeConst.Mixed: return messageContent.JsonToObject<MixedMessageContent>();
                default: return null;
            }
        }

        public static string GetMessageContentMark(string messageContent, string messageType)
        {
            if (string.IsNullOrEmpty(messageContent) || string.IsNullOrEmpty(messageType)) return null;
            switch (messageType)
            {
                case MessageTypeConst.Text: return messageContent.JsonToObject<TextMessageContent>().Content;
                case MessageTypeConst.Emotion: return string.Format("[表情-{0}]", messageContent.JsonToObject<EmotionMessageContent>().Name);
                case MessageTypeConst.Image: return string.Format("[图片]");
                case MessageTypeConst.Voice: return null;
                case MessageTypeConst.Video: return null;
                case MessageTypeConst.Revoke: return null;
                case MessageTypeConst.Link: return string.Format("[链接-{0}]", messageContent.JsonToObject<LinkMessageContent>().Title);
                case MessageTypeConst.File: return string.Format("[文件-{0}]", messageContent.JsonToObject<FileMessageContent>().Name);
                case MessageTypeConst.Mixed: return string.Join("", messageContent.JsonToObject<MixedMessageContent>().Items.Select(u => GetMessageContentMark(u.Content, u.Type)));
                default: return null;
            }
        }
    }
}