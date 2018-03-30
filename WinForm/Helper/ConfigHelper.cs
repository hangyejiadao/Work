using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class ConfigHelper
    {
        private static Dictionary<string, string> _dic = new Dictionary<string, string>();

        public static string GetStringConfig(string key)
        {
            if (_dic.ContainsKey(key))
            {
                return _dic[key];
            }
            else
            {
                string url = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + ConstVar.IPUrl);
                _dic[key] = url;
                return url;
            }
        }

        /// <summary>
        /// ua.log 
        /// </summary>
        /// <returns></returns>
        public static string Ua()
        {
            if (_dic.ContainsKey(ConstVar.Ua))
            {
                return _dic[ConstVar.Ua];
            }
            else
            {
                return File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "ua.log");
            }
        }
    }
}
