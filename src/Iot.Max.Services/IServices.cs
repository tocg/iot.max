using Iot.Max.Common;
using Iot.Max.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Services
{
    public interface IServices
    {
        DapperClientHelper DapperClient { get; set; }
        List<T> Query<T>(PageParameters<T> parameters);
        List<T> Query<T>(PageParameters<T> parameters, out int count);
        T QueryFirst<T>(string id);

        int Insert<T>(T t);
        int Update<T>(T t);
        int Delete<T>(List<T> ids);
    }
}
