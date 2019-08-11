using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Update
{
    internal class Common
    {
        /// <summary>
        /// 获取程序版本号
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetAppVersion(string appPath)
        {
            FileVersionInfo ver = FileVersionInfo.GetVersionInfo(appPath);
            return ver.ProductVersion;
        }

        /// <summary>
        /// 获取根目录文件路径
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static string GetRootPath(string fileName)
        {
            string path = Application.StartupPath;
            return Path.Combine(path, fileName);
        }

        /// <summary>
        /// 计算文件的 MD5 值
        /// </summary>
        /// <param name="fileName">要计算 MD5 值的文件名和路径</param>
        /// <returns>MD5 值16进制字符串</returns>
        public static string MD5File(string fileName)
        {
            return GetMd5ByFilePath2(fileName);
        }

        /// <summary>  
        /// 通过HashAlgorithm的TransformBlock方法对流进行叠加运算获得MD5  
        /// 实现稍微复杂，但可使用与传输文件或接收文件时同步计算MD5值  
        /// 可自定义缓冲区大小，计算速度较快  
        /// </summary>  
        /// <param name="path">文件地址</param>  
        /// <returns>MD5Hash</returns>  
        public static string GetMd5ByFilePath2(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException($"{nameof(path)}<{path}>, 不存在");

            FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            string md5 = GetMd5ByStream2(fileStream);
            fileStream.Close();
            return md5;
        }

        /// <summary>  
        /// 通过HashAlgorithm的TransformBlock方法对流进行叠加运算获得MD5  
        /// 实现稍微复杂，但可使用与传输文件或接收文件时同步计算MD5值  
        /// 可自定义缓冲区大小，计算速度较快  
        /// </summary>  
        /// <param name="stream">字节流</param>  
        /// <returns>MD5Hash</returns>  
        public static string GetMd5ByStream2(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            int bufferSize = 1024 * 16;//自定义缓冲区大小16K  
            byte[] buffer = new byte[bufferSize];
            HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
            int readLength = 0;//每次读取长度 
            stream.Position = 0;    //从头开始读 
            var output = new byte[bufferSize];
            while ((readLength = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                //计算MD5  
                hashAlgorithm.TransformBlock(buffer, 0, readLength, output, 0);
            }
            //完成最后计算，必须调用(由于上一部循环已经完成所有运算，所以调用此方法时后面的两个参数都为0)  
            hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
            string md5 = BitConverter.ToString(hashAlgorithm.Hash);
            hashAlgorithm.Clear();
            md5 = md5.Replace("-", "");
            return md5;
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        /// <returns></returns>
        internal static string CheckUpdate(string clientid)
        {
            WebClient wc = new WebClient();
            try
            {
                wc.Headers.Add("Cache-control", "no-cache");
                wc.Headers.Add("User-Agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; Trident/6.0)");
                wc.Encoding = Encoding.UTF8;
                string url = Global.UpdateUrl + "/" + clientid;
                url = Uri.EscapeUriString(url);
                string jsonStr = wc.DownloadString(url);
                if (jsonStr == "null") jsonStr = null;
                jsonStr = jsonStr?.Trim('"');
                if (!string.IsNullOrWhiteSpace(jsonStr) && !jsonStr.StartsWith("err_"))
                {
                    OtaInfo info = JsonConvert.DeserializeObject<OtaInfo>(jsonStr.FromBase64String());
                    if (string.Compare(GetAppVersion(info.MainFile), info.AppVersion, StringComparison.Ordinal) >= 0) //没有更新
                        jsonStr = string.Empty;
                }

                return jsonStr;
            }
            catch (Exception e)
            {
                return "err_" + e.Message;
            }
        }

        /// <summary>
        /// 写一个文本文件（自动覆盖已有的）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path"></param>
        public static void WriteFile(string value, string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);

                FileStream fs = File.Create(path);
                byte[] bsVal = Encoding.UTF8.GetBytes(value);
                fs.Write(bsVal, 0, bsVal.Length);
                fs.Close();
            }
            catch
            {
            }
        }
    }
}

