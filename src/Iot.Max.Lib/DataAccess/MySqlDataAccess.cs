/**
 * Nuget安装包
 *  1、Microsoft.Extensions.Configuration.Abstractions
 *  2、MySql.Data
 * **/
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Iot.Max.Lib
{
    public class MySqlDataAccess : IDataAccess
    {
        //
        IConfiguration Configuration;
        public MySqlDataAccess(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
        }

        public string strConn { get { return Configuration.GetConnectionString("MySqlConnection"); } set => throw new NotImplementedException(); }

        public Tuple<bool,string, int> Execute(string sql)
        {
            LibResultDto dto = new LibResultDto();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
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
                using MySqlConnection conn = new MySqlConnection(strConn);
                conn.Open();
                using var tran = conn.BeginTransaction();
                try
                {
                    using var cmd = new MySqlCommand();
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
                using MySqlConnection conn = new MySqlConnection(strConn);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = proName;
                cmd.CommandType = CommandType.StoredProcedure;

                parms.ToList().ForEach(p => cmd.Parameters.AddWithValue(p.Key, p.Value));

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
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
                using MySqlConnection conn = new MySqlConnection(strConn);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = proName;
                cmd.CommandType = CommandType.StoredProcedure;

                parms.ToList().ForEach(p => {
                    MySqlParameter sqlParm = new MySqlParameter();
                    sqlParm.ParameterName = p.Key;
                    if (p.Key.ToLower().Equals(outParamName.ToLower()))
                    {
                        sqlParm.Direction = ParameterDirection.Output;
                        sqlParm.Size = 10;
                    }
                    else { sqlParm.Value = p.Value; }
                    cmd.Parameters.Add(sqlParm);
                });

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
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
                using MySqlConnection conn = new MySqlConnection(strConn);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd);
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
