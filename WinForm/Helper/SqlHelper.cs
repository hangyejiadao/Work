using Model.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class SqlHelper
    {
        public string GetInsertSql<T>(T t) where T : Entity
        {
            StringBuilder sbFiled = new StringBuilder("(");
            StringBuilder sbValues = new StringBuilder("(");
            foreach (var item in typeof(T).GetProperties())
            {

                object[] obj = item.GetCustomAttributes(typeof(KeyAttribute), true);
                if (obj.Length > 0)
                {
                    if (item is int || item is Int32 || item is long || item is Int64 || item is Int16)
                    {
                        continue;
                    }
                    else
                    {
                        sbFiled.Append(item.Name + ",");
                        sbValues.Append("'" + item.GetValue(t) + "',");
                    }
                }
                else
                {
                    sbFiled.Append(item.Name + ",");
                    sbValues.Append(item.GetValue(t) + "',");
                }
            }

            string sql = "INSERT " + typeof(T).Name + sbFiled.ToString().TrimEnd(',') + ")" + "values " + sbValues.ToString().TrimEnd(',') + ")";
            return sql;
        }
    }
}
