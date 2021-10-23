using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Xml;

using Ionic.Zip;
using MAutoUpdate.Commons;
using MAutoUpdate.Models;
using MAutoUpdate.Services;

namespace MAutoUpdate
{
    /// <summary>更新服务</summary>
    public class UpdateWorkService
    {
        public UpdateContext context { get; set; }

        public delegate void UpdateProgess(double data);
        /// <summary>升级进度</summary>
        public UpdateProgess OnUpdateProgess;

        public UpdateWorkService(UpdateContext context)
        {
            this.context = context;
        }

        /// <summary>执行升级</summary>
        public void Do()
        {
            /*
            1.创建临时目录
            2.下载升级包
            3.校验升级包HASH
            4.杀掉相关进程
            5.备份
            6.解压升级包
            7.删除备份

            解压失败
            尝试恢复文件
             */

            var bakPath = context.BakPath;
            var tempPath = context.TempPath;

            #region 创建临时目录
            //创建备份目录信息
            DirectoryInfo bakinfo = new DirectoryInfo(bakPath);
            if (!bakinfo.Exists) bakinfo.Create();

            //创建临时目录信息
            DirectoryInfo tempinfo = new DirectoryInfo(tempPath);
            if (!tempinfo.Exists) tempinfo.Create();
            #endregion

            var resp = context.UpgradeInfo;

            var zipPath = Path.Combine(tempPath, $"upgrade_{resp.LastVersion}.zip");
            // 下载文件
            DownLoad(resp.UpgradeZipUrl, zipPath);
            // 校验Hash
            if (!HashHelper.ValidateHash(zipPath, resp.UpgradeHashCode))
            {
                throw new Exception($"文件Hash校验失败！");
            }

            // 杀进程
            var kills = context.KillExeArr;
            foreach (var kill in kills)
            {
                var killProgram = kill?.Trim();
                if (string.IsNullOrEmpty(killProgram)) continue;

                KillProcessExist(killProgram);
            }
            Thread.Sleep(400);

            var mainFile = new FileInfo(context.MainFullName);
            var path = mainFile.DirectoryName;
            // 备份 & 更新
            #region 备份文件
            LogTool.AddLog("更新程序：准备执行备份操作");
            try
            {
                //备份文件
                BackupHelper.RenameFile(path);
            }
            catch (Exception ex)
            {
                LogTool.AddLog($"备份文件失败，准备恢复文件！：{ex}");

                try
                {
                    BackupHelper.ResetFile(path);
                    LogTool.AddLog($"文件还原成功！");

                    return;
                }
                catch
                {
                    LogTool.AddLog($"备份文件失败, 文件还原失败！");
                    throw;
                }
            }
            #endregion

            #region 更新
            try
            {
                LogTool.AddLog("更新程序：解压" + zipPath);
                using (ZipFile zip = new ZipFile(zipPath))
                {
                    zip.ExtractAll(path, ExtractExistingFileAction.OverwriteSilently);
                    LogTool.AddLog("更新程序：" + zipPath + "解压完成");
                }

                //删除备份文件
                LogTool.AddLog($"解压成功！准备删除备份文件");
                BackupHelper.RemoveFile(path);
            }
            catch (Exception ex)
            {
                //解压失败，尝试恢复文件
                LogTool.AddLog($"解压异常！准备恢复文件\n{ex}");

                try
                {
                    BackupHelper.ResetFile(path);
                    return;
                }
                catch
                {
                    //还原失败！
                    LogTool.AddLog($"还原文件异常！");
                    throw;
                }

            }
            #endregion

            OnUpdateProgess?.Invoke(98);
            Thread.Sleep(400);

            StartMain(context.MainFullName);
            LogTool.AddLog("更新程序：更新完成！");
        }


        #region 辅助
        // 下载文件
        private void DownLoad(string url, string zipPath)
        {
            using (WebClient web = new WebClient())
            {
                try
                {
                    LogTool.AddLog("更新程序：下载更新包文件" + url);
                    web.DownloadFile(url, zipPath);
                    OnUpdateProgess?.Invoke(60);
                }
                catch (Exception ex)
                {
                    LogTool.AddLog("更新程序：更新包文件" + zipPath + "下载失败,本次停止更新，异常信息：" + ex.Message);
                    throw ex;
                }
            }
        }

        // 拉起主程序
        private void StartMain(string mainPath)
        {
            LogTool.AddLog("更新程序：启动 " + mainPath);
            Process.Start(mainPath);

            OnUpdateProgess?.Invoke(100);
        }

        /// <summary>
        /// 文件拷贝
        /// </summary>
        /// <param name="srcdir">源目录</param>
        /// <param name="desdir">目标目录</param>
        private void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);
            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }
                    CopyDirectory(file, desfolderdir);
                }
                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);
                    srcfileName = desfolderdir + "\\" + srcfileName;
                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }
                    File.Copy(file, srcfileName, true);
                }
            }
        }

        /// <summary>
        /// 杀掉当前运行的程序进程
        /// </summary>
        /// <param name="programName">程序名称</param>
        private void KillProcessExist(string programName)
        {
            Process[] processes = Process.GetProcessesByName(programName);
            foreach (Process p in processes)
            {
                p.Kill();
                p.Close();
            }
        }
        #endregion

    }
}
