using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("retail_spu")]
    public class RetailSPU
    {
        public string ID { get; set; }
        public string SpuCode { get; set; }
        public string Title { get; set; }
        public string BrandID { get; set; }
        public string CategoryID { get; set; }
        public string Description { get; set; }
        public string Introduct { get; set; }
        public string Sort { get; set; }
        public string State { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }

    }
}
