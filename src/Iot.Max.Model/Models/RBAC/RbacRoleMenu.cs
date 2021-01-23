using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("rbac_role_menu")]
    public class RbacRoleMenu
    {
        public string ID { get; set; }
        public string RoleID { get; set; }
        public string MenuID { get; set; }
    }
}
