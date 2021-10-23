using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MAutoUpdate.Commons
{
    /// <summary>
    /// Hash帮助类
    /// </summary>
    public class HashHelper
    {
        /// <summary>
        /// 验证下载文件的Hash值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static bool ValidateHash(string fileFullPath, string remoteHash)
        {
            var md5 = ComputeMD5(fileFullPath) + "";
            return remoteHash.Equals(md5, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///  计算指定文件的MD5值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static String ComputeMD5(String fileName)
        {
            String hashMD5 = String.Empty;
            //检查文件是否存在，如果文件存在则进行计算，否则返回空值
            if (File.Exists(fileName))
            {
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    //计算文件的MD5值
                    var calculator = MD5.Create();
                    var buffer = calculator.ComputeHash(fs);
                    calculator.Clear();
                    //将字节数组转换成十六进制的字符串形式
                    var stringBuilder = new StringBuilder();
                    for (var i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("x2"));
                    }
                    hashMD5 = stringBuilder.ToString();
                }
            }

            return hashMD5;
        }
    }
}
