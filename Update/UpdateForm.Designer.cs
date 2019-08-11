namespace Update
{
    partial class UpdateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateForm));
            this.lbl1 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.btnUpDate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.progressBarDownLoad = new System.Windows.Forms.ProgressBar();
            this.lblUpdateVer = new System.Windows.Forms.Label();
            this.lblcurrentVer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUpTime = new System.Windows.Forms.Label();
            this.lblFullUrl = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.listboxContent = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(145, 9);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(65, 12);
            this.lbl1.TabIndex = 1;
            this.lbl1.Text = "最新版本：";
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Location = new System.Drawing.Point(145, 32);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(65, 12);
            this.lbl2.TabIndex = 1;
            this.lbl2.Text = "当前版本：";
            // 
            // btnUpDate
            // 
            this.btnUpDate.Location = new System.Drawing.Point(358, 222);
            this.btnUpDate.Name = "btnUpDate";
            this.btnUpDate.Size = new System.Drawing.Size(125, 23);
            this.btnUpDate.TabIndex = 3;
            this.btnUpDate.Text = "开始更新";
            this.btnUpDate.UseVisualStyleBackColor = true;
            this.btnUpDate.Click += new System.EventHandler(this.btnUpDate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(363, 222);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // progressBarDownLoad
            // 
            this.progressBarDownLoad.Location = new System.Drawing.Point(147, 206);
            this.progressBarDownLoad.Name = "progressBarDownLoad";
            this.progressBarDownLoad.Size = new System.Drawing.Size(336, 10);
            this.progressBarDownLoad.Step = 1;
            this.progressBarDownLoad.TabIndex = 4;
            this.progressBarDownLoad.Value = 50;
            this.progressBarDownLoad.Visible = false;
            // 
            // lblUpdateVer
            // 
            this.lblUpdateVer.AutoSize = true;
            this.lblUpdateVer.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUpdateVer.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblUpdateVer.Location = new System.Drawing.Point(206, 9);
            this.lblUpdateVer.Name = "lblUpdateVer";
            this.lblUpdateVer.Size = new System.Drawing.Size(59, 12);
            this.lblUpdateVer.TabIndex = 5;
            this.lblUpdateVer.Text = "V 1.0.0.1";
            // 
            // lblcurrentVer
            // 
            this.lblcurrentVer.AutoSize = true;
            this.lblcurrentVer.ForeColor = System.Drawing.Color.LightSlateGray;
            this.lblcurrentVer.Location = new System.Drawing.Point(206, 32);
            this.lblcurrentVer.Name = "lblcurrentVer";
            this.lblcurrentVer.Size = new System.Drawing.Size(59, 12);
            this.lblcurrentVer.TabIndex = 5;
            this.lblcurrentVer.Text = "V 1.0.0.2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(321, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "发布日期：";
            // 
            // lblUpTime
            // 
            this.lblUpTime.AutoSize = true;
            this.lblUpTime.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUpTime.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblUpTime.Location = new System.Drawing.Point(382, 9);
            this.lblUpTime.Name = "lblUpTime";
            this.lblUpTime.Size = new System.Drawing.Size(101, 12);
            this.lblUpTime.TabIndex = 5;
            this.lblUpTime.Text = "2015-01-01 00:00";
            // 
            // lblFullUrl
            // 
            this.lblFullUrl.AutoSize = true;
            this.lblFullUrl.Location = new System.Drawing.Point(145, 227);
            this.lblFullUrl.Name = "lblFullUrl";
            this.lblFullUrl.Size = new System.Drawing.Size(89, 12);
            this.lblFullUrl.TabIndex = 7;
            this.lblFullUrl.TabStop = true;
            this.lblFullUrl.Text = "下载完整安装包";
            this.lblFullUrl.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(136, 248);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // listboxContent
            // 
            this.listboxContent.FormattingEnabled = true;
            this.listboxContent.ItemHeight = 12;
            this.listboxContent.Location = new System.Drawing.Point(147, 54);
            this.listboxContent.Name = "listboxContent";
            this.listboxContent.ScrollAlwaysVisible = true;
            this.listboxContent.Size = new System.Drawing.Size(336, 148);
            this.listboxContent.TabIndex = 8;
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 248);
            this.Controls.Add(this.listboxContent);
            this.Controls.Add(this.lblFullUrl);
            this.Controls.Add(this.lblcurrentVer);
            this.Controls.Add(this.lblUpTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblUpdateVer);
            this.Controls.Add(this.progressBarDownLoad);
            this.Controls.Add(this.btnUpDate);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "在线更新";
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.Button btnUpDate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ProgressBar progressBarDownLoad;
        private System.Windows.Forms.Label lblUpdateVer;
        private System.Windows.Forms.Label lblcurrentVer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUpTime;
        private System.Windows.Forms.LinkLabel lblFullUrl;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ListBox listboxContent;
    }
}