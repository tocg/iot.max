using System;
using System.Collections.Generic;
using System.Text;

namespace Iot.Max.Common
{
    internal class LibResultDto
    {
        public bool State { get; set; } = true;
        public string Message { get; set; } = "ok";
        public dynamic Data { get; set; } = 0;
    }
}
