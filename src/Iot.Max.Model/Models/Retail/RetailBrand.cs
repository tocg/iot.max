using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Iot.Max.Model.Models
{
    [Table("retail_brand")]
    public class RetailBrand
    {
        public string ID { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Sort { get; set; }
        public string State { get; set; }
        public DateTime? CreateDate { get; set; } = System.DateTime.Now;
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }

    }
}
