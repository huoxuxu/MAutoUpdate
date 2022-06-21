using System;
using System.Collections.Generic;
using System.Text;

namespace MAutoUpdate.Models.Upgrade
{
    /// <summary>升级模型</summary>
    public class UpgradeModel
    {
        /// <summary>主程序显示名</summary>
        public String MainAppDisplayName { get; set; }
        /// <summary>主程序全路径</summary>
        public String MainAppFullName { get; set; }

        /// <summary>升级版本</summary>
        public String LastVersion { get; set; }

        /// <summary>升级压缩包URL，只支持zip</summary>
        public String UpgradeZipPackageUrl { get; set; }
        /// <summary>升级压缩包MD5值</summary>
        public String UpgradeZipPackageMD5 { get; set; }

        /// <summary>升级内容，多行以换行隔开</summary>
        public String UpgradeContent { get; set; }

        /// <summary>升级压缩包全路径，升级程序会优先使用此处升级包</summary>
        public String UpgradeZipFullName { get; set; }

        /// <summary>启动的程序全路径。如果为空，默认为主程序</summary>
        public String StartupExeFullName { get; set; }
        /// <summary>启动的程序的启动参数</summary>
        public String StartupExeArgs { get; set; }

        /// <summary>升级时需要杀死的进程</summary>
        public List<String> KillExeFullNameArr { get; set; }

        /// <summary>升级时需要备份的目录</summary>
        public List<String> BackupDirs { get; set; }


    }
}
