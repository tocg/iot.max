using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("inter_invite")]
    public class InterInvite
    {



        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string Times { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string Images { get; set; }
        public string ID { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string Contacts { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
    }
}
