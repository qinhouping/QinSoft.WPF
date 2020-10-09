﻿using EMChat2.Model.BaseInfo;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static MessageContentInfo CreateTextMessageContent(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            return new MessageContentInfo()
            {
                Type = MessageTypeConst.Text,
                Content = new TextMessageContent()
                {
                    Content = content
                }.ObjectToJson()
            };
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

        public static MessageContentInfo CreateImageMessageContent(FileInfo file)
        {
            if (file == null) return null;
            return new MessageContentInfo()
            {
                Type = MessageTypeConst.Image,
                Content = new ImageMessageContent()
                {
                    Url = file.FullName
                }.ObjectToJson()
            };
        }

        public static MessageContentInfo CreateFileMessageContent(FileInfo file)
        {
            if (file == null) return null;
            return new MessageContentInfo()
            {
                Type = MessageTypeConst.File,
                Content = new FileMessageContent()
                {
                    Url = file.FullName,
                    Name = file.Name,
                    Extension = file.Extension
                }.ObjectToJson()
            };
        }

        public static MessageContentInfo CreateHtmlMessageContent(string html)
        {
            if (string.IsNullOrEmpty(html)) return null;
            List<MessageContentInfo> messageContents = new List<MessageContentInfo>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNode rootNode = htmlDocument.DocumentNode.SelectSingleNode("html");
            HtmlNodeCollection imageHtmlNodes = rootNode.SelectNodes("//img");
            if (imageHtmlNodes != null)
            {
                foreach (HtmlNode imageHtmlNode in imageHtmlNodes)
                {
                    messageContents.Add(new MessageContentInfo()
                    {
                        Type = MessageTypeConst.Image,
                        Content = new ImageMessageContent()
                        {
                            Url = imageHtmlNode.Attributes["src"]?.Value.Trim()
                        }.ObjectToJson()
                    });
                }
            }
            if (!string.IsNullOrEmpty(rootNode.InnerText))
            {
                messageContents.Add(new MessageContentInfo()
                {
                    Type = MessageTypeConst.Text,
                    Content = new TextMessageContent()
                    {
                        Content = rootNode.InnerText
                    }.ObjectToJson()
                });
            }
            return new MessageContentInfo()
            {
                Type = MessageTypeConst.Mixed,
                Content = new MixedMessageContent()
                {
                    Items = messageContents.ToArray()
                }.ObjectToJson()
            };
        }

        public static MessageInfo CreateMessage(StaffInfo staff, ChatInfo chat, MessageContentInfo messageContent, MessageStateEnum state = MessageStateEnum.Sending)
        {
            if (staff == null || chat == null || messageContent == null) return null;
            return new MessageInfo()
            {
                Id = Guid.NewGuid().ToString(),
                ChatId = chat.Id,
                MsgId = null,
                MsgTime = DateTime.Now,
                FromUser = staff.ImUserId,
                ToUsers = chat.ChatUsers.Select(u => u.ImUserId).ToArray(),
                State = state,
                Type = messageContent.Type,
                Content = messageContent.Content
            };
        }
    }
}
