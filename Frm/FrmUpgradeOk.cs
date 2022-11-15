using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MAutoUpdate.Commons;
using MAutoUpdate.Models;

namespace MAutoUpdate
{
    public partial class FrmUpgradeOk : Form
    {
        #region 初始化
        private UpgradeContext context;

        public FrmUpgradeOk(UpgradeContext context)
        {
            InitializeComponent();

            this.context = context;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var name = this.context.MainDisplayName;
            var ver = this.context.UpgradeInfo.LastVersion.Trim('v', 'V');
            this.LBTitle.Text = $"新版本-{name} V{ver}";
            this.lblContent.Text = $"已安装{name}";
        }
        #endregion

        #region 让窗体变成可移动
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("User32.dll")]
        private static extern IntPtr WindowFromPoint(Point p);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        private IntPtr moveObject = IntPtr.Zero;    //拖动窗体的句柄

        private void PNTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (moveObject == IntPtr.Zero)
            {
                if (this.Parent != null)
                {
                    moveObject = this.Parent.Handle;
                }
                else
                {
                    moveObject = this.Handle;
                }
            }
            ReleaseCapture();
            SendMessage(moveObject, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        #endregion

        /// <summary>
        /// 立即使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateNow_Click(object sender, EventArgs e)
        {
            var upgradeInfo = context.UpgradeInfo;
            var startupExe = upgradeInfo.StartupExeFullName;
            var startupExeArgs = upgradeInfo.StartupExeArgs;

            // 启动主程序
            LogTool.AddLog($"更新程序：启动 {startupExe} {startupExeArgs}");
            if (startupExeArgs.IsNullOrEmpty())
            {
                System.Diagnostics.Process.Start(startupExe);
            }
            else
            {
                System.Diagnostics.Process.Start(startupExe, startupExeArgs);
            }

            // 等待主进程启动
            var startupExeName = Path.GetFileNameWithoutExtension(startupExe);
            for (int i = 0; i < 1800; i += 300)
            {
                Thread.Sleep(300);
                var pls = HTools.ProcessTools.GetProcessInfo(startupExeName);
                if (pls?.Any() == true)
                {
                    // 进程已启动
                    break;
                }
            }

            this.DialogResult = DialogResult.OK;
        }

    }
}
