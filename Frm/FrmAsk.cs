﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MAutoUpdate.Models;

namespace MAutoUpdate
{
    public partial class FrmAsk : Form
    {
        #region 初始化
        private UpgradeContext context;

        public FrmAsk(UpgradeContext context)
        {
            InitializeComponent();

            this.context = context;
        }

        private void FrmAsk_Load(object sender, EventArgs e)
        {
            var name = this.context.MainDisplayName;
            var ver = this.context.UpgradeInfo.LastVersion.Trim('v', 'V');
            this.LBTitle.Text = $"新版本-{name} V{ver}";
            this.lblContent.Text = $"{name}正在运行，请问是否关闭{name}立即升级？";
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
        /// 稍后更新,则将更新程序关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateLater_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 开始升级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateNow_Click(object sender, EventArgs e)
        {
            this.Hide();// 隐藏当前窗口

            UpdateForm updateForm = new UpdateForm(this.context);
            if (updateForm.ShowDialog() == DialogResult.OK)
            {
                Application.Exit();
            }
        }

    }
}
