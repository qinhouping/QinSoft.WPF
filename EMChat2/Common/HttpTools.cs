using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Security;

namespace EMChat2.Common
{
    public static class HttpTools
    {
        public static string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.139 Safari/537.36";
        private static int ContentLength = 0;
        public static string ContentType = "application/x-www-form-urlencoded";
        public static string RequestEncoding = "utf-8";
        public static string ResponseEncoding = "utf-8";
        public static CookieContainer cookieContainer = new CookieContainer();

        public static void InitTls()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public static string UrlEncode(IDictionary<string, object> data)
        {
            IEnumerable<string> list = from item in data select string.Format("{0}={1}", HttpUtility.UrlEncode(item.Key), HttpUtility.UrlEncode(Convert.ToString(item.Value)));
            return string.Join("&", list);
        }

        public static IDictionary<string, object> UrlDecode(string url)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            IEnumerable<string> list = url.Split('&');
            foreach (string item in list)
            {
                string[] kv = item.Split('=');
                result[HttpUtility.UrlDecode(kv[0])] = HttpUtility.UrlDecode(kv[1]);
            }
            return result;
        }

        public static WebResponse Get(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies)
        {
            return DoReqeust(WebMethod.GET, url, headers, cookies);
        }

        public static T Get<T>(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies)
        {
            return DoReqeust<T>(WebMethod.GET, url, headers, cookies, null);
        }

        public static async Task<T> GetAsync<T>(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies)
        {
            return await DoRequestAsync<T>(WebMethod.GET, url, headers, cookies, null);
        }

        public static WebResponse Post(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies, string data)
        {
            return DoReqeust(WebMethod.POST, url, headers, cookies, data);
        }

        public static T Post<T>(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies, object data)
        {
            return DoReqeust<T>(WebMethod.POST, url, headers, cookies, data);
        }

        public static async Task<T> PostAsync<T>(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies, object data)
        {
            return await DoRequestAsync<T>(WebMethod.POST, url, headers, cookies, data);
        }

        public static WebResponse Put(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies, string data)
        {
            return DoReqeust(WebMethod.PUT, url, headers, cookies, data);
        }

        public static T Put<T>(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies, object data)
        {
            return DoReqeust<T>(WebMethod.PUT, url, headers, cookies, data);
        }

        public static async Task<T> PutAsync<T>(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies, object data)
        {
            return await DoRequestAsync<T>(WebMethod.PUT, url, headers, cookies, data);
        }

        public static WebResponse Delete(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies)
        {
            return DoReqeust(WebMethod.DELETE, url, headers, cookies);
        }

        public static T Delete<T>(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies)
        {
            return DoReqeust<T>(WebMethod.DELETE, url, headers, cookies);
        }

        public static async Task<T> DeleteAsync<T>(string url, IDictionary<string, string> headers, IDictionary<string, string> cookies)
        {
            return await DoRequestAsync<T>(WebMethod.DELETE, url, headers, cookies);
        }

