using MAutoUpdate.Commons;
using MAutoUpdate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAutoUpdate.Services
{
    /// <summary>备份服务</summary>
    public class BackupService
    {
        /// <summary>
        /// 计算需要备份的文件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="backupFiles">需要备份的文件</param>
        public static List<BackupFileModel> Calc(UpgradeContext context, List<String> backupFiles)
        {
            var appBackupFiles = new List<BackupFileModel>();

            // 判断对应安装目录是否存在此文件
            var mainFile = new FileInfo(context.MainFullName);
            foreach (var item in backupFiles)
            {
                var appFileFName = Path.Combine(mainFile.DirectoryName, item);
                var appFile = new FileInfo(appFileFName);
                if (!appFile.Exists)
                {
                    appBackupFiles.Add(new BackupFileModel
                    {
                        BkFile = appFile,
                        FromEnum = FileFromEnum.Upgrade
                    });
                }
                else
                {
                    var md5 = HashTools.CalcMD5(appFile);
                    appBackupFiles.Add(new BackupFileModel
                    {
                        BkFile = appFile,
                        FromEnum = FileFromEnum.Original,
                        MD5 = md5,
                    });
                }
            }

            // 处理配置中需要备份的目录
            var backDirs = context.UpgradeInfo.BackupDirs ?? new List<string>();
            foreach (var item in backDirs)
            {
                var dn = Path.Combine(mainFile.DirectoryName, item);
                var appBkDir = new DirectoryInfo(dn);
                // 跳过无效备份目录
                if (!appBkDir.Exists) continue;

                appBkDir.Foreach(fileAct: file =>
                {
                    var md5 = HashTools.CalcMD5(file);
                    appBackupFiles.Add(new BackupFileModel
                    {
                        BkFile = file,
                        FromEnum = FileFromEnum.Original,
                        MD5 = md5
                    });
                });
            }

            return appBackupFiles;
        }

        /// <summary>
        /// 校验文件是否被占用
        /// 无法判断是否被独占
        /// </summary>
        /// <param name="files"></param>
        /// <exception cref="Exception"></exception>
        private static void CheckFileShare(List<BackupFileModel> files)
        {
            foreach (var item in files)
            {
                if (!item.BkFile.Exists) continue;

                var flag = FileTools.CanShare(item.BkFile);
                if (!flag) throw new Exception($"文件已被占用，备份失败！{item.BkFile.FullName}");
            }
        }

        /// <summary>备份</summary>
        /// <param name="bkls"></param>
        /// <exception cref="Exception"></exception>
        public static void Backup(List<BackupFileModel> bkls)
        {
            try
            {
                foreach (var item in bkls)
                {
                    if (item.FromEnum == FileFromEnum.Original && item.BkFile.Exists)
                    {
                        FileBackupHelper.RenameFile(item.BkFile, log => LogTool.AddLog(log));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("备份失败！", ex);
            }
        }

        /// <summary>
        /// 移除备份文件
        /// </summary>
        /// <param name="bkls"></param>
        /// <exception cref="Exception"></exception>
        public static void Remove(List<BackupFileModel> bkls)
        {
            try
            {
                foreach (var item in bkls)
                {
                    if (item.FromEnum == FileFromEnum.Original && item.BkFile.Exists)
                    {
                        FileBackupHelper.RemoveFile(item.BkFile, log => LogTool.AddLog(log));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("移除备份失败！", ex);
            }
        }

        /// <summary>
        /// 还原，会校验软件目录下原文件的MD5
        /// </summary>
        /// <param name="bkls"></param>
        /// <exception cref="Exception"></exception>
        public static void Reset(List<BackupFileModel> bkls)
        {
            try
            {
                foreach (var item in bkls)
                {
                    // 判断待还原文件是否存在，且MD5一致
                    if (item.BkFile.Exists)
                    {
                        // 校验MD5
                        var flag = HashTools.CheckMD5(item.BkFile, item.MD5, out var fileMD5);
                        if (flag)
                        {
                            // 无需还原
                            continue;
                        }
                    }

                    FileBackupHelper.ResetFile(item.BkFile, log => LogTool.AddLog(log));

                    if (!item.MD5.IsNullOrEmpty())
                    {
                        // 校验MD5
                        var flag = HashTools.CheckMD5(item.BkFile, item.MD5, out var fileMD5);
                        if (!flag)
                        {
                            throw new Exception($"还原文件失败，MD5校验不通过！" +
                                $"path={item.BkFile} fileMD5={fileMD5} 预期MD5：{item.MD5}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("备份失败！", ex);
            }
        }

    }
}
