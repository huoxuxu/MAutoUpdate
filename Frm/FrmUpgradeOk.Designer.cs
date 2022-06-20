using MAutoUpdate.Control;

namespace MAutoUpdate
{
    partial class FrmUpgradeOk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUpgradeOk));
            this.LBTitle = new System.Windows.Forms.Label();
            this.lblContent = new System.Windows.Forms.Label();
            this.btnStartMainNow = new MAutoUpdate.Control.YButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // LBTitle
            // 
            this.LBTitle.AutoSize = true;
            this.LBTitle.BackColor = System.Drawing.Color.Transparent;
            this.LBTitle.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.LBTitle.ForeColor = System.Drawing.Color.DimGray;
            this.LBTitle.Location = new System.Drawing.Point(11, 13);
            this.LBTitle.Name = "LBTitle";
            this.LBTitle.Size = new System.Drawing.Size(69, 19);
            this.LBTitle.TabIndex = 22;
            this.LBTitle.Text = "升级成功";
            // 
            // lblContent
            // 
            this.lblContent.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.lblContent.ForeColor = System.Drawing.Color.DimGray;
            this.lblContent.Location = new System.Drawing.Point(112, 269);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(358, 39);
            this.lblContent.TabIndex = 24;
            this.lblContent.Text = "已安装xx";
            this.lblContent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStartMainNow
            // 
            this.btnStartMainNow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(173)))), ((int)(((byte)(25)))));
            this.btnStartMainNow.EnterImage = null;
            this.btnStartMainNow.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnStartMainNow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnStartMainNow.IsColorChange = true;
            this.btnStartMainNow.IsFontChange = false;
            this.btnStartMainNow.Location = new System.Drawing.Point(232, 332);
            this.btnStartMainNow.MoveColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(173)))), ((int)(((byte)(25)))));
            this.btnStartMainNow.MoveFontColor = System.Drawing.Color.White;
            this.btnStartMainNow.Name = "btnStartMainNow";
            this.btnStartMainNow.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(173)))), ((int)(((byte)(25)))));
            this.btnStartMainNow.NormalFontColor = System.Drawing.Color.White;
            this.btnStartMainNow.Size = new System.Drawing.Size(141, 45);
            this.btnStartMainNow.TabIndex = 27;
            this.btnStartMainNow.Text = "开始使用";
            this.btnStartMainNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnStartMainNow.Click += new System.EventHandler(this.btnUpdateNow_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(112, 103);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(358, 147);
            this.pictureBox1.TabIndex = 28;
            this.pictureBox1.TabStop = false;
            // 
            // FrmUpgradeOk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(600, 420);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnStartMainNow);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.LBTitle);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmUpgradeOk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PNTop_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LBTitle;
        private System.Windows.Forms.Label lblContent;
        private YButton btnStartMainNow;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}