        private static WebResponse DoReqeust(WebMethod method, string url, IDictionary<string, string> headers = null, IDictionary<string, string> cookies = null, string data = null, int timeout = 60000)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            //方法
            request.Method = method.ToString();
            //超时
            request.Timeout = timeout;
            //用户标示
            request.UserAgent = UserAgent;
            //内容长度
            request.ContentLength = ContentLength;
            //内容类型
            request.ContentType = ContentType;
            //cookie容器
            request.CookieContainer = cookieContainer;

            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    string value = headers[key];
                    Type type = request.GetType();
                    PropertyInfo info = type.GetProperty(key.Replace("-", ""));
                    if (info != null)
                    {
                        info.SetValue(request, value, null);
                    }
                    else
                    {
                        request.Headers.Add(key, value);
                    }
                }
            }
            if (cookies != null)
            {
                foreach (string key in cookies.Keys)
                {
                    string value = headers[key];
                    request.Headers.Add("Cookie", key + "=" + value);
                }
            }
            if (!string.IsNullOrEmpty(data))
            {
                byte[] bytes = Encoding.GetEncoding(RequestEncoding).GetBytes(data.ToString());
                request.ContentLength = bytes.Length;
                request.GetRequestStream().Write(bytes, 0, bytes.Length);
            }
            return request.GetResponse();
        }

        private static T DoReqeust<T>(WebMethod method, string url, IDictionary<string, string> headers = null, IDictionary<string, string> cookies = null, object data = null, int timeout = 60000)
        {
            if (headers == null)
            {
                headers = new Dictionary<string, string>();
            }
            headers["Content-Type"] = "application/json";//Json
            HttpWebResponse response = DoReqeust(method, url, headers, cookies, data.ObjectToJson(), timeout) as HttpWebResponse;
            //if (response.StatusCode != HttpStatusCode.OK)
            //{
            //    throw new InvalidOperationException(string.Format("返回状态异常:{0}", response.StatusCode.ToString()));
            //}
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(ResponseEncoding));
            string content = reader.ReadToEnd();
            Debug.WriteLine("httptools request:{0} response:{1}", url, content);
            return content.JsonToObject<T>();
        }

        /// <summary>
        /// 请求（异步）
        /// </summary>
        private static async Task<T> DoRequestAsync<T>(WebMethod method, string url, IDictionary<string, string> headers = null, IDictionary<string, string> cookies = null, object data = null, int timeout = 60000)
        {
            return await new Func<T>(() =>
            {
                return DoReqeust<T>(method, url, headers, cookies, data, timeout);
            }).ExecuteInTask();
        }

        /// <summary>
        /// 异步文件上传
        /// </summary>
        /// <param name="url">上传路径</param>
        /// <param name="stream">文件流</param>
        /// <param name="name">名称</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="extdata">其他数据</param>
        /// <returns>上传结果</returns>
        public static async Task<HttpResponseMessage> UploadAsync(string url, Stream stream, string name, string fileName, IDictionary<string, string> extdata = null)
        {
            MultipartFormDataContent multipartFormdataContent = new MultipartFormDataContent(DateTime.Now.Ticks.ToString("X"));
            multipartFormdataContent.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
            multipartFormdataContent.Add(new StreamContent(stream), string.Format("\"{0}\"", name), string.Format("\"{0}\"", fileName));

            if (extdata == null)
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync(url, multipartFormdataContent);
                return response;
            }
            else
            {
                foreach (string key in extdata.Keys)
                {
                    multipartFormdataContent.Add(new StringContent(extdata[key]), string.Format("\"{0}\"", key));
                }
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync(url, multipartFormdataContent);
                return response;
            }
        }

        /// <summary>
        /// 异步文件上传
        /// </summary>
        /// <typeparam name="T">结果泛型</typeparam>
        /// <param name="url">上传路径</param>
        /// <param name="stream">文件流</param>
        /// <param name="name">名称</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="extdata">其他数据</param>
        /// <returns>上传结果</returns>
        public static async Task<T> UploadAsync<T>(string url, Stream stream, string name, string fileName, IDictionary<string, string> extdata = null)
        {
            HttpResponseMessage response = await UploadAsync(url, stream, name, fileName, extdata);
            string content = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("httptools request:{0} response:{1}", url, content);
            return content.JsonToObject<T>();
        }

        /// <summary>
        /// 异步文件下载
        /// </summary>
        /// <param name="method">请求方法</param>
        /// <param name="url">下载路径</param>
        /// <param name="headers">请求头部</param>
        /// <param name="cookies">cookie</param>
        /// <param name="timeout">超时</param>
        /// <returns>文件流</returns>
        public static async Task<Stream> DownloadAsync(WebMethod method, string url, IDictionary<string, string> headers = null, IDictionary<string, string> cookies = null, object data = null, int timeout = 60000)
        {
            return await Task.Factory.StartNew(() =>
            {
                if (headers == null)
                {
                    headers = new Dictionary<string, string>();
                }
                headers["Content-Type"] = "application/json";//Json
                WebResponse response = DoReqeust(method, url, headers, cookies, data.ObjectToJson(), timeout);
                return response.GetResponseStream();
            });
        }

        /// <summary>
        /// 异步文件下载
        /// </summary>
        /// <param name="url">下载路径</param>
        /// <param name="headers">请求头部</param>
        /// <param name="cookies">cookie</param>
        /// <param name="timeout">超时</param>
        /// <returns>文件流</returns>
        public static async Task<Stream> DownloadAsync(string url, IDictionary<string, string> headers = null, IDictionary<string, string> cookies = null, int timeout = 60000)
        {
            return await DownloadAsync(WebMethod.GET, url, headers, cookies, null, timeout);
        }
    }

    /// <summary>
    /// 请求方法枚举
    /// </summary>
    public enum WebMethod
    {
        GET,
        POST,
        DELETE,
        PUT,
        OPTIONS
    }
}
