using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MAutoUpdate.Commons;

namespace MAutoUpdate.Models
{
    /// <summary></summary>
    public class UpdateContext
    {
        /// <summary>主业务程序全路径</summary>
        public String MainFullName { get; set; }
        /// <summary>升级时需要杀死的进程</summary>
        public List<String> KillExeArr { get; set; } = new List<string>();

        /// <summary>是否管理员权限执行</summary>
        public bool IsAdmin { get; set; }

        /// <summary>升级信息</summary>
        public RemoteRespModel UpgradeInfo { get; set; }

        //临时目录（WIN7以及以上在C盘只有对于temp目录有操作权限）
        public string TempPath { get; set; } = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), @"MAutoUpdate\temp\");
        public string BakPath { get; set; } = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), @"MAutoUpdate\bak\");

        /// <summary>
        /// 从命令行读取
        /// </summary>
        /// <param name="argModel"></param>
        public void Init(ArgumentModel argModel)
        {
            this.MainFullName = argModel.MainProgram;
            //this.MainExeName = new FileInfo(argModel.MainProgram).Name;

            this.KillExeArr.Clear();
            this.KillExeArr.AddRange(argModel.ProgramNames);

        }

        /// <summary>
        /// 从配置中读取
        /// </summary>
        /// <param name="cfg"></param>
        public void Init(AppSettingModel cfg)
        {
            if (!string.IsNullOrEmpty(cfg.MainFullName))
            {
                this.MainFullName = cfg.MainFullName;
                var programArr = cfg.MainFullName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                this.KillExeArr.AddRange(programArr);

            }

        }


    }
}
