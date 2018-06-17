namespace ASP.NET_Release_Tool
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnReleaseDemoBeta = new System.Windows.Forms.Button();
            this.btnRestoreStable = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPathRootSource = new System.Windows.Forms.TextBox();
            this.btnPathRootSource = new System.Windows.Forms.Button();
            this.btnPathRootProductionClean = new System.Windows.Forms.Button();
            this.txtPathRootProductionClean = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPathRootProductionLive = new System.Windows.Forms.Button();
            this.txtPathRootProductionLive = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnPathRootStable = new System.Windows.Forms.Button();
            this.txtPathRootStable = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPathRootBackups = new System.Windows.Forms.Button();
            this.txtPathRootBackups = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnRestoreBackup = new System.Windows.Forms.Button();
            this.btnPathConfigStable = new System.Windows.Forms.Button();
            this.txtPathConfigStable = new System.Windows.Forms.TextBox();
            this.btnPathConfigProductionLive = new System.Windows.Forms.Button();
            this.txtPathConfigProductionLive = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.progressBarTotal = new System.Windows.Forms.ProgressBar();
            this.progressBarCurrent = new System.Windows.Forms.ProgressBar();
            this.lblProgressCurrent = new System.Windows.Forms.Label();
            this.lblProgressTotal = new System.Windows.Forms.Label();
            this.btnPathConfigDemoBeta = new System.Windows.Forms.Button();
            this.txtPathConfigDemoBeta = new System.Windows.Forms.TextBox();
            this.btnPathRootDemoBeta = new System.Windows.Forms.Button();
            this.txtPathRootDemoBeta = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnReleaseLive = new System.Windows.Forms.Button();
            this.chkClearLiveFiles = new System.Windows.Forms.CheckBox();
            this._SystemTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // btnReleaseDemoBeta
            // 
            this.btnReleaseDemoBeta.Location = new System.Drawing.Point(9, 219);
            this.btnReleaseDemoBeta.Name = "btnReleaseDemoBeta";
            this.btnReleaseDemoBeta.Size = new System.Drawing.Size(97, 23);
            this.btnReleaseDemoBeta.TabIndex = 0;
            this.btnReleaseDemoBeta.Text = "Release Demo";
            this.btnReleaseDemoBeta.UseVisualStyleBackColor = true;
            this.btnReleaseDemoBeta.Click += new System.EventHandler(this.btnReleaseDemoBeta_Click);
            // 
            // btnRestoreStable
            // 
            this.btnRestoreStable.Location = new System.Drawing.Point(9, 248);
            this.btnRestoreStable.Name = "btnRestoreStable";
            this.btnRestoreStable.Size = new System.Drawing.Size(97, 23);
            this.btnRestoreStable.TabIndex = 1;
            this.btnRestoreStable.Text = "Restore Stable";
            this.btnRestoreStable.UseVisualStyleBackColor = true;
            this.btnRestoreStable.Click += new System.EventHandler(this.btnRestoreStable_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Publish Output";
            // 
            // txtPathRootSource
            // 
            this.txtPathRootSource.Enabled = false;
            this.txtPathRootSource.Location = new System.Drawing.Point(127, 23);
            this.txtPathRootSource.Name = "txtPathRootSource";
            this.txtPathRootSource.Size = new System.Drawing.Size(294, 20);
            this.txtPathRootSource.TabIndex = 3;
            // 
            // btnPathRootSource
            // 
            this.btnPathRootSource.Location = new System.Drawing.Point(427, 21);
            this.btnPathRootSource.Name = "btnPathRootSource";
            this.btnPathRootSource.Size = new System.Drawing.Size(29, 23);
            this.btnPathRootSource.TabIndex = 4;
            this.btnPathRootSource.Text = "...";
            this.btnPathRootSource.UseVisualStyleBackColor = true;
            // 
            // btnPathRootProductionClean
            // 
            this.btnPathRootProductionClean.Location = new System.Drawing.Point(427, 47);
            this.btnPathRootProductionClean.Name = "btnPathRootProductionClean";
            this.btnPathRootProductionClean.Size = new System.Drawing.Size(29, 23);
            this.btnPathRootProductionClean.TabIndex = 7;
            this.btnPathRootProductionClean.Text = "...";
            this.btnPathRootProductionClean.UseVisualStyleBackColor = true;
            // 
            // txtPathRootProductionClean
            // 
            this.txtPathRootProductionClean.Enabled = false;
            this.txtPathRootProductionClean.Location = new System.Drawing.Point(127, 49);
            this.txtPathRootProductionClean.Name = "txtPathRootProductionClean";
            this.txtPathRootProductionClean.Size = new System.Drawing.Size(294, 20);
            this.txtPathRootProductionClean.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Production (Clean)";
            // 
            // btnPathRootProductionLive
            // 
            this.btnPathRootProductionLive.Location = new System.Drawing.Point(427, 73);
            this.btnPathRootProductionLive.Name = "btnPathRootProductionLive";
            this.btnPathRootProductionLive.Size = new System.Drawing.Size(29, 23);
            this.btnPathRootProductionLive.TabIndex = 10;
            this.btnPathRootProductionLive.Text = "...";
            this.btnPathRootProductionLive.UseVisualStyleBackColor = true;
            // 
            // txtPathRootProductionLive
            // 
            this.txtPathRootProductionLive.Enabled = false;
            this.txtPathRootProductionLive.Location = new System.Drawing.Point(127, 75);
            this.txtPathRootProductionLive.Name = "txtPathRootProductionLive";
            this.txtPathRootProductionLive.Size = new System.Drawing.Size(294, 20);
            this.txtPathRootProductionLive.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Production (Live)";
            // 
            // btnPathRootStable
            // 
            this.btnPathRootStable.Location = new System.Drawing.Point(427, 99);
            this.btnPathRootStable.Name = "btnPathRootStable";
            this.btnPathRootStable.Size = new System.Drawing.Size(29, 23);
            this.btnPathRootStable.TabIndex = 13;
            this.btnPathRootStable.Text = "...";
            this.btnPathRootStable.UseVisualStyleBackColor = true;
            // 
            // txtPathRootStable
            // 
            this.txtPathRootStable.Enabled = false;
            this.txtPathRootStable.Location = new System.Drawing.Point(127, 101);
            this.txtPathRootStable.Name = "txtPathRootStable";
            this.txtPathRootStable.Size = new System.Drawing.Size(294, 20);
            this.txtPathRootStable.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Stable";
            // 
            // btnPathRootBackups
            // 
            this.btnPathRootBackups.Location = new System.Drawing.Point(427, 151);
            this.btnPathRootBackups.Name = "btnPathRootBackups";
            this.btnPathRootBackups.Size = new System.Drawing.Size(29, 23);
            this.btnPathRootBackups.TabIndex = 16;
            this.btnPathRootBackups.Text = "...";
            this.btnPathRootBackups.UseVisualStyleBackColor = true;
            // 
            // txtPathRootBackups
            // 
            this.txtPathRootBackups.Enabled = false;
            this.txtPathRootBackups.Location = new System.Drawing.Point(127, 153);
            this.txtPathRootBackups.Name = "txtPathRootBackups";
            this.txtPathRootBackups.Size = new System.Drawing.Size(294, 20);
            this.txtPathRootBackups.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Backups";
            // 
            // btnRestoreBackup
            // 
            this.btnRestoreBackup.Location = new System.Drawing.Point(9, 277);
            this.btnRestoreBackup.Name = "btnRestoreBackup";
            this.btnRestoreBackup.Size = new System.Drawing.Size(97, 23);
            this.btnRestoreBackup.TabIndex = 17;
            this.btnRestoreBackup.Text = "Restore Backup";
            this.btnRestoreBackup.UseVisualStyleBackColor = true;
            this.btnRestoreBackup.Click += new System.EventHandler(this.btnRestoreBackup_Click);
            // 
            // btnPathConfigStable
            // 
            this.btnPathConfigStable.Location = new System.Drawing.Point(762, 99);
            this.btnPathConfigStable.Name = "btnPathConfigStable";
            this.btnPathConfigStable.Size = new System.Drawing.Size(29, 23);
            this.btnPathConfigStable.TabIndex = 25;
            this.btnPathConfigStable.Text = "...";
            this.btnPathConfigStable.UseVisualStyleBackColor = true;
            // 
            // txtPathConfigStable
            // 
            this.txtPathConfigStable.Enabled = false;
            this.txtPathConfigStable.Location = new System.Drawing.Point(462, 101);
            this.txtPathConfigStable.Name = "txtPathConfigStable";
            this.txtPathConfigStable.Size = new System.Drawing.Size(294, 20);
            this.txtPathConfigStable.TabIndex = 24;
            // 
            // btnPathConfigProductionLive
            // 
            this.btnPathConfigProductionLive.Location = new System.Drawing.Point(762, 73);
            this.btnPathConfigProductionLive.Name = "btnPathConfigProductionLive";
            this.btnPathConfigProductionLive.Size = new System.Drawing.Size(29, 23);
            this.btnPathConfigProductionLive.TabIndex = 23;
            this.btnPathConfigProductionLive.Text = "...";
            this.btnPathConfigProductionLive.UseVisualStyleBackColor = true;
            // 
            // txtPathConfigProductionLive
            // 
            this.txtPathConfigProductionLive.Enabled = false;
            this.txtPathConfigProductionLive.Location = new System.Drawing.Point(462, 75);
            this.txtPathConfigProductionLive.Name = "txtPathConfigProductionLive";
            this.txtPathConfigProductionLive.Size = new System.Drawing.Size(294, 20);
            this.txtPathConfigProductionLive.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(238, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Site Root Directory";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(587, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Config File";
            // 
            // progressBarTotal
            // 
            this.progressBarTotal.Location = new System.Drawing.Point(127, 235);
            this.progressBarTotal.Name = "progressBarTotal";
            this.progressBarTotal.Size = new System.Drawing.Size(664, 23);
            this.progressBarTotal.TabIndex = 30;
            // 
            // progressBarCurrent
            // 
            this.progressBarCurrent.Location = new System.Drawing.Point(127, 277);
            this.progressBarCurrent.Name = "progressBarCurrent";
            this.progressBarCurrent.Size = new System.Drawing.Size(664, 23);
            this.progressBarCurrent.TabIndex = 31;
            // 
            // lblProgressCurrent
            // 
            this.lblProgressCurrent.AutoSize = true;
            this.lblProgressCurrent.Location = new System.Drawing.Point(124, 261);
            this.lblProgressCurrent.Name = "lblProgressCurrent";
            this.lblProgressCurrent.Size = new System.Drawing.Size(90, 13);
            this.lblProgressCurrent.TabIndex = 32;
            this.lblProgressCurrent.Text = "Current Operation";
            // 
            // lblProgressTotal
            // 
            this.lblProgressTotal.AutoSize = true;
            this.lblProgressTotal.Location = new System.Drawing.Point(124, 219);
            this.lblProgressTotal.Name = "lblProgressTotal";
            this.lblProgressTotal.Size = new System.Drawing.Size(75, 13);
            this.lblProgressTotal.TabIndex = 33;
            this.lblProgressTotal.Text = "Total Progress";
            // 
            // btnPathConfigDemoBeta
            // 
            this.btnPathConfigDemoBeta.Location = new System.Drawing.Point(762, 125);
            this.btnPathConfigDemoBeta.Name = "btnPathConfigDemoBeta";
            this.btnPathConfigDemoBeta.Size = new System.Drawing.Size(29, 23);
            this.btnPathConfigDemoBeta.TabIndex = 38;
            this.btnPathConfigDemoBeta.Text = "...";
            this.btnPathConfigDemoBeta.UseVisualStyleBackColor = true;
            // 
            // txtPathConfigDemoBeta
            // 
            this.txtPathConfigDemoBeta.Enabled = false;
            this.txtPathConfigDemoBeta.Location = new System.Drawing.Point(462, 127);
            this.txtPathConfigDemoBeta.Name = "txtPathConfigDemoBeta";
            this.txtPathConfigDemoBeta.Size = new System.Drawing.Size(294, 20);
            this.txtPathConfigDemoBeta.TabIndex = 37;
            // 
            // btnPathRootDemoBeta
            // 
            this.btnPathRootDemoBeta.Location = new System.Drawing.Point(427, 125);
            this.btnPathRootDemoBeta.Name = "btnPathRootDemoBeta";
            this.btnPathRootDemoBeta.Size = new System.Drawing.Size(29, 23);
            this.btnPathRootDemoBeta.TabIndex = 36;
            this.btnPathRootDemoBeta.Text = "...";
            this.btnPathRootDemoBeta.UseVisualStyleBackColor = true;
            // 
            // txtPathRootDemoBeta
            // 
            this.txtPathRootDemoBeta.Enabled = false;
            this.txtPathRootDemoBeta.Location = new System.Drawing.Point(127, 127);
            this.txtPathRootDemoBeta.Name = "txtPathRootDemoBeta";
            this.txtPathRootDemoBeta.Size = new System.Drawing.Size(294, 20);
            this.txtPathRootDemoBeta.TabIndex = 35;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Demo / Beta";
            // 
            // btnReleaseLive
            // 
            this.btnReleaseLive.Location = new System.Drawing.Point(694, 206);
            this.btnReleaseLive.Name = "btnReleaseLive";
            this.btnReleaseLive.Size = new System.Drawing.Size(97, 23);
            this.btnReleaseLive.TabIndex = 39;
            this.btnReleaseLive.Text = "Release Live (!!!)";
            this.btnReleaseLive.UseVisualStyleBackColor = true;
            this.btnReleaseLive.Click += new System.EventHandler(this.btnReleaseLive_Click);
            // 
            // chkClearLiveFiles
            // 
            this.chkClearLiveFiles.AutoSize = true;
            this.chkClearLiveFiles.Location = new System.Drawing.Point(591, 210);
            this.chkClearLiveFiles.Name = "chkClearLiveFiles";
            this.chkClearLiveFiles.Size = new System.Drawing.Size(97, 17);
            this.chkClearLiveFiles.TabIndex = 40;
            this.chkClearLiveFiles.Text = "Clear Live Files";
            this.chkClearLiveFiles.UseVisualStyleBackColor = true;
            // 
            // _SystemTrayIcon
            // 
            this._SystemTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("_SystemTrayIcon.Icon")));
            this._SystemTrayIcon.Text = "ASP.NET Release Tool";
            this._SystemTrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this._SystemTrayIcon_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 312);
            this.Controls.Add(this.chkClearLiveFiles);
            this.Controls.Add(this.btnReleaseLive);
            this.Controls.Add(this.btnPathConfigDemoBeta);
            this.Controls.Add(this.txtPathConfigDemoBeta);
            this.Controls.Add(this.btnPathRootDemoBeta);
            this.Controls.Add(this.txtPathRootDemoBeta);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblProgressTotal);
            this.Controls.Add(this.lblProgressCurrent);
            this.Controls.Add(this.progressBarCurrent);
            this.Controls.Add(this.progressBarTotal);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnPathConfigStable);
            this.Controls.Add(this.txtPathConfigStable);
            this.Controls.Add(this.btnPathConfigProductionLive);
            this.Controls.Add(this.txtPathConfigProductionLive);
            this.Controls.Add(this.btnRestoreBackup);
            this.Controls.Add(this.btnPathRootBackups);
            this.Controls.Add(this.txtPathRootBackups);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnPathRootStable);
            this.Controls.Add(this.txtPathRootStable);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnPathRootProductionLive);
            this.Controls.Add(this.txtPathRootProductionLive);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnPathRootProductionClean);
            this.Controls.Add(this.txtPathRootProductionClean);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnPathRootSource);
            this.Controls.Add(this.txtPathRootSource);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRestoreStable);
            this.Controls.Add(this.btnReleaseDemoBeta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "ASP.NET Release Tool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnReleaseDemoBeta;
        private System.Windows.Forms.Button btnRestoreStable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPathRootSource;
        private System.Windows.Forms.Button btnPathRootSource;
        private System.Windows.Forms.Button btnPathRootProductionClean;
        private System.Windows.Forms.TextBox txtPathRootProductionClean;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPathRootProductionLive;
        private System.Windows.Forms.TextBox txtPathRootProductionLive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnPathRootStable;
        private System.Windows.Forms.TextBox txtPathRootStable;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPathRootBackups;
        private System.Windows.Forms.TextBox txtPathRootBackups;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRestoreBackup;
        private System.Windows.Forms.Button btnPathConfigStable;
        private System.Windows.Forms.TextBox txtPathConfigStable;
        private System.Windows.Forms.Button btnPathConfigProductionLive;
        private System.Windows.Forms.TextBox txtPathConfigProductionLive;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar progressBarTotal;
        private System.Windows.Forms.ProgressBar progressBarCurrent;
        private System.Windows.Forms.Label lblProgressCurrent;
        private System.Windows.Forms.Label lblProgressTotal;
        private System.Windows.Forms.Button btnPathConfigDemoBeta;
        private System.Windows.Forms.TextBox txtPathConfigDemoBeta;
        private System.Windows.Forms.Button btnPathRootDemoBeta;
        private System.Windows.Forms.TextBox txtPathRootDemoBeta;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnReleaseLive;
        private System.Windows.Forms.CheckBox chkClearLiveFiles;
        private System.Windows.Forms.NotifyIcon _SystemTrayIcon;
    }
}

