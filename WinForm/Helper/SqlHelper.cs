using Model.BaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class SqlHelper
    {
        private static LogHelper Log = new LogHelper(typeof(SqlHelper));

        private static readonly string Constr = ConfigurationManager.ConnectionStrings["Constr"].ToString();



        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static async Task<object> AddAsync<T>(T t) where T : Entity, new()
        {
            return await ExecuteAsync(GetInsertSql<T>(t));
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static    object  Add<T>(T t) where T : Entity, new()
        {
            return   Execute(GetInsertSql<T>(t));
        }
        public static async Task<DataTable> SqlToDataTable<T>(string sql) where T : class, new()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Constr))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        await con.OpenAsync();
                    }

                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            DataTable dt = new DataTable();
                            SqlCommand cmd = new SqlCommand(sql, con, transaction);
                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            adapter.Fill(dt);
                            transaction.Commit();
                            return dt;
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.ToString());
                            transaction.Rollback();
                            return null;
                        }
                    }


                }
            }
            catch (Exception e)
            {

                Log.Error(e.ToString());
                return null;
            }
        }



        /// <summary>
        /// 获取全部对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static async Task<List<T>> GetEntity<T>(Expression<Func<T, bool>> whereLambda) where T : Entity, new()
        {
            List<T> list = DataTableToList<T>(await SqlToDataTable<T>(GetEntitySql<T>()));

            return list.AsQueryable().Where(whereLambda).ToList();
        }

        ///// <summary>
        ///// DataTable转List
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="dt"></param>
        ///// <returns></returns>
        protected static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
        {
            List<T> list = new List<T>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                T t = new T();
                foreach (PropertyInfo p in typeof(T).GetProperties())
                {

                    //p.SetValue(t, Convert.ChangeType(dt.Rows[i][p.Name], p.PropertyType));  
                    var value = dt.Rows[i][p.Name];
                    if (!p.PropertyType.IsGenericType)
                    {
                        p.SetValue(t, value == null ? null : Convert.ChangeType(value, p.PropertyType));
                    }
                    else
                    {
                        Type genericTypeDefinition = p.PropertyType.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(Nullable<>))
                        {
                            p.SetValue(t, value == null ? null : Convert.ChangeType(value, Nullable.GetUnderlyingType(p.PropertyType)));
                        }
                    }
                }
                list.Add(t);
            }
            return list;

        }


        /// <summary>
        /// 返回插入的Id
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static object Execute(string sql)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Constr))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        con.Open();
                    }

                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand(sql, con, transaction);
                            object resl = cmd.ExecuteScalar();
                            transaction.Commit();
                            return resl;
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.ToString());
                            transaction.Rollback();
                            return 0;
                        }
                    }


                }
            }
            catch (Exception e)
            {

                Log.Error(e.ToString());
                return 0;
            }

        }


        /// <summary>
        /// 返回插入的Id
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static async Task<object> ExecuteAsync(string sql)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Constr))
                {
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        await con.OpenAsync();
                    }

                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand(sql, con, transaction);
                            object resl = await cmd.ExecuteScalarAsync();
                            transaction.Commit();
                            return resl;
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.ToString());
                            transaction.Rollback();
                            return 0;
                        }
                    }


                }
            }
            catch (Exception e)
            {

                Log.Error(e.ToString());
                return 0;
            }

        }



        protected static string GetEntitySql<T>() where T : Entity, new()
        {
            return "select * from " + typeof(T).Name + "";
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
