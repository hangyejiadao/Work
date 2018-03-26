using Model.BaseModel;
using System;
using System.Collections.Generic;
using System.Configuration;
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



        public static async Task<bool> Execute(string sql)
        {
            SqlTransaction transaction = null;
            try
            {
                using (SqlConnection con = new SqlConnection(Constr))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        await con.OpenAsync();
                    }

                    SqlCommand cmd = con.CreateCommand();

                    //启动事务
                    transaction = con.BeginTransaction("Transaction");
                    //设定SqlCommand的事务和链接对象
                    cmd.Connection = con;
                    cmd.Transaction = transaction;
                    cmd.CommandText = sql;
                    int resl = cmd.ExecuteNonQuery();
                    if (resl > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                    else
                    {
                        transaction.Rollback();
                        return false; 
                    }

                }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Log.Error(e.ToString());
                return false;
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
            catch (Exception e)
            {

                Log.Error(e.ToString());
                return e.ToString();
            }

        }
    }
}
