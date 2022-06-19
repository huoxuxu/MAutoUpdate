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
        public UpgradeContext context { get; set; }

        public delegate void UpdateProgess(double data);
        /// <summary>升级进度</summary>
        public UpdateProgess OnUpdateProgess;

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
            6.解压升级包
            7.删除备份

            解压失败
            尝试恢复文件
             */

            var bakPath = UpgradeContext.BakPath;
            var tempPath = UpgradeContext.TempPath;

            #region 创建临时目录
            //创建临时目录信息
            DirectoryInfo tempinfo = new DirectoryInfo(tempPath);
            if (!tempinfo.Exists) tempinfo.Create();

            //创建备份目录信息
            DirectoryInfo bakinfo = new DirectoryInfo(bakPath);
            if (!bakinfo.Exists) bakinfo.Create();
            #endregion

            var upgradeInfo = context.UpgradeInfo;
            var mainExeFileInfo = new FileInfo(context.MainFullName);

            var zipPath = Path.Combine(tempPath, $"{mainExeFileInfo.Name}_{upgradeInfo.LastVersion}.zip");
            if (File.Exists(zipPath)) File.Delete(zipPath);

            // 下载文件
            downLoad(upgradeInfo.UpgradeZipPackageUrl, zipPath);

            // 校验Hash
            if (!HashHelper.ValidateHash(zipPath, upgradeInfo.UpgradeZipPackageMD5))
                throw new Exception($"文件Hash校验失败！");

            // 杀进程
            var kills = context.UpgradeInfo.KillExeFullNameArr ?? new List<String>();
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

            var path = mainExeFileInfo.DirectoryName;

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

            // 启动主程序
            LogTool.AddLog($"更新程序：启动 {context.MainFullName} {context.MainArgs}");
            Process.Start(context.MainFullName);

            OnUpdateProgess?.Invoke(100);

            LogTool.AddLog("更新程序：更新完成！");
        }


        #region 辅助
        // 下载文件
        private void downLoad(string url, string zipPath)
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
