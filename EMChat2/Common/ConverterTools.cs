﻿using EMChat2.Model.Api;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common
{
    public static class ConverterTools
    {
        public static UserModel CreateUser(string userId, UserTypeEnum userType)
        {
            switch (userType)
            {
                case UserTypeEnum.Customer: return new CustomerModel() { Id = userId };
                case UserTypeEnum.Staff: return new StaffModel() { Id = userId };
                case UserTypeEnum.SystemUser: return new SystemUserModel() { Id = userId };
                default: return null;
            }

        }

        public static MessageApiModel MessageToApiModel(this MessageModel message)
        {
            if (message == null) return null;
            MessageApiModel apiMessage = new MessageApiModel();
            apiMessage.Id = message.Id;
            apiMessage.Time = message.Time;
            apiMessage.ChatId = message.ChatId;
            apiMessage.FromUserId = message.FromUser.Id;
            apiMessage.FromUserType = message.FromUser.Type;
            apiMessage.ToUsers = message.ToUsers.Select(u => new MessageReceiverApiModel()
            {
                MessageId = message.Id,
                ToUserId = u.Id,
                ToUserType = u.Type
            });
            apiMessage.State = message.State;
            apiMessage.Type = message.Type;
            apiMessage.Content = message.ContentString;
            return apiMessage;
        }

        public static MessageModel MessageToModel(this MessageApiModel apiMessage)
        {
            if (apiMessage == null) return null;
            MessageModel message = new MessageModel();
            message.Id = apiMessage.Id;
            message.Time = apiMessage.Time;
            message.ChatId = apiMessage.ChatId;
            message.FromUser = CreateUser(apiMessage.FromUserId, apiMessage.FromUserType);
            message.ToUsers = null;
            message.State = apiMessage.State;
            message.Type = apiMessage.Type;
            message.ContentString = apiMessage.Content;
            return message;
        }
    }
}
