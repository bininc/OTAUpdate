using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Text;

namespace CreateOTA
{
    public partial class FormMain : Form
    {
        private const string ConfigFile = "AppConfig.json";
        public FormMain()
        {
            InitializeComponent();
            btnSelectDir.Click += btnSelectDir_Click;
            btnSelectSetup.Click += BtnSelectSetup_Click;
            btnCreateOTA.Click += btnCreateOTA_Click;
            cmbClientType.SelectedIndexChanged += CmbClientType_SelectedIndexChanged;
            Text = $"{Text} - {System.Reflection.Assembly.GetEntryAssembly().GetName().Version}";
        }

        private void CmbClientType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClientInfo client = cmbClientType.SelectedValue as ClientInfo;
            if (client != null)
            {
                txtFileDir.Text = client.DirPath;
                txtSetupPath.Text = client.SetupPath;
                if (string.IsNullOrWhiteSpace(txtFileDir.Text)) return;

                fileSystemWatcher.EnableRaisingEvents = false;
                fileSystemWatcher.Path = txtFileDir.Text;
                fileSystemWatcher.EnableRaisingEvents = true;

                if (!string.IsNullOrWhiteSpace(txtFileDir.Text))
                {
                    string clientPath = Path.Combine(txtFileDir.Text, client.MainFile);

                    if (File.Exists(clientPath))
                        txtVer.Text = "V" + Program.GetAppVersion(clientPath);
                    else
                    {
                        txtFileDir.Text = client.DirPath = txtVer.Text = "";
                        fileSystemWatcher.EnableRaisingEvents = false;
                    }
                }
            }
        }

