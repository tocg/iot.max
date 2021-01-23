using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("inter_question")]
    public class InterQuestion
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Answer { get; set; }
        public string CategoryID { get; set; }
        public int ReadCount { get; set; }
        public string Images { get; set; }
        public string State { get; set; }
        public string Sort { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
