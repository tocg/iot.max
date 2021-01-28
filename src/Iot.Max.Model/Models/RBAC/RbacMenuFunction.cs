using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Max.Model.Models
{
    [Table("rbac_menu_function")]
    public class RbacMenuFunction
    {
        public string ID { get; set; }
        public string MenuID { get; set; }
        public string FunctionID { get; set; }
    }
}
