using System;
using System.Collections.Generic;
using System.Text;

namespace MAutoUpdate.Models
{
    /// <summary></summary>
    public class RemoteRespModel
    {
        /// <summary>是否升级</summary>
        public Boolean Upgrade { get; set; }

        /// <summary>最新版本</summary>
        public String LastVersion { get; set; }
        /// <summary>升级内容</summary>
        public String UpgradeContent { get; set; }

        /// <summary>升级包URL</summary>
        public String UpgradeZipUrl { get; set; }
        /// <summary>升级包hash</summary>
        public String UpgradeHashCode { get; set; }

        /// <summary>是否强制升级</summary>
        public Boolean ForceUpgrade { get; set; }
        /// <summary>静默更新</summary>
        public Boolean SilentUpgrade { get; set; }

    }
}
