using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace CreateOTA
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("Newtonsoft.Json"))
            {
                return Assembly.Load(Properties.Resources.Newtonsoft_Json);
            }
            return null;
        }

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
    }
}
