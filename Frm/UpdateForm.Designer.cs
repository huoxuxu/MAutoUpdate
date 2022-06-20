namespace MAutoUpdate
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
            this.updateBar = new System.Windows.Forms.ProgressBar();
            this.LBTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblContent = new System.Windows.Forms.Label();
            this.lblUpgradeRate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // updateBar
            // 
            this.updateBar.BackColor = System.Drawing.Color.Lime;
            this.updateBar.Location = new System.Drawing.Point(40, 299);
            this.updateBar.Name = "updateBar";
            this.updateBar.Size = new System.Drawing.Size(509, 28);
            this.updateBar.TabIndex = 24;
            // 
            // LBTitle
            // 
            this.LBTitle.AutoSize = true;
            this.LBTitle.BackColor = System.Drawing.Color.Transparent;
            this.LBTitle.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.LBTitle.ForeColor = System.Drawing.Color.DimGray;
            this.LBTitle.Location = new System.Drawing.Point(12, 9);
            this.LBTitle.Name = "LBTitle";
            this.LBTitle.Size = new System.Drawing.Size(54, 19);
            this.LBTitle.TabIndex = 25;
            this.LBTitle.Text = "新版本";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(107, 243);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(407, 19);
            this.label1.TabIndex = 26;
            this.label1.Text = "正在升级...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblContent
            // 
            this.lblContent.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.lblContent.ForeColor = System.Drawing.Color.DimGray;
            this.lblContent.Location = new System.Drawing.Point(107, 56);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(358, 159);
            this.lblContent.TabIndex = 27;
            // 
            // lblUpgradeRate
            // 
            this.lblUpgradeRate.BackColor = System.Drawing.Color.Transparent;
            this.lblUpgradeRate.Font = new System.Drawing.Font("微软雅黑", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUpgradeRate.ForeColor = System.Drawing.Color.DimGray;
            this.lblUpgradeRate.Location = new System.Drawing.Point(105, 344);
            this.lblUpgradeRate.Name = "lblUpgradeRate";
            this.lblUpgradeRate.Size = new System.Drawing.Size(407, 46);
            this.lblUpgradeRate.TabIndex = 28;
            this.lblUpgradeRate.Text = "0%";
            this.lblUpgradeRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 428);
            this.Controls.Add(this.lblUpgradeRate);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LBTitle);
            this.Controls.Add(this.updateBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UpdateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "UpdateForm";
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar updateBar;
        private System.Windows.Forms.Label LBTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.Label lblUpgradeRate;
    }
}