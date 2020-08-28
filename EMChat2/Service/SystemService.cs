using EMChat2.Common;
using EMChat2.Model.Entity;
using QinSoft.Event;
using QinSoft.Ioc.Attribute;
using QinSoft.WPF.Core;
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
        public SystemService(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            urlMappingInfos = new List<UrlMappingInfo>();
            LoadUrlMapping();
        }
        #endregion
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private string loginInfoFilePath = ConfigurationManager.AppSettings["LoginInfoFilePath"];
        private string urlMappingFilePath = ConfigurationManager.AppSettings["UrlMappingFilePath"];
        private IList<UrlMappingInfo> urlMappingInfos;

        #region 方法
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
            this.urlMappingInfos = await new Func<IList<UrlMappingInfo>>(() =>
            {
                return urlMappingFilePath.FileToStream().StreamToString().JsonToObject<List<UrlMappingInfo>>() ?? new List<UrlMappingInfo>().ToList();
            }).ExecuteInTask();
        }

        public string GetUrlMapping(string url, bool retOrigin = true)
        {
            UrlMappingInfo urlMappingInfo = this.urlMappingInfos.FirstOrDefault(u => u.Url.Equals(url));
            if (urlMappingInfo == null || !urlMappingInfo.LocalFilePath.IsExistsFile())
            {
                if (retOrigin) return url;
                else return null;
            }
            else
            {
                return urlMappingInfo.LocalFilePath;
            }
        }

        public async void StoreUrlMapping(UrlMappingInfo urlMappingInfo)
        {
            await new Func<object>(() =>
            {
                lock (this)
                {
                    this.urlMappingInfos.Remove(urlMappingInfo);
                    this.urlMappingInfos.Add(urlMappingInfo);
                    urlMappingInfos.ObjectToJson().StringToStream().StreamToFile(urlMappingFilePath);
                }
                return null;
            }).ExecuteInTask();
        }
        #endregion
    }
}
