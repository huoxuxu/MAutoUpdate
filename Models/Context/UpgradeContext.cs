using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MAutoUpdate.Commons;
using MAutoUpdate.Models.Upgrade;

namespace MAutoUpdate.Models
{
    /// <summary>更新上下文</summary>
    public class UpgradeContext
    {
        /// <summary>临时目录</summary>
        private static String tmpFullPath = Environment.GetEnvironmentVariable("TEMP");

        //临时目录（WIN7以及以上在C盘只有对于temp目录有操作权限）
        /// <summary>MAutoUpdate\temp\</summary>
        public static string TempPath { get; set; } = Path.Combine(tmpFullPath, @"MAutoUpdate\temp\");
        /// <summary>MAutoUpdate\bak\</summary>
        public static string BakPath { get; set; } = Path.Combine(tmpFullPath, @"MAutoUpdate\bak\");
        /// <summary>升级应用运行全路径</summary>
        public static String AppPath { get; set; } = "".GetDLLRunDir();
        /// <summary>工作目录</summary>
        public static String WorkDirectory { get; set; } = Environment.CurrentDirectory;

        /// <summary>是否管理员权限执行</summary>
        public bool IsAdmin { get; set; }

        /// <summary>主业务程序显示名</summary>
        public String MainDisplayName { get; set; }
        /// <summary>主业务程序全路径</summary>
        public String MainFullName { get; set; }
        ///// <summary>主业务程序启动参数</summary>
        //public String MainArgs { get; set; }

        /// <summary>升级Json文件全路径，也可支持相对于工作目录的相对路径</summary>
        public String UpgradeJsonFullName { get; set; }
        /// <summary>升级信息</summary>
        public UpgradeModel UpgradeInfo { get; set; }
        /// <summary>升级压缩包全路径</summary>
        public String UpgradeZipFullName { get; set; }

        /// <summary>程序启动时间</summary>
        public DateTime StartupTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 从命令行读取
        /// </summary>
        /// <param name="argModel"></param>
        public void Init(ArgumentModel argModel)
        {
            //this.MainFullName = argModel.MainExeFullName;
            //this.MainArgs = argModel.MainExeArgs;

            //this.UpgradeZipFullName = argModel.UpgradeZipFullName;
            this.UpgradeJsonFullName = argModel.UpgradeJsonFullName;
        }

        /// <summary>
        /// 从配置中读取
        /// </summary>
        /// <param name="cfg"></param>
        public void Init(AppSettingCfgModel cfg)
        {
            //this.MainDisplayName = cfg.MainDisplayName;
            //this.MainFullName = cfg.MainExeFullName;
            //this.MainArgs = cfg.MainExeArgs;

            this.UpgradeJsonFullName = cfg.UpgradeJsonFullName;
        }


    }
}
