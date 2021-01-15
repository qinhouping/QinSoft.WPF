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

        public static MessageContentModel Convert(string type, string content)
        {
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(content)) return null;
            MessageContentModel messageContent = new MessageContentModel();
            messageContent.Type = type;
            messageContent.ContentString = content;
            return messageContent;
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
            if (apiMessage.ToUsers != null)
                message.ToUsers = new ObservableCollection<UserModel>(apiMessage.ToUsers.Select(u => Convert(u.ToUserId, u.ToUserType)));
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
            staff.State = UserStateEnum.Online;
            staff.Businesses = new ObservableCollection<BusinessModel>(apiStaff.UserBusinesses.Select(u => Convert(u.Business)));
            return staff;
        }

        public static StaffModel Convert(this DepartmentStaffViewApiModel apiStaff)
        {
            if (apiStaff == null || apiStaff.Staff == null) return null;
            StaffModel staff = apiStaff.Staff.Convert();
            staff.BusinessId = apiStaff.BusinessId;
            if (apiStaff.Follow != null)
            {
                staff.FollowId = apiStaff.Follow?.Id;
                staff.Remark = apiStaff.Follow.Remark;
                staff.Description = apiStaff.Follow.Description;
            }
            return staff;
        }

        public static BusinessModel Convert(this BusinessApiModel apiBusiness)
        {
            if (apiBusiness == null) return null;
            BusinessModel business = new BusinessModel();
            business.Id = apiBusiness.Id;
            business.Name = apiBusiness.Name;
            business.Description = apiBusiness.Description;
            business.Outside = apiBusiness.Outside;
            return business;
        }

        public static DepartmentModel Convert(this DepartmentViewApiModel apiDepartment)
        {
            if (apiDepartment == null) return null;
            DepartmentModel department = new DepartmentModel();
            department.Id = apiDepartment.Id;
            department.Name = apiDepartment.Name;
            department.Departments = new ObservableCollection<DepartmentModel>(apiDepartment.Departments.Select(u => u.Convert()));
            department.Staffs = new ObservableCollection<StaffModel>(apiDepartment.DepartmentStaffs.Select(u => u.Convert()));
            return department;
        }

        public static QuickReplyModel Convert(this QuickReplyApiModel apiQuickReply)
        {
            if (apiQuickReply == null) return null;
            QuickReplyModel quickReply = new QuickReplyModel();
            quickReply.Id = apiQuickReply.Id;
            quickReply.Name = apiQuickReply.Name;
            quickReply.Content = Convert(apiQuickReply.Type, apiQuickReply.Content);
            return quickReply;
        }

        public static QuickReplyApiModel Convert(this QuickReplyModel quickReply, string quickReplyGroupId)
        {
            if (quickReply == null) return null;
            QuickReplyApiModel apiQuickReply = new QuickReplyApiModel();
            apiQuickReply.QuickReplyGroupId = quickReplyGroupId;
            apiQuickReply.Id = quickReply.Id;
            apiQuickReply.Name = quickReply.Name;
            apiQuickReply.Type = quickReply.Content.Type;
            apiQuickReply.Content = quickReply.Content.ContentString;
            return apiQuickReply;
        }

        public static QuickReplyGroupModel Convert(this QuickReplyGroupApiModel apiQuickReplyGroup)
        {
            if (apiQuickReplyGroup == null) return null;
            QuickReplyGroupModel quickReplyGroup = new QuickReplyGroupModel();
            quickReplyGroup.Id = apiQuickReplyGroup.Id;
            quickReplyGroup.Name = apiQuickReplyGroup.Name;
            quickReplyGroup.Level = apiQuickReplyGroup.Level;
            quickReplyGroup.BusinessId = apiQuickReplyGroup.BusinessId;
            quickReplyGroup.QuickReplies = new ObservableCollection<QuickReplyModel>(apiQuickReplyGroup.QuickReplies.Select(u => u.Convert()));
            return quickReplyGroup;
        }

        public static QuickReplyGroupApiModel Convert(this QuickReplyGroupModel quickReplyGroup, string userId)
        {
            if (quickReplyGroup == null || string.IsNullOrEmpty(userId)) return null;
            QuickReplyGroupApiModel apiQuickReplyGroup = new QuickReplyGroupApiModel();
            apiQuickReplyGroup.UserId = userId;
            apiQuickReplyGroup.Id = quickReplyGroup.Id;
            apiQuickReplyGroup.Name = quickReplyGroup.Name;
            apiQuickReplyGroup.BusinessId = quickReplyGroup.BusinessId;
            apiQuickReplyGroup.Level = quickReplyGroup.Level;
            return apiQuickReplyGroup;
        }

        public static TagModel Convert(this TagApiModel apiTag)
        {
            if (apiTag == null) return null;
            TagModel tag = new TagModel();
            tag.Id = apiTag.Id;
            tag.Name = apiTag.Name;
            return tag;
        }

        public static TagGroupApiModel Convert(this TagGroupModel tagGroup, string userId)
        {
            if (tagGroup == null) return null;
            TagGroupApiModel apiTagGroup = new TagGroupApiModel();
            apiTagGroup.UserId = userId;
            apiTagGroup.Id = tagGroup.Id;
            apiTagGroup.Name = tagGroup.Name;
            apiTagGroup.BusinessId = tagGroup.BusinessId;
            apiTagGroup.Level = tagGroup.Level;
            return apiTagGroup;
        }

        public static TagGroupModel Convert(this TagGroupApiModel apiTagGroup)
        {
            if (apiTagGroup == null) return null;
            TagGroupModel tagGroup = new TagGroupModel();
            tagGroup.Id = apiTagGroup.Id;
            tagGroup.Name = apiTagGroup.Name;
            tagGroup.BusinessId = apiTagGroup.BusinessId;
            tagGroup.Level = apiTagGroup.Level;
            tagGroup.Tags = new ObservableCollection<TagModel>(apiTagGroup.Tags.Select(u => u.Convert()));
            return tagGroup;
        }

        public static ChatModel Convert(this ChatViewApiModel apiChat)
        {
            if (apiChat == null) return null;
            ChatModel chat = new ChatModel();
            chat.Id = apiChat.Id;
            chat.BusinessId = apiChat.BusinessId;
            chat.Name = apiChat.Name;
            chat.Type = apiChat.Type;
            chat.HeaderImage = apiChat.HeaderImageUrl;
            chat.CreateTime = apiChat.CreateTime;
            return chat;
        }

        public static UserModel Convert(this ChatUserViewApiModel apiChatUser)
        {
            if (apiChatUser == null) return null;
            if (apiChatUser.UserType == UserTypeEnum.Staff)
            {
                StaffApiModel apiStaff = apiChatUser.UserInfo.JsonToObject<StaffApiModel>();
                StaffModel staff = apiStaff.Convert();
                staff.BusinessId = apiChatUser.BusinessId;
                if (apiChatUser.Follow != null)
                {
                    staff.FollowId = apiChatUser.Follow?.Id;
                    staff.Remark = apiChatUser.Follow.Remark;
                    staff.Description = apiChatUser.Follow.Description;
                }
                return staff;
            }
            else
            {
                return null;
            }
        }

        public static ChatModel Convert(this UserChatApiModel apiUserChat)
        {
            if (apiUserChat == null) return null;
            ChatModel chat = Convert(apiUserChat.Chat);
            if (chat == null) return null;
            chat.IsTop = apiUserChat.IsTop;
            chat.IsInform = apiUserChat.IsInform;
            chat.ChatAllUsers = new ObservableCollection<UserModel>(apiUserChat.Chat.ChatUsres.Select(u => u.Convert()));
            chat.ChatUsers = new ObservableCollection<UserModel>(apiUserChat.Chat.ChatUsres.Where(u => !u.Exited).Select(u => u.Convert()));
            chat.Messages = new ObservableCollection<MessageModel>();
            chat.Name = chat.ChatAllUsers.FirstOrDefault(u => u.Id != apiUserChat.UserId)?.NickName;
            chat.HeaderImage = chat.ChatAllUsers.FirstOrDefault(u => u.Id != apiUserChat.UserId)?.HeaderImage;
            return chat;
        }

        public static ChatUserApiModel Convert(this UserModel user)
        {
            if (user == null) return null;
            ChatUserApiModel chatUser = new ChatUserApiModel();
            chatUser.ChatId = null;
            chatUser.UserId = user.Id;
            chatUser.UserType = user.Type;
            chatUser.Name = null;
            chatUser.Exited = false;
            return chatUser;
        }

        public static ChatApiModel Convert(this ChatModel chat)
        {
            if (chat == null) return null;
            ChatApiModel apiChat = new ChatApiModel();
            apiChat.Id = chat.Id;
            apiChat.BusinessId = chat.BusinessId;
            apiChat.CreateTime = chat.CreateTime;
            apiChat.Type = chat.Type;
            apiChat.Name = chat.Name;
            apiChat.HeaderImageUrl = chat.HeaderImage;
            apiChat.ChatUsres = chat.ChatUsers.Select(u => u.Convert());
            return apiChat;
        }
    }
}
