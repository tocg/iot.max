using System;
using System.Collections.Generic;
using System.Text;

namespace Iot.Max.Lib
{
    public interface IDapperFactory
    {
        //DapperClientHelper CreateClient(string name);
        DapperClientHelper CreateClient();
    }
}
