using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace UpdateApi.Controllers
{
    public class SetupController : Controller
    {
        public FileResult Index(string client_id)
        {
            if (string.IsNullOrWhiteSpace(client_id)) return null;

            string updateRootDir = ConfigurationManager.AppSettings["UpdateRootDir"];
            if (string.IsNullOrWhiteSpace(updateRootDir))
                updateRootDir = Server.MapPath("~/Update");

            string updatePath = Path.Combine(updateRootDir, client_id);
            bool have = Directory.Exists(updatePath);
            if (!have) return null; //不存在目录

            string setup_name = "安装包.exe";
            string namePath = Path.Combine(updatePath, "Name.txt");
            if (System.IO.File.Exists(namePath))
                setup_name = System.IO.File.ReadAllText(namePath, Encoding.UTF8) + setup_name;

            string[] ups = Directory.GetDirectories(updatePath, "*", SearchOption.TopDirectoryOnly);
            if (ups.Length == 0) return null;   //不存在安装包

            string lastdir = ups.Max(); //最近更新文件夹
            string setupFilePath = Path.Combine(lastdir, "setup.exe");
            if (System.IO.File.Exists(setupFilePath))
                return File(setupFilePath, "application/octet-stream", setup_name);
            return null;
        }
    }
}