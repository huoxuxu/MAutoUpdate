using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAutoUpdate.Commons
{
    /// <summary></summary>
    public class CmdArgTools
    {
        /// <summary>
        /// 处理启动参数
        /// </summary>
        /// <param name="args">
        /// 带-辅参 -upgrade d:/upgrade.json
        /// 不带-辅参 d:/upgrade.json
        /// </param>
        /// <returns></returns>
        public static ArgumentModel GetArgumentModel(string[] args)
        {
            var argModel = new ArgumentModel();
            if (args == null || args.Length == 0) return null;

            // *.exe 1.json
            if (args.Length == 1)
            {
                argModel.UpgradeJsonFullName = args.First()?.Trim();
                return argModel;
            }

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
