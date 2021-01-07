using EMChat2.Api.Models.Response;
using EMChat2.Common;
using EMChat2.Model.Api;
using EMChat2.Model.BaseInfo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service
{
    public static class ApiTools
    {
        public static string ApiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        public static IDictionary<string, string> Headers = new Dictionary<string, string> { { "EMChat2-Auth", "6abb47c2-3c2f-11eb-9b53-00ff587e77ca" } };

        public static bool AddMessage(MessageModel message, out string error, out string messageId)
        {
            error = null;
            messageId = null;
            try
            {
                MessageApiModel apiMessage = ConverterTools.MessageToApiModel(message);
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
    }
}
