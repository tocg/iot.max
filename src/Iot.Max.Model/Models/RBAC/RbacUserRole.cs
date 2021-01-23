using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("rbac_user_role")]
    public class RbacUserRole
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string RoleID { get; set; }
    }
}
