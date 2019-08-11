using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CreateOTA
{
    public class ClientInfo
    {
        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 安装程序对应GUID
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// 客户端主文件
        /// </summary>
        public string MainFile { get; set; }
        /// <summary>
        /// 文件列表
        /// </summary>
        public List<string> FileList { get; set; }
        /// <summary>
        /// 公共文件列表
        /// </summary>
        public List<string> CommonFiles { get; set; }
        /// <summary>
        /// 文件夹路径
        /// </summary>
        public string DirPath { get; set; }
        /// <summary>
        /// 安装包路径
        /// </summary>
        public string SetupPath { get; set; }
        /// <summary>
        /// 没有安装包路径
        /// </summary>
        public bool NoSetup { get; set; }
        /// <summary>
        /// 获得所有文件列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFileList()
        {
            List<string> list = new List<string>();

            if (FileList != null)
                foreach (string fileName in FileList)
                {
                    if (fileName.EndsWith("*"))
                    {
                        var fpaths = Directory.GetFiles(Path.Combine(DirPath, fileName.TrimEnd('*')), "*.*", SearchOption.AllDirectories);
                        foreach (string fpath in fpaths)
                        {
                            list.Add(fpath.Substring(DirPath.Length + 1));
                        }
                    }
                    else
                    {
                        list.Add(fileName);
                    }
                }
            if (!string.IsNullOrWhiteSpace(MainFile))
                if (!list.Contains(MainFile))
                {
                    list.Add(MainFile);
                }

            return list;
        }
        /// <summary>
        /// 获得公共文件列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetCommonFileList()
        {
            List<string> list = new List<string>();
            if (CommonFiles != null)
            {
                foreach (string fileName in CommonFiles)
                {
                    if (fileName.EndsWith("*"))
                    {
                        var fpaths = Directory.GetFiles(Path.Combine(DirPath, fileName.TrimEnd('*')), "*.*", SearchOption.AllDirectories);
                        foreach (string fpath in fpaths)
                        {
                            list.Add(fpath.Substring(DirPath.Length + 1));
                        }
                    }
                    else
                    {
                        list.Add(fileName);
                    }
                }
            }
            return list;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
