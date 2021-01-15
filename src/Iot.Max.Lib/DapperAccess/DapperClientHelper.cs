/*
 * Nuget包安装：
 * 1、Dapper
 * 2、Newtonsoft.Json
 * 3、Microsoft.Extensions.Options
 * 
 * **/

using Dapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Lib
{
    public class DapperClientHelper
    {
        public ConnectionConfig CurrentConnectionConfig { get; set; }

        public DapperClientHelper(IOptionsMonitor<ConnectionConfig> config)
        {
            CurrentConnectionConfig = config.CurrentValue;
        }

        public DapperClientHelper(ConnectionConfig config) { CurrentConnectionConfig = config; }

        IDbConnection _connection = null;
        public IDbConnection Connection
        {
            get
            {
                switch (CurrentConnectionConfig.DbType)
                {
                    case DbStoreType.SqlServer:
                        _connection = new System.Data.SqlClient.SqlConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    case DbStoreType.MySql:
                        _connection = new MySql.Data.MySqlClient.MySqlConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    //case DbStoreType.Sqlite:
                    //    _connection = new SQLiteConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;
                    //case DbStoreType.Oracle:
                    //    _connection = new Oracle.ManagedDataAccess.Client.OracleConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;
                    default:
                        throw new Exception("未指定数据库类型！");
                }
                return _connection;
            }
        }

        /// <summary>
        /// 执行SQL返回集合
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public virtual List<T> Query<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, null).ToList();
            }
        }

        /// <summary>
        /// 执行SQL返回集合
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="obj">参数model</param>
        /// <returns></returns>
        public virtual List<T> Query<T>(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, param).ToList();
            }
        }

        /// <summary>
        /// 执行存储过程返回集合
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="obj">参数model</param>
        /// <returns></returns>
        public virtual List<T> QueryProcedure<T>(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, param, null, true, null, CommandType.StoredProcedure).ToList();
                //conn.Query<T>(strSql, param).ToList();
            }
        }
        /// <summary>
        /// 执行存储过程返回集合带输出参数
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="param">存储过程参数</param>
        /// <param name="outParamName">输出参数的参数名</param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public virtual List<T> QueryProcedureOut<T>(string strSql, DynamicParameters param,string outParamName,out int recordCount)
        {
            using (IDbConnection conn = Connection)
            {
                var list = conn.Query<T>(strSql, param, null, true, null, CommandType.StoredProcedure).ToList();
                recordCount = param.Get<int>(outParamName);
                return list;
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public virtual T QueryFirst<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql).FirstOrDefault<T>();
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public virtual async Task<T> QueryFirstAsync<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                var res = await conn.QueryAsync<T>(strSql);
                return res.FirstOrDefault<T>();
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="obj">参数model</param>
        /// <returns></returns>
        public virtual T QueryFirst<T>(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, param).FirstOrDefault<T>();
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <returns>0成功，-1执行失败</returns>
        public virtual int Execute(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strSql, param) > 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="strProcedure">过程名</param>
        /// <returns></returns>
        public virtual int ExecuteStoredProcedure(string strProcedure)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strProcedure, null, null, null, CommandType.StoredProcedure) > 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="strProcedure">过程名</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public virtual int ExecuteStoredProcedure(string strProcedure, object param)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strProcedure, param, null, null, CommandType.StoredProcedure) > 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="strProcedure">过程名</param>
        /// <param name="param">参数</param>
        /// <param name="outParamName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int ExecuteStoredProcedure(string strProcedure, DynamicParameters param,string outParamName,out string id)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    var result = conn.Execute(strProcedure, param, null, null, CommandType.StoredProcedure);
                    id = param.Get<string>(outParamName);
                    return result > 0 ? 1 : 0;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// ----------存储过程事务(批量插入)
        /// </summary>
        /// <param name="pairs">Dictionary(string,object) sql-object parm</param>
        /// <returns></returns>
        public virtual int ExecuteProcedureTransaction<T>(Dictionary<string, List<T>> pairs)
        {
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                IDbTransaction transaction = conn.BeginTransaction();
                try
                {
                    foreach (KeyValuePair<string, List<T>> kvp in pairs)
                    {
                        foreach (T t in kvp.Value)
                        {
                            conn.Execute(kvp.Key, t, transaction, null, CommandType.StoredProcedure);
                        }

                    }
                    transaction.Commit();

                    return 1;
                }
                catch (Exception exception)
                {

                    transaction.Rollback();
                    //throw exception;
                }
                return -1;
            }
        }

        #region 多表操作
        /// <summary>
        /// 多表操作--事务
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="databaseOption"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual Tuple<bool, string> ExecuteTransaction(List<Tuple<string, object>> trans, int databaseOption = 1, int? commandTimeout = null)
        {
            if (!trans.Any()) return new Tuple<bool, string>(false, "执行事务SQL语句不能为空！");
            using (IDbConnection conn = Connection)
            {
                //开启事务
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var sb = new StringBuilder("ExecuteTransaction 事务： ");
                        foreach (var tran in trans)
                        {
                            sb.Append("SQL语句:" + tran.Item1 + "  \n SQL参数: " + JsonConvert.SerializeObject(tran.Item2) + " \n");
                            // 根据业务添加纪录日志 LogHelper.InfoLog("SQL语句:" + tran.Item1 + "  \n SQL参数: " + 
                            //JsonConvert.SerializeObject(tran.Item2) + " \n");
                            //执行事务
                            conn.Execute(tran.Item1, tran.Item2, transaction, commandTimeout);
                        }
                        var sw = new Stopwatch();
                        sw.Start();
                        //提交事务
                        transaction.Commit();
                        sw.Stop();
                        // 根据业务添加纪录日志 LogHelper.InfoLog(sb.ToString() + "耗时:" + sw.ElapsedMilliseconds + (sw.ElapsedMilliseconds > 1000 ?
                        // "#####" : string.Empty) +"\n");
                        return new Tuple<bool, string>(true, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        //todo:!!!transaction rollback can not work.
                        //LogHelper.ErrorLog(ex);
                        //回滚事务
                        transaction.Rollback();
                        conn.Close();
                        conn.Dispose();
                        return new Tuple<bool, string>(false, ex.ToString());
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
        }
        #endregion
    }
}
