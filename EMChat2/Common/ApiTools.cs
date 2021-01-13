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
                ApiResponse<IEnumerable<DepartmentViewApiModel>> response = HttpTools.Get<ApiResponse<IEnumerable<DepartmentViewApiModel>>>(ApiUrl + "/api/Department/GetDepartmentViews?" + HttpTools.UrlEncode(request), Headers, null);
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
    }
}
