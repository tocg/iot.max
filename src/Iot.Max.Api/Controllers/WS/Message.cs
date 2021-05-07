using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iot.Max.Api.Controllers.WS
{
    public class Message
    {
        public string SendClientId { get; set; }

        public string action { get; set; }

        public string msg { get; set; }

        public string nick { get; set; }
    }
}
