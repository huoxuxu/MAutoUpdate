using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Text;

namespace MAutoUpdate.Commons
{
    /// <summary>
    /// 
    /// </summary>
    public class AppSettingsTools
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String Get(String key)
        {
            var str = ConfigurationManager.AppSettings[key]?.Trim();

            return str;
        }

        /// <summary>
        /// 获取配置，以T的属性为Key获取配置
        /// 内部使用 Convert.ChangeType 转型，已知枚举不支持
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            var t = Activator.CreateInstance<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var val = Get(prop.Name);
                if (string.IsNullOrEmpty(val)) continue;

                prop.SetValue(t, Convert.ChangeType(val, prop.PropertyType), null);
            }

            return t;
        }
    }
}

