using System;
using System.Collections.Generic;
using System.Text;

namespace MAutoUpdate.Commons
{
    public class ArgumentModel
    {
        /// <summary>需要杀掉的程序名称，多个以逗号隔开，第一个为业务主进程，其他为升级前需要杀掉的进程</summary>
        public String Programs { get; set; }

        /// <summary>需要杀掉的程序名称，多个以逗号隔开，第一个为业务主进程，其他为升级前需要杀掉的进程</summary>
        public List<String> ProgramNames { get; set; } = new List<string>();

        /// <summary>主程序全路径</summary>
        public String MainProgram { get; set; }

    }
}
