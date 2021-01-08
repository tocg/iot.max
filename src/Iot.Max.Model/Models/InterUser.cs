using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("inter_user")]
    public class InterUser
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string ClassID { get; set; }
        public string State { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}
