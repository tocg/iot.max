using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("rbac_role")]
    public class RbacRole
    {
        public string ID { get; set; }
        public string RoleName { get; set; }
        public string Remark { get; set; }
        public string State { get; set; }
        public string Sort { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
    }
}
