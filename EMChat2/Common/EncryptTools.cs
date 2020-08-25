using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace EMChat2.Common
{
    /// <summary>
    /// 密码拓展类
    /// </summary>
    public static class EncryptTools
    {
        #region 字符串 字节转换
        public static byte[] ToBytes(this string content, Encoding encoding)
        {
            return encoding.GetBytes(content);
        }

        public static string FromBytes(this byte[] content, Encoding encoding)
        {
            return encoding.GetString(content);
        }
        #endregion

        #region base64
        public static string Base64(this string content, Encoding encoding)
        {
            try
            {
                byte[] res = content.ToBytes(encoding);
                return Convert.ToBase64String(res, 0, res.Length);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string UnBase64(this string value, Encoding encoding)
        {
            try
            {
                byte[] res = Convert.FromBase64String(value);
                return res.FromBytes(encoding);
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region 基础hash 加密
        public static string Hash(this string text, string hashName, string SecretKey = "")
        {
            text = text + SecretKey;
            byte[] data = Encoding.Default.GetBytes(text);
            HashAlgorithm hash = HashAlgorithm.Create(hashName);
            return BitConverter.ToString(hash.ComputeHash(data)).Replace("-", "");
        }

        public static string Hash(this byte[] text, string hashName, string SecretKey = "")
        {
            byte[] data = text.Concat(Encoding.Default.GetBytes(SecretKey)).ToArray();
            HashAlgorithm hash = HashAlgorithm.Create(hashName);
            return BitConverter.ToString(hash.ComputeHash(data)).Replace("-", "");
        }
        #endregion

        #region MD5
        public static string MD5(this string content, string key = "")
        {
            return Hash(content, "MD5", key);
        }

        public static string MD5(this byte[] content, string key = "")
        {
            return Hash(content, "MD5", key);
        }
        #endregion

        #region MD5_16
        public static string MD5_16(this string content)
        {
            byte[] data = Encoding.Default.GetBytes(content);
            HashAlgorithm hash = HashAlgorithm.Create("MD5");
            return BitConverter.ToString(hash.ComputeHash(data), 4, 8).Replace("-", "");
        }

        public static string MD5_16(this byte[] content)
        {
            byte[] data = content;
            HashAlgorithm hash = HashAlgorithm.Create("MD5");
            return BitConverter.ToString(hash.ComputeHash(data), 4, 8).Replace("-", "");
        }
        #endregion

        #region DES加解密
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string DESDecrypt(string data, byte[] key, byte[] iv)
        {
            string result = string.Empty;
            byte[] bytes = Convert.FromBase64String(data);
            using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider()
            {
                Key = key,
                IV = iv
            })
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    using (CryptoStream crypto = new CryptoStream(memory, provider.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        crypto.Write(bytes, 0, bytes.Length);
                        crypto.FlushFinalBlock();
                    }
                    result = Encoding.UTF8.GetString(memory.ToArray());
                }
                return result;
            }
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string DesEncrypt(string data, byte[] key, byte[] iv)
        {
            string result = string.Empty;
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            using (DESCryptoServiceProvider provider = new DESCryptoServiceProvider()
            {
                Key = key,
                IV = iv
            })
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    using (CryptoStream crypto = new CryptoStream(memory, provider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        crypto.Write(bytes, 0, bytes.Length);
                        crypto.FlushFinalBlock();
                    }
                    result = Convert.ToBase64String(memory.ToArray());
                }
                return result;
            }
        }
        #endregion

        #region AES加解密
        #endregion
    }
}
