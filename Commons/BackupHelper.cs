using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAutoUpdate.Commons
{
    /// <summary>
    /// 
    /// </summary>
    public class BackupHelper
    {
        //备份文件的后缀
        private const String SUFFIX = "-backup";

        /// <summary>
        /// 将指定文件夹的文件改名
        /// abc.exe-backup
        /// </summary>
        /// <param name="dirPath"></param>
        public static void RenameFile(String dirPath)
        {
            var files = Directory.GetFiles(dirPath);
            if (!files.Any()) return;

            foreach (var file in files)
            {
                if (file.EndsWith(SUFFIX, StringComparison.OrdinalIgnoreCase)) continue;

                var fi = new FileInfo(file);

                var destPath = $"{fi.DirectoryName}\\{fi.Name}{SUFFIX}";
                if (File.Exists(destPath)) continue;

                addLog($"RENAME {file}=>{destPath}");
                File.Move(file, destPath);
            }
        }

        /// <summary>
        /// 还原文件名称
        /// 去除文件末尾的-backup
        /// </summary>
        /// <param name="dirPath"></param>
        public static void ResetFile(String dirPath)
        {
            var files = Directory.GetFiles(dirPath, $"*{SUFFIX}");
            if (!files.Any()) return;

            foreach (var file in files)
            {
                var ind = file.LastIndexOf(SUFFIX);
                if (ind == -1) continue;

                var destPath = file.Substring(0, ind);
                //还原时 如果已解压部分文件，需要先删除
                if (File.Exists(destPath)) File.Delete(destPath);

                addLog($"RESET {file}=>{destPath}");
                File.Move(file, destPath);
            }
        }

        /// <summary>
        /// 移除所有备份文件
        /// </summary>
        /// <param name="dirPath"></param>
        public static void RemoveFile(String dirPath)
        {
            var files = Directory.GetFiles(dirPath, $"*{SUFFIX}");
            if (!files.Any()) return;

            foreach (var file in files)
            {
                try
                {
                    File.Delete(file);
                    addLog($"REMOVE {file}");
                }
                catch (Exception ex)
                {
                    addLog(ex + "");
                }
            }
        }

        private static void addLog(string log)
        {
            LogTool.AddLog(log);
        }

    }
}
