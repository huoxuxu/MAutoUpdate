using MAutoUpdate.Commons;
using MAutoUpdate.Models;
using MAutoUpdate.Models.Upgrade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAutoUpdate.Services
{
    /// <summary></summary>
    public class AppInitService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static UpgradeContext InitContext(string[] args)
        {
            // 更新上下文
            UpgradeContext context = new UpgradeContext()
            {
                IsAdmin = WindowsIdentityTools.IsAdmin(),
            };

            #region 处理命令行参数和配置文件
            {
                var asModel = AppSettingsTools.Get<AppSettingCfgModel>();
                context.Init(asModel);

                ArgumentModel argModel = CmdArgTools.GetArgumentModel(args);
                if (argModel != null)
                {
                    context.Init(argModel);
                }
            }
            #endregion

            if (context.UpgradeJsonFullName.IsNullOrEmpty())
                throw new Exception($"请指定升级配置文件！");
            if (!File.Exists(context.UpgradeJsonFullName))
                throw new Exception($"升级配置文件不存在！path:{context.UpgradeJsonFullName}");

            #region 解析升级Json
            var json = File.ReadAllText(context.UpgradeJsonFullName);
            if (json.IsNullOrWhiteSpace()) throw new Exception($"升级配置文件，内容为空！");

            context.UpgradeInfo = parseUpgradeModel(json);
            if (context.UpgradeInfo == null) throw new Exception($"升级配置文件，反序列化为空！");

            #region 处理默认值
            context.MainDisplayName = context.UpgradeInfo.MainAppDisplayName;
            context.MainFullName = context.UpgradeInfo.MainAppFullName;

            context.UpgradeZipFullName = context.UpgradeInfo.UpgradeZipFullName;

            if (context.UpgradeInfo.LastVersion.IsNullOrWhiteSpace())
                context.UpgradeInfo.LastVersion = "";
            if (context.UpgradeInfo.UpgradeContent.IsNullOrWhiteSpace())
                context.UpgradeInfo.UpgradeContent = "";

            #endregion
            #endregion
            LogTool.AddLog($"context:{JsonNetHelper.SerializeObject(context)}");

            #region 校验Context
            if (context.MainDisplayName.IsNullOrEmpty()) throw new Exception($"主程序显示名未指定！");

            if (string.IsNullOrEmpty(context.MainFullName)) throw new Exception($"主程序路径未指定！");
            if (!File.Exists(context.MainFullName)) throw new Exception($"主程序路径不存在！");

            if (context.UpgradeInfo.StartupExeFullName.IsNullOrEmpty()) throw new Exception($"启动程序路径未指定！");
            #endregion

            return context;
        }


        #region 辅助
        private static UpgradeModel parseUpgradeModel(String json)
        {
            try
            {
                return JsonNetHelper.DeserializeObject<UpgradeModel>(json);
            }
            catch (Exception ex)
            {
                throw new Exception("升级文件处理失败，请检查！", ex);
            }
        }
        #endregion

    }
}
