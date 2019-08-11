using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Update
{
    public class AppInfo
    {
        /// <summary>
        /// 主程序文件路径
        /// </summary>
        public string MainFileName { get; set; }
        /// <summary>
        /// 创建运行环境路径
        /// </summary>
        public string EnvInitFileName { get; set; }
    }

    class FileInfo
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径 相对于更新程序
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 文件MD5值
        /// </summary>
        public string FileMd5 { get; set; }
        /// <summary>
        /// 文件下载路径 更新时的临时路径
        /// </summary>
        public string DownLoadPath { get; set; }
        /// <summary>
        /// 文件下载URL
        /// </summary>
        public string DownLoadUrl { get; set; }

        /// <summary>
        /// 检查文件是否一致
        /// </summary>
        /// <returns></returns>
        public bool CheckFile()
        {
            if (FilePath == null) throw new ArgumentNullException("FilePath");
            if (FileMd5 == null) throw new ArgumentNullException("FileMd5");
            if (File.Exists(FilePath))
            {
                string localMd5 = Common.MD5File(FilePath);
                bool md5Same = String.Compare(FileMd5, localMd5, StringComparison.OrdinalIgnoreCase) == 0;
                if (md5Same)
                {
                    return true;
                }
            }
            return false;
        }

        public FileInfo(string otaUrl)
        {
            
        }
    }
}
