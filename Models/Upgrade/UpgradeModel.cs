using System;
using System.Collections.Generic;
using System.Text;

namespace MAutoUpdate.Models.Upgrade
{
    /// <summary>升级模型</summary>
    public class UpgradeModel
    {
        /// <summary>最新版本</summary>
        public String LastVersion { get; set; }

        /// <summary>升级压缩包URL，只支持zip</summary>
        public String UpgradeZipPackageUrl { get; set; }
        /// <summary>升级压缩包MD5值</summary>
        public String UpgradeZipPackageMD5 { get; set; }

        /// <summary>升级内容，多行以换行隔开</summary>
        public String UpgradeContent { get; set; }

        /// <summary>升级时需要杀死的进程</summary>
        public List<String> KillExeFullNameArr { get; set; }


    }
}
