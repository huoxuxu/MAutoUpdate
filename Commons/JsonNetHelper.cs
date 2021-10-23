using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data;

namespace System
{
    public static class JsonNetHelper
    {
        private static readonly IsoDateTimeConverter _timeConverter = new IsoDateTimeConverter()
        {
            //解决时间中带T问题
            DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
        };

        #region 序列化
        /// <summary>
        /// 序列化对象（可以是实体或实体集合）
        /// </summary>
        /// <param name="o">源字符串</param>
        /// <returns></returns>
        public static string SerializeObject(object o)
        {
            string jsonStr = JsonConvert.SerializeObject(o, Formatting.None, _timeConverter);
            return jsonStr;
        }

        /// <summary>
        /// 将枚举序列化为JSON
        /// 如果不是枚举则返回null
        /// </summary>
        /// <param name="EnumType"></param>
        /// <returns>像这样的{\"id\":1,\"name\":\"hxx\"}</returns>
        public static string SerializeEnum(Type enumType, string keyName, string valName)
        {
            //[{\"id\":1,\"name\":\"hxx\"}]
            var nameArr = Enum.GetNames(enumType);
            var valArr = Enum.GetValues(enumType);
            StringBuilder sb = new StringBuilder();
            List<string> ls = new List<string>();

            sb.Append("[");
            for (int i = 0; i < nameArr.Length; i++)
            {
                var v = $"{{\"{keyName}\":{(int)valArr.GetValue(i)},\"{valName}\":\"{nameArr[i]}\"}}";
                ls.Add(v);
            }
            sb.Append(string.Join(",", ls.ToArray()));
            sb.Append("]");
            return sb.ToString();
        }
        #endregion

        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="jsonStr">源字符串</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string jsonStr)
        {
            jsonStr = jsonStr ?? "";
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonStr);
            }
            catch (Exception ex)
            {
                var s = jsonStr.Length > 300 ? 300 : jsonStr.Length;
                throw new Exception($"{jsonStr.Substring(0, s)} 反序列化异常！", ex);
            }
        }

        /// <summary>
        /// 反序列化jsonStr为实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static IList<T> DeserializeList<T>(string jsonStr) where T : new()
        {
            jsonStr = jsonStr ?? "";
            try
            {
                //反序列化为实体
                JArray ja = JArray.Parse(jsonStr);

                List<T> ls = new List<T>();
                var results = new List<JToken>();
                foreach (var item in ja)
                {
                    results.Add(item);
                }
                //ja.ToArray();
                if (results.Count == 0) return ls;

                for (int i = 0; i < results.Count; i++)
                {
                    var t = new T();
                    var result = results[i];
                    var resultStr = result.ToString().Replace("\r\n", "");

                    t = JsonConvert.DeserializeObject<T>(resultStr);

                    ls.Add(t);
                }
                return ls;
            }
            catch (Exception ex)
            {
                var s = jsonStr.Length > 300 ? 300 : jsonStr.Length;
                throw new Exception($"{jsonStr.Substring(0, s)} 反序列化异常！", ex);
            }
        }

        /// <summary>
        /// 传入Json数组字符串，解析为DataTable
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static DataTable DeserializeDataTable(string jsonStr)
        {
            jsonStr = jsonStr ?? "";
            try
            {
                //反序列化为实体
                JArray ja = JArray.Parse(jsonStr);
                return JsonConvert.DeserializeObject<DataTable>(ja.ToString());
            }
            catch (Exception ex)
            {
                var s = jsonStr.Length > 300 ? 300 : jsonStr.Length;
                throw new Exception($"{jsonStr.Substring(0, s)} 反序列化异常！", ex);
            }
        }


        ///// <summary>
        ///// 根据传入匿名类型反序列化
        ///// </summary>
        ///// <param name="jsonStr">Json字符串</param>
        ///// <param name="niMingLei">匿名类型</param>
        ///// <returns>
        ///// var a=DeserializeAnonymousType(jsonStr,new{A=string.Empty,B=false})
        ///// a.A;
        ///// a.B;
        ///// </returns>
        //public static dynamic DeserializeAnonymousType(string jsonStr, dynamic niMingLei)
        //{
        //    return JsonConvert.DeserializeAnonymousType(jsonStr, niMingLei);
        //}

        #endregion

    }
}