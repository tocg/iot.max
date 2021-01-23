using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    public class RetailSKUImage
    {
        public string ID { get; set; }
        public string SKUID { get; set; }
        public string ImagePath { get; set; }
        public string ImageFlag { get; set; }
        public string Sort { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}
