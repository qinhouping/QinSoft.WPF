using EMChat2.Api.Models.Response;
using EMChat2.Common;
using EMChat2.Model.Api;
using EMChat2.Model.BaseInfo;
using EMChat2.Model.Enum;
using EMChat2.Model.IM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service
{
    public static class ApiTools
    {
        public static string ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        public static IDictionary<string, string> Headers = new Dictionary<string, string> { { "EMChat2-Auth", null } };

        public static bool LoginStaff(string workCode, string password, out string error, out StaffModel staff, out IMServerModel imServer, out IMUserModel imUser)
        {
            error = null;
            staff = null;
            imServer = null;
            imUser = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "workCode", workCode },
                    { "password", password }
                };
                ApiResponse<StaffLoginResponse> response = HttpTools.Get<ApiResponse<StaffLoginResponse>>(ApiUrl + "api/Staff/LoginStaff?" + HttpTools.UrlEncode(request), Headers, null);
                if (response.Success)
                {
                    staff = ConvertTools.Convert(response.Data.Staff);
                    imServer = response.Data.IMServer;
                    imUser = response.Data.IMUser;
                    Headers["EMChat2-Auth"] = response.Data.Token;
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool AddMessage(MessageModel message, out string error, out string messageId)
        {
            error = null;
            messageId = null;
            try
            {
                MessageApiModel apiMessage = ConvertTools.Convert(message);
                ApiResponse<string> response = HttpTools.Post<ApiResponse<string>>(ApiUrl + "api/Message/AddMessage", Headers, null, apiMessage);
                if (response.Success)
                {
                    messageId = response.Data;
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool ModifyMessage(string messageId, MessageStateEnum state, out string error)
        {
            error = null;
            try
            {
                UpdateMessageApiModel apiUpdateMessage = ConvertTools.Convert(messageId, state);
                ApiResponse response = HttpTools.Put<ApiResponse>(ApiUrl + "api/Message/ModifyMessage", Headers, null, apiUpdateMessage);
                if (response.Success)
                {
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static int BatchModifyMessage(IEnumerable<KeyValuePair<string, MessageStateEnum>> messages, out string error)
        {
            error = null;
            try
            {
                IEnumerable<UpdateMessageApiModel> apiUpdateMessages = messages.Select(u => ConvertTools.Convert(u.Key, u.Value));
                ApiResponse<int> response = HttpTools.Put<ApiResponse<int>>(ApiUrl + "api/Message/BatchModifyMessage", Headers, null, apiUpdateMessages);
                if (response.Success)
                {
                    return response.Data;
                }
                else
                {
                    error = response.Message;
                    return 0;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return -1;
            }
        }

        public static bool GetDepartments(string businessId, string userId, out string error, out IEnumerable<DepartmentModel> departments)
        {
            error = null;
            departments = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "businessId", businessId },
                    { "userId", userId }
                };
                ApiResponse<IEnumerable<DepartmentViewApiModel>> response = HttpTools.Get<ApiResponse<IEnumerable<DepartmentViewApiModel>>>(ApiUrl + "api/Department/GetDepartmentViews?" + HttpTools.UrlEncode(request), Headers, null);
                if (response.Success)
                {
                    departments = response.Data.Select(u => u.Convert());
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool GetQuickReplyGroups(string userId, out string error, out IEnumerable<QuickReplyGroupModel> quickReplyGroups)
        {
            error = null;
            quickReplyGroups = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "userId", userId }
                };
                ApiResponse<IEnumerable<QuickReplyGroupApiModel>> response = HttpTools.Get<ApiResponse<IEnumerable<QuickReplyGroupApiModel>>>(ApiUrl + "api/QuickReply/GetAllQuickReplyGroups?" + HttpTools.UrlEncode(request), Headers, null);
                if (response.Success)
                {
                    quickReplyGroups = response.Data.Select(u => u.Convert());
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool AddQuickReplyGroup(string userId, QuickReplyGroupModel quickReplyGroup, out string error, out string quickReplyGroupId)
        {
            error = null;
            quickReplyGroupId = null;
            try
            {
                QuickReplyGroupApiModel apiQuickReplyGroup = quickReplyGroup.Convert(userId);
                ApiResponse<string> response = HttpTools.Post<ApiResponse<string>>(ApiUrl + "api/QuickReply/AddUserQuickReplyGroup", Headers, null, apiQuickReplyGroup);
                if (response.Success)
                {
                    quickReplyGroupId = response.Data;
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool ModifyQuickReplyGroup(string userId, QuickReplyGroupModel quickReplyGroup, out string error)
        {
            error = null;
            try
            {
                QuickReplyGroupApiModel apiQuickReplyGroup = quickReplyGroup.Convert(userId);
                ApiResponse<string> response = HttpTools.Put<ApiResponse<string>>(ApiUrl + "api/QuickReply/ModifyUserQuickReplyGroup", Headers, null, apiQuickReplyGroup);
                if (response.Success)
                {
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool RemoveQuickReplyGroup(string userId, string quickReplyGroupId, out string error)
        {
            error = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "userId", userId },
                    { "quickReplyGroupId", quickReplyGroupId }
                };
                ApiResponse response = HttpTools.Delete<ApiResponse>(ApiUrl + "api/QuickReply/RemoveUserQuickReplyGroup?" + HttpTools.UrlEncode(request), Headers, null);
                if (response.Success)
                {
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool AddQuickReply(string quickReplyGroupId, QuickReplyModel quickReply, out string error, out string quickReplyId)
        {
            error = null;
            quickReplyId = null;
            try
            {
                QuickReplyApiModel apiQuickReply = quickReply.Convert(quickReplyGroupId);
                ApiResponse<string> response = HttpTools.Post<ApiResponse<string>>(ApiUrl + "api/QuickReply/AddQuickReply", Headers, null, apiQuickReply);
                if (response.Success)
                {
                    quickReplyId = response.Data;
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }
        public static bool ModifyQuickReply(string quickReplyGroupId, QuickReplyModel quickReply, out string error)
        {
            error = null;
            try
            {
                QuickReplyApiModel apiQuickReply = quickReply.Convert(quickReplyGroupId);
                ApiResponse response = HttpTools.Put<ApiResponse>(ApiUrl + "api/QuickReply/ModifyQuickReply", Headers, null, apiQuickReply);
                if (response.Success)
                {
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool RemoveQuickReply(string quickReplyGroupId, string quickReplyId, out string error)
        {
            error = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "quickReplyGroupId", quickReplyGroupId },
                    { "quickReplyId", quickReplyId }
                };
                ApiResponse response = HttpTools.Delete<ApiResponse>(ApiUrl + "api/QuickReply/RemoveQuickReply?" + HttpTools.UrlEncode(request), Headers, null);
                if (response.Success)
                {
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool GetTagGroups(string userId, out string error, out IEnumerable<TagGroupModel> tagGroups)
        {
            error = null;
            tagGroups = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "userId", userId }
                };
                ApiResponse<IEnumerable<TagGroupApiModel>> response = HttpTools.Get<ApiResponse<IEnumerable<TagGroupApiModel>>>(ApiUrl + "api/Tag/GetAllTagGroups?" + HttpTools.UrlEncode(request), Headers, null);
                if (response.Success)
                {
                    tagGroups = response.Data.Select(u => u.Convert());
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool AddTagGroup(string userId, TagGroupModel TagGroup, out string error, out string TagGroupId)
        {
            error = null;
            TagGroupId = null;
            try
            {
                TagGroupApiModel apiTagGroup = TagGroup.Convert(userId);
                ApiResponse<string> response = HttpTools.Post<ApiResponse<string>>(ApiUrl + "api/Tag/AddUserTagGroup", Headers, null, apiTagGroup);
                if (response.Success)
                {
                    TagGroupId = response.Data;
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool ModifyTagGroup(string userId, TagGroupModel TagGroup, out string error)
        {
            error = null;
            try
            {
                TagGroupApiModel apiTagGroup = TagGroup.Convert(userId);
                ApiResponse<string> response = HttpTools.Put<ApiResponse<string>>(ApiUrl + "api/Tag/ModifyUserTagGroup", Headers, null, apiTagGroup);
                if (response.Success)
                {
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool RemoveTagGroup(string userId, string TagGroupId, out string error)
        {
            error = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "userId", userId },
                    { "TagGroupId", TagGroupId }
                };
                ApiResponse response = HttpTools.Delete<ApiResponse>(ApiUrl + "api/Tag/RemoveUserTagGroup?" + HttpTools.UrlEncode(request), Headers, null);
                if (response.Success)
                {
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool GetChats(string userId, out string error, out IEnumerable<ChatModel> chats)
        {
            error = null;
            chats = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "userId", userId }
                };
                ApiResponse<IEnumerable<UserChatApiModel>> response = HttpTools.Get<ApiResponse<IEnumerable<UserChatApiModel>>>(ApiUrl + "api/Chat/GetUserChats?" + HttpTools.UrlEncode(request), Headers, null);
                if (response.Success)
                {
                    chats = response.Data.Select(u => u.Convert());
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool OpenChat(string userId, string chatId, out string error, out ChatModel chat)
        {
            error = null;
            chat = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "userId", userId },
                    { "chatId", chatId }
                };
                ApiResponse<UserChatApiModel> response = HttpTools.Get<ApiResponse<UserChatApiModel>>(ApiUrl + "api/Chat/OpenUserChat?" + HttpTools.UrlEncode(request), Headers, null);
                if (response.Success)
                {
                    chat = response.Data.Convert();
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool CloseChat(string userId, string chatId, out string error)
        {
            error = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "userId", userId },
                    { "chatId", chatId }
                };
                ApiResponse response = HttpTools.Delete<ApiResponse>(ApiUrl + "api/Chat/RemoveUserChat?" + HttpTools.UrlEncode(request), Headers, null);
                if (response.Success)
                {
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool CreateChat(string userId, ref ChatModel chat, out string error)
        {
            error = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "userId", userId }
                };
                ChatApiModel apiChat = chat.Convert();
                ApiResponse<UserChatApiModel> response = HttpTools.Post<ApiResponse<UserChatApiModel>>(ApiUrl + "api/Chat/CreateUserChat?" + HttpTools.UrlEncode(request), Headers, null, apiChat);
                if (response.Success)
                {
                    chat = response.Data.Convert();
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool GetMessages(string chatId, DateTime? maxTime, int count, out string error, out IEnumerable<MessageModel> messages)
        {
            error = null;
            messages = null;
            try
            {
                IDictionary<string, object> request = new Dictionary<string, object>()
                {
                    { "chatId", chatId },
                    { "maxTime", maxTime },
                    { "count", count }
                };
                ApiResponse<IEnumerable<MessageApiModel>> response = HttpTools.Get<ApiResponse<IEnumerable<MessageApiModel>>>(ApiUrl + "api/Message/GetMessages?" + HttpTools.UrlEncode(request), Headers, null);
                if (response.Success)
                {
                    messages = response.Data.Select(u => u.Convert());
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool ModifyChat(string userId, ChatModel chat, out string error)
        {
            error = null;
            try
            {
                UserChatApiModel apiUserChat = chat.Convert(userId);
                ApiResponse response = HttpTools.Put<ApiResponse>(ApiUrl + "api/Chat/ModifyUserChat", Headers, null, apiUserChat);
                if (response.Success)
                {
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool AddFollow(string userId, UserTypeEnum userType, UserModel oUser, out string error, out string id)
        {
            error = null;
            id = null;
            try
            {
                FollowApiModel apiFollow = oUser.Convert(userId, userType);
                ApiResponse<string> response = HttpTools.Post<ApiResponse<string>>(ApiUrl + "api/Follow/AddFollow", Headers, null, apiFollow);
                if (response.Success)
                {
                    id = response.Data;
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }

        public static bool ModifyFollow(string userId, UserTypeEnum userType, UserModel oUser, out string error)
        {
            error = null;
            try
            {
                FollowApiModel apiFollow = oUser.Convert(userId, userType);
                ApiResponse response = HttpTools.Put<ApiResponse>(ApiUrl + "api/Follow/ModifyFollow", Headers, null, apiFollow);
                if (response.Success)
                {
                    return true;
                }
                else
                {
                    error = response.Message;
                    return false;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
        }
    }
}
