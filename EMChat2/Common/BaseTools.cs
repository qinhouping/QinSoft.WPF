using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json.Converters;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EMChat2.Common
{
    /// <summary>
    /// 基础拓展类
    /// </summary>
    public static class BaseTools
    {
        /// <summary>
        /// json日期默认格式
        /// </summary>
        public static DateTimeConverterBase DefaultDateTimeConvert = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
        /// <summary>
        /// 拓展方法：序列化对象成json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            if (null == obj)
            {
                return string.Empty;
            }
            else
            {
                try
                {
                    return JsonConvert.SerializeObject(obj, DefaultDateTimeConvert);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("tojson error:" + e.ToString());
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 拓展方法：反序列化json字符串成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            else
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("fromjson error:" + e.ToString());
                    return default;
                }
            }
        }

        /// <summary>
        /// 拓展方法：序列化对象成XML
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXml(this object obj)
        {
            if (null == obj)
            {
                return string.Empty;
            }
            else
            {
                try
                {
                    using (StreamReader stream = new StreamReader(new MemoryStream()))
                    {
                        XmlSerializer serializer = new XmlSerializer(obj.GetType());
                        serializer.Serialize(stream.BaseStream, obj);
                        stream.BaseStream.Position = 0;
                        return stream.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("toxml error:" + e.ToString());
                    return string.Empty;
                }
            }

        }

        /// <summary>
        /// 拓展方法反序列化xml字符串成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T FromXml<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            else
            {
                try
                {
                    using (StringReader sr = new StringReader(value))
                    {
                        XmlSerializer xmldes = new XmlSerializer(typeof(T));
                        return (T)xmldes.Deserialize(sr);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("fromxml error:" + e.ToString());
                    return default;
                }
            }

        }

        /// <summary>
        /// 拓展方法：将流保存到文件中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filepath"></param>
        public static void ToFile(this Stream stream, string filepath)
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    while (true)
                    {
                        byte[] content = reader.ReadBytes(20480);
                        if (content == null || content.Length == 0)
                        {
                            break;
                        }
                        writer.Write(content);
                    }
                }
            }
        }

        /// <summary>
        /// 拓展方法：打开文件流
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Stream FromFile(this FileInfo file)
        {
            try
            {
                return new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                return null;
            }
        }

        public static Stream FromImage(this Image image, ImageFormat format = null)
        {
            if (format == null)
            {
                format = ImageFormat.Jpeg;
            }
            MemoryStream stream = new MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        public static long ToTimeStamp(this DateTime Date)
        {
            switch (Date.Kind)
            {
                case DateTimeKind.Utc:; break;
                case DateTimeKind.Local: Date = Date.ToUniversalTime(); break;
                case DateTimeKind.Unspecified: DateTime.SpecifyKind(Date, DateTimeKind.Local); break;
            }
            TimeSpan ts = Date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)ts.TotalMilliseconds;
        }

        public static DateTime ToDateTime(this long TimeStamp)
        {
            DateTime Date = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan Span = new TimeSpan(TimeStamp);
            return Date.Add(Span);
        }

        /// <summary>
        /// 是否是网络路径
        /// </summary>
        public static bool IsNetUrl(this string url)
        {
            return Regex.IsMatch(url, @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?");
        }
    }
}
