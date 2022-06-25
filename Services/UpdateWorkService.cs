﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading;
using System.Xml;

using MAutoUpdate.Commons;
using MAutoUpdate.Models;
using MAutoUpdate.Services;

namespace MAutoUpdate
{
    /// <summary>更新服务</summary>
    public class UpdateWorkService
    {
        public UpgradeContext context { get; set; }

        /// <summary>升级进度里程碑</summary>
        public Action<String> OnUpdateMilestone { get; set; }

        /// <summary>升级进度, 参数1：完成率，eg.30</summary>
        public Action<double> OnUpdateProgess { get; set; }

        public UpdateWorkService(UpgradeContext context)
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
            6.更新
            7.删除备份

            解压失败
            尝试恢复文件
             */

            OnUpdateProgess?.Invoke(0);
            var tempPath = UpgradeContext.TempPath;
            var bakPath = UpgradeContext.BakPath;

            #region 创建临时目录
            //创建临时目录信息
            DirectoryInfo tempInfo = new DirectoryInfo(tempPath);
            if (!tempInfo.Exists) tempInfo.Create();

            //创建备份目录信息
            DirectoryInfo bakInfo = new DirectoryInfo(bakPath);
            if (!bakInfo.Exists) bakInfo.Create();
            OnUpdateProgess?.Invoke(1);
            #endregion

            var upgradeInfo = context.UpgradeInfo;
            var mainExeFileInfo = new FileInfo(context.MainFullName);
            // 升级文件压缩包
            FileInfo zipFileInfo;

            // 判断是否走直接解压升级
            if (this.context.UpgradeZipFullName.IsNullOrEmpty())
            {
                // 下载升级
                // 升级压缩包全路径
                var mainExeName = Path.GetFileNameWithoutExtension(mainExeFileInfo.Name);
                var zipName = $"{mainExeName}_{upgradeInfo.LastVersion}";
                this.context.UpgradeZipFullName = Path.Combine(tempPath, $"{mainExeName}/{zipName}.zip");

                zipFileInfo = new FileInfo(this.context.UpgradeZipFullName);
                if (zipFileInfo.Exists) zipFileInfo.Delete();
                OnUpdateProgess?.Invoke(5);

                // 下载文件
                downloadByUrl(upgradeInfo.UpgradeZipPackageUrl, zipFileInfo.FullName, 5, 70);
            }
            else
            {
                // 解压升级
                zipFileInfo = new FileInfo(this.context.UpgradeZipFullName);

                LogTool.AddLog("更新程序：解压升级" + this.context.UpgradeZipFullName);
                OnUpdateProgess?.Invoke(70);
            }

            #region 升级前置条件
            // 校验Hash
            if (!upgradeInfo.UpgradeZipPackageMD5.IsNullOrWhiteSpace())
            {
                OnUpdateMilestone?.Invoke($"正在校验升级包...");
                var flag = HashTools.CheckMD5(zipFileInfo, upgradeInfo.UpgradeZipPackageMD5, out var fileMD5);
                LogTool.AddLog($"计算文件MD5值：{fileMD5} " +
                    $"预期MD5值：{upgradeInfo.UpgradeZipPackageMD5} " +
                    $"path:{zipFileInfo.FullName}");

                if (!flag) throw new Exception($"升级包Hash校验失败！");
            }
            OnUpdateProgess?.Invoke(72);

            // 杀进程
            var kills = context.UpgradeInfo.KillExeNameArr ?? new List<String>();
            if (kills.Any())
            {
                OnUpdateMilestone?.Invoke($"正在停止相关进程...");

                var maxKillCount = 5;
                int killCount = 0;
                for (int i = 0; i < maxKillCount; i++)
                {
                    killCount = ProcessTools.KillAllProcess(kills.ToArray());
                    if (killCount == 0) break;

                    Thread.Sleep(500);
                }

                if (killCount > 0)
                    throw new Exception($"进程自动结束失败，已重试 {maxKillCount} 次。"
                        + $"请手动结束以下进程：\r\n{kills.Join("\r\n")}");
            }
            OnUpdateProgess?.Invoke(75);

            #endregion

            var path = mainExeFileInfo.DirectoryName;
            var mainDir = new DirectoryInfo(path);

            #region 备份
            // 解压到临时目录
            LogTool.AddLog("更新程序：准备解压到临时目录");
            var unzipDir = UnzipService.UnzipToTmp(context, out var zipCount);
            LogTool.AddLog($"更新程序：解压到临时目录：{unzipDir}");

            // 计算后需要备份的文件
            var calcBackupFiles = BackupService.Calc(context, unzipDir);
            LogTool.AddLog($"更新程序：计算后需要备份的文件个数：{calcBackupFiles?.Count ?? 0}");
            if (calcBackupFiles?.Any() == true)
            {
                LogTool.AddLog($"更新程序：校验待备份文件占用情况");
                BackupService.CheckFileShare(calcBackupFiles);

                // 开始备份
                LogTool.AddLog($"更新程序：开始备份");
                BackupService.Backup(calcBackupFiles);
                LogTool.AddLog($"更新程序：执行备份成功");
            }
            #endregion

            #region 更新
            try
            {
                OnUpdateMilestone?.Invoke($"正在解压升级包...");
                LogTool.AddLog($"更新程序：解压{this.context.UpgradeZipFullName}");
                MyExt.ICSharpCodeSharpZipLib.ICSharpCodeSharpZipLibTools.UnZipWithProgress(zipFileInfo, mainExeFileInfo.Directory, unzipRate =>
                {
                    var rate = 81 + ((97 - 81) / 100d * unzipRate);
                    rate = Math.Round(rate, 2);
                    OnUpdateProgess?.Invoke(rate);
                });
                LogTool.AddLog($"更新程序：{this.context.UpgradeZipFullName} 解压完成");
                OnUpdateProgess?.Invoke(98);

                //删除备份文件
                OnUpdateMilestone?.Invoke($"正在移除备份...");
                LogTool.AddLog($"解压成功！准备删除备份文件");
                BackupHelper.RemoveFile(path);

                LogTool.AddLog("更新程序：删除备份成功");
                OnUpdateProgess?.Invoke(99);
            }
            catch (Exception ex)
            {
                //解压失败，尝试恢复文件
                LogTool.AddLog($"解压失败！准备恢复文件\n{ex}");

                try
                {
                    // 还原
                    BackupService.Reset(calcBackupFiles);
                    throw new Exception($"解压失败，已回滚成功！");
                    //return;
                }
                catch (Exception ex2)
                {
                    //还原失败！
                    LogTool.AddLog($"还原文件异常！\n{ex2}");
                    throw;
                }
            }
            #endregion
            OnUpdateProgess?.Invoke(100);
            OnUpdateMilestone?.Invoke($"恭喜您，升级完成！");

            LogTool.AddLog("更新程序：更新完成！");
        }


        #region 辅助
        // 下载文件
        private void downloadByUrl(string url, string zipPath, int currentRate, int maxRate)
        {
            LogTool.AddLog("更新程序：下载更新包文件" + url);
            OnUpdateMilestone?.Invoke($"正在下载升级包...");
            DownLoadTools.DownloadFile(url, zipPath, downloadRate =>
            {
                var rate = currentRate + ((maxRate - currentRate) / 100d * downloadRate);
                rate = Math.Round(rate, 2);
                OnUpdateProgess?.Invoke(rate);
            });
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
        #endregion

    }
}