        void btnCreateOTA_Click(object sender, EventArgs e)
        {
            string verPath = null;
            try
            {
                ClientInfo client = cmbClientType.SelectedValue as ClientInfo;
                if (client != null)
                {
                    if (string.IsNullOrWhiteSpace(txtFileDir.Text) || string.IsNullOrWhiteSpace(txtVer.Text))
                    {
                        MessageBox.Show(this, "请先选择有效的生成文件夹！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtFileDir.Focus();
                        return;
                    }
                    if (!client.NoSetup && string.IsNullOrWhiteSpace(txtSetupPath.Text))
                    {
                        MessageBox.Show(this, "请先选择安装包！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtSetupPath.Focus();
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(txtUpdateContent.Text))
                    {
                        MessageBox.Show(this, "请输入更新内容！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        txtUpdateContent.Focus();
                        return;
                    }

                    //string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    string desktopPath = Application.StartupPath;
                    string softDir = client.Name + "_OTA";
                    string verDir = txtVer.Text;
                    verPath = Path.Combine(desktopPath, softDir, verDir);
                    string clientPath = Path.Combine(verPath, client.ID);
                    string dateDir = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string datePath = Path.Combine(clientPath, dateDir);
                    string otaPath = Path.Combine(datePath, "ota");
                    string commonPath = Path.Combine(verPath, "Common");
                    string commonFilePath = Path.Combine(commonPath, "Files");
                    Directory.CreateDirectory(otaPath);

                    File.WriteAllText(Path.Combine(clientPath, "Name.txt"), client.Name);
                    File.WriteAllText(Path.Combine(clientPath, "GUID.txt"), client.GUID);

                    List<string> commonList = client.GetCommonFileList();
                    if (commonList.Any())
                    {
                        Directory.CreateDirectory(commonFilePath);
                        foreach (string filename in commonList)
                        {
                            string commChilPath = Path.Combine(commonFilePath, filename);
                            Directory.CreateDirectory(Path.GetDirectoryName(commChilPath));
                            File.Copy(Path.Combine(txtFileDir.Text, filename), commChilPath, true);
                        }
                    }
                    File.WriteAllText(Path.Combine(commonPath, client.ID), JsonConvert.SerializeObject(commonList), Encoding.UTF8);

                    foreach (string filename in client.GetAllFileList()) //复制子文件
                    {
                        string otaChilPath = Path.Combine(otaPath, filename);
                        Directory.CreateDirectory(Path.GetDirectoryName(otaChilPath));
                        File.Copy(Path.Combine(txtFileDir.Text, filename), otaChilPath, true);
                    }

                    if (!client.NoSetup)
                        File.Copy(txtSetupPath.Text, Path.Combine(datePath, "Setup.exe"), true); //复制安装包

                    string writeText = "版本：" + txtVer.Text.Substring(1) + Environment.NewLine + Environment.NewLine + "更新说明：" + Environment.NewLine + txtUpdateContent.Text;
                    File.WriteAllText(Path.Combine(datePath, "Version.txt"), writeText); //生成版本信息
                    File.WriteAllText(Path.Combine(datePath, "MainFile.txt"), client.MainFile); //生成主文件信息
                    SaveConfig(client);

                    MessageBox.Show(this, "生成成功！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start(Path.Combine(desktopPath, softDir));
                }
            }
            catch (Exception ex)
            {
                if (verPath != null)
                    Directory.Delete(verPath, true);
                MessageBox.Show(this, "OTA生成失败，请检查文件夹文件是否完整！\n" + ex.Message, "请注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void btnSelectDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                txtFileDir.Text = fbd.SelectedPath;
                if (string.IsNullOrWhiteSpace(txtFileDir.Text)) return;

                fileSystemWatcher.EnableRaisingEvents = false;
                fileSystemWatcher.Path = txtFileDir.Text;
                fileSystemWatcher.EnableRaisingEvents = true;

                ClientInfo client = cmbClientType.SelectedValue as ClientInfo;
                if (client != null)
                {
                    client.DirPath = txtFileDir.Text;
                    string clientPath = Path.Combine(txtFileDir.Text, client.MainFile);

                    if (File.Exists(clientPath))
                        txtVer.Text = "V" + Program.GetAppVersion(clientPath);
                    else
                    {
                        txtFileDir.Text = client.DirPath = txtVer.Text = "";
                        fileSystemWatcher.EnableRaisingEvents = false;
                    }
                }
            }
        }

        private FileSystemWatcher _fileSystemWatcher;
        private FileSystemWatcher fileSystemWatcher
        {
            get
            {
                if (_fileSystemWatcher == null)
                {
                    _fileSystemWatcher = new FileSystemWatcher();
                    _fileSystemWatcher.Changed += _fileSystemWatcher_Changed;
                    _fileSystemWatcher.Created += _fileSystemWatcher_Changed;
                    _fileSystemWatcher.Deleted += _fileSystemWatcher_Changed;
                    _fileSystemWatcher.Renamed += _fileSystemWatcher_Changed;
                    _fileSystemWatcher.EnableRaisingEvents = false;
                }
                return _fileSystemWatcher;
            }
        }

        private void _fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFileDir.Text)) return;

            cmbClientType.Invoke(new Action(() =>
            {
                ClientInfo client = cmbClientType.SelectedValue as ClientInfo;
                if (client != null)
                {
                    if (e.Name == client.MainFile)
                    {
                        //主文件变化
                        string clientPath = Path.Combine(txtFileDir.Text, client.MainFile);

                        if (File.Exists(clientPath))
                            txtVer.Text = "V" + Program.GetAppVersion(clientPath);
                    }
                }
            }));
        }

        private void BtnSelectSetup_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.CheckFileExists = true;
            ofd.AutoUpgradeEnabled = true;
            ofd.Filter = "安装程序|*.exe";
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                txtSetupPath.Text = ofd.FileName;
                if (!string.IsNullOrWhiteSpace(txtSetupPath.Text))
                {
                    ClientInfo client = cmbClientType.SelectedValue as ClientInfo;
                    if (client != null)
                    {
                        client.SetupPath = txtSetupPath.Text;
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cmbClientType.DataSource = ReadConfig();
        }

        private List<ClientInfo> ReadConfig()
        {
            if (File.Exists(ConfigFile))
            {
                return JsonConvert.DeserializeObject<List<ClientInfo>>(File.ReadAllText(ConfigFile));
            }
            return null;
        }

        private void SaveConfig(ClientInfo client)
        {
            if (client == null) return;

            List<ClientInfo> clients = null;
            if (File.Exists(ConfigFile))
            {
                clients = ReadConfig();
                ClientInfo clientold = clients?.FirstOrDefault(c => c.Name == client.Name);
                if (clientold != null)
                {
                    clients.Remove(clientold);
                }
                clients?.Add(client);
            }
            if (clients == null)
                clients = new List<ClientInfo> { client };

            string str = JsonConvert.SerializeObject(clients, Formatting.Indented);
            File.WriteAllText(ConfigFile, str);
        }
    }
}
