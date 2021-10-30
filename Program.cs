using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Xml;
using MAutoUpdate.Commons;
using MAutoUpdate.Models;
using MAutoUpdate.Services;

namespace MAutoUpdate
{
    static class Program
    {
        /// <summary>
        /// 程序主入口    MAutoUpdate.exe d:/main.exe
        /// </summary>
        /// <param name="args">
        /// [0]主程序全路径
        /// </param>
        [STAThread]
        static void Main(string[] args)
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                LogTool.AddLog($"开始更新...");
				
                #region Mutex 更新程序不能存在多个
                var fVerInfo = Process.GetCurrentProcess().MainModule.FileVersionInfo;
                var mutexName = $"{fVerInfo.FileName.Replace("\\", "_").Replace("/", "_")}_{fVerInfo.FileVersion}_Mutex";
                new Mutex(true, mutexName, out var f);//互斥
                if (!f) return;
                #endregion

                UpdateContext context = new UpdateContext();

                #region 处理命令行参数和配置文件
                var asModel = AppSettingsTools.Get<AppSettingModel>();
                context.Init(asModel);

                ArgumentModel argModel = CmdArgTools.GetArgumentModel(args);
                if (argModel != null && !string.IsNullOrEmpty(argModel.Programs))
                {
                    context.Init(argModel);
                }

                if (string.IsNullOrEmpty(context.MainFullName))
                {
                    throw new Exception($"主程序路径未指定！");
                }
                if (!File.Exists(context.MainFullName))
                {
                    throw new Exception($"主程序路径不存在！");
                }


                LogTool.AddLog(JsonNetHelper.SerializeObject(context));
                #endregion

                var resp = UpgradeService.GetUpgrade(context);
                context.UpgradeInfo = resp;

                if (!resp.Upgrade)
                {
                    // 拉起主程序
                    Process.Start(context.MainFullName);
                    MessageBox.Show($"无需更新！");
                    return;
                }

                UpdateWorkService updateWork = new UpdateWorkService(context);

                /* 
                 * 当前用户是管理员的时候，直接启动应用程序
                 * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行 
                 */
                //创建Windows用户主题 
                Application.EnableVisualStyles();

                //判断当前登录用户是否为管理员
                context.IsAdmin = WindowsIdentityTools.IsAdmin();
                if (!context.IsAdmin)
                {
                    #region 非管理员，且更新C盘文件
                    // 如果更新C盘文件
                    string result = Environment.GetEnvironmentVariable("systemdrive");
                    if (AppDomain.CurrentDomain.BaseDirectory.Contains(result))
                    {
                        //创建启动对象 
                        ProcessStartInfo startInfo = new ProcessStartInfo
                        {
                            //设置运行文件 
                            FileName = System.Windows.Forms.Application.ExecutablePath,
                            //设置启动动作,确保以管理员身份运行 
                            Verb = "runas",

                            Arguments = " " + argModel.Programs
                        };
                        //如果不是管理员，则启动UAC 
                        System.Diagnostics.Process.Start(startInfo);
                        return;
                    }
                    #endregion
                }

                // 静默更新
                if (resp.SilentUpgrade)
                {
                    updateWork.Do();
                    return;
                }

                // 强制更新
                if (resp.ForceUpgrade)
                {
                    UpdateForm updateForm = new UpdateForm(updateWork);
                    if (updateForm.ShowDialog() == DialogResult.OK)
                    {
                        Application.Exit();
                        return;
                    }
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(updateWork));

            }
            catch (Exception ex)
            {
                LogTool.AddLog(ex + "");
                MessageBox.Show(ex.Message);
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