using System;
using System.Collections.Generic;
using System.Text;

namespace MAutoUpdate.Models
{
    /// <summary></summary>
    public class AppSettingCfgModel
    {
        /// <summary>主业务程序显示名称</summary>
        public String MainDisplayName { get; set; }

        /// <summary>主业务程序路径</summary>
        public String MainExeFullName { get; set; }
        /// <summary>主业务程序启动参数</summary>
        public String MainExeArgs { get; set; }

        /// <summary>升级Json文件全路径</summary>
        public String UpgradeJsonFullName { get; set; }

    }
}
