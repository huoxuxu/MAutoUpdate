using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MAutoUpdate.Models;

namespace MAutoUpdate.Services
{
    /// <summary></summary>
    public class UpgradeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static RemoteRespModel GetUpgrade(UpdateContext context)
        {
            try
            {
                var jsonName = "ServerInfo.json";
                if (!File.Exists(jsonName)) return new RemoteRespModel();

                var jsonPath = File.ReadAllText(jsonName);
                LogTool.AddLog(jsonPath);

                var resp = JsonNetHelper.DeserializeObject<RemoteRespModel>(jsonPath);

                //var resp = new RemoteRespModel()
                //{
                //    Upgrade = true,
                //    ForceUpgrade = true,
                //    LastVersion = "1.9.0",
                //    UpgradeContent = "升级内容：\n1.XXXX\n2.YYYY",
                //    UpgradeZipUrl = "http://localhost:666/sf/4.7.0_test_0928.zip",
                //    UpgradeHashCode = "EB571F0208C9A9C682468F67ABCEB7F1",
                //};
                return resp;
            }
            catch (Exception ex)
            {
                LogTool.AddLog(ex + "");
            }

            return new RemoteRespModel();
        }


    }
}
