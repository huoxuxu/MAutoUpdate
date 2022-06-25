using MAutoUpdate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAutoUpdate.Services
{
    /// <summary>压缩、解压服务</summary>
    public class UnzipService
    {
        /// <summary>
        /// 获取压缩包内所有文件
        /// </summary>
        /// <param name="zipFile"></param>
        /// <param name="zipEntryCount"></param>
        /// <returns></returns>
        public static List<String> GetUnzipFiles(FileInfo zipFile, out int zipEntryCount)
        {
            var ls = MyExt.ICSharpCodeSharpZipLib.ICSharpCodeSharpZipLibTools.GetFiles(zipFile, out zipEntryCount);

            return ls;
        }

        /// <summary>
        /// 解压到临时目录，返回解压到的目录
        /// </summary>
        /// <param name="context"></param>
        /// <param name="zipEntryCount"></param>
        /// <returns></returns>
        public static DirectoryInfo UnzipToTmp(UpgradeContext context, out int zipEntryCount)
        {
            var mainExeName = Path.GetFileNameWithoutExtension(context.MainFullName);
            var unzipFullPath = Path.Combine(UpgradeContext.TempPath, $"{mainExeName}/Upgrade/{Guid.NewGuid()}/");
            var unzipDir = new DirectoryInfo(unzipFullPath);
            if (!unzipDir.Exists) unzipDir.Create();

            var zipFile = new FileInfo(context.UpgradeZipFullName);
            // 开始解压
            var cou = MyExt.ICSharpCodeSharpZipLib.ICSharpCodeSharpZipLibTools.UnZip(zipFile, unzipDir);
            zipEntryCount = cou;

            return unzipDir;
        }

    }
}
