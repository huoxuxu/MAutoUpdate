using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAutoUpdate.Models
{
    /// <summary></summary>
    public class BackupFileModel
    {
        public FileInfo BkFile { get; set; }

        /// <summary>来自升级包或原始目录</summary>
        public FileFromEnum FromEnum { get; set; }

        /// <summary>MD5</summary>
        public String MD5 { get; set; }

    }

    public enum FileFromEnum
    {
        /// <summary>升级包</summary>
        Upgrade = 10,
        /// <summary>安装目录</summary>
        Original = 20,
    }
}
