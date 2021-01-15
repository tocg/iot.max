using Dapper;
using Iot.Max.Common;
using Iot.Max.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Services
{
    public class WxUserServices : BaseServices
    {

        public override int InsertProcOut<WxUser>(PageParameters<WxUser> parameters, out string id)
        {
            int result;
            try
            {
                result = this.DapperClient.ExecuteStoredProcedure(parameters.Proc.ProcName,parameters.Proc.ProcParm,parameters.Proc.ProcOutName, out id);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

    }
}
