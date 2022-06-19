using MAutoUpdate.Control;

namespace MAutoUpdate
{
    partial class FrmAsk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAsk));
            this.LBTitle = new System.Windows.Forms.Label();
            this.lblContent = new System.Windows.Forms.Label();
            this.btnUpdateNow = new MAutoUpdate.Control.YButton();
            this.btnUpdateLater = new MAutoUpdate.Control.YButton();
            this.SuspendLayout();
            // 
            // LBTitle
            // 
            this.LBTitle.AutoSize = true;
            this.LBTitle.BackColor = System.Drawing.Color.Transparent;
            this.LBTitle.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.LBTitle.ForeColor = System.Drawing.Color.DimGray;
            this.LBTitle.Location = new System.Drawing.Point(15, 16);
            this.LBTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LBTitle.Name = "LBTitle";
            this.LBTitle.Size = new System.Drawing.Size(69, 26);
            this.LBTitle.TabIndex = 22;
            this.LBTitle.Text = "新版本";
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblContent.ForeColor = System.Drawing.Color.Black;
            this.lblContent.Location = new System.Drawing.Point(100, 145);
            this.lblContent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(384, 27);
            this.lblContent.TabIndex = 24;
            this.lblContent.Text = "XX正在运行，请问是否关闭XX立即升级？";
            // 
            // btnUpdateNow
            // 
            this.btnUpdateNow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(173)))), ((int)(((byte)(25)))));
            this.btnUpdateNow.EnterImage = null;
            this.btnUpdateNow.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnUpdateNow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnUpdateNow.IsColorChange = true;
            this.btnUpdateNow.IsFontChange = false;
            this.btnUpdateNow.Location = new System.Drawing.Point(270, 245);
            this.btnUpdateNow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnUpdateNow.MoveColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(173)))), ((int)(((byte)(25)))));
            this.btnUpdateNow.MoveFontColor = System.Drawing.Color.White;
            this.btnUpdateNow.Name = "btnUpdateNow";
            this.btnUpdateNow.NormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(173)))), ((int)(((byte)(25)))));
            this.btnUpdateNow.NormalFontColor = System.Drawing.Color.White;
            this.btnUpdateNow.Size = new System.Drawing.Size(188, 56);
            this.btnUpdateNow.TabIndex = 27;
            this.btnUpdateNow.Text = "立即更新";
            this.btnUpdateNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUpdateNow.Click += new System.EventHandler(this.btnUpdateNow_Click);
            // 
            // btnUpdateLater
            // 
            this.btnUpdateLater.BackColor = System.Drawing.Color.White;
            this.btnUpdateLater.EnterImage = null;
            this.btnUpdateLater.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.btnUpdateLater.ForeColor = System.Drawing.Color.Black;
            this.btnUpdateLater.IsColorChange = true;
            this.btnUpdateLater.IsFontChange = false;
            this.btnUpdateLater.Location = new System.Drawing.Point(480, 245);
            this.btnUpdateLater.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnUpdateLater.MoveColor = System.Drawing.Color.White;
            this.btnUpdateLater.MoveFontColor = System.Drawing.Color.Black;
            this.btnUpdateLater.Name = "btnUpdateLater";
            this.btnUpdateLater.NormalColor = System.Drawing.Color.White;
            this.btnUpdateLater.NormalFontColor = System.Drawing.Color.Black;
            this.btnUpdateLater.Size = new System.Drawing.Size(188, 56);
            this.btnUpdateLater.TabIndex = 26;
            this.btnUpdateLater.Text = "以后提醒我";
            this.btnUpdateLater.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnUpdateLater.Click += new System.EventHandler(this.btnUpdateLater_Click);
            // 
            // FrmAsk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(760, 360);
            this.Controls.Add(this.btnUpdateNow);
            this.Controls.Add(this.btnUpdateLater);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.LBTitle);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmAsk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.TopMost = true;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PNTop_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LBTitle;
        private System.Windows.Forms.Label lblContent;
        private YButton btnUpdateLater;
        private YButton btnUpdateNow;
    }
}