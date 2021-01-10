using DotLiquid.Util;
using EMChat2.Model.Api;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Common
{
    public static class ConvertTools
    {
        public static UserModel Convert(string userId, UserTypeEnum userType)
        {
            switch (userType)
            {
                case UserTypeEnum.Customer: return new CustomerModel() { Id = userId };
                case UserTypeEnum.Staff: return new StaffModel() { Id = userId };
                case UserTypeEnum.SystemUser: return new SystemUserModel() { Id = userId };
                default: return null;
            }

        }

        public static MessageApiModel Convert(this MessageModel message)
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

        public static MessageModel Convert(this MessageApiModel apiMessage)
        {
            if (apiMessage == null) return null;
            MessageModel message = new MessageModel();
            message.Id = apiMessage.Id;
            message.Time = apiMessage.Time;
            message.ChatId = apiMessage.ChatId;
            message.FromUser = Convert(apiMessage.FromUserId, apiMessage.FromUserType);
            message.ToUsers = null;
            message.State = apiMessage.State;
            message.Type = apiMessage.Type;
            message.ContentString = apiMessage.Content;
            return message;
        }

        public static UpdateMessageApiModel Convert(string messageId, MessageStateEnum state)
        {
            if (string.IsNullOrEmpty(messageId)) return null;
            UpdateMessageApiModel apiUpdateMessage = new UpdateMessageApiModel();
            apiUpdateMessage.Id = messageId;
            apiUpdateMessage.State = state;
            return apiUpdateMessage;
        }

        public static StaffModel Convert(this StaffApiModel apiStaff)
        {
            if (apiStaff == null) return null;
            StaffModel staff = new StaffModel();
            staff.Id = apiStaff.Id;
            staff.ImUserId = apiStaff.ImUserId;
            staff.Name = apiStaff.Name;
            staff.HeaderImageUrl = apiStaff.HeaderImageUrl;
            staff.WorkCode = apiStaff.WorkCode;
            staff.Remark = null;
            staff.Description = null;
            staff.Sex = apiStaff.Sex;
            staff.Businesses = new ObservableCollection<BusinessModel>(apiStaff.UserBusinesses.Select(u => Convert(u.Business)));
            return staff;
        }

        public static BusinessModel Convert(this BusinessApiModel apiBusiness)
        {
            if (apiBusiness == null) return null;
            BusinessModel businessModel = new BusinessModel();
            businessModel.Id = apiBusiness.Id;
            businessModel.Name = apiBusiness.Name;
            businessModel.Description = apiBusiness.Description;
            businessModel.Outside = apiBusiness.Outside;
            return businessModel;
        }
    }
}
