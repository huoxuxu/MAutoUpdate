using System;
using System.Collections.Generic;
using System.Text;

namespace MAutoUpdate.Commons
{
    /// <summary></summary>
    public class CmdArgTools
    {
        /// <summary>
        /// 处理启动参数
        /// </summary>
        /// <param name="args">-main d:/main.exe -upgrade d:/upgrade.json</param>
        /// <returns></returns>
        public static ArgumentModel GetArgumentModel(string[] args)
        {
            var argModel = new ArgumentModel();
            if (args == null || args.Length == 0) return null;

            var data = args.Chunk(2);
            foreach (var item in data)
            {
                if (item.Count != 2) continue;

                var key = item[0].Trim();
                var val = item[1].Trim();

                if (key.EqualIgnoreCase("-upgrade"))
                {
                    argModel.UpgradeJsonFullName = val;
                }
                //else if (key.EqualIgnoreCase("-main"))
                //{
                //    argModel.MainExeFullName = val;
                //}
                //else if (key.EqualIgnoreCase("-mainArgs"))
                //{
                //    argModel.MainExeArgs = val;
                //}
                //else if (key.EqualIgnoreCase("-upgradeZip"))
                //{
                //    argModel.UpgradeZipFullName = val;
                //}
            }

            return argModel;
        }


    }
}
