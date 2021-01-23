using Iot.Max.Common;
using Iot.Max.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Services
{
    public class BaseServices : IServices
    {
        public DapperClientHelper DapperClient { get; set; }

        public virtual List<T> Query<T>(PageParameters<T> parameters)
        {
            if (parameters.Proc != null)
            {
                return DapperClient.Query<T>(parameters.Proc.ProcName, parameters.Proc.ProcParm);
            }
            else
            {
                //if (parameters.Model == null)
                //    return null;

                Tuple<bool,string,string> tuple = ReflectionLib.ModelToSelectSql<T>();

                if(tuple.Item1)
                    return DapperClient.Query<T>(tuple.Item3, parameters.Model);

                return null;
            }
        }

        public virtual List<T> Query<T>(PageParameters<T> parameters, out int count)
        {
            if (parameters.Proc != null)
            {
                return DapperClient.QueryProcedureOut<T>(parameters.Proc.ProcName, parameters.Proc.ProcParm, parameters.Proc.ProcOutName, out count);
            }
            else
            {
                count = 0;
                return null;
            }
        }

        public virtual T QueryFirst<T>(string id)
        {
            var tuple = ReflectionLib.ModelToSelectSql<T>();

            if (tuple.Item1)
            {
                return DapperClient.QueryFirst<T>($"{tuple.Item3} where id=@id", new { id });
            }

            return default;
        }

        public virtual int Insert<T>(T t)
        {
            int result = 0;
            var tuple = ReflectionLib.ModelToDapperInsertSql(t);
            if (tuple.Item1)
                result = DapperClient.Execute(tuple.Item3, t);
            return result;
        }

        public virtual int Update<T>(T t)
        {
            int result = 0;
            var tuple = ReflectionLib.ModelToDapperUpdateSql(t);
            if (tuple.Item1)
                result = DapperClient.Execute(tuple.Item3, t);
            return result;
        }

        public virtual int Delete<T>(List<T> models)
        {
            int result;
            try
            {
                string _tableName = ReflectionLib.GetAttributeTableName<T>();

                if (string.IsNullOrEmpty(_tableName))
                    return 0;

                result = DapperClient.Execute($"delete from {_tableName} where id = @id", models);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public virtual int InsertProc<T>(PageParameters<T> parameters) 
        {
            int result = -1;
            if (parameters.Proc != null)
            {
                try
                {
                    result = DapperClient.ExecuteStoredProcedure(parameters.Proc.ProcName, parameters.Proc.ProcParm);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;

        }
        public virtual int InsertProcOut<T>(PageParameters<T> parameters, out string id)
        {
            int result = -1;
            id = "";
            if (parameters.Proc != null)
            {
                try
                {
                    result =  DapperClient.ExecuteStoredProcedure(parameters.Proc.ProcName, parameters.Proc.ProcParm, parameters.Proc.ProcOutName, out id);            
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public virtual int Insert<T>(Dictionary<string, List<T>> parm)
        {
            return DapperClient.ExecuteProcedureTransaction(parm);
        }
    }
}
