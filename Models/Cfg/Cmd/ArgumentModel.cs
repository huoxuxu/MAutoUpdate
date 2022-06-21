using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MAutoUpdate.Commons
{
    /// <summary></summary>
    public class ArgumentModel
    {
        ///// <summary>主程序全路径</summary>
        //[Description("-main")]
        //public String MainExeFullName { get; set; }
        ///// <summary>主程序启动参数</summary>
        //[Description("-mainArgs")]
        //public String MainExeArgs { get; set; }

        ///// <summary>升级压缩包全路径</summary>
        //[Description("-upgradeZip")]
        //public String UpgradeZipFullName { get; set; }

        /// <summary>升级配置文件全路径</summary>
        [Description("-upgrade")]
        public String UpgradeJsonFullName { get; set; }


        /// <summary>
        /// 获取启动参数
        /// </summary>
        /// <returns></returns>
        public String GetStartupArgs()
        {
            var props = this.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            var ls = new List<String>();
            foreach (var prop in props)
            {
                var desc = prop.GetDesc();
                var name = prop.Name;

                ls.Add(desc.Description);
                ls.Add(name);
            }

            return ls.Join(" ");
        }
    }
}
