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

        /// <summary>来自升级包</summary>
        public bool FromUpgrade { get; set; }

        /// <summary>MD5</summary>
        public String MD5 { get; set; }

    }
}
