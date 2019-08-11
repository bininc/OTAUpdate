using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Update
{
    public partial class UpdateForm : Form
    {
        /// <summary>
        /// 更新信息
        /// </summary>
        public OtaInfo upresult;
        /// <summary>
        /// 是否更新自己
        /// </summary>
        bool updateSelf = false;
        /// <summary>
        /// 需要更新的OTA文件
        /// </summary>
        private List<NeedOtaFile> needOtaFiles;

        public bool Updated { get { return needOtaFiles != null && needOtaFiles.Any(); } }

        public UpdateForm(string upinfo)
        {
            InitializeComponent();
            upresult = JsonConvert.DeserializeObject<OtaInfo>(upinfo.FromBase64String());
            if (upresult == null)
            {
                throw new Exception("检查更新失败，请重试！");
            }
            else
            {
                Text += " - " + upresult.AppName;
            }
            lblFullUrl.LinkClicked += LblFullUrl_LinkClicked;
        }

        private void LblFullUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Clipboard.SetText(upresult.SetupUrl);
            }
            catch
            {
                Clipboard.SetText(upresult.SetupUrl);
            }
            Process.Start("iexplore.exe", upresult.SetupUrl);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CheckUpdate();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Activate();
        }

        void CheckUpdate()
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();

                string currentVer = Common.GetAppVersion(upresult.MainFile);

                if (string.Compare(currentVer, upresult.AppVersion, StringComparison.Ordinal) < 0)  //有更新
                {
                    needOtaFiles = CheckOtaFiles(upresult.OtaFiles);
                    lblFullUrl.Visible = true;
                    lblUpdateVer.Text = "V" + upresult.AppVersion;
                    lblUpTime.Text = upresult.AppVerTime.ToString("yyyy-MM-dd HH:mm");
                    lblcurrentVer.Text = "V" + currentVer;
                    listboxContent.DataSource = upresult.AppVerText.Split('\n');
                    this.Show();
                    this.WindowState = FormWindowState.Normal;
                    return;
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private class NeedOtaFile
        {
            private string _status;

            public string otaUrl { get; set; }

            public string fileName { get; set; }
            public string md5 { get; set; }
            string localPath1;
            public string localPath
            {
                get
                {
                    return localPath1;
                }
                set
                {
                    if (value != null)
                    {
                        localPath1 = Common.GetRootPath(value);
                        downloadPath = Common.GetRootPath("ota\\" + value);
                    }
                }
            }
            public string downloadPath { get; private set; }

            public int index { get; set; }

            public string status
            {
                get { return _status; }
                set
                {
                    _status = value;
                }
            }

            public bool CheckMd5()
            {
                if (localPath == null) throw new ArgumentNullException("localPath");
                if (md5 == null) throw new ArgumentNullException("md5");
                
                if (File.Exists(localPath))
                {
                    string localMd5 = Common.MD5File(localPath);
                    bool md5Same = String.Compare(md5, localMd5, StringComparison.OrdinalIgnoreCase) == 0;
                    if (md5Same)
                    {
                        return true;
                    }
                }
                return false;
            }

            public override string ToString()
            {
                return fileName.PadRight(40, ' ') + status;
            }
        }

        private List<NeedOtaFile> CheckOtaFiles(List<OtaFile> listOtaFiles)
        {
            if (listOtaFiles == null) return null;

            List<NeedOtaFile> needOtaFiles = new List<NeedOtaFile>();
            for (var i = 0; i < listOtaFiles.Count; i++)
            {
                OtaFile otaFile = listOtaFiles[i];
                NeedOtaFile needOtaFile = new NeedOtaFile();
                needOtaFile.fileName = otaFile.FileName;
                needOtaFile.otaUrl = otaFile.DownloadUrl;
                needOtaFile.localPath = otaFile.RelativePath.TrimStart('\\');
                needOtaFile.md5 = otaFile.FileMd5;
                if (!needOtaFile.CheckMd5())
                {   //需要更新
                    needOtaFile.index = needOtaFiles.Count;
                    needOtaFiles.Add(needOtaFile);
                }
            }

            return needOtaFiles;
        }

        private void btnUpDate_Click(object sender, EventArgs e)
        {
            var process = Process.GetProcesses();
            Process pc = process.FirstOrDefault(p => p.ProcessName == Path.GetFileNameWithoutExtension(upresult.MainFile));
            pc?.Kill(); //杀死软件进程

            this.btnUpDate.Text = "更新中";
            this.btnUpDate.Enabled = false;
            listboxContent.DataSource = needOtaFiles;
            DownLoadOTAFile(0);
        }

        void DownLoadOTAFile(int i)
        {
            try
            {
                if (i < needOtaFiles.Count)
                {
                    progressBarDownLoad.Visible = true;
                    progressBarDownLoad.Value = 0;
                    if (i > 0)
                    {
                        needOtaFiles[i - 1].status = "已完成";
                    }
                    needOtaFiles[i].status = "更新中...";
                    listboxContent.DataSource = null;
                    listboxContent.DataSource = needOtaFiles;
                    listboxContent.SelectedIndex = i;

                    backgroundWorker1.RunWorkerAsync(needOtaFiles[i]);
                }
                else
                {
                    progressBarDownLoad.Visible = false;
                    //更新完成
                    if (Directory.Exists(Common.GetRootPath("ota")))
                    {
                        string[] dirs = Directory.GetDirectories(Common.GetRootPath("ota"), "*", SearchOption.AllDirectories);
                        string[] files = Directory.GetFiles(Common.GetRootPath("ota"), "*", SearchOption.AllDirectories);
                        foreach (string dir in dirs)
                        {
                            string newPath = dir.Replace("ota\\", "");
                            if (!Directory.Exists(newPath))
                                Directory.CreateDirectory(newPath);
                        }
                        foreach (string file in files)
                        {
                            string newPath = file.Replace("ota\\", "");
                            if (String.Compare(Application.ExecutablePath, newPath, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                newPath = Path.GetFileNameWithoutExtension(newPath) + "_" + Path.GetExtension(newPath);
                                updateSelf = true;
                            }
                            if (File.Exists(newPath))
                                File.Delete(newPath);
                            File.Move(file, newPath);
                        }
                        Directory.Delete(Common.GetRootPath("ota"), true);
                    }
                    //注册表更新版本信息
                    RegistryKey rk = Registry.LocalMachine;
                    RegistryKey rk1 = rk.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
                    RegistryKey rk2 = rk.CreateSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall");

                    string path = upresult.AppGUID + "_is1";
                    RegistryKey rkApp = null;
                    if (rk1?.OpenSubKey(path) == null)
                    {   //不存在
                        if (rk2?.OpenSubKey(path) == null)
                        {
                            //不存在 放弃更改
                        }
                        else
                        {   //存在
                            rkApp = rk2.CreateSubKey(path);
                        }
                    }
                    else
                    {   //存在
                        rkApp = rk1.CreateSubKey(path);
                    }
                    if (rkApp != null)
                    {
                        rkApp.SetValue("DisplayName", upresult.AppName + " 版本 " + upresult.AppVersion, RegistryValueKind.String);
                        rkApp.SetValue("DisplayVersion", upresult.AppVersion, RegistryValueKind.String);
                        rkApp.Close();
                    }
                    rk1?.Close();
                    rk2?.Close();
                    rk.Close();

                    if (updateSelf)
                    {
                        Process.Start(Path.GetFileNameWithoutExtension(Application.ExecutablePath) + "_" + Path.GetExtension(Application.ExecutablePath), "self_update");
                        Application.Exit();
                    }
                    else
                        this.DialogResult = DialogResult.OK;
                }

            }
            catch (Exception ex)
            {
                File.WriteAllText("last_err.log", ex.Message);
                this.btnUpDate.Text = "请重试";
                this.btnUpDate.Enabled = true;
            }
        }

        /// <summary>
        /// 下载方法
        /// </summary>
        private void DownLoad(NeedOtaFile needOtaFile)
        {
            if (needOtaFile == null) throw new ArgumentNullException(nameof(needOtaFile));
            //比如uri=http://localhost/Rabom/1.rar;iis就需要自己配置了。
            string uri = needOtaFile.otaUrl;
            //截取文件名
            string fileName = needOtaFile.fileName;
            //构造文件完全限定名,准备将网络流下载为本地文件
            string fileFullName = needOtaFile.downloadPath;
            string downloadDir = Path.GetDirectoryName(fileFullName);
            if (!Directory.Exists(downloadDir))
                Directory.CreateDirectory(downloadDir);
            //构造文件的配置文件的完全完全限定名
            string fileCfgName = needOtaFile.downloadPath + ".cfg";

            //请求地址
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            WebResponse response = request.GetResponse();

            long fileLength = response.ContentLength;   //获得文件长度
            response.Close();

            long savePosition = 0;
            //本地构造文件流
            FileStream fs;
            FileStream fscfg;
            if (File.Exists(fileFullName))
            {
                string localmd5 = Common.MD5File(fileFullName);
                if (localmd5 == needOtaFile.md5) return;

                fs = new FileStream(fileFullName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                //如果存在配置文件，则继续下载
                if (File.Exists(fileCfgName))
                {
                    fscfg = new FileStream(fileCfgName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                    byte[] buf = new byte[fscfg.Length];
                    int len = fscfg.Read(buf, 0, buf.Length);
                    string otaText = Encoding.UTF8.GetString(buf, 0, len);
                    string[] cfgs = otaText.Split('|');
                    if (cfgs.Length == 4 && cfgs[0] == fileName && cfgs[1] == fileLength.ToString() && cfgs[2] == needOtaFile.md5)
                    {   //ota文件校验正确
                        savePosition = Convert.ToInt64(cfgs[3]);
                    }
                    else
                    {
                        string cfg = fileName + "|" + fileLength + "|" + needOtaFile.md5 + "|" + savePosition;
                        fscfg.Position = 0;
                        buf = Encoding.UTF8.GetBytes(cfg);
                        fscfg.Write(buf, 0, buf.Length);
                        fscfg.Flush();  //创建配置文件
                    }
                }
                else
                {
                    fscfg = new FileStream(fileCfgName, FileMode.Create);
                    string cfg = fileName + "|" + fileLength + "|" + needOtaFile.md5 + "|" + savePosition;
                    byte[] buf = Encoding.UTF8.GetBytes(cfg);
                    fscfg.Write(buf, 0, buf.Length);
                    fscfg.Flush();  //创建配置文件
                }
            }
            else
            {
                fs = new FileStream(fileFullName, FileMode.Create);

                if (File.Exists(fileCfgName))
                    File.Delete(fileCfgName);
                string cfg = fileName + "|" + fileLength + "|" + needOtaFile.md5 + "|" + savePosition;
                fscfg = new FileStream(fileCfgName, FileMode.Create);
                byte[] buf = Encoding.UTF8.GetBytes(cfg);
                fscfg.Write(buf, 0, buf.Length);
                fscfg.Flush();  //创建配置文件
            }

        down:
            //开辟内存空间
            byte[] buffer = new byte[1024];
            request = (HttpWebRequest)WebRequest.Create(uri);
            //请求开始位置
            request.AddRange(savePosition, fileLength);
            fs.Position = savePosition;
            //获取网络流
            Stream ns = request.GetResponse().GetResponseStream();
            do
            {
                //获取文件读取到的长度
                int length = ns.Read(buffer, 0, buffer.Length);
                if (length > 0)
                {
                    //将字节数组写入流
                    fs.Write(buffer, 0, length);
                    fs.Flush(); //写入文件
                    savePosition += length;
                    string cfg = fileName + "|" + fileLength + "|" + needOtaFile.md5 + "|" + savePosition;
                    fscfg.Position = 0;
                    byte[] buf = Encoding.UTF8.GetBytes(cfg);
                    fscfg.Write(buf, 0, buf.Length);
                    fscfg.Flush();  //创建配置文件

                    int percent = (int)(Math.Round((double)savePosition / fileLength, 2) * 100);
                    backgroundWorker1.ReportProgress(percent);
                }
                else
                    break;  //下载完成
            } while (true);

            fs.Close();
            string md5 = Common.MD5File(fileFullName);
            if (md5 != needOtaFile.md5)
            {
                //文件下载错误
                fs = new FileStream(fileFullName, FileMode.Create);
                savePosition = 0;
                string cfg = fileName + "|" + fileLength + "|" + needOtaFile.md5 + "|" + savePosition;
                fscfg.Position = 0;
                byte[] buf = Encoding.UTF8.GetBytes(cfg);
                fscfg.Write(buf, 0, buf.Length);
                fscfg.Flush();  //创建配置文件
                goto down;
            }
            else
            {
                ns.Close();
                fscfg.Close();
                File.Delete(fileCfgName);
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            NeedOtaFile needOtaFile = e.Argument as NeedOtaFile;
            if (needOtaFile == null) return;
            DownLoad(needOtaFile);
            e.Result = needOtaFile.index + 1;
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBarDownLoad.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
                DownLoadOTAFile(Convert.ToInt32(e.Result));
            else
                DownLoadOTAFile(0);
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {

        }
    }
}
