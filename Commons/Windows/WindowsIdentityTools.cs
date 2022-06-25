using System;
using System.Collections.Generic;
using System.Text;

namespace MAutoUpdate.Commons
{
    /// <summary></summary>
    public class WindowsIdentityTools
    {
        /// <summary>是否管理员权限运行</summary>
        /// <returns></returns>
        public static bool IsAdmin()
        {
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            //判断当前登录用户是否为管理员 
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }


    }
}
