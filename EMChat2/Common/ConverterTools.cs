using EMChat2.Model.Api;
using EMChat2.Model.BaseInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common
{
    public static class ConverterTools
    {
        public static MessageContentApiModel MessageContentToApiModel(this MessageContentModel sendMessageContent)
        {
            MessageContentApiModel messageContent = new MessageContentApiModel();
            messageContent.Type = sendMessageContent.Type;
            if (sendMessageContent.Type != MessageTypeConst.Mixed)
            {
                messageContent.Content = sendMessageContent.Content.ObjectToJson();
            }
            else
            {
                List<MessageContentApiModel> items = new List<MessageContentApiModel>();
                foreach (MessageContentModel item in (sendMessageContent.Content as MixedMessageContent).Items)
                {
                    items.Add(item.MessageContentToApiModel());
                }
                messageContent.Content = new MixedMessageContentApiModel() { Items = items.ToArray() }.ObjectToJson();
            }
            return messageContent;
        }

        public static MessageContentModel MessageContentToModel(this MessageContentApiModel recvMessageContent)
        {
            MessageContentModel messageContent = new MessageContentModel();
            messageContent.Type = recvMessageContent.Type;
            switch (recvMessageContent.Type)
            {
                case MessageTypeConst.Text: messageContent.Content = recvMessageContent.Content.JsonToObject<TextMessageContent>(); break;
                case MessageTypeConst.Emotion: messageContent.Content = recvMessageContent.Content.JsonToObject<EmotionMessageContent>(); break;
                case MessageTypeConst.Image: messageContent.Content = recvMessageContent.Content.JsonToObject<ImageMessageContent>(); break;
                case MessageTypeConst.Voice: break;
                case MessageTypeConst.Video: break;
                case MessageTypeConst.Link: messageContent.Content = recvMessageContent.Content.JsonToObject<LinkMessageContent>(); break;
                case MessageTypeConst.File: messageContent.Content = recvMessageContent.Content.JsonToObject<FileMessageContent>(); break;
                case MessageTypeConst.Mixed:
                    {
                        MixedMessageContentApiModel mixedMessageContent = recvMessageContent.Content.JsonToObject<MixedMessageContentApiModel>();
                        List<MessageContentModel> items = new List<MessageContentModel>();
                        foreach (MessageContentApiModel item in mixedMessageContent.Items)
                        {
                            items.Add(item.MessageContentToModel());
                        }
                        messageContent.Content = MessageTools.CreateMixedMessageContent(items.ToArray()).Content;
                    }
                    break;
                case MessageTypeConst.Tips: messageContent.Content = recvMessageContent.Content.JsonToObject<TipsMessageContent>(); break;
                case MessageTypeConst.Event:
                    {
                        EventMessageContent eventMessageContent = recvMessageContent.Content.JsonToObject<EventMessageContent>();
                        switch (eventMessageContent.Event)
                        {
                            case EventMessageTypeConst.RecvMessage: messageContent.Content = recvMessageContent.Content.JsonToObject<RecvMessageEventMessageContent>(); break;
                            case EventMessageTypeConst.ReadMessage: messageContent.Content = recvMessageContent.Content.JsonToObject<ReadMessageEventMessageContent>(); break;
                            case EventMessageTypeConst.RefuseMessage: messageContent.Content = recvMessageContent.Content.JsonToObject<RefuseMessageEventMessageContent>(); break;
                            case EventMessageTypeConst.RevokeMessage: messageContent.Content = recvMessageContent.Content.JsonToObject<RevokeMessageEventMessageContent>(); break;
                        }
                    }
                    break;
            }
            return messageContent;
        }

        public static MessageApiModel MessageToApiModel(this MessageModel sendMessage)
        {
            MessageApiModel message = new MessageApiModel();
            message.Id = sendMessage.Id;
            message.Time = sendMessage.Time;
            message.ChatId = sendMessage.ChatId;
            message.FromUser = sendMessage.FromUser;
            message.ToUsers = sendMessage.ToUsers;
            message.State = (int)sendMessage.State;
            message.Type = sendMessage.Type;
            message.Content = sendMessage.MessageContentToApiModel().Content;
            return message;
        }

        public static MessageModel MessageToModel(this MessageApiModel recvMessage)
        {
            MessageModel message = new MessageModel();
            message.Id = recvMessage.Id;
            message.Time = recvMessage.Time;
            message.ChatId = recvMessage.ChatId;
            message.FromUser = recvMessage.FromUser;
            message.ToUsers = recvMessage.ToUsers;
            message.State = (MessageStateEnum)recvMessage.State;
            message.Type = recvMessage.Type;
            message.Content = recvMessage.MessageContentToModel().Content;
            return message;
        }
    }
}
