using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("rbac_user")]
    public class RbacUser
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string DepartmentID { get; set; }
        public string State { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }
}
