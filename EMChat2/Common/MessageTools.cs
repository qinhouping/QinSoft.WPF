﻿using EMChat2.Model.BaseInfo;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EMChat2.Common
{
    /// <summary>
    /// 消息工具类
    /// </summary>
    public static class MessageTools
    {

        public static string GetMessageContentMark(MessageContentModel messageContent)
        {
            if (messageContent == null) return null;
            switch (messageContent.Type)
            {

                case MessageTypeConst.Text: return Regex.Replace((messageContent.Content as TextMessageContent).Content, @"\s", string.Empty);
                case MessageTypeConst.Emotion: return string.Format("[表情-{0}]", (messageContent.Content as EmotionMessageContent).Name);
                case MessageTypeConst.Image: return string.Format("[图片]");
                case MessageTypeConst.Voice: return null;
                case MessageTypeConst.Video: return null;
                case MessageTypeConst.Link: return string.Format("[链接-{0}]", (messageContent.Content as LinkMessageContent).Title);
                case MessageTypeConst.File: return string.Format("[文件-{0}]", (messageContent.Content as FileMessageContent).Name);
                case MessageTypeConst.Mixed: return string.Join("", (messageContent.Content as MixedMessageContent).Items.Select(u => GetMessageContentMark(u)));
                case MessageTypeConst.Tips: return string.Format("[提示-{0}]", (messageContent.Content as TipsMessageContent).Content);
                case MessageTypeConst.Event: return string.Format("[事件-{0}]", (messageContent.Content as EventMessageContent).Event);
                default: return null;
            }
        }


        public static MessageContentModel CreateTextMessageContent(string content)
        {
            if (string.IsNullOrEmpty(content)) return null;
            return new MessageContentModel()
            {
                Type = MessageTypeConst.Text,
                Content = new TextMessageContent()
                {
                    Content = content
                }
            };
        }

        public static MessageContentModel CreateHtmlMessageContent(string html)
        {
            if (string.IsNullOrEmpty(html)) return null;
            List<MessageContentModel> messageContents = new List<MessageContentModel>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNode rootNode = htmlDocument.DocumentNode;
            HtmlNodeCollection imageHtmlNodes = rootNode.SelectNodes("//img");
            if (imageHtmlNodes != null)
            {
                foreach (HtmlNode imageHtmlNode in imageHtmlNodes)
                {
                    string url = imageHtmlNode.Attributes["src"]?.Value;
                    if (string.IsNullOrEmpty(url) || !url.IsNetUrl()) continue;
                    messageContents.Add(new MessageContentModel()
                    {
                        Type = MessageTypeConst.Image,
                        Content = new ImageMessageContent()
                        {
                            Url = url
                        }
                    });
                }
            }
            if (!string.IsNullOrEmpty(rootNode.InnerText))
            {
                messageContents.Add(new MessageContentModel()
                {
                    Type = MessageTypeConst.Text,
                    Content = new TextMessageContent()
                    {
                        Content = rootNode.InnerText
                    }
                });
            }
            return new MessageContentModel()
            {
                Type = MessageTypeConst.Mixed,
                Content = new MixedMessageContent()
                {
                    Items = messageContents.ToArray()
                }
            };
        }

        public static MessageContentModel CreateEmotionMessageContent(EmotionModel emotion)
        {
            if (emotion == null) return null;
            return new MessageContentModel()
            {
                Type = MessageTypeConst.Emotion,
                Content = new EmotionMessageContent()
                {
                    Url = emotion.Url,
                    Name = emotion.Name,
                    IsGif = emotion.IsGif
                }
            };
        }

        public static MessageContentModel CreateImageMessageContent(FileInfo file)
        {
            if (file == null) return null;
            return new MessageContentModel()
            {
                Type = MessageTypeConst.Image,
                Content = new ImageMessageContent()
                {
                    Url = file.FullName
                }
            };
        }

        public static MessageContentModel CreateLinkMessageContent(Uri url, Uri thumbUrl, string title, string description = null)
        {
            return new MessageContentModel()
            {
                Type = MessageTypeConst.Link,
                Content = new LinkMessageContent()
                {
                    Url = url.AbsoluteUri,
                    ThumbUrl = thumbUrl.AbsoluteUri,
                    Title = title,
                    Description = description
                }
            };
        }

        public static MessageContentModel CreateFileMessageContent(FileInfo file)
        {
            if (file == null) return null;
            return new MessageContentModel()
            {
                Type = MessageTypeConst.File,
                Content = new FileMessageContent()
                {
                    Url = file.FullName,
                    Name = file.Name,
                    Extension = file.Extension
                }
            };
        }

        public static MessageContentModel CreateMixedMessageContent(params MessageContentModel[] items)
        {
            if (items == null || items.Count() == 0) return null;
            return new MessageContentModel()
            {
                Type = MessageTypeConst.Mixed,
                Content = new MixedMessageContent()
                {
                    Items = items
                }
            };
        }

        public static MessageContentModel CreateRecvMessageEventMessageContent(MessageModel message)
        {
            if (message == null) return null;
            if (message.State != MessageStateEnum.Received) throw new ArgumentException("message state is invalid");
            return new MessageContentModel()
            {
                Type = MessageTypeConst.Event,
                Content = new RecvMessageEventMessageContent()
                {
                    Message = message
                }
            };
        }

        public static MessageContentModel CreateReadMessageEventMessageContent(MessageModel message)
        {
            if (message == null) return null;
            if (message.State != MessageStateEnum.Readed) throw new ArgumentException("message state is invalid");
            return new MessageContentModel()
            {
                Type = MessageTypeConst.Event,
                Content = new RecvMessageEventMessageContent()
                {
                    Message = message
                }
            };
        }

        public static MessageContentModel CreateRefuseMessageEventMessageContent(MessageModel message)
        {
            if (message == null) return null;
            if (message.State != MessageStateEnum.Refused) throw new ArgumentException("message state is invalid");
            return new MessageContentModel()
            {
                Type = MessageTypeConst.Event,
                Content = new RecvMessageEventMessageContent()
                {
                    Message = message
                }
            };
        }

        public static MessageContentModel CreateRevokeMessageEventMessageContent(MessageModel message)
        {
            if (message == null) return null;
            if (message.State != MessageStateEnum.Revoked) throw new ArgumentException("message state is invalid");
            return new MessageContentModel()
            {
                Type = MessageTypeConst.Event,
                Content = new RecvMessageEventMessageContent()
                {
                    Message = message
                }
            };
        }



        public static MessageModel CreateMessage(StaffModel staff, ChatModel chat, MessageContentModel messageContent, MessageStateEnum state = MessageStateEnum.Sending)
        {
            if (staff == null || chat == null || messageContent == null) return null;
            return new MessageModel()
            {
                Id = Guid.NewGuid().ToString(),
                ChatId = chat.Id,
                Time = DateTime.Now,
                FromUser = staff.ImUserId,
                ToUsers = chat.ChatUsers.Where(u => !staff.Equals(u)).Select(u => u.ImUserId).ToArray(),
                State = state,
                Type = messageContent.Type,
                Content = messageContent.Content
            };
        }
    }
}
