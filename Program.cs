using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

using MAutoUpdate.Services;
using HTools;

namespace MAutoUpdate
{
    static class Program
    {
        /// <summary>
        /// 程序主入口
        /// 启动命令：MAutoUpdate.exe -upgrade d:/upgrade.json
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                LogTool.AddLog($"================================");
                LogTool.AddLog($"开始更新...");
                LogTool.AddLog($"================================");
                {
                    //var f1 = FileTools.CanShare(new FileInfo($"d:/1.xlsx"));
                }
                #region Mutex 更新程序不能存在多个
                var fVerInfo = Process.GetCurrentProcess().MainModule.FileVersionInfo;
                var mutexName = $"{fVerInfo.FileName.Replace("\\", "_").Replace("/", "_")}_{fVerInfo.FileVersion}_Mutex";
                new Mutex(true, mutexName, out var f);//互斥
                if (!f)
                {
                    LogTool.AddLog($"升级程序已存在，当前程序即将退出！");

                    return;
                }
                #endregion

                // 初始化上下文
                var context = AppInitService.InitContext(args);

                /* 
                 * 当前用户是管理员的时候，直接启动应用程序
                 * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行 
                 */
                //创建Windows用户主题 
                Application.EnableVisualStyles();

                //判断当前登录用户是否为管理员
                if (!context.IsAdmin)
                {
                    #region 非管理员，且更新C盘文件
                    // 如果更新C盘文件
                    string result = Environment.GetEnvironmentVariable("systemdrive");
                    if (context.MainFullName.StartsWithIgnoreCase(result))
                    {
                        ProcessTools.RestartWithUAC(args);
                        return;
                    }
                    #endregion
                }

                // 静默更新
                if (context.UpgradeInfo.SilentUpgrade)
                {
                    LogTool.AddLog($"开始执行静默升级...");
                    var updateService = new UpdateWorkService(context);
                    updateService.Do();

                    Thread.Sleep(400);
                    Environment.Exit(0);
                    return;
                }

                //// 强制更新
                //if (resp.ForceUpgrade)
                //{
                //    UpdateForm updateForm = new UpdateForm(updateWork);
                //    if (updateForm.ShowDialog() == DialogResult.OK)
                //    {
                //        Application.Exit();
                //        return;
                //    }
                //}

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(context));

            }
            catch (Exception ex)
            {
                LogTool.AddLog(ex + "");
                MessageBox.Show(ex.Message);

                Environment.Exit(0);
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            if (ex != null)
            {
                LogTool.AddLog(ex + "");
                MessageBox.Show(ex.Message);
            }

            Environment.Exit(0);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                LogTool.AddLog(ex + "");
                MessageBox.Show(ex.Message);
            }

            Environment.Exit(0);
        }



    }
}