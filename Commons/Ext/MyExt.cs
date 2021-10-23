using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class MyExt
    {
        public static bool Any(this String[] arr)
        {
            return arr.Length > 0;
        }


    }
}
