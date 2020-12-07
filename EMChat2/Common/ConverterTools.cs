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
            message.Content = sendMessage.ContentString;
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
            message.ContentString = recvMessage.Content;
            return message;
        }
    }
}
