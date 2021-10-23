using System;
using System.Collections.Generic;
using System.Text;

namespace MAutoUpdate.Commons
{
    public class CmdArgTools
    {
        public static ArgumentModel GetArgumentModel(string[] args)
        {
            var argModel = new ArgumentModel();
            if (args == null || args.Length == 0) return null;

            string programName = args[0];
            if (string.IsNullOrEmpty(programName)) return argModel;

            argModel.Programs = programName.Trim();

            var programArr = argModel.Programs.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            argModel.ProgramNames.AddRange(programArr);
            // 处理主程序
            argModel.MainProgram = programArr[0]?.Trim();

            //if (args.Length > 1)
            //{
            //    argModel.SilentUpdate = "1".Equals((args[1] + "").Trim());

            //    if (args.Length > 2)
            //    {
            //        argModel.IsClickUpdate = "1".Equals((args[2] + "").Trim());
            //    }
            //}

            return argModel;
        }
    }
}
