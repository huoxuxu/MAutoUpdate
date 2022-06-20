using MAutoUpdate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace MAutoUpdate
{
    /// <summary>下载工具</summary>
    public static class DownLoadTools
    {
        /// <summary>读取网络流超时时间，ms</summary>
        private const int TIMEOUTMS = 30 * 60 * 1000;

        /// <summary>
        /// 执行下载方法
        /// </summary>
        /// <param name="url">下载URL地址</param>
        /// <param name="filePath">保存到的文件路径</param>
        /// <param name="act">下载进度回调</param>
        /// <returns></returns>
        public static void DownloadFile(String url, String filePath, Action<int> act)
        {
            var hc = new HttpWebRequestHelper(url, TIMEOUTMS);

            var fileSize = getFileSizeWithRetry(url);

            var lastRate = 0;
            var rate = 0;
            var cou = 0;
            // 缓冲区
            var bufArr = new Byte[16 * 1024];

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufArr.Length, true))
            {
                hc.HttpWebRequest.GetStream((buffer, offset, count) =>
                {
                    cou += count;
                    rate = (int)((cou * 1d / fileSize) * 100);
                    if (rate > lastRate)
                    {
                        act(rate);
                        lastRate = rate;
                    }

                    fs.Write(buffer, offset, count);
                });
            }
        }

        /// <summary>
        /// 获取文件大小，重试多次
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static long getFileSizeWithRetry(String url)
        {
            // 重试Dic，key=重试次数，val=超时时间
            var dic = new Dictionary<int, int>()
            {
                { 1,3000},
                { 2,6000},
                { 3,10000},
                { 4,15000},
            };

            Exception lastErr = null;
            var i = 0;
            foreach (var item in dic)
            {
                i++;

                try
                {
                    using (HttpWebRequestHelper hc = new HttpWebRequestHelper(url, item.Value))
                    {
                        hc.HttpWebRequest.ReadWriteTimeout = item.Value;
                        return getFileSize(hc);
                    }
                }
                catch (Exception ex)
                {
                    LogTool.AddLog(ex + "");

                    lastErr = ex;
                    LogTool.AddLog($"getFileSizeWithRetry:{i}");
                }
                Thread.Sleep(200);
            }

            if (lastErr != null) throw lastErr;

            return -1;
        }

        #region 辅助
        private static long getFileSize(HttpWebRequestHelper hc)
        {
            using (var resp = hc.HttpWebRequest.ReadResponse())
            {
                return resp?.ContentLength ?? -1;
            }
        }
        #endregion

    }
}
