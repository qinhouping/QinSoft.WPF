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
        #region 构造函数
        public SystemService()
        {
            urlMappingInfos = new List<UrlMappingInfo>();
            LoadUrlMapping();
        }
        #endregion

        private string loginInfoFilePath = ConfigurationManager.AppSettings["LoginInfoFilePath"];
        private string urlMappingFilePath = ConfigurationManager.AppSettings["UrlMappingFilePath"];
        private List<UrlMappingInfo> urlMappingInfos = new List<UrlMappingInfo>();

        public async Task<LoginInfo> LoadLoginInfo()
        {
            return await new Func<LoginInfo>(() =>
            {
                LoginInfo loginInfo = loginInfoFilePath.FileToStream().StreamToString().JsonToObject<LoginInfo>() ?? new LoginInfo();
                loginInfo.Password = loginInfo.Password.UnBase64();
                return loginInfo.Clone();
            }).ExecuteInTask();
        }

        public async void StoreLoginInfo(LoginInfo loginInfo)
        {
            await new Func<object>(() =>
            {
                loginInfo = loginInfo.Clone();
                loginInfo.Password = loginInfo.Password.Base64();
                loginInfo.ObjectToJson().StringToStream().StreamToFile(loginInfoFilePath);
                return null;
            }).ExecuteInTask();
        }

        private async void LoadUrlMapping()
        {
            this.urlMappingInfos = await new Func<List<UrlMappingInfo>>(() =>
             {
                 return urlMappingFilePath.FileToStream().StreamToString().JsonToObject<List<UrlMappingInfo>>() ?? new List<UrlMappingInfo>();
             }).ExecuteInTask();
        }

        public string GetUrlMapping(string url, bool retOrigin = true)
        {
            UrlMappingInfo urlMappingInfo = this.urlMappingInfos.FirstOrDefault(u => u.Url.Equals(url));
            if (urlMappingInfo == null && retOrigin) return url;
            else return urlMappingInfo?.LocalFilePath;
        }

        public async void StoreUrlMapping(UrlMappingInfo urlMappingInfo)
        {
            await new Func<object>(() =>
            {
                lock (this)
                {
                    this.urlMappingInfos.Add(urlMappingInfo);
                    urlMappingInfos.ObjectToJson().StringToStream().StreamToFile(urlMappingFilePath);
                }
                return null;
            }).ExecuteInTask();
        }
    }
}
