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
    /// 系统服务，负责本地数据加载和持久化相关逻辑
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
            LoginInfoModel loginInfo = null;
            await new Action(() =>
            {
                loginInfo = loginInfoFilePath.FileToStream().StreamToString().JsonToObject<LoginInfoModel>() ?? new LoginInfoModel();
            }).ExecuteInTask();
            loginInfo.Password = loginInfo.Password.UnBase64();
            return loginInfo.CloneObject();
        }

        public virtual async void StoreLoginInfo(LoginInfoModel loginInfo)
        {
            loginInfo = loginInfo.CloneObject();
            if (loginInfo.IsRememberPassword) loginInfo.Password = loginInfo.Password.Base64();
            else loginInfo.Password = null;
            await new Action(() =>
            {
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
            url = HttpUtility.UrlDecode(url);
            UrlMappingModel urlMapping = await new Func<UrlMappingModel>(() =>
            {
                lock (this.urlMappingInfos)
                {
                    return this.urlMappingInfos.FirstOrDefault(u => u.Url.Equals(url));
                }
            }).ExecuteInTask();
            if (urlMapping == null || !urlMapping.LocalFilePath.IsExistsFile())
            {
                return retOrigin ? url : null;
            }
            else
            {
                return urlMapping.LocalFilePath;
            }
        }

        public virtual async void StoreUrlMapping(UrlMappingModel urlMappingInfo)
        {
            urlMappingInfo.Url = HttpUtility.UrlDecode(urlMappingInfo.Url);
            await new Action(() =>
            {
                lock (this.urlMappingInfos)
                {
                    this.urlMappingInfos.Remove(urlMappingInfo);
                    this.urlMappingInfos.Add(urlMappingInfo);
                    urlMappingInfos.ObjectToJson().StringToStream().StreamToFile(urlMappingFilePath);
                }
            }).ExecuteInTask();
        }
        #endregion
    }
}
