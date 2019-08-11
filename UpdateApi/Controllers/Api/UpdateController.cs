using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Hosting;
using System.Web.Http;
using Newtonsoft.Json;
using UpdateApi.Class;
using System.Text;

namespace UpdateApi.Controllers.Api
{
    public class UpdateController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(string id)
        {
            try
            {
                string rootPath = HostingEnvironment.MapPath("~");
                string updatePath = Path.Combine(rootPath, "Update", id);
                if (!Directory.Exists(updatePath))
                    return "err_" + "请求的路径无效";

                string[] ups = Directory.GetDirectories(updatePath, "*", SearchOption.TopDirectoryOnly);
                string newdir = ups.Max();
                if (string.IsNullOrWhiteSpace(newdir))
                    return null;

                string upStr = null;
                string cachePath = Path.Combine(newdir, "ota", "CacheUpdate");
                if (File.Exists(cachePath))
                    upStr = File.ReadAllText(cachePath);
                else
                {
                    OtaInfo otaInfo = new OtaInfo() { AppID = id };
                    otaInfo.AppName = File.ReadAllText(Path.Combine(updatePath, "Name.txt"));
                    otaInfo.AppGUID = File.ReadAllText(Path.Combine(updatePath, "GUID.txt"));

                    DateTime upTime = DateTime.ParseExact(Path.GetFileNameWithoutExtension(newdir), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    otaInfo.AppVerTime = upTime;
                    string[] lines = File.ReadAllLines(Path.Combine(newdir, "Version.txt"));
                    string newVersion = lines[0].Substring(3);
                    otaInfo.AppVersion = newVersion;
                    otaInfo.AppVerText = string.Join("\n", lines);
                    otaInfo.MainFile = File.ReadAllText(Path.Combine(newdir, "MainFile.txt"));
                    otaInfo.SetupUrl = LocalPath2WebPath(Path.Combine(newdir, "Setup.exe"));
                    otaInfo.OtaFiles = new List<OtaFile>();

                    List<string> otas = Directory.GetFiles(Path.Combine(newdir, "ota"), "*", SearchOption.AllDirectories).ToList();
                    if (File.Exists(Path.Combine(rootPath, "Update", "Common", id)))
                    {
                        string[] commFileNames = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(Path.Combine(rootPath, "Update", "Common", id), Encoding.UTF8));
                        for (int i = 0; i < commFileNames.Length; i++)
                        {
                            otas.Add(Path.Combine(rootPath, "Update", "Common", "Files", commFileNames[i]));
                        }
                    }

                    for (int i = 0; i < otas.Count; i++)
                    {
                        string otaFilePath = otas[i];
                        OtaFile file = new OtaFile();
                        file.FileName = Path.GetFileName(otaFilePath);
                        file.FileMd5 = GetMd5ByFilePath2(otaFilePath);
                        file.DownloadUrl = LocalPath2WebPath(otaFilePath);
                        string tmpPath = Path.Combine(newdir, "ota");
                        if (otaFilePath.StartsWith(tmpPath))
                        {
                            file.RelativePath = otaFilePath.Substring(tmpPath.Length);
                        }
                        else
                        {
                            tmpPath = Path.Combine(rootPath, "Update", "Common", "Files");
                            if (otaFilePath.StartsWith(tmpPath))
                            {
                                file.RelativePath = otaFilePath.Substring(tmpPath.Length);
                            }
                        }
                        otaInfo.OtaFiles.Add(file);
                    }

                    upStr = JsonConvert.SerializeObject(otaInfo);
                    upStr = upStr.ToBase64String();
                    File.WriteAllText(cachePath, upStr);
                }
                return upStr;
            }
            catch (Exception e)
            {
                return "err_" + e.Message;
            }
        }

        private string LocalPath2WebPath(string localPath)
        {
            int index = Request.RequestUri.AbsoluteUri.IndexOf("/api/");
            string webRoot = Request.RequestUri.AbsoluteUri.Substring(0, index + 1);
            string rootPath = HostingEnvironment.MapPath("~");

            return webRoot + localPath.Substring(rootPath.Length).Replace('\\', '/');
        }

        class OtaInfo
        {
            /// <summary>
            /// 应用程序ID
            /// </summary>
            public string AppID { get; set; }
            /// <summary>
            /// 应用名称
            /// </summary>
            public string AppName { get; set; }
            /// <summary>
            /// 应用程序入口
            /// </summary>
            public string MainFile { get; set; }
            /// <summary>
            /// 应用GUID
            /// </summary>
            public string AppGUID { get; set; }
            /// <summary>
            /// 应用版本
            /// </summary>
            public string AppVersion { get; set; }
            /// <summary>
            /// 应用更新说明
            /// </summary>
            public string AppVerText { get; set; }
            /// <summary>
            /// 应用更新日期
            /// </summary>
            public DateTime AppVerTime { get; set; }
            /// <summary>
            /// 安装包网站
            /// </summary>
            public string SetupUrl { get; set; }
            /// <summary>
            /// OTA文件列表
            /// </summary>
            public List<OtaFile> OtaFiles { get; set; }
        }

        class OtaFile
        {
            /// <summary>
            /// 文件名
            /// </summary>
            public string FileName { get; set; }
            /// <summary>
            /// 文件MD5
            /// </summary>
            public string FileMd5 { get; set; }
            /// <summary>
            /// 文件下载地址
            /// </summary>
            public string DownloadUrl { get; set; }
            /// <summary>
            /// 相对路径
            /// </summary>
            public string RelativePath { get; set; }
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

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}