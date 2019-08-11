using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Update
{

    static class Program
    {
        public const string App_ID = "OTAUpdate";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool createNew; //只允许一个实例运行
            Mutex mutex = new Mutex(true, App_ID, out createNew);
            try
            {
                if (!createNew)
                {
                    MessageBox.Show("另一个更新程序正在运行，请等待！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                string currentFileFullName = Path.GetFileName(Application.ExecutablePath);
                string currentFileName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
                string currentFileExtension = Path.GetExtension(Application.ExecutablePath);
                string currentFile = (currentFileName.EndsWith("_")
                                         ? currentFileName.Remove(currentFileName.Length - 1)
                                         : currentFileName) + currentFileExtension;
                string updateFile = currentFileName.EndsWith("_")
                    ? currentFileFullName
                    : currentFileName + "_" + currentFileExtension;
                if (File.Exists(updateFile))
                {
                    Thread.Sleep(500);
                    if (Common.MD5File(currentFile) == Common.MD5File(updateFile))
                    {
                        if (!currentFileName.EndsWith("_"))
                            File.Delete(updateFile);
                    }
                    else
                    {
                        if (String.Compare(currentFileFullName, updateFile, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            File.Copy(updateFile, currentFile, true);
                            mutex.Close();
                            Process.Start(currentFile, "self_升级成功，请重新启动软件！");
                            Application.Exit();
                        }
                        else
                        {
                            mutex.Close();
                            Process.Start(updateFile, "self_update");
                            Application.Exit();
                        }
                        return;
                    }
                }

                string argstr = null;
                if (args.Length > 0)
                    argstr = string.Join("", args);

                if (string.IsNullOrWhiteSpace(argstr))
                {
                    ShareMemoryManager manager = new ShareMemoryManager(App_ID);
                    //读取共享内存中的数据：
                    //是否有数据写过来 等待10秒
                    argstr = manager.ReceiveString(new TimeSpan(0, 0, 10));
                    manager.Dispose();
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (!string.IsNullOrWhiteSpace(argstr))
                {
                    if (argstr.StartsWith("self_"))
                    {
                        if (argstr == "self_update") return;
                        MessageBox.Show(argstr.Substring(5), "升级完成", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        return;
                    }
                    else if (argstr.StartsWith("checkupdate_"))
                    {
                        string clientId = argstr.Substring(12);
                        string returnStr = Common.CheckUpdate(clientId);
                        //MessageBox.Show(returnStr);                              
                        Console.Write("end_" + returnStr);

                        // Environment.Exit(0);
                        return;
                    }

                    string errinfo = argstr.StartsWith("err_") ? argstr.Substring(4) : null;
                    if (!string.IsNullOrWhiteSpace(errinfo))
                        MessageBox.Show("更新错误！\n" + errinfo, "系统提示", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    else
                    {
                        UpdateForm uf = new UpdateForm(argstr);
                        if (uf.ShowDialog() != DialogResult.OK)
                        {
                            return;
                        }

                        Process.Start(uf.upresult.MainFile); // 运行软件
                    }
                }
                else
                    MessageBox.Show("请在应用程序中检查更新!", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "更新程序发生错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mutex.Close();
            }
        }

        public static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("Newtonsoft.Json"))
            {
                return System.Reflection.Assembly.Load(Properties.Resources.Newtonsoft_Json);
            }
            return null;
        }
    }
}
