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

        public delegate void UpdateUI(int step);//声明一个更新主线程的委托
        public UpdateUI UpdateUIDelegate;

        private UpdateWorkService work;

        public UpdateForm(UpgradeContext context)
        {
            InitializeComponent();

            this.context = context;
            this.work = new UpdateWorkService(this.context);

            UpdateUIDelegate = new UpdateUI((obj) =>
            {
                this.updateBar.Value = obj;
            });

            work.OnUpdateProgess += new UpdateWorkService.UpdateProgess((obj) =>
            {
                this.Invoke(UpdateUIDelegate, (int)obj);
            });
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
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
