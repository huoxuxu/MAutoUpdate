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
    public class HashTools
    {
        /// <summary>
        ///  计算指定文件的MD5值
        /// </summary>
        /// <param name="fileName">指定文件的完全限定名称</param>
        /// <returns>返回值的字符串形式</returns>
        public static String GetFileMd5(FileInfo file)
        {
            using (var fs = file.OpenRead())
            {
                //计算文件的MD5值
                using (var calc = MD5.Create())
                {
                    var buffer = calc.ComputeHash(fs);
                    calc.Clear();
                    //将字节数组转换成十六进制的字符串形式
                    var stringBuilder = new StringBuilder();
                    for (var i = 0; i < buffer.Length; i++)
                    {
                        stringBuilder.Append(buffer[i].ToString("x2"));
                    }

                    return stringBuilder.ToString();
                }
            }
        }

        /// <summary>计算文件MD5</summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string CalcMD5(FileInfo file)
        {
            using (FileStream fs = file.OpenRead())
            {
                using (var crypto = MD5.Create())
                {
                    var md5Hash = crypto.ComputeHash(fs);

                    return BitConverter.ToString(md5Hash);
                }
            }
        }

        /// <summary>
        /// 校验文件MD5
        /// </summary>
        /// <param name="file">待计算MD5的文件</param>
        /// <param name="md5">预期MD5</param>
        /// <param name="fileMD5">返回文件的MD5</param>
        public static bool CheckMD5(FileInfo file, string md5, out String fileMD5)
        {
            fileMD5 = CalcMD5(file);
            return fileMD5.EqualIgnoreCase(md5);
        }

    }
}
