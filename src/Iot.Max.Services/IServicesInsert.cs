using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Services
{
    public interface IServicesInsert<T> where T:class
    {
        int InsertProc(T t);
        int InsertProc(T t,out string id);
    }
}
