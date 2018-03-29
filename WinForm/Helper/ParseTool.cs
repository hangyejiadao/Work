using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class ParseTool
    {
        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int StringToInt(string key)
        {
            int tem = 0;
            int.TryParse(key, out tem);
            return tem;
        }
        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static double StringToDouble(string key)
        {
            double tem = 0;
            double.TryParse(key, out tem);
            return tem;
        }
        /// <summary>
        /// string转guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Guid StringToGuid(string str)
        {
            Guid guid = Guid.NewGuid();
            Guid.TryParse(str, out guid);
            return guid;
        }

        /// <summary>
        /// string转guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string str)
        {
            DateTime guid = new DateTime();
            DateTime.TryParse(str, out guid);
            return guid;
        }


        /// <summary>
        /// str转Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T StringToEnum<T>(string str) where T : struct
        {
            T t;
            Enum.TryParse(str, out t);
            return t;
        }


        /// <summary>
        /// string value 转enum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        public static object StringIntValueToEnum(string value, Type type)
        {
            
            return Enum.Parse(type,  value);
        }



        /// <summary>
        /// str 和Type转int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int StringAndTypeToInt(string str, Type type)
        {
            return (int)Enum.Parse(type, str);
        }



        /// <summary>
        /// 根据类的全名获取类型
        /// </summary>
        /// <param name="FullName"></param>
        /// <returns></returns>
        public static Type TransFerType(string FullName)
        {
            string dllName = FullName.Substring(0, FullName.IndexOf(".", StringComparison.Ordinal));
            string path = AppDomain.CurrentDomain.BaseDirectory + dllName + ".dll";
            if (File.Exists(path))
            {
                Assembly assembly = Assembly.LoadFrom(path);
                return assembly.GetType(FullName);
            }
            return null;
        }

    }
}
