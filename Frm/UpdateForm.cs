using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using MAutoUpdate.Models;

namespace MAutoUpdate
{
    public partial class UpdateForm : Form
    {
        private UpgradeContext context;
        private UpdateWorkService work;

        public UpdateForm(UpgradeContext context)
        {
            InitializeComponent();

            this.context = context;
            this.work = new UpdateWorkService(this.context);

            work.OnUpdateProgess = ((rate) =>
            {
                var rateInt = (int)rate;
                this.Invoke((Action)(() =>
                {
                    this.updateBar.Value = rateInt;
                    this.lblUpgradeRate.Text = $"{rateInt}%";
                }));
            });
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            var name = this.context.MainDisplayName;
            var ver = this.context.UpgradeInfo.LastVersion.Trim('v', 'V');
            this.LBTitle.Text = $"新版本-{name} V{ver}";
            this.lblContent.Text = this.context.UpgradeInfo.UpgradeContent;

            ThreadPool.QueueUserWorkItem((obj) =>
            {
                try
                {
                    work.Do();
                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    LogTool.AddLog(ex + "");
                    MessageBox.Show(ex.Message);
                }
            });
        }


    }
}
