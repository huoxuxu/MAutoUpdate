using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace MAutoUpdate.Models
{
    public class MainLocalInfo
    {
        /// <summary>主进程版本</summary>
        public String MainVer { get; set; }
        /// <summary>升级程序版本</summary>
        public String UpgradeVer { get; set; }

        /// <summary>升级时需要上传的文件，多个以逗号隔开</summary>
        public String UploadFiles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainFullName"></param>
        /// <returns></returns>
        public MainLocalInfo Init(string mainFullName)
        {
            {
                FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(mainFullName);
                MainVer = myFileVersion.FileVersion;
            }
            {
                var asm = Assembly.GetEntryAssembly();
                UpgradeVer = getFileVer(asm);
            }

            return this;
        }


        // 获取程序集文件版本
        private static String getFileVer(Assembly asm)
        {
            if (asm != null)
            {
                var fileVers = asm.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                if (fileVers.Length > 0)
                {
                    var ver = ((AssemblyFileVersionAttribute)fileVers[0]).Version + "";

                    return ver;
                }
            }

            return "";
        }

    }
}
