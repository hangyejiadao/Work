using Model.BaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class SqlHelper
    {
        private static LogHelper Log = new LogHelper(typeof(SqlHelper));

        private static readonly string Constr = ConfigurationManager.ConnectionStrings["Constr"].ToString();



        public static async Task<object> Add<T>(T t) where T : Entity, new()
        {
            return await Execute(SqlHelper.GetInsertSql<T>(t));
        }




        /// <summary>
        /// 返回插入的Id
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static async Task<object> Execute(string sql)
        {
        
            try
            {
                using (SqlConnection con = new SqlConnection(Constr))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        await con.OpenAsync();
                    }

                    SqlCommand cmd = new SqlCommand(sql, con);
                
                    object resl =  await cmd.ExecuteScalarAsync();
                    return resl;

                }
            }
            catch (Exception e)
            {
               
                Log.Error(e.ToString());
                return 0;
            }

        }





        protected static string GetInsertSql<T>(T t) where T : Entity
        {
            try
            {
                StringBuilder sbFiled = new StringBuilder("(");
                StringBuilder sbValues = new StringBuilder("(");
                foreach (var item in typeof(T).GetProperties())
                {

                    object[] obj = item.GetCustomAttributes(typeof(KeyAttribute), true);
                    if (obj.Length > 0)
                    {
                        var type = item.GetType();
                        if (
                            item.PropertyType.Name.Equals(typeof(int).Name, StringComparison.OrdinalIgnoreCase) ||
                            item.PropertyType.Name.Equals(typeof(Int64).Name, StringComparison.OrdinalIgnoreCase) ||
                            item.PropertyType.Name.Equals(typeof(Int32).Name, StringComparison.OrdinalIgnoreCase) ||
                            item.PropertyType.Name.Equals(typeof(Int16).Name, StringComparison.OrdinalIgnoreCase) ||
                            item.PropertyType.Name.Equals(typeof(long).Name, StringComparison.OrdinalIgnoreCase)
                            )
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
                        sbValues.Append("'" + item.GetValue(t) + "',");
                    }
                }

                string sql = "INSERT INTO  " + typeof(T).Name + sbFiled.ToString().TrimEnd(',') + ")" + "values " + sbValues.ToString().TrimEnd(',') + ") select Id=@@IDENTITY";
                return sql;
            }
            catch (Exception e)
            {

                Log.Error(e.ToString());
                return e.ToString();
            }

        }
    }
}
