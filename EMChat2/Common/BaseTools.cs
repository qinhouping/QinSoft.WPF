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


        public static string ObjectToJson(this object obj)
        {
            if (null == obj) return null;
            try
            {
                return JsonConvert.SerializeObject(obj, DefaultDateTimeConvert);
            }
            catch (Exception e)
            {
                Debug.WriteLine("ObjectToJson Error:" + e.ToString());
                return null;
            }
        }


        public static T JsonToObject<T>(this string value)
        {
            if (string.IsNullOrEmpty(value)) return default;
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception e)
            {
                Debug.WriteLine("JsonToObject Error:" + e.ToString());
                return default;
            }
        }


        public static string ObjectToXml(this object obj)
        {
            if (null == obj) return null;
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
                Debug.WriteLine("ObjectToXml Error:" + e.ToString());
                return null;
            }
        }


        public static T XmlToObject<T>(this string value)
        {
            if (string.IsNullOrEmpty(value)) return default;
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
                Debug.WriteLine("XmlToObject Error:" + e.ToString());
                return default;
            }
        }

        public static void StreamToFile(this Stream stream, string filepath)
        {
            if (stream == null || string.IsNullOrEmpty(filepath)) return;
            try
            {
                SafeFilePath(filepath);
                using (BinaryWriter writer = new BinaryWriter(new FileStream(filepath, FileMode.Create, FileAccess.Write)))
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
            catch (Exception e)
            {
                Debug.WriteLine("StreamToFile Error:" + e.ToString());
            }
        }

        public static Stream FileToStream(this string filepath)
        {
            if (string.IsNullOrEmpty(filepath)) return null;
            try
            {
                return new FileStream(filepath, FileMode.Open, FileAccess.Read);
            }
            catch (Exception e)
            {
                Debug.WriteLine("StreamToFile Error:" + e.ToString());
                return null;
            }
        }

        public static Stream ImageToStream(this Image image, ImageFormat format = null)
        {
            if (image == null) return null;
            try
            {
                if (image == null) throw new ArgumentNullException();
                if (format == null)
                {
                    format = ImageFormat.Jpeg;
                }
                MemoryStream stream = new MemoryStream();
                image.Save(stream, format);
                stream.Position = 0;
                return stream;
            }
            catch (Exception e)
            {
                Debug.WriteLine("ImageToStream Error:" + e.ToString());
                return null;
            }
        }

        public static Image StreamToImage(this Stream stream)
        {
            if (stream == null) return null;
            try
            {
                Bitmap bitmap = new Bitmap(stream);
                return bitmap;
            }
            catch (Exception e)
            {
                Debug.WriteLine("StreamToImage Error:" + e.ToString());
                return null;
            }
        }

        public static Stream StringToStream(this string content, Encoding encoding = null)
        {
            if (content == null) return null;
            try
            {
                encoding = encoding ?? Encoding.UTF8;
                MemoryStream stream = new MemoryStream();
                byte[] buffer = encoding.GetBytes(content);
                stream.Write(buffer, 0, buffer.Length);
                stream.Position = 0;
                return stream;
            }
            catch (Exception e)
            {
                Debug.WriteLine("StringToStream Error:" + e.ToString());
                return null;
            }
        }

        public static string StreamToString(this Stream stream, Encoding encoding = null)
        {
            if (stream == null) return null;
            try
            {
                encoding = encoding ?? Encoding.UTF8;
                using (TextReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("StreamToString Error:" + e.ToString());
                return null;
            }
        }

        public static long DateTimeToTimeStamp(this DateTime Date)
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

        public static DateTime TimeStampToDateTime(this long TimeStamp)
        {
            DateTime Date = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan Span = new TimeSpan(TimeStamp);
            return Date.Add(Span);
        }

        public static bool IsNetUrl(this string url)
        {
            return Regex.IsMatch(url, @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?");
        }

        public static bool IsExistsFile(this string filePath)
        {
            return File.Exists(filePath);
        }

        public static void SafeFilePath(string filepath)
        {
            string directorypath = Path.GetDirectoryName(filepath);
            if (!Directory.Exists(directorypath))
                Directory.CreateDirectory(directorypath);
        }

        public static T Clone<T>(this T obj)
        {
            if (obj == null) return default;
            Type type = obj.GetType();
            object newObj = Activator.CreateInstance(type);
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (propertyInfo.CanRead && propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(newObj, propertyInfo.GetValue(obj));
                }
            }
            return (T)newObj;
        }

        public static T Assign<T>(this T obj, T value)
        {
            if (obj == null) return value;
            else if (value == null) return default;
            else
            {
                Type type = obj.GetType();
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    if (propertyInfo.CanRead && propertyInfo.CanWrite && propertyInfo.GetCustomAttribute(typeof(AssignIgnoreAttribute)) == null)
                    {
                        propertyInfo.SetValue(obj, propertyInfo.GetValue(value));
                    }
                }
                return obj;
            }
        }
    }

    public class AssignIgnoreAttribute : Attribute
    {

    }
}
