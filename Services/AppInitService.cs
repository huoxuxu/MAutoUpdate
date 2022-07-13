using MAutoUpdate.Commons;
using MAutoUpdate.Models;
using MAutoUpdate.Models.Upgrade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace MAutoUpdate.Services
{
    /// <summary></summary>
    public class AppInitService
    {
        /// <summary></summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static UpgradeContext InitContext(string[] args)
        {
            // 更新上下文
            var context = new UpgradeContext()
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

            {
                var oriVal = context.UpgradeJsonFullName;
                if (oriVal.IsNullOrEmpty())
                    throw new Exception($"请指定升级配置文件！如：-upgrade d:/1.json");

                var newVal = processPath(oriVal);
                if (newVal.IsNullOrEmpty())
                    throw new Exception($"升级配置文件不存在！path:{oriVal}");

                context.UpgradeJsonFullName = newVal;
            }
            #region 解析升级Json
            var json = File.ReadAllText(context.UpgradeJsonFullName);
            if (json.IsNullOrWhiteSpace()) throw new Exception($"升级配置文件，内容为空！");

            context.UpgradeInfo = parseUpgradeModel(json);
            if (context.UpgradeInfo == null) throw new Exception($"升级配置文件，反序列化为空！");

            context.MainDisplayName = context.UpgradeInfo.MainAppDisplayName;

            {
                var oriVal = context.UpgradeInfo.MainAppFullName;
                if (oriVal.IsNullOrEmpty())
                    throw new Exception($"主程序全路径不存在！path:{oriVal}");

                var newVal = processPath(oriVal);
                if (newVal.IsNullOrEmpty())
                    throw new Exception($"主程序全路径不存在！path:{oriVal}");

                context.MainFullName = newVal;
                context.UpgradeInfo.MainAppFullName = newVal;
            }

            {
                var oriVal = context.UpgradeInfo.UpgradeZipFullName;
                if (!oriVal.IsNullOrEmpty())
                {
                    var newVal = processPath(oriVal);
                    if (newVal.IsNullOrEmpty())
                        throw new Exception($"升级压缩包全路径不存在！path:{oriVal}");

                    context.UpgradeZipFullName = newVal;
                    context.UpgradeInfo.UpgradeZipFullName = newVal;
                }
            }

            #region 处理默认值
            if (context.UpgradeInfo.LastVersion.IsNullOrWhiteSpace())
                context.UpgradeInfo.LastVersion = "未知";
            if (context.UpgradeInfo.UpgradeContent.IsNullOrWhiteSpace())
                context.UpgradeInfo.UpgradeContent = "发现新版本...";

            {
                var oriVal = context.UpgradeInfo.StartupExeFullName;
                if (oriVal.IsNullOrEmpty())
                {
                    context.UpgradeInfo.StartupExeFullName = context.MainFullName;
                }
                else
                {
                    var newVal = processPath(oriVal);
                    if (newVal.IsNullOrEmpty())
                    {
                        throw new Exception($"启动程序全路径不存在！path:{oriVal}");
                    }
                    context.UpgradeInfo.StartupExeFullName = newVal;
                }
            }
            #endregion
            #endregion
            LogTool.AddLog($"context:{JsonNetHelper.SerializeObject(context)}");

            #region 校验Context
            if (context.MainDisplayName.IsNullOrEmpty()) throw new Exception($"主程序显示名未指定！");

            if (string.IsNullOrEmpty(context.MainFullName)) throw new Exception($"主程序路径未指定！");
            if (!File.Exists(context.MainFullName)) throw new Exception($"主程序路径不存在！");

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

        /// <summary>
        /// 处理路径，
        /// 如果路径为空，返回空
        /// 如果是绝对路径，直接返回，如果是相对路径，则在工作目录下寻找，未找到返回null
        /// </summary>
        /// <param name="upgradeJsonFullName"></param>
        /// <returns></returns>
        private static String processPath(String upgradeJsonFullName)
        {
            var upgradeFile = new FileInfo(upgradeJsonFullName);
            if (!upgradeFile.Exists)
            {
                upgradeJsonFullName = Path.Combine(UpgradeContext.WorkDirectory, upgradeJsonFullName);
                upgradeFile = new FileInfo(upgradeJsonFullName);
                if (upgradeFile.Exists)
                {
                    return upgradeFile.FullName.DirFormat();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return upgradeJsonFullName;
            }
        }
        #endregion

    }
}
