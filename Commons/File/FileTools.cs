using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MAutoUpdate.Commons
{
    /// <summary></summary>
    public static class FileTools
    {
        /// <summary>
        /// 是否可共享，true可以，false不可共享
        /// 无法判断是否被独占
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool CanShare(FileInfo file)
        {
            try
            {
                using (file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {

                }

                return true;
            }
            catch
            {
            }

            return false;
        }


    }
}
