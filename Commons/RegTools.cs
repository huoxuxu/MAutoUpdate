using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace MAutoUpdate.Commons
{
    public class RegTools
    {
        public void SaveReg(string subKey)
        {
            RegistryKey Key;
            Key = Registry.CurrentUser;
            //Key = Key.OpenSubKey("SOFTWARE\\GoodMES\\Update");
            Key = Key.OpenSubKey(subKey, true);

            foreach (var item in this.GetType().GetProperties())
            {
                Key.SetValue(item.Name.ToString(), this.GetType().GetProperty(item.Name.ToString()).GetValue(this, null).ToString());
            }
        }

        public void LoadReg(string subKey)
        {
            //获取本地配置文件
            RegistryKey Key;
            Key = Registry.CurrentUser;
            Key = Key.OpenSubKey(subKey);

            foreach (var item in this.GetType().GetProperties())
            {
                this.GetType().GetProperty(item.Name.ToString()).SetValue(this, Key.GetValue(item.Name.ToString()).ToString(), null);
            }
            Key.Close();
        }

        /// <summary>
        /// 设置注册表值
        /// </summary>
        /// <param name="subKey"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetRegistryKey(String subKey, String key, String value)
        {
            RegistryKey reg;
            RegistryKey reglocal = Registry.CurrentUser;

            reg = reglocal.OpenSubKey(subKey, true);
            if (reg == null)
                reg = reglocal.CreateSubKey(subKey);
            reg.SetValue(key, value, RegistryValueKind.String);
            if (reg != null)
            {
                reg.Close();
            }
        }
        private void DelRegistryKey(String subKey, String key)
        {
            RegistryKey reg;
            RegistryKey reglocal = Registry.CurrentUser;

            reg = reglocal.OpenSubKey(subKey, true);
            if (reg != null)
            {
                var res = reg.GetValue(key);
                if (res != null)
                {
                    reg.DeleteValue(key);

                }
            }
            reg.Close();
        }

    }
}
