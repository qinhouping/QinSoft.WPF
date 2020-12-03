using EMChat2.Common;
using EMChat2.Model.BaseInfo;
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
using System.Web;

namespace EMChat2.Service
{
    /// <summary>
    /// 系统服务，负责本地数据加载和持久化等逻辑
    /// </summary>
    [Component]
    public class SystemService
    {
        #region 构造函数
        public SystemService(IWindowManager windowManager, EventAggregator eventAggregator)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
            urlMappingInfos = new List<UrlMappingModel>();
            LoadUrlMapping();
        }
        #endregion
        private IWindowManager windowManager;
        private EventAggregator eventAggregator;
        private string loginInfoFilePath = ConfigurationManager.AppSettings["LoginInfoFilePath"];
        private string urlMappingFilePath = ConfigurationManager.AppSettings["UrlMappingFilePath"];
        private IList<UrlMappingModel> urlMappingInfos;

        #region 方法
        public virtual async Task<LoginInfoModel> LoadLoginInfo()
        {
            return await new Func<LoginInfoModel>(() =>
            {
                LoginInfoModel loginInfo = loginInfoFilePath.FileToStream().StreamToString().JsonToObject<LoginInfoModel>() ?? new LoginInfoModel();
                loginInfo.Password = loginInfo.Password.UnBase64();
                return loginInfo.Clone();
            }).ExecuteInTask();
        }

        public virtual async void StoreLoginInfo(LoginInfoModel loginInfo)
        {
            await new Action(() =>
            {
                loginInfo = loginInfo.Clone();
                if (loginInfo.IsRememberPassword) loginInfo.Password = loginInfo.Password.Base64();
                else loginInfo.Password = null;
                loginInfo.ObjectToJson().StringToStream().StreamToFile(loginInfoFilePath);
            }).ExecuteInTask();
        }

        protected virtual async void LoadUrlMapping()
        {
            this.urlMappingInfos = await new Func<IList<UrlMappingModel>>(() =>
            {
                return urlMappingFilePath.FileToStream().StreamToString().JsonToObject<List<UrlMappingModel>>() ?? new List<UrlMappingModel>().ToList();
            }).ExecuteInTask();
        }

        public virtual async Task<string> GetUrlMapping(string url, bool retOrigin = true)
        {
            return await new Func<string>(() =>
            {
                lock (this.urlMappingInfos)
                {
                    url = HttpUtility.UrlDecode(url);
                    UrlMappingModel urlMappingInfo = this.urlMappingInfos.FirstOrDefault(u => u.Url.Equals(url));
                    if (urlMappingInfo == null || !urlMappingInfo.LocalFilePath.IsExistsFile())
                    {
                        return retOrigin ? url : null;
                    }
                    else
                    {
                        return urlMappingInfo.LocalFilePath;
                    }
                }
            }).ExecuteInTask();
        }

        public virtual async void StoreUrlMapping(UrlMappingModel urlMappingInfo)
        {
            await new Action(() =>
            {
                lock (this.urlMappingInfos)
                {
                    urlMappingInfo.Url = HttpUtility.UrlDecode(urlMappingInfo.Url);
                    this.urlMappingInfos.Remove(urlMappingInfo);
                    this.urlMappingInfos.Add(urlMappingInfo);
                    urlMappingInfos.ObjectToJson().StringToStream().StreamToFile(urlMappingFilePath);
                }
            }).ExecuteInTask();
        }
        #endregion
    }
}
