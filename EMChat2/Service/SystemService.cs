using EMChat2.Common;
using EMChat2.Model.Entity;
using QinSoft.Ioc.Attribute;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMChat2.Service
{
    [Component]
    public class SystemService
    {
        private string loginInfoFilePath = ConfigurationManager.AppSettings["LoginInfoFilePath"];
        public async Task<LoginInfo> LoadLoginInfo()
        {
            return await Task.Factory.StartNew(() =>
            {
                LoginInfo loginInfo = loginInfoFilePath.FileToStream().StreamToString().JsonToObject<LoginInfo>() ?? new LoginInfo();
                loginInfo.Password = loginInfo.Password.UnBase64();
                return loginInfo.Clone();
            });
        }

        public async Task StoreLoginInfo(LoginInfo loginInfo)
        {
            await Task.Factory.StartNew(() =>
            {
                loginInfo = loginInfo.Clone();
                loginInfo.Password = loginInfo.Password.Base64();
                loginInfo.ObjectToJson().StringToStream().StreamToFile(loginInfoFilePath);
            });
        }
    }
}
