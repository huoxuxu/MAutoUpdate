using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MAutoUpdate.Commons
{
    /// <summary></summary>
    public class ProcessTools
    {
        /// <summary>
        /// 使用UAC启动重启应用程序
        /// </summary>
        /// <param name="args"></param>
        public static void RestartWithUAC(string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                //设置运行文件 
                FileName = System.Windows.Forms.Application.ExecutablePath,
                //设置启动动作,确保以管理员身份运行 
                Verb = "runas",

                Arguments = $" {args}"
            };
            //如果不是管理员，则启动UAC 
            System.Diagnostics.Process.Start(startInfo);
        }

        #region Kill
        /// <summary>
        /// 杀掉当前运行的程序进程
        /// </summary>
        /// <param name="programName">程序名称</param>
        public static int KillProcess(string programName)
        {
            var cou = 0;
            Process[] processes = Process.GetProcessesByName(programName);
            foreach (var p in processes)
            {
                p.Kill();
                p.Close();
                cou++;
            }

            return cou;
        }

        /// <summary>
        /// 杀掉当前运行的程序进程
        /// </summary>
        /// <param name="programNameArr">程序名称集合</param>
        /// <returns>返回kill进程的个数</returns>
        public static int KillAllProcess(params string[] programNameArr)
        {
            var cou = 0;
            foreach (var item in programNameArr)
            {
                var programName = item?.Trim();
                if (programName.IsNullOrEmpty()) continue;

                cou += KillProcess(programName);
            }

            return cou;
        }
        #endregion

    }
}
