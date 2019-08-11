namespace CreateOTA
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnCreateOTA = new System.Windows.Forms.Button();
            this.txtUpdateContent = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFileDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectDir = new System.Windows.Forms.Button();
            this.cmbClientType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSetupPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSelectSetup = new System.Windows.Forms.Button();
            this.txtVer = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCreateOTA
            // 
            this.btnCreateOTA.Location = new System.Drawing.Point(290, 323);
            this.btnCreateOTA.Name = "btnCreateOTA";
            this.btnCreateOTA.Size = new System.Drawing.Size(75, 23);
            this.btnCreateOTA.TabIndex = 0;
            this.btnCreateOTA.Text = "生成OTA";
            this.btnCreateOTA.UseVisualStyleBackColor = true;
            // 
            // txtUpdateContent
            // 
            this.txtUpdateContent.Location = new System.Drawing.Point(17, 117);
            this.txtUpdateContent.Multiline = true;
            this.txtUpdateContent.Name = "txtUpdateContent";
            this.txtUpdateContent.Size = new System.Drawing.Size(348, 200);
            this.txtUpdateContent.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "↓更新日志↓";
            // 
            // txtFileDir
            // 
            this.txtFileDir.Location = new System.Drawing.Point(98, 37);
            this.txtFileDir.Name = "txtFileDir";
            this.txtFileDir.ReadOnly = true;
            this.txtFileDir.Size = new System.Drawing.Size(210, 21);
            this.txtFileDir.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "生成文件夹：";
            // 
            // btnSelectDir
            // 
            this.btnSelectDir.Location = new System.Drawing.Point(310, 36);
            this.btnSelectDir.Name = "btnSelectDir";
            this.btnSelectDir.Size = new System.Drawing.Size(55, 23);
            this.btnSelectDir.TabIndex = 7;
            this.btnSelectDir.Text = "浏览";
            this.btnSelectDir.UseVisualStyleBackColor = true;
            // 
            // cmbClientType
            // 
            this.cmbClientType.FormattingEnabled = true;
            this.cmbClientType.Location = new System.Drawing.Point(98, 10);
            this.cmbClientType.Name = "cmbClientType";
            this.cmbClientType.Size = new System.Drawing.Size(210, 20);
            this.cmbClientType.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "客户端类型：";
            // 
            // txtSetupPath
            // 
            this.txtSetupPath.Location = new System.Drawing.Point(98, 64);
            this.txtSetupPath.Name = "txtSetupPath";
            this.txtSetupPath.ReadOnly = true;
            this.txtSetupPath.Size = new System.Drawing.Size(210, 21);
            this.txtSetupPath.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "安装文件包：";
            // 
            // btnSelectSetup
            // 
            this.btnSelectSetup.Location = new System.Drawing.Point(310, 63);
            this.btnSelectSetup.Name = "btnSelectSetup";
            this.btnSelectSetup.Size = new System.Drawing.Size(55, 23);
            this.btnSelectSetup.TabIndex = 7;
            this.btnSelectSetup.Text = "浏览";
            this.btnSelectSetup.UseVisualStyleBackColor = true;
            // 
            // txtVer
            // 
            this.txtVer.BackColor = System.Drawing.Color.White;
            this.txtVer.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.txtVer.Location = new System.Drawing.Point(98, 91);
            this.txtVer.Name = "txtVer";
            this.txtVer.ReadOnly = true;
            this.txtVer.Size = new System.Drawing.Size(120, 21);
            this.txtVer.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 356);
            this.Controls.Add(this.txtVer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbClientType);
            this.Controls.Add(this.btnSelectSetup);
            this.Controls.Add(this.btnSelectDir);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSetupPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFileDir);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUpdateContent);
            this.Controls.Add(this.btnCreateOTA);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OTA创建工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateOTA;
        private System.Windows.Forms.TextBox txtUpdateContent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFileDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectDir;
        private System.Windows.Forms.ComboBox cmbClientType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSetupPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSelectSetup;
        private System.Windows.Forms.TextBox txtVer;
    }
}

