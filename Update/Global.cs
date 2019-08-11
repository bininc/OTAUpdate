using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Update;


public class Global
{
    //public const string UpdateUrl = "http://125.71.215.66:81/updateApi/api/update";   //四川天地华宏(车辆调派)更新地址
    public const string UpdateUrl = "http://service.cyhkgps.com/updateApi/api/update";  //本公司 更新地址
}

public class OtaInfo
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

public class OtaFile
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

