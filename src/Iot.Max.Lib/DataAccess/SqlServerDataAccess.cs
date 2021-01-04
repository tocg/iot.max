/**
 * Nuget安装包
 *  1、Microsoft.Extensions.Configuration.Abstractions
 *  2、System.Data.SqlClient
 * **/

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Iot.Max.Lib
{
    public class SqlServerDataAccess : IDataAccess
    {
        IConfiguration Configuration;
        public SqlServerDataAccess(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }

        public string strConn { get  { return Configuration.GetConnectionString("SqlServerConnection"); } set => throw new NotImplementedException(); }

        public Tuple<bool,string,int> Execute(string sql)
        {
            LibResultDto dto = new LibResultDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    dto.Data = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                dto.State = false;
                dto.Message = ex.Message;
            }
            return Tuple.Create(dto.State, dto.Message, dto.Data);
        }

        public Tuple<bool,string, int> Execute(List<string> sqls)
        {
            LibResultDto dto = new LibResultDto();
            try
            {
                using SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                using var tran = conn.BeginTransaction();
                try
                {
                    using var cmd = new SqlCommand();
                    cmd.Connection = conn;

                    foreach (var item in sqls)
                    {
                        cmd.CommandText = item;
                        cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                    dto.Data = sqls.Count;
                }
                catch (Exception _ex)
                {
                    tran.Rollback();
                    dto.State = false;
                    dto.Message = _ex.Message;
                }
            }
            catch (Exception ex)
            {
                dto.State = false;
                dto.Message = ex.Message;
            }
            return Tuple.Create(dto.State, dto.Message, dto.Data);
        }

        public Tuple<bool,string, DataSet> ExecutePro(string proName, Dictionary<string, object> parms)
        {
            LibResultDto dto = new LibResultDto();
            try
            {
                using SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = proName;
                cmd.CommandType = CommandType.StoredProcedure;

                parms.ToList().ForEach(p => cmd.Parameters.AddWithValue(p.Key, p.Value));

                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                dto.Data = ds;
            }
            catch (Exception ex)
            {
                dto.State = false;
                dto.Message = ex.Message;
            }
            return Tuple.Create(dto.State, dto.Message, dto.Data);
        }

        public Tuple<bool,string, DataSet> ExecutePro(string proName, Dictionary<string, object> parms, string outParamName, out object outValue)
        {
            LibResultDto dto = new LibResultDto();
            try
            {
                using SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = proName;
                cmd.CommandType = CommandType.StoredProcedure;

                parms.ToList().ForEach(p => {
                    SqlParameter sqlParm = new SqlParameter();
                    sqlParm.ParameterName = p.Key;
                    if (p.Key.ToLower().Equals(outParamName.ToLower()))
                    {
                        sqlParm.Direction = ParameterDirection.Output;
                        sqlParm.Size = 10;
                    }
                    else { sqlParm.Value = p.Value; }
                    cmd.Parameters.Add(sqlParm); 
                });

                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                dto.Data = ds;

                outValue = cmd.Parameters[outParamName].Value;
            }
            catch (Exception ex)
            {
                outValue = null;
                dto.State = false;
                dto.Message = ex.Message;
            }
            return Tuple.Create(dto.State, dto.Message, dto.Data);
        }

        public Tuple<bool,string, DataSet> Query(string sql)
        {
            LibResultDto dto = new LibResultDto();
            try
            {
                using SqlConnection conn = new SqlConnection(strConn);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                dto.Data = ds;
            }
            catch (Exception ex)
            {
                dto.State = false;
                dto.Message = ex.Message;
            }
            return Tuple.Create(dto.State, dto.Message, dto.Data);
        }

    }
}
