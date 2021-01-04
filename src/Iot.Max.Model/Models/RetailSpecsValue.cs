using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    public class RetailSpecsValue
    {
        public string ID { get; set; }
        public string SpecsID { get; set; }
        public string SpecsValue { get; set; }
        public string Sort { get; set; }
        public string State { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

    }
}
