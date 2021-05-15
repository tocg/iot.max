using Iot.Max.Model.Models.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Dtos
{
    public class DocumentOrderDto : DocumentOrder
    {
        //附件个数
        public int Annexs { get; set; }

        //附件对应的路径(逗号隔开的)
        public string Paths { get; set; }
    }
}
