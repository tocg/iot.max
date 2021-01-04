using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    public class RetailSKU
    {
        public string ID { get; set; }
        public string SKUCode { get; set; }
        public string SPUID { get; set; }
        public string SKUSpecsValues { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string SaleAble { get; set; }
        public string Sort { get; set; }
        public string State { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

    }
}
